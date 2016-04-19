using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_PolicySnapshots : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Ctors

        public Form_PolicySnapshots(Sql.Policy policy, bool useBaseline, DateTime selectDate)
        {
            InitializeComponent();

            m_Policy = policy;
            m_UseBaseline = useBaseline;
            m_SelectDate = selectDate;

            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // load value lists for grid display
            m_statusValueList.Key = "statusValueList";
            m_statusValueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
            _grid.DisplayLayout.ValueLists.Add(m_statusValueList);

            ValueListItem listItem;

            m_statusValueList.ValueListItems.Clear();
            listItem = new ValueListItem("S", "Successful");
            m_statusValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem("W", "Warnings");
            m_statusValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem("E", "Errors");
            m_statusValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem("I", "In Progress");
            m_statusValueList.ValueListItems.Add(listItem);

            m_baselineValueList.Key = "baselineValueList";
            m_baselineValueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
            _grid.DisplayLayout.ValueLists.Add(m_baselineValueList);

            m_baselineValueList.ValueListItems.Clear();
            listItem = new ValueListItem("Y", "Yes");
            m_baselineValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem("N", "No");
            m_baselineValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(DBNull.Value, "No");
            m_baselineValueList.ValueListItems.Add(listItem);

            _grid.DrawFilter = new Utility.HideFocusRectangleDrawFilter();
            _grid.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            this.Description = string.Format(DescriptionDisplay,
                                             m_UseBaseline ? @"baseline " : string.Empty,
                                             m_SelectDate.ToLocalTime().ToString(Constants.DATETIME_FORMAT));
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_PolicySnapshots");

        private Sql.Policy m_Policy;
        private bool m_UseBaseline;
        private DateTime m_SelectDate;

        private ValueList m_statusValueList = new ValueList();
        private ValueList m_baselineValueList = new ValueList();

        #endregion

        #region Queries, Columns & Constants

        // Columns for handling the Snapshot query results
        //private enum Col
        //{
        //    Name = 0,
        //    Type,
        //    Login,
        //    Sid,
        //    Domain
        //}
        private const string colServer = @"connectionname";
        private const string colStartTime = @"starttime";
        private const string colStatus = @"status";
        private const string colBaseline = @"Baseline";

        private const string PrintTitle = @"Snapshots";
        private const string PrintHeaderDisplay = "{0}snapshots as of {1}";
        private const string DescriptionDisplay = "View the {0}snapshots that will be used for this assessment as of {1}";

        #endregion

        #region Properties

        #endregion

        #region Helpers

        private bool LoadSnapshots()
        {
            logX.loggerX.Info("Loading snapshot list for assessment");
            // Get the snapshot list.
            DataTable snapshots = null;
            try
            {
                snapshots = Sql.Policy.GetPolicySnapshots(m_Policy.PolicyId, m_Policy.AssessmentId, m_SelectDate, m_UseBaseline);
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetSnapshots, ex);
                return false;
            }

            snapshots.Columns["connectionname"].SetOrdinal(0);

            _grid.SetDataBinding(snapshots, null);

            return true;
        }

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Forms.Form_GridColumnChooser.Process(grid, PrintTitle);
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            // Associate the print document with the grid & preview dialog here
            // for consistency with other forms that require it
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = PrintTitle;
            _ultraGridPrintDocument.FitWidthToPages = 1;
            _ultraGridPrintDocument.Header.TextLeft = 
                    string.Format(PrintHeaderDisplay,
                                         m_UseBaseline ? @"baseline " : string.Empty,
                                         m_SelectDate.ToLocalTime().ToString(Constants.DATETIME_FORMAT)
                                    );
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ExportToExcelCaption, Utility.ErrorMsgs.FailedToExportToExcelFile, ex);
                }
            }
        }

        #endregion

        #region Methods

        public static void ShowSnapshots(Sql.Policy policy, bool useBaseline, DateTime selectDate)
        {
            bool updated = false;
            // Create the form.
            Form_PolicySnapshots form = new Form_PolicySnapshots(policy, useBaseline, selectDate);
            if (form.LoadSnapshots())
            {
                form.ShowDialog();
            }
        }

        #endregion

        #region Events

        private void _toolStripButton_ColumnChooser_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridGroupBy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            printGrid(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            saveGrid(_grid);

            Cursor = Cursors.Default;
        }

        private void _grid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            // the datasource is all columns from a view, so hide them all and then show the ones we want
            foreach(UltraGridColumn col in band.Columns)
            {
                col.Hidden = true;
                col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            band.Columns[colServer].Hidden = false;
            band.Columns[colServer].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colServer].Header.Caption = @"Server";
            band.Columns[colServer].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colServer].Width = 230;

            band.Columns[colStartTime].Hidden = false;
            band.Columns[colStartTime].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colStartTime].Header.Caption = @"Collected";
            band.Columns[colStartTime].Format = Utility.Constants.DATETIME_FORMAT;
            band.Columns[colStartTime].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colStartTime].Width = 120;

            band.Columns[colStatus].Hidden = false;
            band.Columns[colStatus].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colStatus].Header.Caption = @"Status";
            band.Columns[colStatus].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colStatus].Width = 80;
            band.Columns[colStatus].ValueList = m_statusValueList;

            band.Columns[colBaseline].Hidden = false;
            band.Columns[colBaseline].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colBaseline].Header.Caption = @"Baseline";
            band.Columns[colBaseline].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colBaseline].Width = 80;
            band.Columns[colBaseline].ValueList = m_baselineValueList;
        }

        #endregion

        private void Form_PolicySnapshots_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Program.gController.ShowTopic(Utility.Help.RefreshAuditDataHelpTopic);
        }
    }
}

