using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using BBS.License;

namespace Idera.SQLsecure.UI.Console.Utility
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
        private List<LicenseData> m_Licenses;
        private LicenseData m_CombinedLicensedData;

        string  m_connectionString;
        string  m_scopeString;
        int     m_productID;
        Version m_productVersion;

        #endregion

        #region Properties

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
            m_scopeString = scope;
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
                        m_CombinedLicensedData.numLicensedServers += licData.numLicensedServers;                            
                    }
                    if (m_CombinedLicensedData.expirationDateStr != licData.expirationDateStr)
                    {
                        if(!licData.isAboutToExpire)
                        {
                            m_CombinedLicensedData.isAboutToExpire = false;
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

                    Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                    NonQueryRemoveLicense, new SqlParameter[] { paramLicenseId });

                }
            }
            catch (Exception ex)
            {

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

                        Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                        NonQueryAddLicense, new SqlParameter[] { paramLicenseKey });

                    }
                }
                catch (Exception ex)
                {

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
                    licState = IsLicenseValid(lic);
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
                    licState = IsLicenseValid(lic);
                    bValid = licState == LicenseState.Valid ? true : false;
                }
            }

            return bValid;
        }

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

            string key = lic.KeyString;

            return key;
        }

        #endregion

        #region Helpers

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

            }


        }

        // Remove specified license from repository
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

                    Sql.SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                    NonQueryRemoveLicense, new SqlParameter[] { paramLicenseId });

                }
            }
            catch (Exception ex)
            {

            }

        }

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
                    licData.key = bbsLic.KeyString;
                    licData.numLicensedServers = GetLicenseCount(bbsLic);
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
        // Checks ProductID, Scope, Expiration, Is Duplicate
        private LicenseState IsLicenseValid(BBSLic lic)
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
                        if (licData2.key == lic.KeyString)
                        {
                            bDuplicate = true;
                            break;
                        }
                    }
                    if (bDuplicate)
                    {
                        licState = LicenseState.InvalidDuplicateLicense;
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

        // Is the ProductID valid for SQLsecure
        private bool IsLicenseVersionValid(BBSLic lic)
        {
            bool bValid = false;
            if (lic != null)
            {
                if (lic.ProductVersion == m_productVersion)
                {
                    bValid = true;
                }
            }
            return bValid;
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
            bool bValid = false;
            LicenseState licState = LicenseState.InvalidKey;
            bbsLic = new BBSLic();
            LicErr rc = bbsLic.LoadKeyString(license);
            if (rc == LicErr.OK)
            {
                licState = IsLicenseValid(bbsLic);
            }

            return licState;
        }

        #endregion

    }
}
