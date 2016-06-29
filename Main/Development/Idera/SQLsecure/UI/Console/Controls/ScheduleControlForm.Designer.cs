namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class ScheduleControlForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox_Occurs = new System.Windows.Forms.GroupBox();
            this.radioButton_Monthly = new System.Windows.Forms.RadioButton();
            this.radioButton_Weekly = new System.Windows.Forms.RadioButton();
            this.radioButton_Daily = new System.Windows.Forms.RadioButton();
            this.groupBox_Weekly = new System.Windows.Forms.GroupBox();
            this.checkBox_WeeklySunday = new System.Windows.Forms.CheckBox();
            this.checkBox_WeeklySaturday = new System.Windows.Forms.CheckBox();
            this.checkBox_WeeklyFriday = new System.Windows.Forms.CheckBox();
            this.checkBox_WeeklyThursday = new System.Windows.Forms.CheckBox();
            this.checkBox_WeeklyWednesday = new System.Windows.Forms.CheckBox();
            this.checkBox_WeeklyTuesday = new System.Windows.Forms.CheckBox();
            this.checkBox_WeeklyMonday = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown_WeeklyWeeks = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox_Monthly = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown_MonthlyTheMonths = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_MonthlyDay = new System.Windows.Forms.ComboBox();
            this.comboBox_MonthlyDayCount = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown_MonthlyMonth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_MonthlyDay = new System.Windows.Forms.NumericUpDown();
            this.radioButton_MonthlyThe = new System.Windows.Forms.RadioButton();
            this.radioButton_MonthlyDay = new System.Windows.Forms.RadioButton();
            this.groupBox_Daily = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown_DailyDays = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox_Freq = new System.Windows.Forms.GroupBox();
            this.dateTimePicker_FreqEveryEnd = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker_FreqEveryStart = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_FreqEveryUnit = new System.Windows.Forms.ComboBox();
            this.numericUpDown_FreqEveryCount = new System.Windows.Forms.NumericUpDown();
            this.radioButton_FreqEvery = new System.Windows.Forms.RadioButton();
            this.dateTimePicker_FreqOnceTime = new System.Windows.Forms.DateTimePicker();
            this.radioButton_FreqOnce = new System.Windows.Forms.RadioButton();
            this.groupBox_Occurs.SuspendLayout();
            this.groupBox_Weekly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_WeeklyWeeks)).BeginInit();
            this.groupBox_Monthly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MonthlyTheMonths)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MonthlyMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MonthlyDay)).BeginInit();
            this.groupBox_Daily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DailyDays)).BeginInit();
            this.groupBox_Freq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FreqEveryCount)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_Occurs
            // 
            this.groupBox_Occurs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Occurs.Controls.Add(this.radioButton_Monthly);
            this.groupBox_Occurs.Controls.Add(this.radioButton_Weekly);
            this.groupBox_Occurs.Controls.Add(this.radioButton_Daily);
            this.groupBox_Occurs.Location = new System.Drawing.Point(14, 6);
            this.groupBox_Occurs.Name = "groupBox_Occurs";
            this.groupBox_Occurs.Size = new System.Drawing.Size(460, 55);
            this.groupBox_Occurs.TabIndex = 0;
            this.groupBox_Occurs.TabStop = false;
            this.groupBox_Occurs.Text = "Occurs";
            // 
            // radioButton_Monthly
            // 
            this.radioButton_Monthly.AutoSize = true;
            this.radioButton_Monthly.Location = new System.Drawing.Point(375, 22);
            this.radioButton_Monthly.Name = "radioButton_Monthly";
            this.radioButton_Monthly.Size = new System.Drawing.Size(62, 17);
            this.radioButton_Monthly.TabIndex = 2;
            this.radioButton_Monthly.TabStop = true;
            this.radioButton_Monthly.Text = "M&onthly";
            this.radioButton_Monthly.UseVisualStyleBackColor = true;
            this.radioButton_Monthly.CheckedChanged += new System.EventHandler(this.radioButton_Monthly_CheckedChanged);
            // 
            // radioButton_Weekly
            // 
            this.radioButton_Weekly.AutoSize = true;
            this.radioButton_Weekly.Location = new System.Drawing.Point(186, 22);
            this.radioButton_Weekly.Name = "radioButton_Weekly";
            this.radioButton_Weekly.Size = new System.Drawing.Size(61, 17);
            this.radioButton_Weekly.TabIndex = 1;
            this.radioButton_Weekly.Text = "&Weekly";
            this.radioButton_Weekly.UseVisualStyleBackColor = true;
            this.radioButton_Weekly.CheckedChanged += new System.EventHandler(this.radioButton_Weekly_CheckedChanged);
            // 
            // radioButton_Daily
            // 
            this.radioButton_Daily.AutoSize = true;
            this.radioButton_Daily.Location = new System.Drawing.Point(10, 22);
            this.radioButton_Daily.Name = "radioButton_Daily";
            this.radioButton_Daily.Size = new System.Drawing.Size(48, 17);
            this.radioButton_Daily.TabIndex = 0;
            this.radioButton_Daily.TabStop = true;
            this.radioButton_Daily.Text = "&Daily";
            this.radioButton_Daily.UseVisualStyleBackColor = true;
            this.radioButton_Daily.CheckedChanged += new System.EventHandler(this.radioButton_Daily_CheckedChanged);
            // 
            // groupBox_Weekly
            // 
            this.groupBox_Weekly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Weekly.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_Weekly.Controls.Add(this.checkBox_WeeklySunday);
            this.groupBox_Weekly.Controls.Add(this.checkBox_WeeklySaturday);
            this.groupBox_Weekly.Controls.Add(this.checkBox_WeeklyFriday);
            this.groupBox_Weekly.Controls.Add(this.checkBox_WeeklyThursday);
            this.groupBox_Weekly.Controls.Add(this.checkBox_WeeklyWednesday);
            this.groupBox_Weekly.Controls.Add(this.checkBox_WeeklyTuesday);
            this.groupBox_Weekly.Controls.Add(this.checkBox_WeeklyMonday);
            this.groupBox_Weekly.Controls.Add(this.label8);
            this.groupBox_Weekly.Controls.Add(this.numericUpDown_WeeklyWeeks);
            this.groupBox_Weekly.Controls.Add(this.label7);
            this.groupBox_Weekly.Location = new System.Drawing.Point(14, 68);
            this.groupBox_Weekly.Name = "groupBox_Weekly";
            this.groupBox_Weekly.Size = new System.Drawing.Size(460, 102);
            this.groupBox_Weekly.TabIndex = 1;
            this.groupBox_Weekly.TabStop = false;
            this.groupBox_Weekly.Text = "Weekly";
            // 
            // checkBox_WeeklySunday
            // 
            this.checkBox_WeeklySunday.AutoSize = true;
            this.checkBox_WeeklySunday.Location = new System.Drawing.Point(126, 71);
            this.checkBox_WeeklySunday.Name = "checkBox_WeeklySunday";
            this.checkBox_WeeklySunday.Size = new System.Drawing.Size(62, 17);
            this.checkBox_WeeklySunday.TabIndex = 9;
            this.checkBox_WeeklySunday.Text = "S&unday";
            this.checkBox_WeeklySunday.UseVisualStyleBackColor = true;
            // 
            // checkBox_WeeklySaturday
            // 
            this.checkBox_WeeklySaturday.AutoSize = true;
            this.checkBox_WeeklySaturday.Location = new System.Drawing.Point(50, 71);
            this.checkBox_WeeklySaturday.Name = "checkBox_WeeklySaturday";
            this.checkBox_WeeklySaturday.Size = new System.Drawing.Size(68, 17);
            this.checkBox_WeeklySaturday.TabIndex = 8;
            this.checkBox_WeeklySaturday.Text = "&Saturday";
            this.checkBox_WeeklySaturday.UseVisualStyleBackColor = true;
            // 
            // checkBox_WeeklyFriday
            // 
            this.checkBox_WeeklyFriday.AutoSize = true;
            this.checkBox_WeeklyFriday.Location = new System.Drawing.Point(382, 49);
            this.checkBox_WeeklyFriday.Name = "checkBox_WeeklyFriday";
            this.checkBox_WeeklyFriday.Size = new System.Drawing.Size(54, 17);
            this.checkBox_WeeklyFriday.TabIndex = 7;
            this.checkBox_WeeklyFriday.Text = "&Friday";
            this.checkBox_WeeklyFriday.UseVisualStyleBackColor = true;
            // 
            // checkBox_WeeklyThursday
            // 
            this.checkBox_WeeklyThursday.AutoSize = true;
            this.checkBox_WeeklyThursday.Location = new System.Drawing.Point(300, 49);
            this.checkBox_WeeklyThursday.Name = "checkBox_WeeklyThursday";
            this.checkBox_WeeklyThursday.Size = new System.Drawing.Size(70, 17);
            this.checkBox_WeeklyThursday.TabIndex = 6;
            this.checkBox_WeeklyThursday.Text = "T&hursday";
            this.checkBox_WeeklyThursday.UseVisualStyleBackColor = true;
            // 
            // checkBox_WeeklyWednesday
            // 
            this.checkBox_WeeklyWednesday.AutoSize = true;
            this.checkBox_WeeklyWednesday.Location = new System.Drawing.Point(205, 49);
            this.checkBox_WeeklyWednesday.Name = "checkBox_WeeklyWednesday";
            this.checkBox_WeeklyWednesday.Size = new System.Drawing.Size(83, 17);
            this.checkBox_WeeklyWednesday.TabIndex = 5;
            this.checkBox_WeeklyWednesday.Text = "&Wednesday";
            this.checkBox_WeeklyWednesday.UseVisualStyleBackColor = true;
            // 
            // checkBox_WeeklyTuesday
            // 
            this.checkBox_WeeklyTuesday.AutoSize = true;
            this.checkBox_WeeklyTuesday.Location = new System.Drawing.Point(126, 49);
            this.checkBox_WeeklyTuesday.Name = "checkBox_WeeklyTuesday";
            this.checkBox_WeeklyTuesday.Size = new System.Drawing.Size(67, 17);
            this.checkBox_WeeklyTuesday.TabIndex = 4;
            this.checkBox_WeeklyTuesday.Text = "&Tuesday";
            this.checkBox_WeeklyTuesday.UseVisualStyleBackColor = true;
            // 
            // checkBox_WeeklyMonday
            // 
            this.checkBox_WeeklyMonday.AutoSize = true;
            this.checkBox_WeeklyMonday.Location = new System.Drawing.Point(50, 49);
            this.checkBox_WeeklyMonday.Name = "checkBox_WeeklyMonday";
            this.checkBox_WeeklyMonday.Size = new System.Drawing.Size(64, 17);
            this.checkBox_WeeklyMonday.TabIndex = 3;
            this.checkBox_WeeklyMonday.Text = "&Monday";
            this.checkBox_WeeklyMonday.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(113, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "week(s) on:";
            // 
            // numericUpDown_WeeklyWeeks
            // 
            this.numericUpDown_WeeklyWeeks.Location = new System.Drawing.Point(50, 20);
            this.numericUpDown_WeeklyWeeks.Maximum = new decimal(new int[] {
            52,
            0,
            0,
            0});
            this.numericUpDown_WeeklyWeeks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_WeeklyWeeks.Name = "numericUpDown_WeeklyWeeks";
            this.numericUpDown_WeeklyWeeks.Size = new System.Drawing.Size(57, 20);
            this.numericUpDown_WeeklyWeeks.TabIndex = 1;
            this.numericUpDown_WeeklyWeeks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "&Every";
            // 
            // groupBox_Monthly
            // 
            this.groupBox_Monthly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Monthly.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_Monthly.Controls.Add(this.label4);
            this.groupBox_Monthly.Controls.Add(this.numericUpDown_MonthlyTheMonths);
            this.groupBox_Monthly.Controls.Add(this.label3);
            this.groupBox_Monthly.Controls.Add(this.comboBox_MonthlyDay);
            this.groupBox_Monthly.Controls.Add(this.comboBox_MonthlyDayCount);
            this.groupBox_Monthly.Controls.Add(this.label2);
            this.groupBox_Monthly.Controls.Add(this.numericUpDown_MonthlyMonth);
            this.groupBox_Monthly.Controls.Add(this.label1);
            this.groupBox_Monthly.Controls.Add(this.numericUpDown_MonthlyDay);
            this.groupBox_Monthly.Controls.Add(this.radioButton_MonthlyThe);
            this.groupBox_Monthly.Controls.Add(this.radioButton_MonthlyDay);
            this.groupBox_Monthly.Location = new System.Drawing.Point(14, 68);
            this.groupBox_Monthly.Name = "groupBox_Monthly";
            this.groupBox_Monthly.Size = new System.Drawing.Size(460, 102);
            this.groupBox_Monthly.TabIndex = 3;
            this.groupBox_Monthly.TabStop = false;
            this.groupBox_Monthly.Text = "Monthly";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "month(s)";
            // 
            // numericUpDown_MonthlyTheMonths
            // 
            this.numericUpDown_MonthlyTheMonths.Location = new System.Drawing.Point(274, 54);
            this.numericUpDown_MonthlyTheMonths.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown_MonthlyTheMonths.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_MonthlyTheMonths.Name = "numericUpDown_MonthlyTheMonths";
            this.numericUpDown_MonthlyTheMonths.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown_MonthlyTheMonths.TabIndex = 9;
            this.numericUpDown_MonthlyTheMonths.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "of every";
            // 
            // comboBox_MonthlyDay
            // 
            this.comboBox_MonthlyDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_MonthlyDay.FormattingEnabled = true;
            this.comboBox_MonthlyDay.Location = new System.Drawing.Point(124, 54);
            this.comboBox_MonthlyDay.Name = "comboBox_MonthlyDay";
            this.comboBox_MonthlyDay.Size = new System.Drawing.Size(92, 21);
            this.comboBox_MonthlyDay.TabIndex = 7;
            // 
            // comboBox_MonthlyDayCount
            // 
            this.comboBox_MonthlyDayCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_MonthlyDayCount.FormattingEnabled = true;
            this.comboBox_MonthlyDayCount.Location = new System.Drawing.Point(57, 54);
            this.comboBox_MonthlyDayCount.Name = "comboBox_MonthlyDayCount";
            this.comboBox_MonthlyDayCount.Size = new System.Drawing.Size(58, 21);
            this.comboBox_MonthlyDayCount.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "month(s)";
            // 
            // numericUpDown_MonthlyMonth
            // 
            this.numericUpDown_MonthlyMonth.Location = new System.Drawing.Point(172, 22);
            this.numericUpDown_MonthlyMonth.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown_MonthlyMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_MonthlyMonth.Name = "numericUpDown_MonthlyMonth";
            this.numericUpDown_MonthlyMonth.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown_MonthlyMonth.TabIndex = 4;
            this.numericUpDown_MonthlyMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "of every";
            // 
            // numericUpDown_MonthlyDay
            // 
            this.numericUpDown_MonthlyDay.Location = new System.Drawing.Point(57, 22);
            this.numericUpDown_MonthlyDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDown_MonthlyDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_MonthlyDay.Name = "numericUpDown_MonthlyDay";
            this.numericUpDown_MonthlyDay.Size = new System.Drawing.Size(58, 20);
            this.numericUpDown_MonthlyDay.TabIndex = 2;
            this.numericUpDown_MonthlyDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButton_MonthlyThe
            // 
            this.radioButton_MonthlyThe.AutoSize = true;
            this.radioButton_MonthlyThe.Location = new System.Drawing.Point(10, 56);
            this.radioButton_MonthlyThe.Name = "radioButton_MonthlyThe";
            this.radioButton_MonthlyThe.Size = new System.Drawing.Size(44, 17);
            this.radioButton_MonthlyThe.TabIndex = 1;
            this.radioButton_MonthlyThe.TabStop = true;
            this.radioButton_MonthlyThe.Text = "&The";
            this.radioButton_MonthlyThe.UseVisualStyleBackColor = true;
            this.radioButton_MonthlyThe.CheckedChanged += new System.EventHandler(this.radioButton_MonthlyThe_CheckedChanged);
            // 
            // radioButton_MonthlyDay
            // 
            this.radioButton_MonthlyDay.AutoSize = true;
            this.radioButton_MonthlyDay.Location = new System.Drawing.Point(10, 22);
            this.radioButton_MonthlyDay.Name = "radioButton_MonthlyDay";
            this.radioButton_MonthlyDay.Size = new System.Drawing.Size(44, 17);
            this.radioButton_MonthlyDay.TabIndex = 0;
            this.radioButton_MonthlyDay.TabStop = true;
            this.radioButton_MonthlyDay.Text = "Da&y";
            this.radioButton_MonthlyDay.UseVisualStyleBackColor = true;
            this.radioButton_MonthlyDay.CheckedChanged += new System.EventHandler(this.radioButton_MonthlyDay_CheckedChanged);
            // 
            // groupBox_Daily
            // 
            this.groupBox_Daily.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Daily.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_Daily.Controls.Add(this.label10);
            this.groupBox_Daily.Controls.Add(this.numericUpDown_DailyDays);
            this.groupBox_Daily.Controls.Add(this.label9);
            this.groupBox_Daily.Location = new System.Drawing.Point(14, 68);
            this.groupBox_Daily.Name = "groupBox_Daily";
            this.groupBox_Daily.Size = new System.Drawing.Size(460, 102);
            this.groupBox_Daily.TabIndex = 2;
            this.groupBox_Daily.TabStop = false;
            this.groupBox_Daily.Text = "Daily";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(121, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "day(s)";
            // 
            // numericUpDown_DailyDays
            // 
            this.numericUpDown_DailyDays.Location = new System.Drawing.Point(50, 20);
            this.numericUpDown_DailyDays.Maximum = new decimal(new int[] {
            366,
            0,
            0,
            0});
            this.numericUpDown_DailyDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_DailyDays.Name = "numericUpDown_DailyDays";
            this.numericUpDown_DailyDays.Size = new System.Drawing.Size(60, 20);
            this.numericUpDown_DailyDays.TabIndex = 1;
            this.numericUpDown_DailyDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "E&very";
            // 
            // groupBox_Freq
            // 
            this.groupBox_Freq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Freq.Controls.Add(this.dateTimePicker_FreqEveryEnd);
            this.groupBox_Freq.Controls.Add(this.label6);
            this.groupBox_Freq.Controls.Add(this.dateTimePicker_FreqEveryStart);
            this.groupBox_Freq.Controls.Add(this.label5);
            this.groupBox_Freq.Controls.Add(this.comboBox_FreqEveryUnit);
            this.groupBox_Freq.Controls.Add(this.numericUpDown_FreqEveryCount);
            this.groupBox_Freq.Controls.Add(this.radioButton_FreqEvery);
            this.groupBox_Freq.Controls.Add(this.dateTimePicker_FreqOnceTime);
            this.groupBox_Freq.Controls.Add(this.radioButton_FreqOnce);
            this.groupBox_Freq.Location = new System.Drawing.Point(14, 176);
            this.groupBox_Freq.Name = "groupBox_Freq";
            this.groupBox_Freq.Size = new System.Drawing.Size(460, 117);
            this.groupBox_Freq.TabIndex = 2;
            this.groupBox_Freq.TabStop = false;
            this.groupBox_Freq.Text = "Daily frequency";
            // 
            // dateTimePicker_FreqEveryEnd
            // 
            this.dateTimePicker_FreqEveryEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_FreqEveryEnd.Location = new System.Drawing.Point(341, 80);
            this.dateTimePicker_FreqEveryEnd.Name = "dateTimePicker_FreqEveryEnd";
            this.dateTimePicker_FreqEveryEnd.ShowUpDown = true;
            this.dateTimePicker_FreqEveryEnd.Size = new System.Drawing.Size(96, 20);
            this.dateTimePicker_FreqEveryEnd.TabIndex = 8;
            this.dateTimePicker_FreqEveryEnd.Value = new System.DateTime(2006, 8, 8, 23, 59, 0, 0);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(279, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "E&nding at:";
            // 
            // dateTimePicker_FreqEveryStart
            // 
            this.dateTimePicker_FreqEveryStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_FreqEveryStart.Location = new System.Drawing.Point(341, 54);
            this.dateTimePicker_FreqEveryStart.Name = "dateTimePicker_FreqEveryStart";
            this.dateTimePicker_FreqEveryStart.ShowUpDown = true;
            this.dateTimePicker_FreqEveryStart.Size = new System.Drawing.Size(96, 20);
            this.dateTimePicker_FreqEveryStart.TabIndex = 6;
            this.dateTimePicker_FreqEveryStart.Value = new System.DateTime(2006, 8, 8, 0, 0, 0, 0);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(279, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Sta&rting at:";
            // 
            // comboBox_FreqEveryUnit
            // 
            this.comboBox_FreqEveryUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_FreqEveryUnit.FormattingEnabled = true;
            this.comboBox_FreqEveryUnit.Location = new System.Drawing.Point(184, 54);
            this.comboBox_FreqEveryUnit.Name = "comboBox_FreqEveryUnit";
            this.comboBox_FreqEveryUnit.Size = new System.Drawing.Size(81, 21);
            this.comboBox_FreqEveryUnit.TabIndex = 4;
            this.comboBox_FreqEveryUnit.SelectedIndexChanged += new System.EventHandler(this.comboBox_FreqEveryUnit_SelectedIndexChanged);
            // 
            // numericUpDown_FreqEveryCount
            // 
            this.numericUpDown_FreqEveryCount.Location = new System.Drawing.Point(124, 54);
            this.numericUpDown_FreqEveryCount.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numericUpDown_FreqEveryCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_FreqEveryCount.Name = "numericUpDown_FreqEveryCount";
            this.numericUpDown_FreqEveryCount.Size = new System.Drawing.Size(54, 20);
            this.numericUpDown_FreqEveryCount.TabIndex = 3;
            this.numericUpDown_FreqEveryCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButton_FreqEvery
            // 
            this.radioButton_FreqEvery.AutoSize = true;
            this.radioButton_FreqEvery.Location = new System.Drawing.Point(10, 54);
            this.radioButton_FreqEvery.Name = "radioButton_FreqEvery";
            this.radioButton_FreqEvery.Size = new System.Drawing.Size(91, 17);
            this.radioButton_FreqEvery.TabIndex = 2;
            this.radioButton_FreqEvery.TabStop = true;
            this.radioButton_FreqEvery.Text = "Occurs &every:";
            this.radioButton_FreqEvery.UseVisualStyleBackColor = true;
            this.radioButton_FreqEvery.CheckedChanged += new System.EventHandler(this.radioButton_FreqEvery_CheckedChanged);
            // 
            // dateTimePicker_FreqOnceTime
            // 
            this.dateTimePicker_FreqOnceTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_FreqOnceTime.Location = new System.Drawing.Point(124, 22);
            this.dateTimePicker_FreqOnceTime.Name = "dateTimePicker_FreqOnceTime";
            this.dateTimePicker_FreqOnceTime.ShowUpDown = true;
            this.dateTimePicker_FreqOnceTime.Size = new System.Drawing.Size(92, 20);
            this.dateTimePicker_FreqOnceTime.TabIndex = 1;
            this.dateTimePicker_FreqOnceTime.Value = new System.DateTime(2006, 8, 8, 3, 0, 0, 0);
            // 
            // radioButton_FreqOnce
            // 
            this.radioButton_FreqOnce.AutoSize = true;
            this.radioButton_FreqOnce.Location = new System.Drawing.Point(10, 22);
            this.radioButton_FreqOnce.Name = "radioButton_FreqOnce";
            this.radioButton_FreqOnce.Size = new System.Drawing.Size(101, 17);
            this.radioButton_FreqOnce.TabIndex = 0;
            this.radioButton_FreqOnce.Text = "Occurs once &at:";
            this.radioButton_FreqOnce.UseVisualStyleBackColor = true;
            this.radioButton_FreqOnce.CheckedChanged += new System.EventHandler(this.radioButton_FreqOnce_CheckedChanged);
            // 
            // ScheduleControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox_Monthly);
            this.Controls.Add(this.groupBox_Daily);
            this.Controls.Add(this.groupBox_Weekly);
            this.Controls.Add(this.groupBox_Occurs);
            this.Controls.Add(this.groupBox_Freq);
            this.Name = "ScheduleControlForm";
            this.Size = new System.Drawing.Size(489, 296);
            this.Load += new System.EventHandler(this.ScheduleControlForm_Load);
            this.groupBox_Occurs.ResumeLayout(false);
            this.groupBox_Occurs.PerformLayout();
            this.groupBox_Weekly.ResumeLayout(false);
            this.groupBox_Weekly.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_WeeklyWeeks)).EndInit();
            this.groupBox_Monthly.ResumeLayout(false);
            this.groupBox_Monthly.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MonthlyTheMonths)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MonthlyMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MonthlyDay)).EndInit();
            this.groupBox_Daily.ResumeLayout(false);
            this.groupBox_Daily.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DailyDays)).EndInit();
            this.groupBox_Freq.ResumeLayout(false);
            this.groupBox_Freq.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FreqEveryCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Occurs;
        private System.Windows.Forms.GroupBox groupBox_Weekly;
        private System.Windows.Forms.RadioButton radioButton_Monthly;
        private System.Windows.Forms.RadioButton radioButton_Weekly;
        private System.Windows.Forms.RadioButton radioButton_Daily;
        private System.Windows.Forms.GroupBox groupBox_Freq;
        private System.Windows.Forms.GroupBox groupBox_Monthly;
        private System.Windows.Forms.GroupBox groupBox_Daily;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_MonthlyDay;
        private System.Windows.Forms.RadioButton radioButton_MonthlyThe;
        private System.Windows.Forms.RadioButton radioButton_MonthlyDay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_MonthlyTheMonths;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_MonthlyDay;
        private System.Windows.Forms.ComboBox comboBox_MonthlyDayCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown_MonthlyMonth;
        private System.Windows.Forms.RadioButton radioButton_FreqOnce;
        private System.Windows.Forms.DateTimePicker dateTimePicker_FreqEveryStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_FreqEveryUnit;
        private System.Windows.Forms.NumericUpDown numericUpDown_FreqEveryCount;
        private System.Windows.Forms.RadioButton radioButton_FreqEvery;
        private System.Windows.Forms.DateTimePicker dateTimePicker_FreqOnceTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker_FreqEveryEnd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_WeeklyWeeks;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox_WeeklyFriday;
        private System.Windows.Forms.CheckBox checkBox_WeeklyThursday;
        private System.Windows.Forms.CheckBox checkBox_WeeklyWednesday;
        private System.Windows.Forms.CheckBox checkBox_WeeklyTuesday;
        private System.Windows.Forms.CheckBox checkBox_WeeklyMonday;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox_WeeklySunday;
        private System.Windows.Forms.CheckBox checkBox_WeeklySaturday;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDown_DailyDays;
        private System.Windows.Forms.Label label9;
    }
}
