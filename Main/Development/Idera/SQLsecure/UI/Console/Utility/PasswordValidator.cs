using System;
using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Utility
{
    public class PasswordValidator
    {
        public static bool ValidatePasswordLength(string password)
        {
            if (!string.IsNullOrEmpty(password) &&
                password.Length < Constants.MINIMUM_PASSWORD_LENGTH)
            {
                return false;
            }
            return true;
        }
    }
}
