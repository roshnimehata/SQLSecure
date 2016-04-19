using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class PolicyChangeLog : UserControl, Interfaces.IView, Interfaces.ICommandHandler, Interfaces.IRefresh
    {

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            m_context = (Data.PolicyAssessment)contextIn;
            m_policy = m_context.Policy;

            setMenuConfiguration();

            loadDataSource();
        }

        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.SecuritySummaryChangeLogHelpTopic; }
        }

        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.SecuritySummaryChangeLogConceptTopic; }
        }

        String Interfaces.IView.Title
        {
            get { return string.Empty; }
        }

        #endregion

        #region ICommandHandler Members

        void Interfaces.ICommandHandler.ProcessCommand(Utility.ViewSpecificCommand command)
        {
            switch (command)
            {
                case Utility.ViewSpecificCommand.NewAuditServer:
                    showNewAuditServer();
                    break;
                case Utility.ViewSpecificCommand.NewLogin:
                    showNewLogin();
                    break;
                case Utility.ViewSpecificCommand.Baseline:
                    showBaseline();
                    break;
                case Utility.ViewSpecificCommand.Collect:
                    showCollect();
                    break;
                case Utility.ViewSpecificCommand.Configure:
                    showConfigure();
                    break;
                case Utility.ViewSpecificCommand.Delete:
                    showDelete();
                    break;
                case Utility.ViewSpecificCommand.Properties:
                    showProperties();
                    break;
                case Utility.ViewSpecificCommand.Refresh:
                    showRefresh();
                    break;
                case Utility.ViewSpecificCommand.UserPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                    break;
                case Utility.ViewSpecificCommand.ObjectPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                    break;
                default:
                    Debug.Assert(false, "Unknown command passed to PolicyChangeLog");
                    break;
            }
        }

        protected virtual void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();
        }

        protected virtual void showNewLogin()
        {
            Forms.Form_WizardNewLogin.Process();
        }

        protected virtual void showBaseline()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - PolicyChangeLog showBaseline command called erroneously");
        }

        protected virtual void showCollect()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - PolicyChangeLog showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ReportCard showCollect command called erroneously");
        }

        protected virtual void showDelete()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - PolicyChangeLog showDelete command called erroneously");
        }

        protected virtual void showProperties()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - PolicyChangeLog showProperties command called erroneously");
        }

        protected virtual void showRefresh()
        {
            //loadDataSource();
            // refresh the entire view to keep all info in sync
            Program.gController.RefreshCurrentView();
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - PolicyChangeLog showProperties command called erroneously");
        }

        #endregion

        #region IRefresh Members

        public void RefreshView()
        {
            showRefresh();
        }

        #endregion

        #region ctors

        public PolicyChangeLog()
        {
            InitializeComponent();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid_ChangeLog.Tag = _label_ChangeLog;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_ChangeLog.Tag = _grid_ChangeLog;

            // Hookup all application images
            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // set all grids to start in the same initial display mode
            _grid_ChangeLog.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            // Initialize the grids
            initDataSources();

            // Hide the focus rectangles on tabs and grids
            _grid_ChangeLog.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.PolicyChangeLog");
        private bool m_Initialized = false;

        private Utility.MenuConfiguration m_menuConfiguration;
        private Data.PolicyAssessment m_context;
        private Control m_focused = null; // To prevent focus from switching to the splitters, etc.
        private bool m_gridCellClicked = false;

        private Sql.Policy m_policy;
        private DataTable m_ChangeLogTable;

        #endregion

        #region query, columns and constants

        private const string QueryGetChangeLog =
            @"SELECT 
                    changedate,  
                    assessmentstatename,  
                    changedby,  
                    changedescription
                from SQLsecure.dbo.vwpolicychangelog 
                where policyid = @policyid 
                    and assessmentid = @assessmentid";
        private const string ParamPolicyId = @"policyid";
        private const string ParamAssessmentId = @"assessmentid";

        // Assessment details columns
        private const string colState = @"assessmentstatename";
        private const string colChangedDate = @"changedate";
        private const string colChangedBy = @"changedby";
        private const string colChangeDescription = @"changedescription";

        private const string HeaderChangeLog = @"Log Entries";
        private const string HeaderDisplay = "{0} Log Entr{1}";
        private const string DisplayNoLog = @"No log entries available";
        private const string PrintHeaderDisplay = "{0}\nPolicy Change Log as of {1}\n\n{2}";
        private const string PrintEmptyHeaderDisplay = "{0}";

        #endregion

        #region helpers

        protected void setMenuConfiguration()
        {
            //This is all currently being handled at the view level, but leaving functions for future
            //m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
            //m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;

            //Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        // Find the control with focus

        private Control getFocused(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c.Focused)
                {
                    return c;
                }
                else if (c.ContainsFocus)
                {
                    return getFocused(c.Controls);
                }
            }

            return null;
        }

        private DataTable createDataSource()
        {
            // Create Report Card default datasources
            DataTable dt = new DataTable();
            dt.Columns.Add(colChangedDate, typeof(DateTime));
            dt.Columns.Add(colState, typeof(string));
            dt.Columns.Add(colChangedBy, typeof(string));
            dt.Columns.Add(colChangeDescription, typeof(string));

            return dt;
        }

        private void initDataSources()
        {
            // Initialize the details
            m_ChangeLogTable = createDataSource();

            _label_ChangeLog.Text = HeaderChangeLog;
            _grid_ChangeLog.SetDataBinding(m_ChangeLogTable.DefaultView, null);
        }

        private void loadDataSource()
        {
            logX.loggerX.Info("Retrieve Policy Report Card");

            try
            {
                _textBox_Description.Text = DisplayNoLog;

                // Open connection to repository and query permissions.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup parameters for all queries
                    SqlParameter paramPolicyId = new SqlParameter(ParamPolicyId, m_policy.PolicyId);
                    SqlParameter paramAssessmentId = new SqlParameter(ParamAssessmentId, m_policy.AssessmentId);

                    // Get Change Log
                    SqlCommand cmd = new SqlCommand(QueryGetChangeLog, connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                    cmd.Parameters.Add(paramPolicyId);
                    cmd.Parameters.Add(paramAssessmentId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    m_ChangeLogTable = ds.Tables[0];

                    // fix the time by converting to local for display
                    foreach (DataRow row in m_ChangeLogTable.Rows)
                    {
                        row[colChangedDate] = ((DateTime) row[colChangedDate]).ToLocalTime();
                    }

                    m_ChangeLogTable.DefaultView.Sort = colChangedDate;

                    _grid_ChangeLog.SuspendLayout();

                    // Save the user configuration of the grid to restore it after setting the datasource again
                    Utility.GridSettings gridSettings = GridSettings.GetSettings(_grid_ChangeLog);

                    _label_ChangeLog.Text =
                        string.Format(HeaderDisplay,
                                        m_ChangeLogTable.DefaultView.Count,
                                        m_ChangeLogTable.DefaultView.Count == 1 ? @"y" : @"ies");

                    _grid_ChangeLog.SetDataBinding(m_ChangeLogTable.DefaultView, null);

                    // Reapply the user's settings after rebuilding the grid
                    GridSettings.ApplySettingsToGrid(gridSettings, _grid_ChangeLog);

                    if (!m_Initialized)
                    {
                        foreach (UltraGridColumn col in _grid_ChangeLog.DisplayLayout.Bands[0].Columns)
                        {
                            //if (col.Key != colSeverityCode)
                            {
                                col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                            }
                        }
                        _grid_ChangeLog.DisplayLayout.Bands[0].SortedColumns.Add(colChangedDate, true, false);

                        m_Initialized = true;
                    }



                    _grid_ChangeLog.ResumeLayout();
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve policy change log", ex);
                MsgBox.ShowError(string.Format(ErrorMsgs.CantGetPolicyInfoMsg, "Policy Change Log"),
                                 ErrorMsgs.ErrorProcessPolicyInfo,
                                 ex);
                initDataSources();

                _grid_ChangeLog.ResumeLayout();
            }
        }

        #region Grid

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;

            if (m_policy != null)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        m_policy.PolicyName,
                                        DateTime.Now,
                                        ((ToolStripLabel)grid.Tag).Text
                                    );
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintEmptyHeaderDisplay,
                                        ((ToolStripLabel)grid.Tag).Text
                                    );
            }
            //_ultraGridPrintDocument.Footer.TextCenter =
            //    string.Format("Page {0}",
            //                    _ultraGridPrintDocument.PageNumber,
            //                    _ultraPrintPreviewDialog.pag
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            //bool iconHidden = false;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ////save the current state of the icon column and then hide it before exporting
                    //if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                    //{
                    //    // this column doesn't exist in the raw data hack
                    //    iconHidden = grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden;
                    //    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = true;
                    //    // Fix the sub list display on the effective grid
                    //    if (grid.DisplayLayout.Bands.Count > 1
                    //        && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                    //    {
                    //        grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = true;
                    //    }
                    //}

                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                //if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                //{
                //    // this column doesn't exist in the raw data hack
                //    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = iconHidden;
                //}
                //// Fix the sub list display on the effective grid
                //if (grid.DisplayLayout.Bands.Count > 1
                //    && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                //{
                //    grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = iconHidden;
                //}
            }
        }

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            // set any column chooser options before showing????
            string gridHeading = ((ToolStripLabel)grid.Tag).Text;
            if (gridHeading.IndexOf("(") > 0)
            {
                gridHeading = gridHeading.Remove(gridHeading.IndexOf("(") - 1);
            }

            Forms.Form_GridColumnChooser.Process(grid, gridHeading);
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        #endregion

        #endregion

        #region Events

        #region tool strips

        private void _toolStripButton_GridGroupBy_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_ColumnChooser_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        #region Grids

        private void _grid_ChangeLog_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByRowExpansionStyle = GroupByRowExpansionStyle.Disabled;
            e.Layout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = DefaultableBoolean.False;

            UltraGridBand band = e.Layout.Bands[0];

            band.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            band.Columns[colChangedDate].Header.Caption = "Changed At";
            band.Columns[colChangedDate].Format = Utility.Constants.DATETIME_FORMAT;

            band.Columns[colState].Header.Caption = "Assessment State";

            band.Columns[colChangedBy].Header.Caption = "Changed By";

            band.Columns[colChangeDescription].Header.Caption = "Change";
        }

        private void _grid_ChangeLog_AfterRowActivate(object sender, EventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            string results = string.Empty;

            if (grid.ActiveRow.IsDataRow)
            {
                results = grid.ActiveRow.Cells[colChangeDescription].Text;
            }
            else if (grid.Selected.Rows.Count > 0)
            {
                results = grid.Selected.Rows[0].Cells[colChangeDescription].Text;
            }

            _textBox_Description.Text = results;
        }

        private void _grid_MouseDown(object sender, MouseEventArgs e)
        {
            // Note: this event handler is used for the MouseDown event on all grids
            UltraGrid grid = (UltraGrid)sender;

            Infragistics.Win.UIElement elementMain;
            Infragistics.Win.UIElement elementUnderMouse;

            elementMain = grid.DisplayLayout.UIElement;

            elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));

            if (elementUnderMouse != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = elementUnderMouse.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)) as Infragistics.Win.UltraWinGrid.UltraGridCell;

                if (cell != null)
                {
                    if (cell.Row.IsDataRow)
                    {
                        m_gridCellClicked = true;
                        grid.Selected.Rows.Clear();
                        cell.Row.Selected = true;
                        grid.ActiveRow = cell.Row;
                    }
                }
                else
                {
                    m_gridCellClicked = false;
                    Infragistics.Win.UltraWinGrid.HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)) as Infragistics.Win.UltraWinGrid.HeaderUIElement;
                    Infragistics.Win.UltraWinGrid.ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.ColScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.ColScrollbarUIElement;
                    Infragistics.Win.UltraWinGrid.RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.RowScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.RowScrollbarUIElement;

                    if (he == null && ce == null && re == null)
                    {
                        grid.Selected.Rows.Clear();
                        grid.ActiveRow = null;
                    }
                }
            }
        }

        #endregion

        #region Splitter Handlers

        private void _splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            m_focused = getFocused(this.Controls);
        }

        private void _splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_focused != null)
            {
                m_focused.Focus();
                m_focused = null;
            }
        }

        #endregion

        private void PolicyChangeLog_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        #endregion
    }
}
