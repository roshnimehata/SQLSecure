using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Forms;
using Idera.SQLsecure.UI.Console.Interfaces;
using Idera.SQLsecure.UI.Console.SQL;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Help = Idera.SQLsecure.UI.Console.Utility.Help;
using Policy = Idera.SQLsecure.UI.Console.Sql.Policy;
using System.Collections.Generic;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_ServerTags : BaseView, IView
    {
        private const string TitleConst = "Manage Tags";
        #region IView

        void IView.SetContext(IDataContext contextIn)
        {
            Title = TitleConst;// Move to constants
            LoadTags(true);
        }
        String IView.HelpTopic
        {
            get { return Help.ManageTagsHelpTopic; }
        }
        String IView.ConceptTopic
        {
            get { return Help.ManageTagsConceptTopic; }
        }
        String IView.Title
        {
            get { return Title; }
        }

        #endregion


        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.View_ServerTags");
        private DataTable _dt_Tags = new DataTable();
        private DataTable _dt_Servers = new DataTable();
        private bool m_gridCellClicked;


        // UI Column Headings
        private const string colTypeIcon = "TypeIcon";
        private const string colHeaderTagName = "TagName";
        private const string colHeaderHiddenTagID = "TagId";
        private const string colHeaderHiddenServerId = "ServerId";
        private const string colHeaderServerName = "ServerName";
        private const string colHeaderDesc = "TagDescription";


        private const string TagHeaderText = @"Tags";
        private const string TagHeaderDisplay = TagHeaderText + " ({0} items)";

        private const string ServerHeaderText = @"Servers";
        private const string ServerHeaderDisplay = ServerHeaderText + " - {0} ({1} items)";

        private const string PrintHeaderDisplay = "{0} ({1} items)";
        private const string PrintHeaderTimeDisplay = "{0} as of {1}";

        private const string valueListSeverity = @"Severity";

        private const string RiskConfigured = "{0} configured";
        private const string RiskFindings = "{0} of {1}";

        #endregion


        #region Overrides

        protected override void ShowRefresh()
        {
            Cursor = Cursors.WaitCursor;
            _grid_Tags.BeginUpdate();

            string activeRowName = null;
            if (_grid_Tags.ActiveRow != null && _grid_Tags.ActiveRow.IsDataRow)
            {
                activeRowName = _grid_Tags.ActiveRow.Cells[colHeaderTagName].Text;
            }

            LoadTags(false);

            SetActiveRow(_grid_Tags, activeRowName);

            _grid_Tags.EndUpdate();

            Cursor = Cursors.Default;
        }

        #endregion


        #region CTOR

        public View_ServerTags()
        {
            InitializeComponent();

            _label_Summary.Text = "Manage Tags";

            _toolStripButton_PoliciesColumnChooser.Image =
                _toolStripButton_AuditsColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_PoliciesGroupBy.Image =
                _toolStripButton_AuditsGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_PoliciesSave.Image =
                _toolStripButton_AuditsSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_PoliciesPrint.Image =
                _toolStripButton_AuditsPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);


            _smallTask_CreateTag.TaskHandler += CreateTag;
            _smallTask_EditTag.TaskHandler += EditTag;


            _grid_TagServers.Tag = _label_Assessments;
            _grid_Tags.Tag = _label_Policies;

            _dt_Tags.Columns.Add(colHeaderTagName, typeof(String));
            _dt_Tags.Columns.Add(colHeaderDesc, typeof(string));
            _dt_Tags.Columns.Add(colHeaderHiddenTagID, typeof(int));


            _dt_Servers.Columns.Add(colHeaderServerName, typeof(string));
            _dt_Servers.Columns.Add(colHeaderHiddenServerId, typeof(int));



            _grid_Tags.DrawFilter = new HideFocusRectangleDrawFilter();
            _grid_TagServers.DrawFilter = new HideFocusRectangleDrawFilter();



        }
        #endregion


        #region Helpers

        private void LoadTags(bool bUpdate)
        {
            _dt_Tags.Clear();
            try
            {
                // Retrieve policy information.
                var tags = TagWorker.GetTags();

                foreach (Tag p in tags)
                {
                    DataRow newRow = _dt_Tags.NewRow();

                    newRow[colHeaderTagName] = p.Name;
                    newRow[colHeaderDesc] = p.Description;
                    newRow[colHeaderHiddenTagID] = p.Id;
                    _dt_Tags.Rows.Add(newRow);
                }

            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve tags", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetLoginsCaption, ex);
            }
            if (_dt_Tags.Rows.Count != 0)
            {
                _grid_Tags.BeginUpdate();
                _grid_Tags.DataSource = _dt_Tags;
                _grid_Tags.Selected.Rows.Clear();
                _grid_Tags.Selected.Rows.Add(_grid_Tags.Rows[0]);

                _grid_Tags.DataMember = "";
                if (bUpdate)
                {
                    _grid_Tags.EndUpdate();
                }
            }

            _label_Policies.Text = string.Format(TagHeaderDisplay, _grid_Tags.Rows.Count);

        }

        private bool LoadTagServers(int tagId, bool bUpdate)
        {
            _dt_Servers.Clear();
            try
            {
                var servers = TagWorker.GetTagServers(tagId);

                if (servers.Count == 0) return false;
                foreach (var server in servers)
                {
                    DataRow newRow = _dt_Servers.NewRow();
                    newRow[colHeaderServerName] = server.Name;
                    newRow[colHeaderHiddenServerId] = server.Id;
                    _dt_Servers.Rows.Add(newRow);
                }

            }
            catch (Exception ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve Servers", ex);
                MsgBox.ShowError(ErrorMsgs.CantGetLoginsCaption, ex);
            }

            _grid_TagServers.BeginUpdate();
            _grid_TagServers.DataSource = _dt_Servers;
            _grid_TagServers.DataMember = "";
            if (bUpdate)
            {
                _grid_TagServers.EndUpdate();
            }



            if (_grid_TagServers.Rows.Count == 0)
            {
                _label_NoServers.Visible = true;
                _grid_TagServers.Visible = false;
            }
            else
            {
                _label_NoServers.Visible = false;
                _grid_TagServers.Visible = true;
            }
            return true;

        }



        private void SetActiveRow(UltraGrid grid, string tagName)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                RowsCollection sr = grid.Rows;
                if (sr.Count > 0)
                {
                    foreach (UltraGridRow row in sr)
                    {
                        row.Selected = false;
                        if (row.Cells[colHeaderTagName].Text == tagName)
                        {
                            row.Selected = true;
                            grid.ActiveRow = row;
                        }
                    }
                }
            }
        }

        #endregion


        #region Small Task Handlers


        private void EditTag(int tagId)
        {
            if (tagId > 0)
            {
                try
                {
                    var t = TagWorker.GetTagById(tagId);
                    if (t != null)
                        Form_CreateTag.Process(t);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

        }

        private void EditTag(object sender, EventArgs e)
        {
            if (_grid_Tags.ActiveRow != null && _grid_Tags.ActiveRow.IsDataRow)
            {
                string tagName = _grid_Tags.ActiveRow.Cells[colHeaderTagName].Text;

                int tagId = (int)_grid_Tags.ActiveRow.Cells[colHeaderHiddenTagID].Value;

                EditTag(tagId);

                ShowRefresh();
                SetActiveRow(_grid_Tags, tagName);
            }
        }

        private void AddServersToTag(object sender, EventArgs e)
        {
            if (_grid_Tags.ActiveRow != null && _grid_Tags.ActiveRow.IsDataRow)
            {
                Form_SelectServer serverListForm = new Form_SelectServer(true);
                if (serverListForm.LoadServers(true))
                {
                    if (serverListForm.ShowDialog() == DialogResult.OK)
                    {
                        var servers = serverListForm.SelectedServers;

                        int tagId = (int)_grid_Tags.ActiveRow.Cells[colHeaderHiddenTagID].Value;
                        if (tagId > 0)
                        {
                            TagWorker.AssignServerToTag(tagId, servers);
                        }
                    }
                }
            }
            ShowRefresh();
        }
        private void CreateTag(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Form_CreateTag.Process(null);

            ShowRefresh();

            Cursor = Cursors.Default;

        }

        private void DeleteTag(object sender, EventArgs e)
        {
            if (_grid_Tags.ActiveRow != null && _grid_Tags.ActiveRow.IsDataRow)
            {
                try
                {
                    int tagId = (int)_grid_Tags.ActiveRow.Cells[colHeaderHiddenTagID].Value;
                    TagWorker.DeleteTag(tagId);
                    ShowRefresh();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.RegisterSqlServerCaption, ex.Message, ex);
                }

            }
        }

        private void TakeSnapshot(object sender, EventArgs e)
        {
            if (_grid_Tags.ActiveRow != null && _grid_Tags.ActiveRow.IsDataRow)
            {

                int tagId = (int)_grid_Tags.ActiveRow.Cells[colHeaderHiddenTagID].Value;
                var servers = TagWorker.GetTagServers(tagId);
                List<RegisteredServer> serversToRun = new List<RegisteredServer>();
                foreach (TaggedServer item in servers)
                {
                    serversToRun.Add(Program.gController.Repository.GetServer(item.Name));

                }
                List<string> failedServer = new List<string>();
                foreach (RegisteredServer server in serversToRun)
                {
                    Guid guid;
                    if (server.StartJob(out guid, false))
                    {
                        server.SetJobId(guid);
                    }
                    else
                    {
                        failedServer.Add(server.ConnectionName);
                    }
                }
                if (failedServer.Count != 0)
                {
                    MsgBox.ShowError(ErrorMsgs.RegisterSqlServerCaption, String.Format("Unable to start snapshot for next server(s) \n{0}", string.Join("\n,", failedServer.ToArray())));
                }


            }
        }

        #endregion

        #region Grid Events

        private void _grid_Tag_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colHeaderTagName].Width = 200;
            band.Columns[colHeaderTagName].Header.ToolTipText = "Tag Name";

            band.Columns[colHeaderHiddenTagID].Header.Caption = "Tag ID";
            band.Columns[colHeaderHiddenTagID].Header.ToolTipText = "Tag ID used in Repository";
            band.Columns[colHeaderHiddenTagID].Hidden = true;

            band.Columns[colHeaderDesc].Header.Caption = "Tag Description";
            band.Columns[colHeaderDesc].Header.ToolTipText = "";//todo add tool tip
            band.Columns[colHeaderDesc].CellAppearance.TextHAlign = HAlign.Left;



        }

        private void _grid_Assessments_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];

            band.Columns[colHeaderServerName].Width = 200;
            band.Columns[colHeaderServerName].Header.ToolTipText = "Server Name";
            band.Columns[colHeaderServerName].Header.Caption = "Server Name";


            band.Columns[colHeaderHiddenServerId].Header.Caption = "Server Id";
            band.Columns[colHeaderHiddenServerId].Header.ToolTipText = "Sever Id";
            band.Columns[colHeaderHiddenServerId].Hidden = true;
            band.Columns[colHeaderHiddenServerId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

        }

        private void _grid_Tags_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            EditTag(null, null);
        }

        private void _grid_Policies_AfterRowActivate(object sender, EventArgs e)
        {
            ValidateRow();
        }

        private void _grid_Servers_AfterRowActivate(object sender, EventArgs e)
        {
            ValideteSelectedRow();
        }

        private void ValideteSelectedRow()
        {
            bool enabled = _grid_TagServers.ActiveRow != null && _grid_TagServers.ActiveRow.IsDataRow;
            _cmsi_Servers_Delete.Enabled = enabled;
        }

        private void _grid_MouseDown(object sender, MouseEventArgs e)
        {
            // Note: this event handler is used for the MouseDown event on all grids
            UltraGrid grid = (UltraGrid)sender;

            UIElement elementMain = grid.DisplayLayout.UIElement;

            var elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));

            if (elementUnderMouse != null)
            {
                UltraGridCell cell = elementUnderMouse.GetContext(typeof(UltraGridCell)) as UltraGridCell;

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
                    HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(HeaderUIElement)) as HeaderUIElement;
                    ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(ColScrollbarUIElement)) as ColScrollbarUIElement;
                    RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(RowScrollbarUIElement)) as RowScrollbarUIElement;

                    if (he == null && ce == null && re == null)
                    {
                        grid.Selected.Rows.Clear();
                        grid.ActiveRow = null;
                    }
                }
            }
        }

        #endregion

        #region Common Grid Functions

        protected void PrintGrid(UltraGrid grid, string headerText)
        {
            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;
            if (grid.Rows.Count > 0)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderTimeDisplay,
                                   string.Format(PrintHeaderDisplay, headerText, grid.Rows.FilteredInNonGroupByRowCount),
                                   DateTime.Now);
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft = headerText;
            }
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void SaveGrid(UltraGrid grid, string fileName)
        {
            _saveFileDialog.FileName = fileName;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
            }
        }

        protected void showGridColumnChooser(UltraGrid grid)
        {
            // set any column chooser options before showing????
            string gridHeading = ((ToolStripLabel)grid.Tag).Text;
            if (gridHeading.IndexOf("(") > 0)
            {
                gridHeading = gridHeading.Remove(gridHeading.IndexOf("(") - 1);
            }

            Form_GridColumnChooser.Process(grid, gridHeading);
        }

        protected void toggleGridGroupByBox(UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }



        #endregion

        private void _toolStripButton_TagsSave_Click(object sender, EventArgs e)
        {
            SaveGrid(_grid_Tags, "Tags.xls");
        }

        private void _toolStripButton_TagsPrint_Click(object sender, EventArgs e)
        {
            PrintGrid(_grid_Tags, TagHeaderText);
        }

        private void _toolStripButton_ServerSave_Click(object sender, EventArgs e)
        {
            SaveGrid(_grid_TagServers, "Servers.xls");
        }

        private void _toolStripButton_ServerPrint_Click(object sender, EventArgs e)
        {
            PrintGrid(_grid_TagServers, ServerHeaderText);
        }

        private void _toolStripButton_TagsColumnChooser_Click(object sender, EventArgs e)
        {
            showGridColumnChooser(_grid_Tags);
        }

        private void _toolStripButton_TagsGroupBy_Click(object sender, EventArgs e)
        {
            toggleGridGroupByBox(_grid_Tags);
        }

        private void _toolStripButton_AuditsColumnChooser_Click(object sender, EventArgs e)
        {
            showGridColumnChooser(_grid_TagServers);
        }

        private void _toolStripButton_AuditsGroupBy_Click(object sender, EventArgs e)
        {
            toggleGridGroupByBox(_grid_TagServers);
        }

        private void _cmsi_ServerTag_Delete_Click(object sender, EventArgs e)
        {
            if (_grid_TagServers.ActiveRow != null && _grid_TagServers.ActiveRow.IsDataRow)
            {
                int tagId = (int)_grid_Tags.ActiveRow.Cells[colHeaderHiddenTagID].Value;
                int serverId = (int)_grid_TagServers.ActiveRow.Cells[colHeaderHiddenServerId].Value;
                TagWorker.RemoveServerFromTag(tagId, serverId);
                ShowRefresh();

            }
        }





        private void _grid_TagServers_MouseUp(object sender, MouseEventArgs e)
        {
            ValideteSelectedRow();
        }

        private void _grid_Tags_MouseUp(object sender, MouseEventArgs e)
        {
            ValidateRow();
        }

        private void ValidateRow()
        {
            bool enabled = false;
            bool hasServers = false;
            if (_grid_Tags.ActiveRow != null && _grid_Tags.ActiveRow.IsDataRow)
            {
                int policyId = (int)_grid_Tags.ActiveRow.Cells[colHeaderHiddenTagID].Value;              
                hasServers = LoadTagServers(policyId, true);
                enabled = true;
            }
            _cmsi_Tags_Delete.Enabled = enabled;
            _cmsi_Tags_AddServer.Enabled = enabled;
            _cmsi_Tags_edit.Enabled = enabled;
            _cmsi_Tags_TakeSnapshot.Enabled = hasServers;
        }
    }


}

