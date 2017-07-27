namespace YSensorApp
{
    partial class FormYSens
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormYSens));
            this.comboYDevSel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InventoryTimer = new System.Windows.Forms.Timer(this.components);
            this.chartSens = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.strConnectPlzMsg = new System.Windows.Forms.Label();
            this.strLoadingMsg = new System.Windows.Forms.Label();
            this.sensorVal = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.recordButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartSens)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboYDevSel
            // 
            this.comboYDevSel.AccessibleDescription = "";
            this.comboYDevSel.DisplayMember = "friendlyName";
            this.comboYDevSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboYDevSel.Enabled = false;
            this.comboYDevSel.FormattingEnabled = true;
            this.comboYDevSel.Location = new System.Drawing.Point(121, 12);
            this.comboYDevSel.Name = "comboYDevSel";
            this.comboYDevSel.Size = new System.Drawing.Size(290, 21);
            this.comboYDevSel.TabIndex = 0;
            this.toolTip1.SetToolTip(this.comboYDevSel, "Available sensor functions");
            this.comboYDevSel.SelectedIndexChanged += new System.EventHandler(this.comboYDevSel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available sensors:";
            // 
            // InventoryTimer
            // 
            this.InventoryTimer.Interval = 500;
            this.InventoryTimer.Tick += new System.EventHandler(this.InventoryTimer_Tick);
            // 
            // chartSens
            // 
            this.chartSens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chartSens.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            chartArea1.ShadowOffset = 3;
            this.chartSens.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chartSens.Legends.Add(legend1);
            this.chartSens.Location = new System.Drawing.Point(15, 39);
            this.chartSens.Name = "chartSens";
            this.chartSens.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Color = System.Drawing.Color.Red;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.chartSens.Series.Add(series1);
            this.chartSens.Size = new System.Drawing.Size(494, 281);
            this.chartSens.TabIndex = 2;
            this.chartSens.Text = "chartSens";
            this.chartSens.Visible = false;
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status,
            this.progressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 323);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(680, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Status
            // 
            this.Status.AutoSize = false;
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(200, 17);
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // strConnectPlzMsg
            // 
            this.strConnectPlzMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.strConnectPlzMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strConnectPlzMsg.Location = new System.Drawing.Point(63, 152);
            this.strConnectPlzMsg.Name = "strConnectPlzMsg";
            this.strConnectPlzMsg.Size = new System.Drawing.Size(404, 37);
            this.strConnectPlzMsg.TabIndex = 7;
            this.strConnectPlzMsg.Text = "Please connect a Yoctopuce device featuring a sensor.";
            this.strConnectPlzMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.strConnectPlzMsg.Visible = false;
            // 
            // strLoadingMsg
            // 
            this.strLoadingMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.strLoadingMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strLoadingMsg.Location = new System.Drawing.Point(63, 172);
            this.strLoadingMsg.Name = "strLoadingMsg";
            this.strLoadingMsg.Size = new System.Drawing.Size(404, 37);
            this.strLoadingMsg.TabIndex = 8;
            this.strLoadingMsg.Text = "Loading data, please wait";
            this.strLoadingMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.strLoadingMsg.Visible = false;
            // 
            // sensorVal
            // 
            this.sensorVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensorVal.Location = new System.Drawing.Point(506, 57);
            this.sensorVal.Name = "sensorVal";
            this.sensorVal.Size = new System.Drawing.Size(82, 26);
            this.sensorVal.TabIndex = 9;
            this.sensorVal.Text = "0.0";
            this.sensorVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUnit.Location = new System.Drawing.Point(594, 60);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(0, 20);
            this.labelUnit.TabIndex = 10;
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadButton.Image = global::YoctoApp.Properties.Resources.graph;
            this.loadButton.Location = new System.Drawing.Point(550, 12);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(25, 25);
            this.loadButton.TabIndex = 11;
            this.toolTip1.SetToolTip(this.loadButton, "Load data from logger");
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteButton.Image")));
            this.deleteButton.Location = new System.Drawing.Point(643, 12);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(25, 25);
            this.deleteButton.TabIndex = 5;
            this.toolTip1.SetToolTip(this.deleteButton, "Clear Datalogger");
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DataLoggerButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pauseButton.Image = ((System.Drawing.Image)(resources.GetObject("pauseButton.Image")));
            this.pauseButton.Location = new System.Drawing.Point(612, 12);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(25, 25);
            this.pauseButton.TabIndex = 4;
            this.toolTip1.SetToolTip(this.pauseButton, "Stop datalogger");
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.DataLoggerButton_Click);
            // 
            // recordButton
            // 
            this.recordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recordButton.Image = ((System.Drawing.Image)(resources.GetObject("recordButton.Image")));
            this.recordButton.Location = new System.Drawing.Point(581, 12);
            this.recordButton.Name = "recordButton";
            this.recordButton.Size = new System.Drawing.Size(25, 25);
            this.recordButton.TabIndex = 3;
            this.toolTip1.SetToolTip(this.recordButton, "Start datalogger");
            this.recordButton.UseVisualStyleBackColor = true;
            this.recordButton.Click += new System.EventHandler(this.DataLoggerButton_Click);
            // 
            // FormYSens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 345);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.sensorVal);
            this.Controls.Add(this.strLoadingMsg);
            this.Controls.Add(this.strConnectPlzMsg);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.recordButton);
            this.Controls.Add(this.chartSens);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboYDevSel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormYSens";
            this.Text = "Yoctopuce Sensor App";
            this.Load += new System.EventHandler(this.FormYSens_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartSens)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboYDevSel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer InventoryTimer;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSens;
        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Button recordButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.Label strConnectPlzMsg;
        private System.Windows.Forms.Label strLoadingMsg;
        private System.Windows.Forms.TextBox sensorVal;
        private System.Windows.Forms.Label labelUnit;
        public System.Windows.Forms.Button loadButton;
    }
}

