namespace Idera.SQLsecure.UI.Console.SQL
{
    public class SQLServerProperties
    {
        public HadrManagerStatus HadrManagerStatus { get; set; }
        public string MachineName { get; set; }
        public string ServerName { get; set; }
        public string InstanceName { get; set; }
        public string LocalNetAddress { get; set; }
        public string Version { get; set; }
        public string HadrClusterName { get; set; }

        public bool IsServerInAoag
        {
            get
            {
                return HadrManagerStatus == HadrManagerStatus.StartedAndRunning;
            }
        }

        public SQLServerProperties()
        {
            MachineName = string.Empty;
            ServerName = string.Empty;
            InstanceName = string.Empty;
            LocalNetAddress = string.Empty;
            Version = string.Empty;
            HadrClusterName = string.Empty;
            HadrManagerStatus = HadrManagerStatus.NotApplicable;
        }
    }
}
