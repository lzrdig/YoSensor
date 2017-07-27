using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace YSensorApp
{

    public partial class FormYSens : Form
    {
        // constructor
        public FormYSens()
        {
            InitializeComponent();
        }

        // used to compute the graph length and time label format
        private double FirstPointDate;
        private double LastPointDate;
        
        // form contents init
        private void FormYSens_Load(object sender, EventArgs e)
        {
            // we wanna know when device list changes
            YAPI.RegisterDeviceArrivalCallback(deviceArrival);
            YAPI.RegisterDeviceRemovalCallback(deviceRemoval);
            InventoryTimer.Interval = 500;
            InventoryTimer.Start();
            RefreshTimer.Interval = 500;
            RefreshTimer.Start();
        }

        // MS doesn't seem to like UNIX timestamps, we have to do the convertion ourselves :-)
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        // update the UI according to the sensors count
        public void setSensorCount()
        {
            if (comboYDevSel.Items.Count <= 0) Status.Text = "No sensor found, check USB cable";
            else if (comboYDevSel.Items.Count == 1) Status.Text = "One sensor found";
            else  Status.Text = comboYDevSel.Items.Count+" sensors found";
            if (comboYDevSel.Items.Count<=0) chartSens.Visible = false;
            strConnectPlzMsg.Visible = comboYDevSel.Items.Count <= 0;
            Application.DoEvents();
        }

        // automatically called each time a new yoctopuce device is plugged
        public void deviceArrival(YModule m)
        {   // new device just arrived, lets enumerate all sensors and
            // add the one missing to the combobox           
            YSensor s = YSensor.FirstSensor();
            while (s != null)
            {
                if (!comboYDevSel.Items.Contains(s))
                {
                   int index =  comboYDevSel.Items.Add(s);
                }
                s = s.nextSensor();
                if(s!=null) labelUnit.Text = s.get_unit();
            }
            comboYDevSel.Enabled = comboYDevSel.Items.Count>0;
            if ((comboYDevSel.SelectedIndex<0) && (comboYDevSel.Items.Count>0))
               comboYDevSel.SelectedIndex=0;

            setSensorCount();
        }

        // automatically called each time a new yoctopuce device is unplugged
        public void deviceRemoval(YModule m)
        {   // a device vas just removed, lets remove the offline sensors
            // from the combo box
            for (int i = comboYDevSel.Items.Count - 1; i >= 0; i--)
            {
                if (!((YSensor)comboYDevSel.Items[i]).isOnline())
                    comboYDevSel.Items.RemoveAt(i);
            }
            setSensorCount();
        }

        // automatically called on a regular basis with sensor value
        public void newSensorValue(YFunction f,  YMeasure m)
        {
            double t = m.get_endTimeUTC();
            chartSens.Series[0].Points.AddXY(UnixTimeStampToDateTime(t), m.get_averageValue());
            if (FirstPointDate<0)  FirstPointDate=t ;
            LastPointDate = t;
            setGraphScale();
            sensorVal.Text = (m.get_averageValue()).ToString();
        }
        
        // will force a new an USB device inventory 
        private void InventoryTimer_Tick(object sender, EventArgs e)
        {   string errmsg="";
            YAPI.UpdateDeviceList(ref errmsg);
        }

        // will cause Timed report from Yocto device to pop
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            string errmsg = "";
            YAPI.HandleEvents(ref errmsg);
        }

        // returns the sensor selected in the combobox
        private YSensor getSelectedSensor()
        {  int index= comboYDevSel.SelectedIndex;
            if (index<0) return null;
            return  (YSensor)comboYDevSel.Items[index];
        }
     
        // update the date labels format according to graph length
        private void setGraphScale()
        {   int count = chartSens.Series[0].Points.Count;
            if (count > 0)
            {   double total = LastPointDate - FirstPointDate;
                if (total < 180) chartSens.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm:ss";
                else if (total < 3600) chartSens.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm";
                else if (total < 3600 * 24) chartSens.ChartAreas[0].AxisX.LabelStyle.Format = "h:mm";
                else if (total < 3600 * 24 * 7) chartSens.ChartAreas[0].AxisX.LabelStyle.Format = "ddd H";
                else if (total < 3600 * 24 * 30) chartSens.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM";
                else chartSens.ChartAreas[0].AxisX.LabelStyle.Format = "MMM";
            }
            else chartSens.ChartAreas[0].AxisX.LabelStyle.Format = "mm:ss";
        }

        // clear the graph
        private void clearGraph()
        {
            chartSens.Series[0].XValueType = ChartValueType.DateTime;
            chartSens.Series[0].Points.SuspendUpdates();
            //chart1.Series[0].Points.Clear();  indecently slow
            while (chartSens.Series[0].Points.Count > 0)
                chartSens.Series[0].Points.RemoveAt(chartSens.Series[0].Points.Count - 1);
            chartSens.Series[0].Points.ResumeUpdates();
        }

        // the core function :  load data from datalogger to send it to the graph
        private void comboYDevSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // lets hide the graph while updating
            chartSens.Visible=false;
            comboYDevSel.Enabled = false;
              

            // remove any previous timed report call back 
            for (int i = 0; i < comboYDevSel.Items.Count; i++)
              ((YSensor)comboYDevSel.Items[i]).registerTimedReportCallback(null);

            // allow zooming
            chartSens.ChartAreas[0].CursorX.Interval = 0.001;
            chartSens.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartSens.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chartSens.ChartAreas[0].CursorX.AutoScroll = true;
            chartSens.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chartSens.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            int index = comboYDevSel.SelectedIndex;
            if (index >= 0) clearGraph();
          

           YSensor s= getSelectedSensor();
           if (s != null)
           {   FirstPointDate=-1;
               LastPointDate=-1;
               // some ui control
               strLoadingMsg.Visible=true;
               refreshDatloggerButton(null);
               progressBar.Visible = true;
               Status.Text = "Loading data from datalogger...";
               for (int i = 0; i < 100;i++ ) Application.DoEvents(); // makes sure the UI changes are repainted
               
               // load data from datalogger
               YDataSet data = s.get_recordedData(0, 0);
               int progress = data.loadMore();
               while (progress < 100)
               {
                   try {
                       progressBar.Value = progress;
                   } catch { return;}
                   
                   Application.DoEvents();
                   progress = data.loadMore();
               }

               // sets the unit (because ° is not a ASCII-128  character, Yoctopuce temperature
               // sensors report unit as 'C , so we fix it).
               chartSens.ChartAreas[0].AxisY.Title = s.get_unit().Replace("'C","°C");
               chartSens.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Regular);

               // send the data to the graph
               List<YMeasure> alldata = data.get_measures();
               for (int i = 0; i < alldata.Count; i++)
               {   
                   chartSens.Series[0].Points.AddXY(UnixTimeStampToDateTime(alldata[i].get_endTimeUTC()), alldata[i].get_averageValue());

               }

               // used to compute graph length
               if (alldata.Count>0)
               {
                   FirstPointDate = alldata[0].get_endTimeUTC();
                   LastPointDate = alldata[alldata.Count - 1].get_endTimeUTC();

               }
               setGraphScale();
               
               // restore UI
               comboYDevSel.Enabled=true;
               progressBar.Visible = false;
               setSensorCount();
               s.set_reportFrequency("3/s");
               s.registerTimedReportCallback(newSensorValue);
               strLoadingMsg.Visible = false;
               chartSens.Visible=true;
               refreshDatloggerButton(s);
           }
        }

        // update the datalogger control buttons
        private void refreshDatloggerButton(YSensor s)
        {
            if (s != null)
            {
                YModule m = s.get_module();  // get the module harboring the sensor
                YDataLogger dtl = YDataLogger.FindDataLogger(m.get_serialNumber() + ".dataLogger");
                if (dtl.isOnline())
                {
                    if (dtl.get_recording() == YDataLogger.RECORDING_ON)
                    {
                        recordButton.Enabled = false;
                        pauseButton.Enabled = true;
                        deleteButton.Enabled = false;
                        return;
                    }
                    else
                    {
                        recordButton.Enabled = true;
                        pauseButton.Enabled = false;
                        deleteButton.Enabled = true;
                        return;
                    }
                }
            }
            recordButton.Enabled = false;
            pauseButton.Enabled = false;
            deleteButton.Enabled = false;
        }

        // Datalogger buttons handling
        private void DataLoggerButton_Click(object sender, EventArgs e)
        {   YSensor s = getSelectedSensor();
            if (s != null)
            {   YModule m = s.get_module();  // get the module harboring the sensor
                YDataLogger dtl = YDataLogger.FindDataLogger(m.get_serialNumber() + ".dataLogger");
                if (dtl.isOnline())
                { if (sender==recordButton) dtl.set_recording(YDataLogger.RECORDING_ON);
                  if (sender == pauseButton) dtl.set_recording(YDataLogger.RECORDING_OFF);
                  if (sender == deleteButton)
                  {   dtl.set_recording(YDataLogger.RECORDING_OFF);
                      MessageBox.Show("clear");
                      dtl.forgetAllDataStreams();
                      clearGraph();
                  }             
                }
            }
            refreshDatloggerButton(s);
        }
    }
}
