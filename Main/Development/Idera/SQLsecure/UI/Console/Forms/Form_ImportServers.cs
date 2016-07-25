using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Import;
using Idera.SQLsecure.UI.Console.Import.Models;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win.UltraWinListView;
using Infragistics.Win.UltraWinTabs;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_ImportServers : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
       
        public Form_ImportServers()
        {
            InitializeComponent();
            backgroundWorker.DoWork += ProcessServersImport;
            backgroundWorker.RunWorkerCompleted += ImportFinished;

        }

        delegate void SetImportItemState(UltraListViewItem lvImport, ImportStatusIcon statusIcon, string statusMessage);
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_ImportServers");
        public static void Process()
        {
            var form = new Form_ImportServers();
            form.ShowDialog();
        }




      
        private void ImportFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            using (logX.loggerX.InfoCall())
            {
                if (e.Error != null)
                {
                    MsgBox.ShowError(ErrorMsgs.ImportServersCaption, ErrorMsgs.ImportedWithErrors);
                }
                else if (e.Cancelled)
                {
                    MsgBox.ShowWarning(ErrorMsgs.ImportServersCaption, ErrorMsgs.ImportCancelled);
                    Close();
                }
                else MsgBox.ShowInfo(ErrorMsgs.ImportServersCaption, ErrorMsgs.ImportSuccessfull);
                Program.gController.SignalRefreshServersEvent(true, string.Empty);
                UnLockControls();
            }
        }


        private void button_Browse_Click(object sender, EventArgs e)
        {
            using (logX.loggerX.InfoCall())
            {
                try
                {
                    if (ofd_OpenFileToImport.ShowDialog() == DialogResult.OK)
                    {
                        textBox_ServersImportFile.Text = ofd_OpenFileToImport.FileName;

                        lvImportStatus.Items.Clear();

                        //parse
                        List<ImportItem> parsedData = ParseFile();

                        if (parsedData != null)
                        {
                            UpdateListViewItemsFromParsedData(parsedData);
                        }
                        CheckIfCanImport(lvImportStatus);

                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(ex.Message);
                    MsgBox.ShowError(ErrorMsgs.ImportServersCaption, ex.Message);

                }
            }
        }

        private void UpdateListViewItemsFromParsedData(List<ImportItem> parsedData)
        {
            using (logX.loggerX.InfoCall())
            {

                foreach (var importItem in parsedData)
                {

                    var lvItem = new UltraListViewItem(importItem.ServerName, new UltraListViewSubItem[1])
                    {
                        Tag = importItem,
                        CheckState = !importItem.HasErrors() ? CheckState.Checked : CheckState.Unchecked
                    };
                    lvImportStatus.Items.Add(lvItem);
                    if (importItem.HasErrors())
                        ApplyImportStatus(lvItem, ImportStatusIcon.Error, importItem.GetErrors());
                }

            }
        }



        private List<ImportItem> ParseFile()
        {
            using (logX.loggerX.InfoCall())
            {
                try
                {
                    var dataProvider = new CsvDataProvider();
                    return
                        dataProvider.ParseStream(new FileStream(textBox_ServersImportFile.Text, FileMode.Open));
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(ex.Message);
                    throw new InvalidDataException(ErrorMsgs.InvalidImportFileFormat, ex);
                }
            }
        }


        private void ultraButton_OK_Click(object sender, EventArgs e)
        {
            using (logX.loggerX.InfoCall())
            {
                try
                {

                    if (!File.Exists(textBox_ServersImportFile.Text))
                    {
                        MsgBox.ShowError(ErrorMsgs.ImportServersCaption, ErrorMsgs.PleaseProvideValidFile);
                        return;
                    }


                    if (!ValidateIfServersExists()) return;
                    if (!backgroundWorker.IsBusy)
                    {
                        LockControls();
                        backgroundWorker.RunWorkerAsync(Program.gController.Repository);
                    }


                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(ex);
                    MsgBox.ShowError(ErrorMsgs.ImportServersCaption, ex.Message);
                }

            }
        }

        private void LockControls()
        {
            ultraButton_OK.Enabled = false;
            button_Browse.Enabled = false;
            textBox_ServersImportFile.Enabled = false;
        }

        private void UnLockControls()
        {
            ultraButton_OK.Enabled = true;
            button_Browse.Enabled = true;
            textBox_ServersImportFile.Enabled = true;
        }
        private void ProcessServersImport(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            using (logX.loggerX.InfoCall())
            {
                var worker = sender as BackgroundWorker;
                if (backgroundWorker.CancellationPending)
                {
                    doWorkEventArgs.Cancel = true;
                }
                else
                {
                    var repository = doWorkEventArgs.Argument as Repository;
                    if (repository == null) throw new InvalidCastException(ErrorMsgs.RepositoryObjectNull);
                    foreach (UltraListViewItem lvImport in lvImportStatus.Items)
                    {
                        if (backgroundWorker.CancellationPending)
                        {
                            doWorkEventArgs.Cancel = true;
                            break;
                        }
                        try
                        {
                            ImportItem importItem = lvImport.Tag as ImportItem;
                            if (importItem == null)
                            {
                                UpdateItemImportStatus(lvImport, ImportStatusIcon.Error, "Not imported. Data is empty");

                                continue; //skip element
                            }

                            if (lvImport.CheckState == CheckState.Unchecked)
                            {
                                UpdateItemImportStatus(lvImport, ImportStatusIcon.Undefined, "Skipped");
                                continue;
                            }

                            var importSettings = new ImportSettings
                            {
                                ForcedServerRegistration = cbRegisterAnyway.Checked
                            };
                            importSettings.OnImportStatusChanged +=
                                delegate(ImportStatusIcon importIcon, string statusMessage)
                                {
                                    UpdateItemImportStatus(lvImport, importIcon, statusMessage);
                                };

                            if (ServerImportManager.ImportItem(importItem, repository, importSettings) && worker != null)
                                worker.ReportProgress(0,
                                    importItem.ServerName.Trim().ToUpper(CultureInfo.InvariantCulture));

                        }
                        catch (Exception ex)
                        {
                            logX.loggerX.Error(ex.Message);
                            UpdateItemImportStatus(lvImport, ImportStatusIcon.Error, ex.Message);
                        }
                    }
                }
            }
        }


        private bool ValidateIfServersExists()
        {
            using (logX.loggerX.InfoCall())
            {
                bool allowServerUpdates = false;
                foreach (UltraListViewItem lvImport in lvImportStatus.Items)
                {
                    ImportItem importItem = lvImport.Tag as ImportItem;
                    if (importItem == null || lvImport.CheckState != CheckState.Checked) continue; //skip element

                    if (!allowServerUpdates)
                        if (Program.gController.Repository.RegisteredServers.Find(importItem.ServerName) != null)
                        {
                            if (
                                MsgBox.ShowConfirm(ErrorMsgs.ImportServersCaption, ErrorMsgs.AllowSqlServersUpdate) ==
                                DialogResult.No)
                            {
                                
                                return false;
                            }
                            else allowServerUpdates = true;
                        }
                    ;
                }
                return true;
            }
        }




        private void UpdateItemImportStatus(UltraListViewItem lvImport, ImportStatusIcon statusIcon,
            string statusMessage)
        {
            using (logX.loggerX.InfoCall())
            {
                if (lvImportStatus.InvokeRequired)
                {
                    SetImportItemState setState = new SetImportItemState(UpdateItemImportStatus);
                    this.Invoke(setState, new object[] {lvImport, statusIcon, statusMessage});
                }
                else
                {
                    ApplyImportStatus(lvImport, statusIcon, statusMessage);
                }
            }

        }

        private void ApplyImportStatus(UltraListViewItem lvImport, ImportStatusIcon statusIcon, string statusMessage)
        {
            using (logX.loggerX.InfoCall())
            {
                lvImport.SubItems[0].Value = statusMessage;
                Image image;
                switch (statusIcon)
                {
                    case ImportStatusIcon.Imported:
                        image = imageList1.Images["Ok"];
                        break;
                    case ImportStatusIcon.Warning:
                        image = imageList1.Images["Warning"];
                        break;
                    case ImportStatusIcon.Error:
                        image = imageList1.Images["Error"];
                        break;
                    case ImportStatusIcon.Importing:
                        image = imageList1.Images["Importing"];
                        break;
                    default:
                        image = imageList1.Images["Undefined"];
                        break;
                }
                lvImport.Appearance.Image = image;
            }
        }



        private void lvImportStatus_ItemCheckStateChanged(object sender, ItemCheckStateChangedEventArgs e)
        {
            var lv = sender as UltraListView;
            CheckIfCanImport(lv);
        }

        private void CheckIfCanImport(UltraListView listView)
        {
            using (logX.loggerX.InfoCall())
            {
                if (listView != null)
                {
                    foreach (var item in listView.Items)
                    {
                        if (item.CheckState == CheckState.Checked)
                        {
                            ultraButton_OK.Enabled = true;
                            return;
                        }
                    }
                    ultraButton_OK.Enabled = false;
                }
            }
        }

        private void ultraButton_Cancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy && backgroundWorker.WorkerSupportsCancellation &&
                !backgroundWorker.CancellationPending)
            {
                ultraButton_Cancel.Enabled = false;
                backgroundWorker.CancelAsync();
            }
            else Close();
        }

        private void ultraButton_Help_Click(object sender, EventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.ImportServerHelpTopic);
        }
    }
}
