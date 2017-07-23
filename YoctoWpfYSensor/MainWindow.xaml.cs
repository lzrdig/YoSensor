using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls.DataVisualization.Charting;
using System.Threading;

namespace WpfYSensor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private data
        // used to compute the graph length and time label format
        private double _firstPointDate;
        private double _lastPointDate;
        // used to store dynamic data for the chart
        private readonly ObservableCollection<KeyValuePair<DateTime, double>> _chartPts = 
            new ObservableCollection<KeyValuePair<DateTime, double>>();


        /// <summary>
        /// Variable to track if YAPI hub is initialized
        /// </summary>
        //private int YAPIhubON = YAPI.NOT_INITIALIZED;
        #endregion

        #region public data
        /// <summary>
        /// Timers for raising events for collection of data from YSensor, and for runnindg a simulation
        /// </summary>
        readonly DispatcherTimer _simulationTimer = new DispatcherTimer();
        readonly DispatcherTimer _inventoryTimer = new DispatcherTimer();
        readonly DispatcherTimer _refreshTimer = new DispatcherTimer();
        #endregion

        /// <summary>
        /// MainWindow constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            LineSeries chartLineSeries = new LineSeries();
            chartLineSeries.ItemsSource = _chartPts;
            chartLineSeries.Title = "Voltage";

            /* The following replaces                     
                    ItemsSource="{Binding}" 
                    IndependentValueBinding = "{Binding Path=Key}"
                    DependentValueBinding = "{Binding Path=Value}"      in XAML */
            chartLineSeries.DependentValuePath = "Value";
            chartLineSeries.IndependentValuePath = "Key";

            chartYSensVals.Series.Clear();
            chartYSensVals.Series.Add(chartLineSeries);

            //chartYSensVals.DataContext = ChartPts;    // for some reason this does not plot the points. It requires adding  DataContext="{Binding}" in the XAML
        }


        #region UI handlers
        /// <summary>
        /// called on Loaded event (similar to Form_Load in Window Forms)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string errmsg="";

            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
            {
                MessageBox.Show(errmsg);
            }
            else
            {
                // we wanna know when device list changes
                YAPI.RegisterDeviceArrivalCallback(DeviceArrival);
                YAPI.RegisterDeviceRemovalCallback(DeviceRemoval);

                // configure timers that will raise events periodically
                _inventoryTimer.Interval = TimeSpan.FromMilliseconds(500);
                _inventoryTimer.Tick += new EventHandler(InventoryTimer_Tick);
                _inventoryTimer.IsEnabled = true;
                _inventoryTimer.Start();

                _refreshTimer.Interval = TimeSpan.FromMilliseconds(500);
                _refreshTimer.Tick += new EventHandler(RefreshTimer_Tick);
                _refreshTimer.IsEnabled = true;
                _refreshTimer.Start();

                _simulationTimer.Interval = TimeSpan.FromMilliseconds(300);
                _simulationTimer.Tick += new EventHandler(SimulationTimer_Tick);
                _simulationTimer.IsEnabled = true;
                _simulationTimer.Start();

                toggleButtonSimulation.IsChecked = false;
                YSensor s = YSensor.FirstSensor();
                if (s == null)
                {
                    toggleButtonSimulation.IsChecked = true;
                    comboBoxYSensors.Visibility = Visibility.Hidden;
                    progressBar.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// update the UI according to the sensors count
        /// </summary>
        public void SetSensorComboElmCount()
        {
            if (comboBoxYSensors.Items.Count <= 0) Status.Text = "No sensor found, check USB cable";
            else if (comboBoxYSensors.Items.Count == 1) Status.Text = "One sensor found";
            else Status.Text = comboBoxYSensors.Items.Count + " sensors found";
            if (comboBoxYSensors.Items.Count <= 0) chartYSensVals.Visibility = Visibility.Hidden;
            if (comboBoxYSensors.Items.Count > 0)
                comboBoxYSensors.Visibility = Visibility.Visible;
        }

        #endregion


        /// <summary>
        /// MS doesn't seem to like UNIX timestamps, we have to do the convertion ourselves :-)
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }



        #region Device Handlers

        /// <summary>
        /// automatically called each time a new yoctopuce device is plugged
        /// </summary>
        /// <param name="m"></param>
        public void DeviceArrival(YModule m)
        {   // new device just arrived, lets enumerate all sensors and
            // add the one missing to the combobox           
            YSensor s = YSensor.FirstSensor();
            while (s != null)
            {
                if (!comboBoxYSensors.Items.Contains(s))
                {
                    int index = comboBoxYSensors.Items.Add(s);
                }
                labelUnit.Content = s.get_unit();
                s = s.nextSensor();                
            }
            comboBoxYSensors.IsEnabled = comboBoxYSensors.Items.Count > 0;
            if ((comboBoxYSensors.SelectedIndex < 0) && (comboBoxYSensors.Items.Count > 0))
            {
                comboBoxYSensors.SelectedIndex = 0;
            }
            SetSensorComboElmCount();
        }

        /// <summary>
        /// automatically called each time a new yoctopuce device is unplugged
        /// </summary>
        /// <param name="m"></param>
        public void DeviceRemoval(YModule m)
        {   // a device vas just removed, lets remove the offline sensors
            // from the combo box
            for (int i = comboBoxYSensors.Items.Count - 1; i >= 0; i--)
            {
                if (!((YSensor)comboBoxYSensors.Items[i]).isOnline())
                    comboBoxYSensors.Items.RemoveAt(i);
            }
            SetSensorComboElmCount();
        }

        /// <summary>
        /// automatically called on a regular basis with sensor value
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        public void NewSensorValue(YFunction f, YMeasure m)
        {
            double tDbl = m.get_endTimeUTC();
            DateTime timeDt = UnixTimeStampToDateTime(tDbl);
            double xval = timeDt.ToOADate();
            double yval = m.get_averageValue();

            _chartPts.Add(new KeyValuePair<DateTime, double>(timeDt, yval));

            if (_firstPointDate < 0) _firstPointDate = tDbl;
            _lastPointDate = tDbl;
            SetGraphScale();
            sensorVal.Text = string.Format("f4", m.get_averageValue());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// the core function :  load data from datalogger to send it to the graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxYSensors_SelectedIndexChanged(object sender, EventArgs e)
        {
            // hide the graph while updating
            chartYSensVals.Visibility = Visibility.Hidden;

            // and disable the combo box fow now
            comboBoxYSensors.IsEnabled = false;

            // remove any previous timed report call back 
            for (int i = 0; i < comboBoxYSensors.Items.Count; i++)
                ((YSensor)comboBoxYSensors.Items[i]).registerTimedReportCallback(null);

            int index = comboBoxYSensors.SelectedIndex;
            if (index >= 0) clearGraph();

            YSensor s = GetSelectedSensor();
            if (s != null)
            {
                _firstPointDate = -1;
                _lastPointDate = -1;

                // some ui control
                RefreshDatloggerButton(null);
                progressBar.Visibility = Visibility.Visible;
                Status.Text = "Loading data from datalogger...";

                // load data from datalogger
                YDataSet data = s.get_recordedData(0, 0);
                int progress = data.loadMore();
                while (progress < 100)
                {
                    try
                    {
                        progressBar.Value = progress;
                    }
                    catch { return; }

                    progress = data.loadMore();
                }

                // send the data to the graph
                List<YMeasure> alldata = data.get_measures();

                // used to compute graph length
                if (alldata.Count > 0)
                {
                    _firstPointDate = alldata[0].get_endTimeUTC();
                    _lastPointDate = alldata[alldata.Count - 1].get_endTimeUTC();
                }
                SetGraphScale();

                // restore UI
                comboBoxYSensors.IsEnabled = true;
                progressBar.Visibility = Visibility.Hidden;
                SetSensorComboElmCount();
                s.set_reportFrequency("3/s");
                s.registerTimedReportCallback(NewSensorValue);
                chartYSensVals.Visibility = Visibility.Visible;
                RefreshDatloggerButton(s);
            }
        }

        /// <summary>
        /// // will force a new USB device inventory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryTimer_Tick(object sender, EventArgs e)
        {
            string errmsg = "";
            YAPI.UpdateDeviceList(ref errmsg);
        }

        /// <summary>
        /// will cause Timed report to pop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            string errmsg = "";
            YAPI.HandleEvents(ref errmsg);
        }
        
        /// <summary>
        /// will cause Timed raise of event for the simulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            if (toggleButtonSimulation.IsChecked == true)
            {
                DateTime timeDt = DateTime.Now;
                double doubleT = timeDt.ToOADate();

                Random simulValue = new Random();
                double simulValueDouble = simulValue.NextDouble();

                #region TestChart2
                _chartPts.Add(new KeyValuePair<DateTime, double>(timeDt, simulValueDouble));
                #endregion

                if (_firstPointDate < 0) _firstPointDate = doubleT;
                _lastPointDate = doubleT;

                SetGraphScale();

                sensorVal.Text = simulValueDouble.ToString("f4");
            }
        }

        /// <summary>
        /// returns the sensor selected in the combobox
        /// </summary>
        /// <returns></returns>
        private YSensor GetSelectedSensor()
        {
            int index = comboBoxYSensors.SelectedIndex;
            if (index < 0) return null;
            return (YSensor)comboBoxYSensors.Items[index];
        }

        /// <summary>
        /// update the date labels format according to graph length
        /// </summary>
        private void SetGraphScale()
        {
            // this needs some work on since WPF does not understand this format for the time axis labeling
            int count = _chartPts.Count;
            if (count > 0)
            {
                double total = _lastPointDate - _firstPointDate;
                //    if (total < 180) chartYSensVals.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm:ss";
                //    else if (total < 3600) chartYSensVals.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm";
                //    else if (total < 3600 * 24) chartYSensVals.ChartAreas[0].AxisX.LabelStyle.Format = "h:mm";
                //    else if (total < 3600 * 24 * 7) chartYSensVals.ChartAreas[0].AxisX.LabelStyle.Format = "ddd H";
                //    else if (total < 3600 * 24 * 30) chartYSensVals.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM";
                //    else chartYSensVals.ChartAreas[0].AxisX.LabelStyle.Format = "MMM";
            }
            //else
            //    chartYSensVals.ChartAreas[0].AxisX.LabelStyle.Format = "mm:ss";
        }

        /// <summary>
        /// clear the graph
        /// </summary>
        private void clearGraph()
        {
            // Simplistic implementation of clearGraph. It might need addition of format change for axes
            _chartPts.Clear();
        }

        /// <summary>
        /// update the datalogger control buttons
        /// </summary>
        /// <param name="s"></param>
        private void RefreshDatloggerButton(YSensor s)
        {
            if (s != null)
            {
                YModule m = s.get_module();  // get the module harboring the sensor
                YDataLogger dtl = YDataLogger.FindDataLogger(m.get_serialNumber() + ".dataLogger");
                if (dtl.isOnline())
                {
                    if (dtl.get_recording() == YDataLogger.RECORDING_ON)
                    {
                        buttonRecord.IsEnabled = false;
                        buttonPause.IsEnabled = true;
                        buttonDelete.IsEnabled = false;
                        return;
                    }
                    else
                    {
                        buttonRecord.IsEnabled = true;
                        buttonPause.IsEnabled = false;
                        buttonDelete.IsEnabled = true;
                        return;
                    }
                }
            }
            buttonRecord.IsEnabled = true;
            buttonPause.IsEnabled = false;
            buttonDelete.IsEnabled = true;
        }

        /// <summary>
        /// Datalogger buttons handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataLoggerButton_Click(object sender, EventArgs e)
        {
            YSensor s = GetSelectedSensor();
            if (s != null)
            {
                YModule m = s.get_module();  // get the module harboring the sensor
                YDataLogger dtl = YDataLogger.FindDataLogger(m.get_serialNumber() + ".dataLogger");
                if (dtl.isOnline())
                {
                    if (sender == buttonRecord) dtl.set_recording(YDataLogger.RECORDING_ON);
                    if (sender == buttonPause) dtl.set_recording(YDataLogger.RECORDING_OFF);
                    if (sender == buttonDelete)
                    {
                        dtl.set_recording(YDataLogger.RECORDING_OFF);
                        MessageBox.Show("clear");
                        dtl.forgetAllDataStreams();
                        clearGraph();
                    }
                }
            }
            RefreshDatloggerButton(s);
        }

        /// <summary>
        /// event handler when the clear graph button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearGraph(object sender, RoutedEventArgs e)
        {
            _chartPts.Clear();
        }

        /// <summary>
        /// handle the button responsible for the Simulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simulButton_Changed(object sender, RoutedEventArgs e)
        {
            ToggleButton simulBtn = (ToggleButton)sender;

            if (simulBtn.IsChecked == true)
            {
                simulBtn.Content = "Simulation ON";
                chartYSensVals.Visibility = Visibility.Visible;
                _simulationTimer.Start();
                labelUnit.Content = "V";
            }
            else
            {
                simulBtn.Content = "Simulation OFF";
                _simulationTimer.Stop();
                labelUnit.Content = "";
            }
        }

        #endregion
    }
}
