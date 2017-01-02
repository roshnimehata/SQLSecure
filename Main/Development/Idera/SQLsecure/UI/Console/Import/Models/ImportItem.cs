using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.VisualStyles;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.UI.Console.Utility;
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

        private const string CannotUseCredentialsString = "You can not use SqlServer Credentials for Windows Authentication";
        private const string UnsupportedAuthTypeString = "Unsupported Authentication Type.";
        private const string ServerCannotBeEmptyString = "Server name can not be empty";
        private const string UserCannotBeEmptyString = "User name cannot be empty";
        private const string PasswordCannotBeEmptyString = "Password cannot be empty";
        private const string SpecifyUserString = "Please specify Windows User Name and Password";
        private readonly List<string> _validationErrors = new List<string>();
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
            if (!Enum.IsDefined(typeof(SqlServerAuthenticationType), AuthType)) _validationErrors.Add(UnsupportedAuthTypeString);
            if (string.IsNullOrEmpty(ServerName)) _validationErrors.Add(ServerCannotBeEmptyString);

            ValidateCredentials();


            _validated = true;
        }

        private void ValidateCredentials()
        {
            if (AuthType == SqlServerAuthenticationType.SqlServerAuthentication && UseSameCredentials)
                _validationErrors.Add(CannotUseCredentialsString);
            if (!IsWindowsCredentialsFormatValid()) _validationErrors.Add(ErrorMsgs.SqlLoginWindowsUserNotSpecifiedMsg);
            if (string.IsNullOrEmpty(UserName)) _validationErrors.Add(UserCannotBeEmptyString);
            if (string.IsNullOrEmpty(Password)) _validationErrors.Add(PasswordCannotBeEmptyString);

            var isPasswordLengthValid = PasswordValidator.ValidatePasswordLength(Password);
            if (!isPasswordLengthValid)
            {
                _validationErrors.Add(string.Format(Utility.Constants.PASSWORD_LENGTH_MESSAGE_FORMAT, Utility.Constants.MINIMUM_PASSWORD_LENGTH));
            }
        }


        private bool IsWindowsCredentialsFormatValid()
        {
            string domain, user;

            Path.SplitSamPath(UserName, out domain, out user);
            if (AuthType == SqlServerAuthenticationType.WindowsAuthentication &&
                (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(user))) return false;
            return true;
        }
    }
}
