using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Idera.SQLsecure.UI.Console.Sql;
using Infragistics.Win.UltraWinListView;
using Idera.SQLsecure.UI.Console.Data;

namespace Idera.SQLsecure.UI.Console.Controls
{    
   
    public partial class FilterSelection : UserControl
    {
        #region Fields

        private bool m_isDirty = false;
        private bool m_isInitialized = false;
        private bool m_isListViewUpdating = false;
        private ServerInfo m_ServerInfo = null;

        private DataCollectionFilter m_Filter;
        private Dictionary<RuleObjectType, FilterObject> m_FilterObjects;

        private const string RuleHeader = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\froman\fprq2\fcharset2 Symbol;}{\f2\fmodern\fprq1\fcharset0 Courier New;}{\f3\fswiss\fcharset0 Arial;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs16 Collect SQL Server permissions for: \par";
        private const string AlwaysCollected = @"\pard\fi-360\li720\tx720\f1\'b7\    \f0  All Server objects, Database Security objects, Stored Procedures and Extended Stored Procedures \par";
        private const string DatabasePrefix = @"\pard\fi-360\li720\tx720\f1\'b7\    \f0 ";
        private const string RuleFooter = @"\pard\f3\par}";

        private const string RulePrefix = @"\pard\fi-360\li720\tx720\f2\  o\  \f0 ";
        private const string RuleSuffix = @"\par";

        #endregion

        #region CTOR
        public FilterSelection()
        {
            m_isListViewUpdating = false;
            m_isInitialized = false;

            InitializeComponent();
        }
        #endregion

        #region Public Methods
        public void Initialize(DataCollectionFilter filter, ServerInfo serverInfo)
        {

            m_ServerInfo = serverInfo;

            CreateFilterObjects();

            if (filter != null)
            {
                m_Filter = filter;
                FillFilterObjects();
            }
            else
            {
                m_Filter = new DataCollectionFilter("temp", "temp", "temp");
            }
            DisplayFilterObjects(false);

            if (!Program.gController.isAdmin)
            {
                Enabled = false;
            }

        }

        public bool GetFilter(out DataCollectionFilter filter)
        {
            BuildFilterRules();

            filter = m_Filter;

            return m_isDirty;
        }

        #endregion

        #region Events

        private void FilterSelection_Load(object sender, EventArgs e)
        {
            if (m_FilterObjects != null)
            {
                DisplayFilterObjects(false);
            }
        }
        private void ultraListViewFilters_Enter(object sender, EventArgs e)
        {
            m_isInitialized = true;
        }

        private void ultraListViewFilters_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UltraListViewSubItem item;
                item = ultraListViewFilters.SubItemFromPoint(e.Location);
                if (item != null)
                {
                    if(item.Key == "Scope" && !string.IsNullOrEmpty(item.Text))
                    {
                        FilterObject filterObject;
                        filterObject = (FilterObject)item.Item.Tag;
                        if (filterObject.IsConfigurable)
                        {
                            if (m_FilterObjects.TryGetValue(filterObject.ObjectType, out filterObject))
                            {
                                contextMenuStripType.ShowImageMargin = false;
                                contextMenuStripType.Tag = filterObject;
                                contextMenuStripType.Show(ultraListViewFilters.PointToScreen(e.Location));
                            }
                        }
                    }
                    else if (item.Key == "Name" && !string.IsNullOrEmpty(item.Text))
                    {
                        FilterObject filterObject;
                        filterObject = (FilterObject)item.Item.Tag;
                        if (filterObject.IsConfigurable)
                        {
                            if (m_FilterObjects.TryGetValue(filterObject.ObjectType, out filterObject))
                            {
                                FilterObject dbFilter = (FilterObject)ultraListViewFilters.Items[0].Tag;
                                bool isDirty;
                                filterObject.MatchStringList =
                                    Forms.Form_NameMatching.Process(filterObject, dbFilter, m_ServerInfo, out isDirty);
                                if (isDirty)
                                {
                                    m_isDirty = true;
                                }
                                DisplayFilterObjects(false);
                            }
                        }
                    }                  
                }
            }
        }

        private void ultraListViewFilters_MouseMove(object sender, MouseEventArgs e)
        {
            UltraListViewSubItem item;
            item = ultraListViewFilters.SubItemFromPoint(e.Location);
            if (item != null)
            {
                if ( (item.Key == "Scope" || item.Key == "Name") && !string.IsNullOrEmpty(item.Text) )
                {
                        FilterObject filterObject;
                        filterObject = (FilterObject)item.Item.Tag;
                        if (filterObject.IsConfigurable)
                        {
                            Cursor = Cursors.Hand;
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                        }
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                if (Cursor == Cursors.Hand)
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void ultraListViewFilters_ItemCheckStateChanged(object sender, Infragistics.Win.UltraWinListView.ItemCheckStateChangedEventArgs e)
        {
            if (m_isInitialized && !m_isListViewUpdating && e.Item != null)
            {
                FilterObject filterObject;

                if (m_FilterObjects.TryGetValue(((FilterObject)e.Item.Tag).ObjectType, out filterObject))
                {
                    if (filterObject.IsChecked != (e.Item.CheckState == CheckState.Checked))
                    {
                        m_isDirty = true;
                        filterObject.IsChecked = (e.Item.CheckState == CheckState.Checked) ? true : false;
                        if (  ( filterObject.ObjectType == RuleObjectType.Database 
                                || filterObject.ObjectType == RuleObjectType.ExtendedStoredProcedure)
                             && filterObject.IsChecked == false)
                        {
                            e.Item.CheckState = CheckState.Checked;
                            filterObject.IsChecked = true;
                        }
                        DisplayFilterObjects(true);
                    }
                }
            }
        }


      

        // Context Menu for Scope
        // ----------------------
        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_isDirty = true;
            ((FilterObject)contextMenuStripType.Tag).ObjectScope = RuleScope.User;
            DisplayFilterObjects(false);
        }

        private void systemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_isDirty = true;
            ((FilterObject)contextMenuStripType.Tag).ObjectScope = RuleScope.System;
            DisplayFilterObjects(false);
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_isDirty = true;
            ((FilterObject)contextMenuStripType.Tag).ObjectScope = RuleScope.All;
            DisplayFilterObjects(false);
        }
  

        #endregion

        #region Helpers

        private void CreateFilterObjects()
        {
            m_FilterObjects = new Dictionary<RuleObjectType, FilterObject>();
            string matchAll = null;
            // Databases
            FilterObject filterObj = new FilterObject(RuleObjectType.Database, RuleScope.All, matchAll);
            m_FilterObjects.Add(RuleObjectType.Database, filterObj);

            // Extended Stored Procedures
            filterObj = new FilterObject(RuleObjectType.ExtendedStoredProcedure, RuleScope.System, matchAll);
            m_FilterObjects.Add(RuleObjectType.ExtendedStoredProcedure, filterObj);

            // Tables
            filterObj = new FilterObject(RuleObjectType.Table, RuleScope.All, matchAll);
            m_FilterObjects.Add(RuleObjectType.Table, filterObj);

            // Stored Procedures
            filterObj = new FilterObject(RuleObjectType.StoredProcedure, RuleScope.All, matchAll);
            m_FilterObjects.Add(RuleObjectType.StoredProcedure, filterObj);

            // Views
            filterObj = new FilterObject(RuleObjectType.View, RuleScope.All, matchAll);
            m_FilterObjects.Add(RuleObjectType.View, filterObj);

            // Functions
            filterObj = new FilterObject(RuleObjectType.Function, RuleScope.All, matchAll);
            m_FilterObjects.Add(RuleObjectType.Function, filterObj);

            if (m_ServerInfo.version > ServerVersion.SQL2000 && m_ServerInfo.version != ServerVersion.Unsupported)
            {
                // Database Principles
                filterObj = new FilterObject(RuleObjectType.User, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.User, filterObj);

                // Roles
                filterObj = new FilterObject(RuleObjectType.Role, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.Role, filterObj);

                // Synonyms
                filterObj = new FilterObject(RuleObjectType.Synonym, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.Synonym, filterObj);

                // Assemblies
                filterObj = new FilterObject(RuleObjectType.Assembly, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.Assembly, filterObj);

                // User-defined Data Types
                filterObj = new FilterObject(RuleObjectType.UserDefinedDataType, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.UserDefinedDataType, filterObj);

                // XML Schema Collections
                filterObj = new FilterObject(RuleObjectType.XMLSchemaCollection, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.XMLSchemaCollection, filterObj);

                // Full Text Catalogs
                filterObj = new FilterObject(RuleObjectType.FullTextCatalog, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.FullTextCatalog, filterObj);

                //Keys
                filterObj = new FilterObject(RuleObjectType.Key, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.Key, filterObj);

                // Sequence Objects
                filterObj = new FilterObject(RuleObjectType.SequenceObject, RuleScope.All, matchAll);
                m_FilterObjects.Add(RuleObjectType.SequenceObject, filterObj);
            }            
        }

        private void BuildFilterRules()
        {
            m_Filter.ClearRules();

            foreach (KeyValuePair<RuleObjectType, FilterObject> kvp in m_FilterObjects)
            {
                FilterObject fo = (FilterObject)kvp.Value;
                if (fo.IsChecked)
                {
                    foreach (string str in fo.MatchStringList)
                    {
                        m_Filter.AddRule(new Sql.DataCollectionFilter.Rule(fo.ObjectType,
                            fo.ObjectScope, (str != null) ? str : string.Empty));
                    }
                    // ExtendedStoredProcedures are handled special.
                    // They must create a new rule.
                    // ---------------------------------------------
                    if (fo.ObjectType == RuleObjectType.ExtendedStoredProcedure)
                    {
                        m_isDirty = true;
                    }
                }
            }

        }

        private void FillFilterObjects()
        {
            if (m_Filter.Rules.Count > 0)
            {
                // This means we have existing rules, so first clear all default settings
                // ----------------------------------------------------------------------
                foreach (KeyValuePair<RuleObjectType, FilterObject> kvp in m_FilterObjects)
                {
                    kvp.Value.IsChecked = false;
                }

                // Now process all existing rules and set values
                // ---------------------------------------------
                foreach (DataCollectionFilter.Rule rule in m_Filter.Rules)
                {
                    FilterObject filterObject;
                    if (m_FilterObjects.TryGetValue(rule.ObjectType, out filterObject))
                    {
                        filterObject.ObjectScope = rule.ObjectScope;
                        filterObject.MatchString = rule.MatchString;
                        filterObject.IsChecked = true;
                    }
                    else
                    {
                        Debug.Assert(false, "Unsupported Object in Rule");
                    }
                }
            }
        }

        private void DisplayFilterObjects(bool updateRTFOnly)
        {
            m_isListViewUpdating = true;
            if (!updateRTFOnly)
            {
                ultraListViewFilters.Items.Clear();
            }

            StringBuilder rtfDisplay = new StringBuilder();
            rtfDisplay.Append(RuleHeader);
            rtfDisplay.Append(AlwaysCollected);

            foreach (KeyValuePair<RuleObjectType, FilterObject> kvp in m_FilterObjects)
            {
                if (!updateRTFOnly)
                {
                    if (kvp.Value.ObjectType == RuleObjectType.ExtendedStoredProcedure ||
                        kvp.Value.ObjectType == RuleObjectType.StoredProcedure ||
                        kvp.Value.ObjectType == RuleObjectType.Role ||
                        kvp.Value.ObjectType == RuleObjectType.User)
                    {
                        
                    }
                    else
                    {
                        UltraListViewItem li = ultraListViewFilters.Items.Add(null, kvp.Value.ObjectTypeDisplay);
                        li.CheckState = kvp.Value.IsChecked ? CheckState.Checked : CheckState.Unchecked;
                        li.Tag = kvp.Value;
                        if (kvp.Value.ObjectType == RuleObjectType.Database)
                        {
                            li.Appearance.ForeColor = SystemColors.GrayText;
                        }
                        if (kvp.Value.IsConfigurable)
                        {
                            li.SubItems["Scope"].Value = kvp.Value.ObjectScopeDisplay;
                            li.SubItems["Scope"].Tag = FilterObject.FilterObjectSubItems.Scope;
                            li.SubItems["Scope"].Appearance.ForeColor = SystemColors.ActiveCaption;
                            li.SubItems["Scope"].Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True;

                            li.SubItems["Name"].Value = kvp.Value.MatchStringsDisplay;
                            li.SubItems["Name"].Tag = FilterObject.FilterObjectSubItems.MatchString;
                            li.SubItems["Name"].Appearance.ForeColor = SystemColors.ActiveCaption;
                            li.SubItems["Name"].Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True;
                        }
                    }
                }

                if (kvp.Value.IsChecked)
                {
                    if (kvp.Value.ObjectType == RuleObjectType.Database)
                    {
                        FilterObject esp;
                        //if (m_FilterObjects.TryGetValue(RuleObjectType.ExtendedStoredProcedure, out esp))
                        //{
                        //    if (esp.IsChecked)
                        //    {
                        //        rtfDisplay.Append(DatabasePrefix);
                        //        rtfDisplay.Append("Master Database");
                        //        rtfDisplay.Append(RuleSuffix);
                        //        rtfDisplay.Append(RulePrefix);
                        //        rtfDisplay.Append(esp.rtfDisplay);
                        //        rtfDisplay.Append(RuleSuffix);
                        //    }
                        //}
                        rtfDisplay.Append(DatabasePrefix);
                        rtfDisplay.Append(kvp.Value.rtfDisplay);
                        rtfDisplay.Append(RuleSuffix);
                    }
                    else if (kvp.Value.ObjectType == RuleObjectType.ExtendedStoredProcedure
                        || kvp.Value.ObjectType == RuleObjectType.StoredProcedure
                        || kvp.Value.ObjectType == RuleObjectType.User
                        || kvp.Value.ObjectType == RuleObjectType.Role)
                    {
//                        continue;
                    }
                    else
                    {
                        rtfDisplay.Append(RulePrefix);
                        rtfDisplay.Append(kvp.Value.rtfDisplay);
                        rtfDisplay.Append(RuleSuffix);
                    }
                }
            }
            rtfDisplay.Append(RuleFooter);
            richTextBox1.Rtf = rtfDisplay.ToString();

            m_isListViewUpdating = false;
        }

        #endregion

 
   
          
    }

    public class FilterObject
    {
        #region CTOR

        public FilterObject(RuleObjectType type, RuleScope scope, string matchString)
        {
            m_objectScope = scope;
            m_objectType = type;
            m_objectMatchString = matchString;
            m_objectMatchStringList = new List<string>();
            m_objectMatchStringList.Add(matchString);

            switch (type)
            {
                case RuleObjectType.Database:
                case RuleObjectType.Table:
                case RuleObjectType.StoredProcedure:
                case RuleObjectType.View:
                case RuleObjectType.Function:
                case RuleObjectType.ExtendedStoredProcedure:
                    m_isChecked = true;
                    break;
                default:
                    m_isChecked = true;
                    break;
            }

            InitializeAndUpdate();
        }
        #endregion

        #region Fields
        public enum FilterObjectSubItems
        {
            Scope,
            MatchString
        }

        private bool m_isChecked;
        private bool m_isConfigurable;
        private RuleObjectType m_objectType;
        private RuleScope m_objectScope;
        private List<string>  m_objectMatchStringList;
        private string m_objectMatchString;
        private string m_rtfDisplay;
        private string m_objectTypeDisplay;
        private string m_objectScopeDisplay;
        private string m_objectMatchStringDisplay;

        #endregion

        #region Properties

        public string ObjectTypeDisplay
        {
            get { return m_objectTypeDisplay; }
        }

        public string ObjectScopeDisplay
        {
            get { return m_objectScopeDisplay; }
        }

        public string MatchStringsDisplay
        {
            get { return m_objectMatchStringDisplay; }
        }

        public string rtfDisplay
        {
            get { return m_rtfDisplay; }
        }

        public string MatchStrings
        {
            get { return m_objectMatchString; }
        }

        public RuleObjectType ObjectType
        {
            get { return m_objectType; }
            set 
            { 
                m_objectType= value;
                InitializeAndUpdate();
            }
        }

        public List<string> MatchStringList
        {
            get { return m_objectMatchStringList; }
            set
            {
                if (value == null)
                {
                    m_objectMatchStringList.Clear();
                }
                else
                {
                    m_objectMatchStringList = value;
                }
                InitializeAndUpdate();
            }
        }
        public RuleScope ObjectScope
        {
            get { return m_objectScope; }
            set 
            {
                //if (value == RuleScope.Unknown)
                //{
                //    m_objectScope = RuleScope.Unknown;
                //}
                //else
                //{
                //    switch (m_objectScope)
                //    {
                //        case RuleScope.Unknown:
                //            m_objectScope = value;
                //            break;
                //        case RuleScope.All:
                //            break;
                //        case RuleScope.System:
                //            if (value != RuleScope.System)
                //            {
                //                m_objectScope = RuleScope.All;
                //            }
                //            break;
                //        case RuleScope.User:
                //            if (value != RuleScope.User)
                //            {
                //                m_objectScope = RuleScope.All;
                //            }
                //            break;
                //    }
                //}
                m_objectScope = value;
                InitializeAndUpdate();
            }
        }

        public string MatchString
        {
            set
            {
                if( m_objectMatchStringList.Count == 1 && string.IsNullOrEmpty(m_objectMatchStringList[0]) )
                {
                    if (m_objectMatchStringList[0] == null)
                    {
                        m_objectMatchStringList.Clear();
                    }
                    else
                    {
                        return;
                    }
                }
                if( string.IsNullOrEmpty(value) )
                {
                    m_objectMatchStringList.Clear();
                }
                if(!m_objectMatchStringList.Contains(value))
                {
                    m_objectMatchStringList.Add(value);
                }
                InitializeAndUpdate();
            }
        }


        public bool IsConfigurable
        {
            get { return m_isConfigurable; }
        }

        public bool IsChecked
        {
            get { return m_isChecked; }
            set { m_isChecked = value; }
        }

        #endregion

        #region Public

        public void ReplaceMatchString(string str)
        {
            m_objectMatchStringList.Clear();
            m_objectMatchStringList.Add(str);
        }

        #endregion

        #region Helpers

        private void InitializeAndUpdateMatchString()
        {
            if (m_objectMatchStringList != null && m_objectMatchStringList.Count == 1)
            {
                string temp = m_objectMatchStringList[0];
                m_objectMatchString = temp;
                if (string.IsNullOrEmpty(temp) )
                {
                    m_objectMatchStringDisplay = "Any";
                }
                else
                {
                    m_objectMatchStringDisplay = temp;
                }
            }
            else
            {
                bool isFirst = true;
                StringBuilder names = new StringBuilder();
                foreach (string str in m_objectMatchStringList)
                {
                    if (!isFirst)
                    {
                        names.Append(", ");
                    }
                    names.Append(str);
                    isFirst = false;
                }
                m_objectMatchString = names.ToString();
                m_objectMatchStringDisplay = names.ToString();

            }
        }


        private void InitializeAndUpdateScope()
        {
            switch (m_objectScope)
            {
                case RuleScope.User:
                    m_objectScopeDisplay = "User";
                    break;
                case RuleScope.System:
                    m_objectScopeDisplay = "System";
                    break;
                case RuleScope.All:
                    m_objectScopeDisplay = "System or User";
                    break;
                case RuleScope.Unknown:
                    m_objectScopeDisplay = "Unknown";
                    break;
            }

        }

        private string rtfFormat = "All {0} {1}";
        private string rtfNameMatching = " matching {0}";

        private string BuildRTFString(string Scope, string Object, string Match)
        {
            string rtf;
            Scope = Scope.Replace("or", "and");
            rtf = string.Format(rtfFormat, Scope, Object);
            if(!string.IsNullOrEmpty(Match))
            {
                rtf = rtf + string.Format(rtfNameMatching, Match);
            }
            return rtf;
        }

        private void InitializeAndUpdate()
        {
            InitializeAndUpdateScope();

            InitializeAndUpdateMatchString();

            switch (m_objectType)
            {
                case RuleObjectType.Database:
                    m_objectTypeDisplay = "Databases where";
                    m_rtfDisplay = BuildRTFString(m_objectScopeDisplay, "Databases", m_objectMatchString);
                    m_isConfigurable = true;
                    break;
                case RuleObjectType.Table:
                    m_objectTypeDisplay = "Tables where";
                    m_rtfDisplay = BuildRTFString(m_objectScopeDisplay, "Tables", m_objectMatchString);
                    m_isConfigurable = true;
                    break;
                case RuleObjectType.StoredProcedure:
                    m_objectTypeDisplay = "Stored Procedures where";
                    m_rtfDisplay = BuildRTFString(m_objectScopeDisplay, "Stored Procedures", m_objectMatchString);
                    m_isConfigurable = true;
                    break;
                case RuleObjectType.View:
                    m_objectTypeDisplay = "Views where";
                    m_rtfDisplay = BuildRTFString(m_objectScopeDisplay, "Views", m_objectMatchString);
                    m_isConfigurable = true;
                    break;
                case RuleObjectType.Function:
                    m_objectTypeDisplay = "Functions where";
                    m_rtfDisplay = BuildRTFString(m_objectScopeDisplay, "Functions", m_objectMatchString);
                    m_isConfigurable = true;
                    break;
                case RuleObjectType.User:
                    m_objectTypeDisplay = "All Database Principles";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.Role:
                    m_objectTypeDisplay = "All Roles";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.Synonym:
                    m_objectTypeDisplay = "All Synonyms";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.Assembly:
                    m_objectTypeDisplay = "All Assemblies";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.UserDefinedDataType:
                    m_objectTypeDisplay = "All User-defined Data Types";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.XMLSchemaCollection:
                    m_objectTypeDisplay = "All XML Schema Collections";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.FullTextCatalog:
                    m_objectTypeDisplay = "All Full Text Catalogs";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.ExtendedStoredProcedure:
                    m_objectTypeDisplay = "All Extended Stored Procedures";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.SequenceObject:
                    m_objectTypeDisplay = "All Sequence Objects";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                case RuleObjectType.Key:
                    m_objectTypeDisplay = "All Keys";
                    m_rtfDisplay = m_objectTypeDisplay;
                    m_isConfigurable = false;
                    break;
                default:
                    Debug.Assert(false);
                    m_isConfigurable = false;
                    m_objectTypeDisplay = "Invalid Object";
                    break;
            }

        }

        #endregion
    }
}
