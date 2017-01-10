using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Import
{
    internal class OperationResult<T>
    {
        public T Value { get; set; }
        private List<OperationStatusEntry> statusEvents { get; set; }

        public OperationResult()
        {
            statusEvents = new List<OperationStatusEntry>();
        }

        public string GetEventAllMessagesString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var statusEvent in statusEvents)
            {
                result.AppendLine(statusEvent.Message);
            }
            return result.ToString();
        }
        public List<string> GetEventMessagesStringList()
        {
            var result = new List<string>();
            foreach (var statusEvent in statusEvents)
            {
                result.Add(statusEvent.Message);
            }

            return result;
        }

        public void AddStatusEvent(OperationStatusEntryType entryType, string message)
        {
            statusEvents.Add(new OperationStatusEntry(entryType, message));
        }



        public void AddErrorEvent(string message)
        {
            AddStatusEvent(OperationStatusEntryType.Error, message);
        }


        public void AddInfoEvent(string message)
        {
            AddStatusEvent(OperationStatusEntryType.Info, message);
        }

        public void AddWarningEvent(string message)
        {
            AddStatusEvent(OperationStatusEntryType.Warning, message);
        }

    }
}