using Idera.SQLsecure.UI.Console.Forms;

namespace Idera.SQLsecure.UI.Console.Import
{
    public class ImportSettings
    {
        public static ImportSettings Default = new ImportSettings();
        public bool ForcedServerRegistration { get; set; }


        public delegate void ImportStatusChangedHandler(ImportStatusIcon importIcon, string statusMessage);

        public event ImportStatusChangedHandler OnImportStatusChanged;
        public void ChangeStatus(ImportStatusIcon importIcon, string statusMessage)
        {
            if (OnImportStatusChanged != null) OnImportStatusChanged(importIcon, statusMessage);
        }
    }
}