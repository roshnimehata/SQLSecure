using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Import;
using Idera.SQLsecure.UI.Console.Import.Models;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win.UltraWinListView;
using Help = Idera.SQLsecure.UI.Console.Utility.Help;
using Resources = Idera.SQLsecure.UI.Console.Properties.Resources;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_ImportServers : BaseDialogForm
    {

        private Form_ProcessDialog processdDialog;

        private void CancelImportHandler(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy && backgroundWorker.WorkerSupportsCancellation &&
                    !backgroundWorker.CancellationPending)
            {
                backgroundWorker.CancelAsync();
            }
        }

        public Form_ImportServers()
        {
            InitializeComponent();

            backgroundWorker.DoWork += ProcessServersImport;
            backgroundWorker.RunWorkerCompleted += ImportFinished;

        }

        delegate void SetImportItemState(UltraListViewItem lvImport, ImportStatusIcon statusIcon, string statusMessage);
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_ImportServers");
        private bool _imported;

        public string GetAssemblyPath
        {
            get
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        public static void Process()
        {
            var form = new Form_ImportServers();
            form.ShowDialog();
        }





        private void ImportFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            using (logX.loggerX.InfoCall())
            {
                processdDialog.Close();
                if (e.Error != null)
                {
                    MsgBox.ShowError(ErrorMsgs.ImportServersCaption, ErrorMsgs.ImportedWithErrors);
                }
                else if (e.Cancelled)
                {
                    MsgBox.ShowWarning(ErrorMsgs.ImportServersCaption, ErrorMsgs.ImportCancelled);

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
                        if (!textBox_ServersImportFile.Text.Equals(ofd_OpenFileToImport.FileName,
                            StringComparison.InvariantCultureIgnoreCase))
                            _imported = false;

                        textBox_ServersImportFile.Text = ofd_OpenFileToImport.FileName;

                        lvImportStatus.Items.Clear();

                        //parse
                        List<ImportItem> parsedData = ParseFile();

                        if (parsedData.Count == 0) throw new InvalidDataException(ErrorMsgs.EmptyImportFileError);


                        UpdateListViewItemsFromParsedData(parsedData);

                        CheckIfCanImport(lvImportStatus);

                    }
                }
                catch (Exception ex)
                {
                    textBox_ServersImportFile.Text = string.Empty;
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
                    List<string> lines = File.ReadAllLines(textBox_ServersImportFile.Text).ToList();
                    var totalcols = lines[0].Split(',').Length;
                    string filename = textBox_ServersImportFile.Text;
                    //converting old csv format to new one by adding 2 new columns and the default values for those
                    if (totalcols == 7)
                    {
                        lines[0] += ",PortNumber, ServerType";
                        int index = 1;
                        //add new column value for each row.
                        lines.Skip(1).ToList().ForEach(line =>
                        {
                        //-1 for header
                        lines[index] += ",1433, 0";
                            index++;
                        });
                        //write the new content
                        filename = string.Format("{0}\\{1}", GetAssemblyPath, "test.csv");
                        File.WriteAllLines(filename , lines);
                        
                       
                    }
                    return
                        dataProvider.ParseStream(new FileStream(filename, FileMode.Open));
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

                    if (!ValidateIfLicenseExists()) return;
                    if (!ValidateIfServersExists()) return;
                    if (!backgroundWorker.IsBusy)
                    {
                        _imported = true;
                        LockControls();
                        ShowProcessDialog();
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

        private bool ValidateIfLicenseExists()
        {
            var serversToImport = GetCheckedNewItems();
            if (!Program.gController.Repository.bbsProductLicense.IsLicneseGoodForServerCount(Program.gController.Repository.RegisteredServers.Count + serversToImport.Count))
            {
                MsgBox.ShowError(ErrorMsgs.RegisterSqlServerCaption, ErrorMsgs.RegisterSqlServerNoLicenseMsg);
                return false;
            }
            return true;
        }

        private List<UltraListViewItem> GetCheckedNewItems()
        {
            var result = new List<UltraListViewItem>();
            foreach (var item in lvImportStatus.Items)
            {
                if (item.CheckState == CheckState.Checked && !IsServerAlreadyRegistered(item.Text)) result.Add(item);
            }
            return result;
        }

        private static bool IsServerAlreadyRegistered(string serverName)
        {
            return Program.gController.Repository.RegisteredServers.Find(serverName) != null;
        }

        private void ShowProcessDialog()
        {
            if (processdDialog == null || processdDialog.IsDisposed)
                processdDialog = new Form_ProcessDialog(ErrorMsgs.ImportServersCaption,
                    "Import in progress\nPress Cancel to stop", CancelImportHandler, Resources.ImportServers_48);
            processdDialog.Show();
        }

        private void LockControls()
        {
            ultraButton_OK.Enabled = false;
            button_Browse.Enabled = false;
            textBox_ServersImportFile.Enabled = false;
            ultraButton_Cancel.Enabled = false;
        }

        private void UnLockControls()
        {
            ultraButton_OK.Enabled = true;
            button_Browse.Enabled = true;
            ultraButton_Cancel.Enabled = true;
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
                            UpdateItemImportStatus(lvImport, ImportStatusIcon.Undefined, "Cancelled");
                        }
                        else
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
                                    delegate (ImportStatusIcon importIcon, string statusMessage)
                                    {
                                        UpdateItemImportStatus(lvImport, importIcon, statusMessage);
                                    };

                                if (ServerImportManager.ImportItem(importItem, repository, importSettings) && worker != null)
                                    worker.ReportProgress(0,
                                        importItem.ServerName.Trim().ToUpper(CultureInfo.InvariantCulture));
                                //checking if temporary file was created 
                                // if it was , then deleting that file
                                string tempFilename= string.Format("{0}\\{1}", GetAssemblyPath, "test.csv");
                                if (File.Exists(tempFilename))
                                    File.Delete(tempFilename);

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
                        if (IsServerAlreadyRegistered(importItem.ServerName))
                        {
                            if (
                                MsgBox.ShowConfirm(ErrorMsgs.ImportServersCaption, ErrorMsgs.AllowSqlServersUpdate) ==
                                DialogResult.No)
                            {

                                return false;
                            }
                            allowServerUpdates = true;
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
                    SetImportItemState setState = UpdateItemImportStatus;
                    Invoke(setState, lvImport, statusIcon, statusMessage);
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
            try
            {
                if (_imported && FileExists() &&
                    (cbDeleteCsvFileOnClose.Checked &&
                     MessageBox.Show(ErrorMsgs.ConfirmCsvFileRemove, ErrorMsgs.ImportServersCaption,
                         MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
                {
                    File.Delete(textBox_ServersImportFile.Text);
                }


                Close();
            }
            catch (Exception ex)
            {

                logX.loggerX.Error(ex);
                MsgBox.ShowError(ErrorMsgs.ImportServersCaption, ex.Message);
                Close();

            }
        }

        private bool FileExists()
        {
            return textBox_ServersImportFile.Text != string.Empty && File.Exists(textBox_ServersImportFile.Text);
        }

        private void ultraButton_Help_Click(object sender, EventArgs e)
        {
            Program.gController.ShowTopic(Help.ImportServerHelpTopic);
        }
    }
}
