using System;
using Wintellect.PowerCollections;

namespace Idera.SQLsecure.UI.Console.SQL
{
    public class AvailabilityGroupReplica
    {
        private Guid _replicaid;
        private int _serverreplicaId;
        private int _snapshotId;
        private Guid _groupId;
        private string _replicaServerName;
        private string _ownersid;
        private string _endpointUrl;
        private byte _availabilityMode;
        private string _availabilityModeDesc;
        private byte _failoverMode;
        private string _failoverModeDesc;
        private DateTime _createDate;
        private DateTime _modifyDate;
        private string _ownerName;


        public Guid Replicaid
        {
            get { return _replicaid; }
            set { _replicaid = value; }
        }

        public int ServerreplicaId
        {
            get { return _serverreplicaId; }
            set { _serverreplicaId = value; }
        }

        public int SnapshotId
        {
            get { return _snapshotId; }
            set { _snapshotId = value; }
        }

        public Guid GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        public string ReplicaServerName
        {
            get { return _replicaServerName; }
            set { _replicaServerName = value; }
        }

        public string Ownersid
        {
            get { return _ownersid; }
            set { _ownersid = value; }
        }

        public string EndpointUrl
        {
            get { return _endpointUrl; }
            set { _endpointUrl = value; }
        }

        public byte AvailabilityMode
        {
            get { return _availabilityMode; }
            set { _availabilityMode = value; }
        }

        public string AvailabilityModeDesc
        {
            get { return _availabilityModeDesc; }
            set { _availabilityModeDesc = value; }
        }

        public byte FailoverMode
        {
            get { return _failoverMode; }
            set { _failoverMode = value; }
        }

        public string FailoverModeDesc
        {
            get { return _failoverModeDesc; }
            set { _failoverModeDesc = value; }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public DateTime ModifyDate
        {
            get { return _modifyDate; }
            set { _modifyDate = value; }
        }

        public string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }
    }


    public enum AvailabilityGroupReplicaColumns
    {
        Replicaid = 0,
        ServerreplicaId = 1,
        SnapshotId = 2,
        GroupId = 3,
        ReplicaServerName = 4,
        Ownersid = 5,
        EndpointUrl = 6,
        AvailabilityMode = 7,
        AvailabilityModeDesc = 8,
        FailoverMode = 9,
        FailoverModeDesc = 10,
        CreateDate = 11,
        ModifyDate = 12,
        OwnerName = 13
    };
}
