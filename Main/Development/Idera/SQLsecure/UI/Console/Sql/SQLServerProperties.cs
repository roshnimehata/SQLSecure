namespace Idera.SQLsecure.UI.Console.SQL
{
    public class SQLServerProperties
    {
        private HadrManagerStatus _hadrManagerStatus;
        private string _machineName;
        private string _serverName;
        private string _instanceName;
        private string _localNetAddress;
        private string _clientNetAddress;
        private string _version;
        private string _hadrClusterName;

        public HadrManagerStatus HadrManagerStatus
        {
            get { return _hadrManagerStatus; }
            set { _hadrManagerStatus = value; }
        }

        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }

        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        public string InstanceName
        {
            get { return _instanceName; }
            set { _instanceName = value; }
        }

        public string LocalNetAddress
        {
            get { return _localNetAddress; }
            set { _localNetAddress = value; }
        }

        public string ClientNetAddress
        {
            get { return _clientNetAddress; }
            set { _clientNetAddress = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string HadrClusterName
        {
            get { return _hadrClusterName; }
            set { _hadrClusterName = value; }
        }

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
