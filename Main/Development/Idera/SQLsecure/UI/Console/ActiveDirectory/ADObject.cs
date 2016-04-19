using System;

using Idera.SQLsecure.Core.Accounts;

namespace Idera.SQLsecure.UI.Console.ActiveDirectory
{
	/// <summary>
	/// Summary description for ADObject.
	/// </summary>
	public struct ADObject
	{
        private string adsPath;
		private string className;
        private string name;
        private string samAccountName;
        private Sid sid;
        private string upn;

        public string AdsPath
        {
            get
            {
                return adsPath;
            }
            set
            {
                adsPath = value;
            }
        }

		public string ClassName
		{
			get
			{
				return className;
			}
			set
			{
				className = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

        public string SamAccountName
        {
            get
            {
                return samAccountName;
            }
            set
            {
                samAccountName = value;
            }
        }

        public Sid Sid
        {
            get
            {
                return sid;
            }
            set
            {
                sid = value;
            }
        }

		public string UPN
		{
			get
			{
				return upn;
			}
			set
			{
				upn = value;
			}
		}
    }
}
