using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ReportingServices.Interfaces;

namespace Idera.SQLsecure.UI.Console.Import.Models
{
    //Server Name, AuthType, User, Password, UseSameCredentials, WindowsUser, WindowsUserPassword
    public class ImportItem
    {
        [PropertyOrder(0)]
        public string ServerName { get; set; }
        [PropertyOrder(1)]
        public SqlServerAuthenticationType AuthType { get; set; }
        [PropertyOrder(2)]
        public string UserName { get; set; }
        [PropertyOrder(4)]
        public string Password { get; set; }

        [PropertyOrder(5)]
        public bool UseSameCredentials { get; set; }
        [PropertyOrder(6)]
        public string WindowsUserName { get; set; }
        [PropertyOrder(7)]
        public string WindowsUserPassword { get; set; }





        private readonly List<string> _validationErrors  = new List<string>();
        private bool _validated = false;
        

        public bool HasErrors()
        {
            if (!_validated) Validate();
            return _validationErrors.Count > 0;
        }

       

        public string GetErrors()
        {
            return string.Join("\n", _validationErrors.ToArray());
        }

        private void Validate()
        {
            if (AuthType == SqlServerAuthenticationType.SqlServerAuthentication && UseSameCredentials)
                _validationErrors.Add("You can not use SqlServer Credentials for Windows Authentication");

            if (string.IsNullOrEmpty(ServerName)) _validationErrors.Add("Server name can not be empty");
            if (string.IsNullOrEmpty(UserName)) _validationErrors.Add("User name cannot be empty");
            if (string.IsNullOrEmpty(Password)) _validationErrors.Add("Password cannot be empty");


            _validated = true;
        }
    }
}
