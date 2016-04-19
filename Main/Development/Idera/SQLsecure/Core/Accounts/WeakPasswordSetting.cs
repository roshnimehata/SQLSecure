using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using Idera.SQLsecure.Core.Logger;
using System.Text;

namespace Idera.SQLsecure.Core.Accounts
{
    public class WeakPasswordSetting
    {

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.Core.Accounts.WeakPasswordSetting");
        private int passwordListId;
        private List<string> customPasswordList;
        private DateTime customListUpdated;
        private List<string> additionalPasswordList;
        private DateTime additionalListUpdated;
        private bool passwordCheckingEnabled;

        #endregion

        #region Queries

        private const string QueryGetPasswordList = @"SQLsecure.dbo.isp_sqlsecure_getweakpasswordlist";
        private const string QueryUpdatePasswordList = @"SQLsecure.dbo.isp_sqlsecure_updateweakpasswordlist";
        private const string ParamPasswordListId = @"@passwordListId";
        private const string ParamCustomPasswordList = @"@customPasswordList";
        private const string ParamCustomListUpdated = @"@customListUpdated";
        private const string ParamAdditionalPasswordList = @"@additionalPasswordList";
        private const string ParamAdditionListUpdated = @"@additionalListUpdated";
        private const string ParamPasswordCheckingEnabled = @"@passwordCheckingEnabled";

        private enum WeakPasswordColumns
        {
            Id = 0,
            CustomList,
            CustonListUpdated,
            AdditionalList,
            AdditionListUpdated,
            PasswordCheckingEnabled
        }

        #endregion

        #region Ctors

        WeakPasswordSetting(int passwordListId,
                            List<string> customPasswordList,
                            DateTime customListUpdated,
                            List<string> additionalPasswordList,
                            DateTime additionalListUpdated,
                            bool passwordCheckingEnabled)
        {
            this.passwordListId = passwordListId;
            this.customPasswordList = customPasswordList;
            this.customListUpdated = customListUpdated;
            this.additionalPasswordList = additionalPasswordList;
            this.additionalListUpdated = additionalListUpdated;
            this.passwordCheckingEnabled = passwordCheckingEnabled;
        }

        #endregion

        #region Properties

        public int PasswordListId
        {
            get { return passwordListId; }
        }

        public List<string> CustomPasswordList
        {
            get { return customPasswordList; }
        }

        public DateTime CustomListUpdated
        {
            get { return customListUpdated; }
            set { customListUpdated = value; }
        }

        public List<string> AdditionalPasswordList
        {
            get { return additionalPasswordList; }
        }

        public DateTime AdditionalListUpdated
        {
            get { return additionalListUpdated; }
            set { additionalListUpdated = value; }
        }

        public bool PasswordCheckingEnabled
        {
            get { return passwordCheckingEnabled; }
            set { passwordCheckingEnabled = value; }
        }

        #endregion

        #region Methods

        public static void UpdateWeakPasswordSettings(WeakPasswordSetting weakPasswordSettings, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    WeakPasswordSetting.UpdateWeakPasswordSettings(weakPasswordSettings, connection);
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when connecting to update weak password lists, ", ex.ToString());
            }
        }

        public static List<WeakPasswordSetting> UpdateWeakPasswordSettings(WeakPasswordSetting weakPasswordSettings, SqlConnection connection)
        {
            List<WeakPasswordSetting> settings = new List<WeakPasswordSetting>();

            try
            {
                using (SqlCommand cmd = new SqlCommand(QueryUpdatePasswordList, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

             
                    SqlParameter passwordListId = new SqlParameter(ParamPasswordListId, weakPasswordSettings.PasswordListId);
                    SqlParameter customPasswordList;
                    SqlParameter customListUpdated;
                    SqlParameter additionalPasswordList;
                    SqlParameter additionalListUpdated;

                    string passwordList;

                    //only set the custom list if it has been changed
                    if (weakPasswordSettings.CustomListUpdated != DateTime.MinValue)
                    {
                        passwordList = WeakPasswordSetting.ConvertListToString(weakPasswordSettings.CustomPasswordList);
                        //an empty list shoud be set to null since empty means blank
                        if (String.IsNullOrEmpty(passwordList))
                            customPasswordList = new SqlParameter(ParamCustomPasswordList, DBNull.Value);
                        else
                            customPasswordList = new SqlParameter(ParamCustomPasswordList, passwordList);
                        customListUpdated = new SqlParameter(ParamCustomListUpdated, weakPasswordSettings.CustomListUpdated);
                    }
                    else
                    {
                        customPasswordList = new SqlParameter(ParamCustomPasswordList, DBNull.Value);
                        customListUpdated = new SqlParameter(ParamCustomListUpdated, DBNull.Value);
                    }


                    //only set the additional list if it has been changed
                    if (weakPasswordSettings.AdditionalListUpdated != DateTime.MinValue)
                    {
                        passwordList = WeakPasswordSetting.ConvertListToString(weakPasswordSettings.AdditionalPasswordList);
                        //an empty list shoud be set to null since empty means blank
                        if (String.IsNullOrEmpty(passwordList))
                            additionalPasswordList = new SqlParameter(ParamAdditionalPasswordList, DBNull.Value);
                        else
                            additionalPasswordList = new SqlParameter(ParamAdditionalPasswordList, passwordList);
                        additionalListUpdated = new SqlParameter(ParamAdditionListUpdated, weakPasswordSettings.AdditionalListUpdated);
                    }
                    else
                    {
                        additionalPasswordList = new SqlParameter(ParamAdditionalPasswordList, DBNull.Value);
                        additionalListUpdated = new SqlParameter(ParamAdditionListUpdated, DBNull.Value);
                    }
                    SqlParameter passwordCheckingEnabled = new SqlParameter(ParamPasswordCheckingEnabled, (weakPasswordSettings.PasswordCheckingEnabled ? 'Y' : 'N'));

                    cmd.Parameters.Add(passwordListId);
                    cmd.Parameters.Add(customPasswordList);
                    cmd.Parameters.Add(customListUpdated);
                    cmd.Parameters.Add(additionalPasswordList);
                    cmd.Parameters.Add(additionalListUpdated);
                    cmd.Parameters.Add(passwordCheckingEnabled);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when updating weak password lists, ", ex.ToString());
            }
            return settings;
        }

        public static List<WeakPasswordSetting> GetWeakPasswordSettings(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return WeakPasswordSetting.GetWeakPasswordSettings(connection);
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when connecting to read weak password lists, ", ex.ToString());
                return new List<WeakPasswordSetting>();
            }
        }

        public static List<WeakPasswordSetting> GetWeakPasswordSettings(SqlConnection connection)
        {
            List<WeakPasswordSetting> settings = new List<WeakPasswordSetting>();

            try
            {
                using (SqlCommand cmd = new SqlCommand(QueryGetPasswordList, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //pass null to get all the entire list
                    SqlParameter passwordListId = new SqlParameter(ParamPasswordListId, DBNull.Value);
                    cmd.Parameters.Add(passwordListId);

                    // Read info from the repository.
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int listId = WeakPasswordSetting.GetId(rdr.GetSqlInt32((int)WeakPasswordColumns.Id));
                            List<string> customList = WeakPasswordSetting.GetPasswordList(rdr.GetSqlString((int)WeakPasswordColumns.CustomList));
                            DateTime customUpdated = WeakPasswordSetting.GetDateTime(rdr.GetSqlDateTime((int)WeakPasswordColumns.CustonListUpdated));
                            List<string> additionalList = WeakPasswordSetting.GetPasswordList(rdr.GetSqlString((int)WeakPasswordColumns.AdditionalList));
                            DateTime additionalUpdate = WeakPasswordSetting.GetDateTime(rdr.GetSqlDateTime((int)WeakPasswordColumns.AdditionListUpdated));
                            bool enabled = WeakPasswordSetting.GetBoolean(rdr.GetSqlString((int)WeakPasswordColumns.PasswordCheckingEnabled));

                            WeakPasswordSetting setting = new WeakPasswordSetting(listId, customList, customUpdated, additionalList, additionalUpdate, enabled);
                            settings.Add(setting);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("ERROR - exception raised when reading weak password lists, ", ex.ToString());
            }

            if (settings.Count == 0)
            {
                //since there is nothing in the repository, create an empy object.
                WeakPasswordSetting passwordSettings = new WeakPasswordSetting(-1, new List<string>(), DateTime.MinValue, new List<string>(), DateTime.MinValue, true);
                settings.Add(passwordSettings);
            }
            return settings;
        }

        private static List<string> GetPasswordList(SqlString passwordList)
        {
            List<string> list = new List<string>();

            if (!passwordList.IsNull)
            {
                list.AddRange(((string)passwordList.Value).Split(new char[]{';'}));
            }
            return list;
        }

        private static DateTime GetDateTime(SqlDateTime date)
        {
            if (date.IsNull)
                return DateTime.MinValue;
            return ((DateTime)date.Value);
        }

        private static int GetId(SqlInt32 id)
        {
            if (id.IsNull)
                return -1;
            return (int)id.Value;              
        }

        private static bool GetBoolean(SqlString boolean)
        {
            if (!boolean.IsNull)
                return (((string)boolean.Value) == "Y" ? true : false);
            return false;
        }

        public static string ConvertListToString(List<string> list)
        {
            StringBuilder delimitedString = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                {
                    delimitedString.Append(";");
                }
                delimitedString.Append(list[i]);
            }
            return delimitedString.ToString();
        }
        #endregion

    }
}
