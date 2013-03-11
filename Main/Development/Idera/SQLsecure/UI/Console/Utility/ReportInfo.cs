using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Diagnostics;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Utility
{
    public class ReportInfo : IComparable<ReportInfo>
    {
        #region Constants

#if DEBUG
        public const string RelativeReportsPath = @"..\..\Reports\RDL";
#else
        public const string RelativeReportsPath = @"..\Reports";
#endif
        public const string RDLXmlFileName = "rdl.xml";

        #endregion

        #region Data Members

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Utility.ReportInfo");

        private string _name;
        private string _description;
        private string _sproc;
        private string _fileName;
        private bool _hidden;
        private List<string> _params;

        #endregion

        #region Constructors

        public ReportInfo()
        {
            _params = null;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string StoredProcedure
        {
            get { return _sproc; }
            set { _sproc = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; }
        }

        #endregion

        public List<string> GetParameters(SqlConnection conn)
        {
            logX.loggerX.Info("Get Parameters");

            if (_params != null)
                return _params;
            _params = new List<string>();

            using (SqlCommand cmd = new SqlCommand(_sproc, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(cmd);
                foreach (SqlParameter p in cmd.Parameters)
                {
                    if (p.Direction == ParameterDirection.Input ||
                       p.Direction == ParameterDirection.InputOutput)
                    {
                        _params.Add(p.ParameterName);
                    }
                }
            }
            return _params;
        }

        public int CompareTo(ReportInfo other)
        {
            if (other == null)
                return -1;
            else
                return _name.CompareTo(other._name);
        }
    }
}
