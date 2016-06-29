using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.Cryptography;
using BBS.License;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.Core.Accounts
{
    public static class BBSLicenseConstants
    {
        public const string LicenseTypeProduction = "Production";
        public const string LicenseTypeTrial = "Trial";
        public const string LicenseTypeEnterprise = "Enterprise";
        public const string LicenseNoExpirationDate = "None";
        public const string LicenseExpired = "License Expired";
        public const string CombinedLicenses = "Resulting Combined Licenses";
        public const string CombinedLicensesMultiExpirationDates = "Multiple Expiration Dates";
        public const string CombinedLicensesMultiTypes = "Mixed Enterprise and {0}";
        public const string LicenseCountNA = "Not Applicable";
        public const string LicenseCountUnlimited = "Unlimited";

        public const int ExpirationDayToWarnProduction = 45;
        public const int ExpirationDayToWarnTrial = 7;

        public const string RegistryHiveLicensing = "IDEBT";

    }
 

    public class BBSProductLicense
    {
        #region Queries
        // Add
        public const string NonQueryAddLicense = @"SQLsecure.dbo.isp_sqlsecure_addlicense";
        public const string ParamLicenseKey = "@licensekey";
        // Get
        public const string ViewLicenses = @"SELECT licenseid, licensekey FROM SQLsecure.dbo.vwapplicationlicense";
        // Remove
        public const string NonQueryRemoveLicense = @"SQLsecure.dbo.isp_sqlsecure_removelicense";
        public const string ParamLicenseId = "@licenseid";

        enum SQLLicenseColumns
        {
            ColLicenseID,
            ColLicenseKey,
            ColCreatedTM,
            ColCreatedBy
        }


        #endregion


        #region Data Structs

        // Handy structure for passing information back to "Manage SQLsecure Licenses" form
        public struct LicenseData
        {
            public LicenseState licState;
            public int licenseRepositoryID;
            public string key;
            public int numLicensedServers;
            public bool isTrial;
            public string numLicensedServersStr;
            public string typeStr;
            public string forStr;
            public string expirationDateStr;
            public string daysToExpireStr;
            public int daysToExpire;
            public bool isAboutToExpire;

            public override string ToString()
            {
                return key;
            }
            public void Initialize()
            {
                licState = LicenseState.InvalidKey;
                numLicensedServers = 0;
                isTrial = true;
                licenseRepositoryID = 0;
                key = string.Empty;
                numLicensedServersStr = string.Empty;
                daysToExpireStr = string.Empty;
                expirationDateStr = string.Empty;
                forStr = string.Empty;
                typeStr = string.Empty;
                daysToExpire = 0;
                isAboutToExpire = false;
            }

        }

        // License error codes, allows UI to provide more meaningful messages
        public enum LicenseState
        {
            Valid,
            InvalidKey,
            InvalidProductID,
            InvalidScope,
            InvalidExpired,
            InvalidMixedTypes,
            InvalidDuplicateLicense,
            InvalidProductVersion
        }

        #endregion

        #region Fields
        private static LogX logX = new LogX("Idera.SQLsecure.Core.Accounts.BBSProductLicense");
        private List<LicenseData> m_Licenses;
        private LicenseData m_CombinedLicensedData;

        string  m_connectionString;
        string  m_scopeString;
        int     m_productID;
        Version m_productVersion;

        #endregion

        #region Properties
        public string OrginalScopeString
        {
            get { return m_scopeString; }
        }

        public List<BBSProductLicense.LicenseData> Licenses
        {
            get { return m_Licenses; }
        }

        public BBSProductLicense.LicenseData CombinedLicense
        {
            get { return m_CombinedLicensedData; }
        }

        #endregion

        #region CTOR

        public BBSProductLicense(string connectionString, string scope, int productID, string productVersion)
        {
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            Debug.Assert(!string.IsNullOrEmpty(scope));

            m_connectionString = connectionString;
            m_scopeString = scope.Split(',')[0];        // Strip off any port number for licensing
            m_productID = productID;
            m_productVersion = new Version(productVersion);

            LoadLicenses();
        }

        #endregion

        #region Public Methods

        // Check to see if SQLsecure is licensed
        // Returns the combined license results and list of all licenses;
        public bool IsProductLicensed()
        {
            bool bInitialized = false;
            bool bLicensed = false;
            m_CombinedLicensedData = new LicenseData();
            m_CombinedLicensedData.Initialize();

            foreach( LicenseData licData in m_Licenses )
            {
                if (!bInitialized)
                {
                    bLicensed = true;
                    m_CombinedLicensedData = licData;
                    bInitialized = true;
                }
                else
                {
                    m_CombinedLicensedData.key = BBSLicenseConstants.CombinedLicenses;
                    if (!licData.isTrial && licData.licState == LicenseState.Valid)
                    {
                        if (licData.numLicensedServers == -1)
                        {
                            m_CombinedLicensedData.numLicensedServers = -1;
                            m_CombinedLicensedData.numLicensedServersStr = CountToString(m_CombinedLicensedData.numLicensedServers);
                        }
                        else if (m_CombinedLicensedData.numLicensedServers != -1)
                        {
                            m_CombinedLicensedData.numLicensedServers += licData.numLicensedServers;
                            m_CombinedLicensedData.numLicensedServersStr = CountToString(m_CombinedLicensedData.numLicensedServers);
                        }
                    }
                    if (m_CombinedLicensedData.expirationDateStr != licData.expirationDateStr)
                    {
                        if(licData.isAboutToExpire)
                        {
                            m_CombinedLicensedData.isAboutToExpire = true;
                        }
                        m_CombinedLicensedData.expirationDateStr = BBSLicenseConstants.CombinedLicensesMultiExpirationDates;
                        m_CombinedLicensedData.daysToExpireStr = BBSLicenseConstants.CombinedLicensesMultiExpirationDates;
                    }
                    if (m_CombinedLicensedData.forStr != licData.forStr)
                    {
                        m_CombinedLicensedData.forStr = string.Format(BBSLicenseConstants.CombinedLicensesMultiTypes, m_scopeString);
                    }
                }
            }

            if (!bLicensed)
            {
                m_CombinedLicensedData.Initialize();
            }

            return bLicensed;
        }

        // Remove all license from the repository
        public void RemoveAllLicenses()
        {
            foreach (LicenseData licData in m_Licenses)
            {
                RemoveLicenseBatchMode(licData.licenseRepositoryID);
            }

            ReLoadLicenses();
        }

        public void RemoveLicense(int id)
        {
            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(m_connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup remove job params.
                    SqlParameter paramLicenseId = new SqlParameter(ParamLicenseId, id);

                    ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                    NonQueryRemoveLicense, new SqlParameter[] { paramLicenseId });

                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Removing License: ", ex.Message);
            }

            ReLoadLicenses();
        }

        // Add validated license to repository. 
        public void AddLicense(string licenseString)
        {
            LicenseState licState;

            if (IsLicenseStringValid(licenseString, out licState))
            {
                try
                {
                    // Open connection to repository and add server.
                    using (SqlConnection connection = new SqlConnection(m_connectionString))
                    {
                        // Open the connection.
                        connection.Open();

                        // Setup remove job params.
                        SqlParameter paramLicenseKey = new SqlParameter(ParamLicenseKey, licenseString);

                        ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                        NonQueryAddLicense, new SqlParameter[] { paramLicenseKey });

                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error("Error Adding License: ", ex.Message);
                }
            }
        }

        // Check if the given string is a valid trial license
        // UI calls this before accepting license string
        public bool IsLicenseStringTrial(string licenseStr)
        {
            bool bTrial = false;
            LicenseState licState = LicenseState.InvalidKey;

            if (!string.IsNullOrEmpty(licenseStr))
            {
                BBSLic lic = new BBSLic();
                LicErr rc = lic.LoadKeyString(licenseStr);
                if (rc == LicErr.OK)
                {
                    licState = IsLicenseValid(lic, licenseStr);
                    if (licState == LicenseState.Valid)
                    {
                        bTrial = lic.IsTrial;
                    }
                }
            }

            return bTrial;

        }

        // Check if the given string is a valid license
        // UI calls this before accepting license string
        public bool IsLicenseStringValid(string license, out LicenseState licState)
        {
            bool bValid = false;
            licState = LicenseState.InvalidKey;

            if (!string.IsNullOrEmpty(license))
            {
                BBSLic lic = new BBSLic();
                LicErr rc = lic.LoadKeyString(license);
                if (rc == LicErr.OK)
                {
                    licState = IsLicenseValid(lic, license);
                    bValid = licState == LicenseState.Valid ? true : false;
                }
            }

            return bValid;
        }

        // Generate a trial license
        public string GenerateTrialLicense()
        {
            BBSLic lic = new BBSLic();

            lic.IsTrial = true;
            lic.KeyID = 0;
            lic.DaysToExpiration = 14;
            lic.ProductID = (short)m_productID;
            lic.SetScopeHash(m_scopeString);
            lic.Limit1 = 10;
            lic.ProductVersion = m_productVersion;

//            string key = lic.KeyString;

            string key = lic.GetKeyString(PW());

            return key;
        }

        // Write to special registry location that trial has been used on this server
        public void TagTrialLicenseUsed()
        {

            RegistryKey hklm;
            RegistryKey hkSoftware = null;
            RegistryKey hkMicrosoft = null;
            RegistryKey hkLicensing = null;

            try
            {
                hklm = Registry.LocalMachine;
                hkSoftware = hklm.OpenSubKey("SOFTWARE");
                hkMicrosoft = hkSoftware.OpenSubKey("Microsoft", true);
                string key = BBSLicenseConstants.RegistryHiveLicensing + m_productID.ToString() + m_productVersion.ToString();
                hkLicensing = hkMicrosoft.OpenSubKey(key, true);
                if (hkLicensing == null)
                {
                    hkLicensing = hkMicrosoft.CreateSubKey(key);
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Tagging Trial license in Registry: ", ex.Message);
            }
            finally
            {
                if (hkSoftware != null) hkSoftware.Close();
                if (hkMicrosoft != null) hkMicrosoft.Close();
                if (hkLicensing != null) hkLicensing.Close();
            }
        }

        // Check special registry location to see if trial has been used for this server
        public bool HasTrialLicneseBeenUsed()
        {
            bool bUsed = true;
            RegistryKey hklm;
            RegistryKey hkSoftware = null;
            RegistryKey hkMicrosoft = null;
            RegistryKey hkLicensing = null;

            try
            {
                hklm = Registry.LocalMachine;
                hkSoftware = hklm.OpenSubKey("SOFTWARE");
                hkMicrosoft = hkSoftware.OpenSubKey("Microsoft", true);
                string key = BBSLicenseConstants.RegistryHiveLicensing + m_productID.ToString() + m_productVersion.ToString();
                hkLicensing = hkMicrosoft.OpenSubKey(key, true);
                if (hkLicensing == null)
                {
                    bUsed = false;
                }
            }
            catch
            {
                bUsed = true;
            }
            finally
            {
                if (hkSoftware != null) hkSoftware.Close();
                if (hkMicrosoft != null) hkMicrosoft.Close();
                if (hkLicensing != null) hkLicensing.Close();
            }
            return bUsed;
        }

        public bool IsLicneseGoodForServerCount(int serverCnt)
        {
            bool isOK = false;
            if (m_CombinedLicensedData.numLicensedServers == -1)
            {
                isOK = true;
            }
            else
            {
                isOK = (m_CombinedLicensedData.numLicensedServers >= serverCnt);
            }

            return isOK;
        }

        #endregion

        #region Helpers

        private static byte[] PW()
        {
            Process currentProcess = Process.GetCurrentProcess();
            string data = currentProcess.MachineName + currentProcess.Id;
            return BBSLic.GetHash(data);
        }


        // Returns display string for the scope
        // License is validated when class is instanciated.
        // Since license is valid then scope is either enterprise or our repository
        private string GetLicenseScopeStr(BBSLic bbsLic)
        {
            string scope = string.Empty;
            if (bbsLic != null)
            {
                if (bbsLic.IsEnterprise)
                {
                    scope = BBSLicenseConstants.LicenseTypeEnterprise;
                }
                else
                {
                    scope = m_scopeString;
                }
            }
            return scope;
        }

        // Returns display string for type (Production or Trial)
        private string GetLicenseTypeStr(BBSLic bbsLic)
        {
            string type = string.Empty;
            if (bbsLic != null)
            {
                if (bbsLic.IsTrial)
                {
                    type = BBSLicenseConstants.LicenseTypeTrial;
                }
                else
                {
                    type = BBSLicenseConstants.LicenseTypeProduction;
                }
            }
            return type;
        }

        // Returns display string for expiration date (None if no expiration)
        private string GetLicenseExpirationDateStr(BBSLic bbsLic)
        {
            string date = string.Empty;
            if (bbsLic != null)
            {
                if (bbsLic.IsPermanent)
                {
                    date = BBSLicenseConstants.LicenseNoExpirationDate;
                }
                else
                {
                    date = bbsLic.ExpirationDate.ToShortDateString();
                }
            }
            return date;
        }

        // Returns display string for days to expiration or None if no expiration date
        private string GetLicenseDaysToExpirationStr(BBSLic bbsLic)
        {
            string days = string.Empty;
            if (bbsLic != null)
            {
                if (bbsLic.IsPermanent)
                {
                    days = BBSLicenseConstants.LicenseNoExpirationDate;
                }
                else if (bbsLic.IsExpired)
                {
                    days = BBSLicenseConstants.LicenseExpired;
                }
                else
                {
                    days = bbsLic.DaysToExpiration.ToString();
                }
            }
            return days;
        }

        // Returns display string for number of licensed servers
        private string GetLicenseCountStr(BBSLic bbsLic)
        {
            string count = string.Empty;
            if (bbsLic != null)
            {
                count = CountToString(bbsLic.Limit1);
            }
            return count;
        }


        private int GetLicenseCount(BBSLic bbsLic)
        {
            int count = 0;
            if (bbsLic != null)
            {
                count = bbsLic.Limit1;
            }
            return count;
        }

        private bool IsLicensePermament(BBSLic bbsLic)
        {
            bool isPermament = false;
            if (bbsLic != null)
            {
                isPermament = bbsLic.IsPermanent;
            }
            return isPermament;

        }

        private bool IsLicenseTrial(BBSLic bbsLic)
        {
            bool isTrial = false;
            if (bbsLic != null)
            {
                isTrial = bbsLic.IsTrial;
            }
            return isTrial;
        }

        private void ReLoadLicenses()
        {
            m_Licenses.Clear();
            LoadLicenses();
        }

        // Get all licenses from repository
        private void LoadLicenses()
        {
            m_Licenses = new List<LicenseData>();
            try
            {

                using (SqlConnection connection = new SqlConnection(m_connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    //Get Schedule
                    SqlCommand cmd = new SqlCommand(ViewLicenses, connection);
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        int id = (int)dr[(int)SQLLicenseColumns.ColLicenseID];
                        string key = (string)dr[(int)SQLLicenseColumns.ColLicenseKey];

                        LicenseData licData;
                        FillLicenseData(key, out licData);
                        licData.licenseRepositoryID = id;
                        m_Licenses.Add(licData);
                    }
                }
            }

            catch (Exception ex)
            {
                logX.loggerX.Error("Error Loading License: ", ex.Message);
            }


        }

        // Remove specified license from repository, but don't refresh.
        // Refresh is done by calling code when all are removed.
        private void RemoveLicenseBatchMode(int id)
        {
            try
            {
                // Open connection to repository and add server.
                using (SqlConnection connection = new SqlConnection(m_connectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup remove job params.
                    SqlParameter paramLicenseId = new SqlParameter(ParamLicenseId, id);

                    ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                    NonQueryRemoveLicense, new SqlParameter[] { paramLicenseId });

                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Removing License: ", ex.Message);
            }

        }

        // Fill the LicenseData structure with information about this license key
        private void FillLicenseData(string licenseKey, out LicenseData licData)
        {
            licData = new LicenseData();
            if (!string.IsNullOrEmpty(licenseKey))
            {
                BBSLic bbsLic;
                licData.Initialize();
                licData.licState = LoadAndValidateLicense(licenseKey, out bbsLic);
                if (bbsLic != null)
                {
                    licData.isTrial = IsLicenseTrial(bbsLic);
                    licData.key = licenseKey;
                    licData.numLicensedServers = GetLicenseCount(bbsLic);
                    licData.numLicensedServersStr = CountToString(licData.numLicensedServers);
                    licData.licenseRepositoryID = 0;
                    licData.forStr = GetLicenseScopeStr(bbsLic);
                    licData.typeStr = GetLicenseTypeStr(bbsLic);
                    licData.expirationDateStr = GetLicenseExpirationDateStr(bbsLic);
                    licData.daysToExpireStr = GetLicenseDaysToExpirationStr(bbsLic);
                    licData.daysToExpire = bbsLic.DaysToExpiration;
                    if (licData.typeStr == BBSLicenseConstants.LicenseTypeProduction && licData.daysToExpire <= BBSLicenseConstants.ExpirationDayToWarnProduction ||
                        licData.typeStr == BBSLicenseConstants.LicenseTypeTrial && licData.daysToExpire <= BBSLicenseConstants.ExpirationDayToWarnTrial)
                    {
                        licData.isAboutToExpire = true;
                    }
                    else
                    {
                        licData.isAboutToExpire = false;
                    }

                }
            }
        }

        // Checks if license is valid for SQLsecure
        // Checks ProductID, Version, Scope, Expiration, Is Duplicate
        private LicenseState IsLicenseValid(BBSLic lic, string licenseKey)
        {
            LicenseState licState = LicenseState.InvalidKey;

            if (lic != null)
            {
                while (licState == LicenseState.InvalidKey)
                {
                    licState = LicenseState.Valid;

                    // Is the product ID for SQLsecure
                    if (!IsLicenseProductIDValid(lic))
                    {
                        licState = LicenseState.InvalidProductID;
                        break;
                    }
                    // Is this for correct version
                    if( !IsLicenseVersionValid(lic) )
                    {
                        licState = LicenseState.InvalidProductVersion;
                        break;
                    }
                    // Is it registered for this repository or enterprise
                    if (!IsLicenseScopeValid(lic))
                    {
                        licState = LicenseState.InvalidScope;
                        break;
                    }
                    // Is license expired
                    if (lic.IsExpired)
                    {
                        licState = LicenseState.InvalidExpired;
                        break;
                    }
                    // Does it already exist
                    bool bDuplicate = false;
                    foreach (LicenseData licData2 in m_Licenses)
                    {
                        if (licData2.key == licenseKey)
                        {
                            bDuplicate = true;
                            break;
                        }
                    }
                    if (bDuplicate)
                    {
                        licState = LicenseState.InvalidDuplicateLicense;
                    }

                    if(!IsLicenseReasonable(lic))
                    {
                        licState = LicenseState.InvalidKey;
                        break;
                    }
                }
            }

            return licState;
        }

        // Is the scope hash valid for our repository
        private bool IsLicenseScopeValid(BBSLic lic)
        {
            bool bValid = false;
            if (lic != null)
            {
                if (lic.IsEnterprise || lic.CheckScopeHash(m_scopeString))
                {
                    bValid = true;
                }
            }
            return bValid;
        }

        // Is the ProductID valid for SQLsecure
        private bool IsLicenseProductIDValid(BBSLic lic)
        {
            bool bValid = false;
            if (lic != null)
            {
                if (lic.ProductID == m_productID)
                {
                    bValid = true;
                }
            }
            return bValid;
        }

        // Is the Product Version valid for SQLsecure
        private bool IsLicenseVersionValid(BBSLic lic)
        {
            return true;
            //bool bValid = false;
            //if (lic != null)
            //{
            //    if (lic.ProductVersion >= m_productVersion)
            //    {
            //        bValid = true;
            //    }
            //}
            //return bValid;
        }

        private string CountToString(int count)
        {
            if (count == BBSLic.NotApplicable)
            {
                return BBSLicenseConstants.LicenseCountNA;
            }
            else if (count == BBSLic.Unlimited)
            {
                return BBSLicenseConstants.LicenseCountUnlimited;
            }
            else
            {
                return count.ToString();
            }
        }

   
        private LicenseState LoadAndValidateLicense(string license, out BBSLic bbsLic)
        {
            LicenseState licState = LicenseState.InvalidKey;
            bbsLic = new BBSLic();
            LicErr rc = bbsLic.LoadKeyString(license);
            if (rc == LicErr.OK)
            {
                licState = IsLicenseValid(bbsLic, license);
            }

            return licState;
        }

        //---------------------------------------------------------------------
        // IsLicenseReasonable - Our license key checksum is not solid so
        //                       you can change characters in the key and
        //                       still have a valid license. This could allow
        //                       a customer to bump up their license cound.
        //                       However the changes always create unresonable 
        //                       licenses like 1000s of seats. To avoid
        //                       problems of upgrading license DLL we just are
        //                       putting in a reasonableness check in the products
        //---------------------------------------------------------------------
        private bool IsLicenseReasonable(BBSLic license)
        {
            // Trials only valid for 0-90 days
            if (license.IsTrial)
            {
                if (license.IsPermanent) return false;
                if (license.DaysToExpiration < 0) return false;
                if (license.DaysToExpiration > 90) return false;
            }
            else // Purchase license only valid for 0-400 days or unlimited
            {
                if (license.DaysToExpiration < 0) return false;
                if (license.DaysToExpiration > 400 && license.DaysToExpiration != 32767) return false;
            }

            // License only good for up to 500 licenses
            if (license.Limit1 < -1) return false;
            if (license.Limit1 > 500) return false;
            if (license.Limit2 < -2 || license.Limit2 > 1) return false; // some products code limit 2 as 1 instead of unlimited

            return true;
        }



        #region SQLHelper

        private static int ExecuteNonQuery(
                 SqlConnection connection,
                 CommandType commandType,
                 string commandText,
                 params SqlParameter[] commandParameters
             )
        {
            Debug.Assert(connection != null);

            // Create a command and prepare it for execution
            int retval = 0;
            using (SqlCommand cmd = new SqlCommand())
            {
                // Prepare and execute the command.
                try
                {
                    // Create the command object.
                    prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

                    // Execute the command
                    retval = cmd.ExecuteNonQuery();

                    // Detach the SqlParameters from the command object, so they can be used again
                    // Detach the SqlParameters from the command object, so they can be used again.
                    // HACK: There is a problem here, the output parameter values are fletched 
                    // when the reader is closed, so if the parameters are detached from the command
                    // then the SqlReader can´t set its values. 
                    // When this happen, the parameters can´t be used again in other command.
                    bool canClear = true;
                    foreach (SqlParameter commandParameter in cmd.Parameters)
                    {
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }

                    if (canClear)
                    {
                        cmd.Parameters.Clear();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }

                return retval;
            }
        }

        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command
        /// </summary>
        /// <param name="command">The SqlCommand to be prepared</param>
        /// <param name="connection">A valid SqlConnection, on which to execute this command</param>
        /// <param name="transaction">A valid SqlTransaction, or 'null'</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        private static void prepareCommand(
                SqlCommand command,
                SqlConnection connection,
                SqlTransaction transaction,
                CommandType commandType,
                string commandText,
                SqlParameter[] commandParameters
            )
        {
            Debug.Assert(command != null);
            Debug.Assert(commandText != null || commandText.Length != 0);
            Debug.Assert(connection.State == ConnectionState.Open);
            Debug.Assert((transaction == null) ? true : transaction.Connection != null);

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                attachParameters(command, commandParameters);
            }
        }

        /// <summary>
        /// This method is used to attach array of SqlParameters to a SqlCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of SqlParameters to be added to command</param>
        private static void attachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            Debug.Assert(command != null);

            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        #endregion

        #endregion

    }
}
