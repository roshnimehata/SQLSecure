namespace Idera.SQLsecure.UI.Console.Import
{
    internal class OperationStatusEntry
    {
        public OperationStatusEntryType OperationStatusEntryType { get; set; }
        public string Message { get; set; }

        public OperationStatusEntry(OperationStatusEntryType entryType, string message)
        {
            OperationStatusEntryType = entryType;
            Message = message;
        }
    }
}