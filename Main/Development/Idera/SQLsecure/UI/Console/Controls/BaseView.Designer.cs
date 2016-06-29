namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class BaseView
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
            this._vw_MainPanel = new System.Windows.Forms.Panel();
            this._vw_TasksPanel = new System.Windows.Forms.Panel();
            this._label_Summary = new System.Windows.Forms.Label();
            this._pnl_Sep = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._smallTask_Help = new Idera.SQLsecure.UI.Console.Controls.SmallTask();
            this._vw_TasksPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _vw_MainPanel
            // 
            this._vw_MainPanel.BackColor = System.Drawing.Color.Transparent;
            this._vw_MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._vw_MainPanel.Location = new System.Drawing.Point(0, 68);
            this._vw_MainPanel.Name = "_vw_MainPanel";
            this._vw_MainPanel.Size = new System.Drawing.Size(652, 520);
            this._vw_MainPanel.TabIndex = 3;
            // 
            // _vw_TasksPanel
            // 
            this._vw_TasksPanel.BackColor = System.Drawing.Color.Transparent;
            this._vw_TasksPanel.Controls.Add(this._pnl_Sep);
            this._vw_TasksPanel.Controls.Add(this._label_Summary);
            this._vw_TasksPanel.Controls.Add(this._smallTask_Help);
            this._vw_TasksPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._vw_TasksPanel.Location = new System.Drawing.Point(0, 0);
            this._vw_TasksPanel.Name = "_vw_TasksPanel";
            this._vw_TasksPanel.Size = new System.Drawing.Size(652, 68);
            this._vw_TasksPanel.TabIndex = 0;
            // 
            // _label_Summary
            // 
            this._label_Summary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_Summary.BackColor = System.Drawing.Color.Transparent;
            this._label_Summary.Location = new System.Drawing.Point(8, 8);
            this._label_Summary.Name = "_label_Summary";
            this._label_Summary.Size = new System.Drawing.Size(636, 29);
            this._label_Summary.TabIndex = 1;
            this._label_Summary.Text = "View Summary";
            // 
            // _pnl_Sep
            // 
            this._pnl_Sep.BackColor = System.Drawing.Color.WhiteSmoke;
            this._pnl_Sep.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnl_Sep.GradientBorderMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.None;
            this._pnl_Sep.GradientColor = System.Drawing.Color.LightGray;
            this._pnl_Sep.GradientCornerMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.Square;
            this._pnl_Sep.Location = new System.Drawing.Point(0, 63);
            this._pnl_Sep.Name = "_pnl_Sep";
            this._pnl_Sep.Rotation = 90F;
            this._pnl_Sep.Size = new System.Drawing.Size(652, 5);
            this._pnl_Sep.TabIndex = 3;
            // 
            // _smallTask_Help
            // 
            this._smallTask_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._smallTask_Help.BackColor = System.Drawing.Color.Transparent;
            this._smallTask_Help.Location = new System.Drawing.Point(547, 40);
            this._smallTask_Help.Name = "_smallTask_Help";
            this._smallTask_Help.Size = new System.Drawing.Size(97, 34);
            this._smallTask_Help.TabIndex = 2;
            this._smallTask_Help.TaskDescription = "";
            this._smallTask_Help.TaskImage = null;
            this._smallTask_Help.TaskText = "Tell me more";
            // 
            // BaseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(652, 0);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._vw_MainPanel);
            this.Controls.Add(this._vw_TasksPanel);
            this.DoubleBuffered = true;
            this.Name = "BaseView";
            this.Size = new System.Drawing.Size(652, 588);
            this.Enter += new System.EventHandler(this.BaseView_Enter);
            this._vw_TasksPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel _vw_MainPanel;
        protected System.Windows.Forms.Panel _vw_TasksPanel;
        protected SmallTask _smallTask_Help;
        protected System.Windows.Forms.Label _label_Summary;
        private GradientPanel _pnl_Sep;


    }
}
