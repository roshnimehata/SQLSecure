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
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;
using Policy = Idera.SQLsecure.UI.Console.Sql.Policy;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_ExplanationNotes : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Ctors

        public Form_ExplanationNotes(int policyId, int assessmentId, int metricId, string serverName)
        {
            InitializeComponent();

            this.Description = DESCRIPTION;

            m_PolicyId = policyId;
            m_AssessmentId = assessmentId;
            m_MetricId = metricId;
            m_ServerName = serverName;

            _toolStripButton_ExplanationNotesSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ExplanationNotesPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid_ExplanationNotes.Tag = _label_ExplanationNotes;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_ExplanationNotes.Tag = _grid_ExplanationNotes;

            // load value lists for grid display
            ValueList severityValueList = new ValueList();
            severityValueList.Key = valueListSeverity;
            severityValueList.DisplayStyle = ValueListDisplayStyle.Picture;
            severityValueList.Appearance.ImageVAlign = VAlign.Top;
            _grid_ExplanationNotes.DisplayLayout.ValueLists.Add(severityValueList);

            ValueListItem listItem;

            severityValueList.ValueListItems.Clear();
            listItem = new ValueListItem(Utility.Policy.Severity.Ok, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Ok));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.check_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.Severity.Low, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Low));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.LowRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.Severity.Medium, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Medium));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.MediumRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.Severity.High, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.High));
            listItem.Appearance.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.HighRisk_16;
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Policy.Severity.Undetermined, DescriptionHelper.GetEnumDescription(Utility.Policy.Severity.Undetermined));
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.Unknown);
            listItem.Appearance.ImageVAlign = VAlign.Top;
            severityValueList.ValueListItems.Add(listItem);

            _grid_ExplanationNotes.DrawFilter = new Utility.HideFocusRectangleDrawFilter();

            loadDataSource();
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_ExplanationNotes");
        private int m_PolicyId;
        private int m_AssessmentId;
        private int m_MetricId;
        private string m_ServerName;
        private List<PolicyAssessmentNotes> m_Notes;

        #endregion

        #region constants

        private const string DESCRIPTION = @"Indicate whether this security check finding can be explained for these audited servers.";

        private const string valueListSeverity = @"Severity";

        private const string colServer = "ServerName";
        private const string colPolicyId = "PolicyId";
        private const string colAssessmentId = "AssessmentId";
        private const string colMetricId = "MetricId";
        private const string colSnapshotId = "SnapshotId";
        private const string colIsExplained = "IsExplained";
        private const string colNotes = "Notes";
        private const string colSeverityCode = "SeverityCode";
        private const string colMetricName = "MetricName";

        private const string HeaderDisplay = @"{0} Server{1}, {2} Risk{3}, {4} Explained";
        private const string PrintTitle = @"Explanation Notes";
        private const string PrintHeaderDisplay = "{0}\nExplanation Notes for Security Check {1} as of {2}";

        #endregion

        #region Properties

        private List<PolicyAssessmentNotes> Notes
        {
            get { return m_Notes; }
        }

        #endregion

        #region Helpers

        private void loadDataSource()
        {
            if (m_ServerName != null)
            {
                m_Notes = PolicyAssessmentNotes.GetPolicyAssessmentNotes(m_PolicyId, m_AssessmentId, m_MetricId, m_ServerName);
            }
            else
            {
                m_Notes = PolicyAssessmentNotes.GetPolicyAssessmentNotes(m_PolicyId, m_AssessmentId, m_MetricId);
            }

            _grid_ExplanationNotes.SuspendLayout();

            _grid_ExplanationNotes.SetDataBinding(m_Notes, null);

            // let the grid sort the list because it is not necessarily sorted
            _grid_ExplanationNotes.DisplayLayout.Bands[0].SortedColumns.Add(colServer, false);

            _grid_ExplanationNotes.DisplayLayout.Bands[0].Columns[colServer].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);

            if (_grid_ExplanationNotes.DisplayLayout.Bands[0].Columns[colServer].Width > _grid_ExplanationNotes.Width / 2)
            {
                _grid_ExplanationNotes.DisplayLayout.Bands[0].Columns[colServer].Width = _grid_ExplanationNotes.Width/2;
            }

            int riskCount = 0;
            int explainedCount = 0;
            string metricName = null;
            foreach (PolicyAssessmentNotes notes in m_Notes)
            {
                if (metricName == null)
                {
                    metricName = notes.MetricName;
                }
                if (notes.SeverityCode > 0)
                {
                    riskCount++;
                }
                if (notes.IsExplained)
                {
                    explainedCount++;
                }
            }
            this.Text = string.Format("Edit Explanation Notes for {0}", metricName);

            _label_ExplanationNotes.Text =
                string.Format(HeaderDisplay,
                    m_Notes.Count,
                    m_Notes.Count == 1 ? string.Empty : "s",
                    riskCount,
                    riskCount == 1 ? string.Empty : "s",
                    explainedCount);

            _grid_ExplanationNotes.ResumeLayout();
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

            Policy policy = Program.gController.Repository.Policies.Find(m_PolicyId);
            if (policy.AssessmentId != m_AssessmentId && policy.HasAssessment(m_AssessmentId))
            {
                policy = policy.Assessments.Find(m_AssessmentId);
            }

            PolicyMetric check = null;

            foreach (PolicyMetric metric in policy.GetPolicyMetrics())
            {
                if (metric.MetricId == m_MetricId)
                {
                    check = metric;
                    break;
                }
            }

            if (policy != null && check != null)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                    policy.PolicyAssessmentName,
                                    check.MetricName,
                                    DateTime.Now.ToShortDateString()
                        );
                _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

                // Call ShowDialog to show the print preview dialog.
                _ultraPrintPreviewDialog.ShowDialog();
            }
            else
            {
                logX.loggerX.Error(string.Format(Utility.ErrorMsgs.ErrorStub, Utility.ErrorMsgs.CantGetPolicy));
                MsgBox.ShowError(Utility.ErrorMsgs.PolicyCaption,Utility.ErrorMsgs.CantGetPolicy);
            }
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //display the details column, which is always hidden from the user
                    if (grid.DisplayLayout.Bands[0].Columns.Exists(colSeverityCode))
                    {
                        grid.DisplayLayout.Bands[0].Columns[colSeverityCode].Hidden = false;
                    }

                    grid.DisplayLayout.ValueLists[valueListSeverity].DisplayStyle = ValueListDisplayStyle.DisplayText;

                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);

                    grid.DisplayLayout.ValueLists[valueListSeverity].DisplayStyle = ValueListDisplayStyle.Picture;
                }
                catch (Exception ex)
                {
                    grid.DisplayLayout.ValueLists[valueListSeverity].DisplayStyle = ValueListDisplayStyle.Picture;

                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                if (grid.DisplayLayout.Bands[0].Columns.Exists(colSeverityCode))
                {
                    grid.DisplayLayout.Bands[0].Columns[colSeverityCode].Hidden = true;
                }
            }
        }

        #endregion

        #region Methods

        public static bool Process(int policyId, int assessmentId, int metricId, string server)
        {
            bool updated = false;
            // Create the form.
            Form_ExplanationNotes form = new Form_ExplanationNotes(policyId, assessmentId, metricId, server);

            if (DialogResult.OK == form.ShowDialog())
            {
                foreach (PolicyAssessmentNotes notes in form.Notes)
                {
                    if (notes.UpdatePolicyAssessmentNotesToRepository())
                    {
                        updated = true;
                    }
                }
            }

            if (updated)
            {
                Program.gController.SignalRefreshPoliciesEvent(policyId);
            }

            return updated;
        }

        #endregion

        #region events

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            printGrid(_grid_ExplanationNotes);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            saveGrid(_grid_ExplanationNotes);

            Cursor = Cursors.Default;
        }

        private void _grid_ExplanationNotes_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colServer].Header.Caption = @"Server";
            band.Columns[colServer].Header.SetVisiblePosition(0, false);
            band.Columns[colServer].CellClickAction = CellClickAction.CellSelect;

            band.Columns[colPolicyId].Hidden = true;
            band.Columns[colPolicyId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colAssessmentId].Hidden = true;
            band.Columns[colAssessmentId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colMetricId].Hidden = true;
            band.Columns[colMetricId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colSnapshotId].Hidden = true;
            band.Columns[colSnapshotId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colSeverityCode].Header.Caption = @"Risk";
            band.Columns[colSeverityCode].Header.SetVisiblePosition(1, false);
            band.Columns[colSeverityCode].CellClickAction = CellClickAction.CellSelect;
            band.Columns[colSeverityCode].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colSeverityCode].ValueList = e.Layout.ValueLists[valueListSeverity];
            band.Columns[colSeverityCode].Width = 40;
            band.Columns[colSeverityCode].MinWidth = 40;
            band.Columns[colSeverityCode].MaxWidth = 40;
            band.Columns[colSeverityCode].LockedWidth = true;
            EditorWithText textEditor = new EditorWithText();
            band.Columns[colSeverityCode].Editor = textEditor;

            band.Columns[colIsExplained].Header.Caption = @"Explained";
            band.Columns[colIsExplained].Header.SetVisiblePosition(2, false);
            band.Columns[colIsExplained].Width = 60;
            band.Columns[colIsExplained].MaxWidth = 60;
            band.Columns[colIsExplained].MinWidth = 60;

            band.Columns[colNotes].Header.Caption = @"Notes";
            band.Columns[colNotes].MaxLength = 4000;
            band.Columns[colNotes].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

            band.Columns[colMetricName].Hidden = true;
            band.Columns[colMetricName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        }

        private void _button_OK_Click(object sender, EventArgs e)
        {
            if ((_grid_ExplanationNotes.CurrentState & UltraGridState.InEdit) == UltraGridState.InEdit)
            {
                _grid_ExplanationNotes.PerformAction(UltraGridAction.ExitEditMode);
                _grid_ExplanationNotes.UpdateData();
            }
        }

        private void _button_Help_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void Form_ExplanationNotes_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            Program.gController.ShowTopic(Utility.Help.EditExplanationNotesHelpTopic);
        }

        #endregion
    }
}

