namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_GridColumnChooser
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
            this._btn_OK = new System.Windows.Forms.Button();
            this._ultraGridColumnChooser = new Infragistics.Win.UltraWinGrid.UltraGridColumnChooser();
            this._lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _btn_OK
            // 
            this._btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_OK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btn_OK.Location = new System.Drawing.Point(123, 244);
            this._btn_OK.Name = "_btn_OK";
            this._btn_OK.Size = new System.Drawing.Size(75, 23);
            this._btn_OK.TabIndex = 2;
            this._btn_OK.Text = "OK";
            this._btn_OK.UseVisualStyleBackColor = true;
            // 
            // _ultraGridColumnChooser
            // 
            this._ultraGridColumnChooser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ultraGridColumnChooser.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this._ultraGridColumnChooser.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this._ultraGridColumnChooser.DisplayLayout.MaxColScrollRegions = 1;
            this._ultraGridColumnChooser.DisplayLayout.MaxRowScrollRegions = 1;
            this._ultraGridColumnChooser.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this._ultraGridColumnChooser.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this._ultraGridColumnChooser.DisplayLayout.Override.AllowRowLayoutCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.None;
            this._ultraGridColumnChooser.DisplayLayout.Override.AllowRowLayoutLabelSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.None;
            this._ultraGridColumnChooser.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._ultraGridColumnChooser.DisplayLayout.Override.CellPadding = 2;
            this._ultraGridColumnChooser.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.Never;
            this._ultraGridColumnChooser.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this._ultraGridColumnChooser.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this._ultraGridColumnChooser.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFixed;
            this._ultraGridColumnChooser.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._ultraGridColumnChooser.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._ultraGridColumnChooser.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._ultraGridColumnChooser.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._ultraGridColumnChooser.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._ultraGridColumnChooser.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._ultraGridColumnChooser.Location = new System.Drawing.Point(12, 25);
            this._ultraGridColumnChooser.Name = "_ultraGridColumnChooser";
            this._ultraGridColumnChooser.Size = new System.Drawing.Size(186, 213);
            this._ultraGridColumnChooser.SourceGrid = null;
            this._ultraGridColumnChooser.TabIndex = 4;
            this._ultraGridColumnChooser.Text = "ultraGridColumnChooser1";
            // 
            // _lbl
            // 
            this._lbl.AutoSize = true;
            this._lbl.Location = new System.Drawing.Point(12, 9);
            this._lbl.Name = "_lbl";
            this._lbl.Size = new System.Drawing.Size(126, 13);
            this._lbl.TabIndex = 5;
            this._lbl.Text = "Select columns to display";
            // 
            // Form_GridColumnChooser
            // 
            this.AcceptButton = this._btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btn_OK;
            this.ClientSize = new System.Drawing.Size(210, 279);
            this.Controls.Add(this._lbl);
            this.Controls.Add(this._ultraGridColumnChooser);
            this.Controls.Add(this._btn_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form_GridColumnChooser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Form_GridColumnChooser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btn_OK;
        private Infragistics.Win.UltraWinGrid.UltraGridColumnChooser _ultraGridColumnChooser;
        private System.Windows.Forms.Label _lbl;
    }
}