/******************************************************************
 * Name: UserData.cs
 *
 * Description: This class encapsulates user UI options.   It has
 * support for persisting the information to an xml file.
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Win32;

namespace Idera.SQLsecure.UI.Console.Utility
{
    public class UserData
    {
        #region Types
            #region Repository
            public class RepositoryData
            {
                #region Fields
                String m_ServerName;
            string userName;
            string password;
            string authenticationMode;//SQLsecure 3.1 (Tushar)--Supporting windows auth for repository connection
                #endregion

                #region Ctors
                public RepositoryData()
                {
                    // get the default value from the registry
                    try
                    {
                        m_ServerName = Registry.GetValue(Utility.Constants.REGISTRY_PATH, Utility.Constants.RegKey_DefaultRepository, Utility.Constants.RepositoryServerDefault).ToString();
                    }
                    catch
                    {
                        //make sure there is always a value
                        m_ServerName = string.Empty; 
                    }
                }
                #endregion

                #region Properties
                public String ServerName
                {
                    get { return m_ServerName; }
                    set { m_ServerName = value; }
                }
            public string UserName
            {
                get { return userName; }
                set { userName = value; }
            }
            public string Password
            {
                get { return password; }
                set { password = value; }
            }
            //Start-SQLsecure 3.1 (Tushar)--Supporting windows auth for repository connection
            public string AuthenticationMode
            {
                get { return authenticationMode; }
                set { authenticationMode = value;}
            }
            //End-SQLsecure 3.1 (Tushar)--Supporting windows auth for repository connection
            #endregion
        }
        #endregion

            #region MainFormData
            public class MainFormData
            {
                #region Fields
                Size m_Size;
                FormStartPosition m_StartPosition;
                Point m_Location;

                Point m_MenuStripLocation;
                Point m_ToolStripLocation;
                int m_SplitterDistance;
                FormWindowState m_WindowState;
                #endregion

                #region Ctors
                public MainFormData()
                {
                    // Create the fields.
                    m_Size = new Size(900,700);
                    m_StartPosition = FormStartPosition.WindowsDefaultLocation;
                    m_Location = new Point(0,0);
                    m_WindowState = FormWindowState.Normal;
                    m_MenuStripLocation = new Point(0,0);
                    m_ToolStripLocation = new Point(3,24);
                    m_SplitterDistance = 205;
                }
                #endregion

                #region Properties
                public Size Size
                {
                    get { return m_Size; }
                    set { m_Size = value; }
                }
                public FormStartPosition StartPosition
                {
                    get { return m_StartPosition; }
                    set { m_StartPosition = value; }
                }
                public Point Location
                {
                    get { return m_Location; }
                    set { m_Location = value; }
                }
                public FormWindowState WindowState
                {
                    get { return m_WindowState; }
                    set { m_WindowState = value; }
                }
                public Point MenuStripLocation
                {
                    get { return m_MenuStripLocation; }
                    set { m_MenuStripLocation = value; }
                }
                public Point ToolStripLocation
                {
                    get { return m_ToolStripLocation; }
                    set { m_ToolStripLocation = value; }
                }
                public int SplitterDistance
                {
                    get { return m_SplitterDistance; }
                    set { m_SplitterDistance = value; }
                }
                #endregion
            }
            #endregion

            #region ViewData
            public class ViewData
            {
                #region Fields
                private bool m_TaskPanelVisible;
                #endregion

                #region Property Change Notification Events
                public delegate void ViewDataEventHandler();
                private event ViewDataEventHandler m_ViewDataEvent;
                public event ViewDataEventHandler ViewDataEvent
                {
                    add { m_ViewDataEvent += value; }
                    remove { m_ViewDataEvent -= value; }
                }
                #endregion

                #region Ctors
                public ViewData()
                {
                    m_TaskPanelVisible = true;
                }
                #endregion

                #region Properties
                public bool TaskPanelVisible
                {
                    get { return m_TaskPanelVisible; }
                    set 
                    { 
                        m_TaskPanelVisible = value;
                        if (m_ViewDataEvent != null) { m_ViewDataEvent(); }
                    }
                }
                #endregion
            }
            #endregion

            #region ObjectPermissionsGrid
            public class ObjectPermissionsGridData
            {
                #region Fields
                private bool m_IsIconHidden = false;
                private bool m_IsPermissionHidden = false;
                private bool m_IsGranteeHidden = false;
                private bool m_IsGrantCheckBoxHidden = false;
                private bool m_IsWithGrantCheckBoxHidden = false;
                private bool m_IsDenyCheckBoxHidden = false;
                private bool m_IsGrantorHidden = false;
                private bool m_IsObjectNameHidden = false;
                private bool m_IsObjPermObjectNameHidden = true;
                private bool m_IsSourceNameHidden = false;
                private bool m_IsSourceTypeHidden = false;
                private bool m_IsSourcePermissionHidden = false;
                private bool m_IsObjectTypeNameHidden = false;
                #endregion

                #region Ctor
                public ObjectPermissionsGridData()
                {
                }

                #endregion

                #region Properties

                public bool IsIconHidden
                {
                    get { return m_IsIconHidden; }
                    set { m_IsIconHidden = value; }
                }
                public bool IsPermissionHidden
                {
                    get { return m_IsPermissionHidden; }
                    set { m_IsPermissionHidden = value; }
                }
                public bool IsGranteeHidden
                {
                    get { return m_IsGranteeHidden; }
                    set { m_IsGranteeHidden = value; }
                }
                public bool IsGrantCheckBoxHidden
                {
                    get { return m_IsGrantCheckBoxHidden; }
                    set { m_IsGrantCheckBoxHidden = value; }
                }
                public bool IsWIthGrantCheckBoxHidden
                {
                    get { return m_IsWithGrantCheckBoxHidden; }
                    set { m_IsWithGrantCheckBoxHidden = value; }
                }
                public bool IsDenyCheckBoxHidden
                {
                    get { return m_IsDenyCheckBoxHidden; }
                    set { m_IsDenyCheckBoxHidden = value; }
                }
                public bool IsGrantorHidden
                {
                    get { return m_IsGrantorHidden; }
                    set { m_IsGrantorHidden = value; }
                }
                public bool IsObjectNameHidden
                {
                    get { return m_IsObjectNameHidden; }
                    set { m_IsObjectNameHidden = value; }
                }
                public bool IsObjPermObjectNameHidden
                {
                    get { return m_IsObjPermObjectNameHidden; }
                    set { m_IsObjPermObjectNameHidden = value; }
                }
                public bool IsSourceNameHidden
                {
                    get { return m_IsSourceNameHidden; }
                    set { m_IsSourceNameHidden = value; }
                }
                public bool IsSourceTypeHidden
                {
                    get { return m_IsSourceTypeHidden; }
                    set { m_IsSourceTypeHidden = value; }
                }
                public bool IsSourcePermissionHidden
                {
                    get { return m_IsSourcePermissionHidden; }
                    set { m_IsSourcePermissionHidden = value; }
                }
                public bool IsObjectTypeNameHidden
                {
                    get { return m_IsObjectTypeNameHidden; }
                    set { m_IsObjectTypeNameHidden = value; }
                }
                #endregion

            }
            #endregion
        #endregion

        #region Helpers
        /// <summary>
        /// Get the options file path.
        /// </summary>
        private static String optionsPath
        {
            get
            {
                // get the path to the current version
                return getOptionsPath(Constants.PRODUCT_VER_STR);
            }
        }
        /// <summary>
        /// Get the option file path for the specified product version
        /// </summary>
        /// <param name="ver">The version of the product to create the path for</param>
        /// <returns></returns>
        private static String getOptionsPath(string ver)
        {
            // Build the directory path.
            StringBuilder path = new StringBuilder();
            path.Append(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            path.Append(Path.DirectorySeparatorChar);
            path.Append(Constants.COMPANY_STR);
            path.Append(Path.DirectorySeparatorChar);
            path.Append(Constants.PRODUCT_STR);
            path.Append(Path.DirectorySeparatorChar);
            path.Append(ver);

            // If the dir does not exist, create it.
            String dir = path.ToString();
            if (!Directory.Exists(dir))
            {
                lock (typeof(UserData))
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }

            // Add file name to the path.
            path.Append(Path.DirectorySeparatorChar);
            path.Append(Path.GetFileName(Constants.COMPONENT_STR));
            path.Append(Constants.OPTIONS_FILE_EXTENSION_STR);

            return path.ToString();
        }
        #endregion

        #region Ctors
        /// <summary>
        /// Private ctor for singleton support.
        /// </summary>
        private UserData()
        {
            RepositoryInfo = new RepositoryData();
            MainForm = new MainFormData();
            View = new ViewData();
            ObjectPermissions = new ObjectPermissionsGridData();
        }
        #endregion

        #region Singleton
        /// <summary>
        /// Static singleton instance.
        /// </summary>
        private static UserData m_Current = null;
        /// <summary>
        /// Loads user options from xml file, if the file
        /// does not exist a new instance is created.
        /// </summary>
        /// <returns>UserData instance</returns>
        private static UserData load()
        {
            UserData retObj = null;

            // If no options file, search for previous version or
            // create a new instance of the options object.
            if (!File.Exists(optionsPath))
            {
                foreach (string ver in Utility.Constants.PRODUCT_VER_STR_PREV)
                {
                    if (File.Exists(getOptionsPath(ver)))
                    {
                        retObj = readfile(getOptionsPath(ver));
                    }
                }
                if (retObj == null)
                {
                    retObj = new UserData();
                }
            }
            else // Load it from the options file.
            {
                retObj = readfile(optionsPath);
            }

            //Decrypting password.
            if(!string.IsNullOrEmpty(retObj.RepositoryInfo.Password))
                retObj.RepositoryInfo.Password = Idera.SQLsecure.Core.Accounts.Encryptor.Decrypt(retObj.RepositoryInfo.Password);

            return retObj;
        }
        /// <summary>
        /// Read the Xml options file from the specified path
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>a UserData object containing the last saved options
        /// or null if the object cannot be created from the file
        /// </returns>
        private static UserData readfile(string path)
        {
            UserData retObj = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UserData));
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ConformanceLevel = ConformanceLevel.Fragment;
                    settings.CloseInput =
                        settings.IgnoreComments =
                        settings.IgnoreWhitespace = true;
                    XmlReader reader = XmlTextReader.Create(stream, settings);
                    retObj = (UserData)serializer.Deserialize(reader);
                }
            }
            catch { }

            return retObj;
        }

        /// <summary>
        /// Get the UserData singleton object.
        /// </summary>
        public static UserData Current
        {
            get
            {
                if (m_Current == null)
                {
                    lock (typeof(UserData))
                    {
                        if (m_Current == null)
                        {
                            m_Current = load();
                        }
                    }
                }
                return m_Current;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method saves the user options object instance
        /// to the xml file.
        /// </summary>
        public void Save()
        {
            //Encrypting password before saving settings to config.
            this.RepositoryInfo.Password = Idera.SQLsecure.Core.Accounts.Encryptor.Encrypt(this.RepositoryInfo.Password);

            XmlSerializer serializer = new XmlSerializer(typeof(UserData));
            using (StreamWriter writer = new StreamWriter(optionsPath))
            {
                serializer.Serialize(writer, this);
                writer.Close();
            }
        }
        #endregion

        #region User data fields
        public RepositoryData RepositoryInfo;
        public MainFormData MainForm;
        public ViewData View;
        public ObjectPermissionsGridData ObjectPermissions;
        #endregion
    }
}
