using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Idera.SQLsecure.UI.Console.Data;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_WizardCreateFilterRule : Form
    {
        #region Constants

        private const string WizardIntro = @"{\rtf1\ansi\ansicpg1252\uc1\deff0\stshfdbch0\stshfloch0\stshfhich0\stshfbi0\deflang1033\deflangfe1033{\fonttbl{\f0\froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}
{\f2\fmodern\fcharset0\fprq1{\*\panose 02070309020205020404}Courier New;}{\f3\froman\fcharset2\fprq2{\*\panose 05050102010706020507}Symbol;}{\f10\fnil\fcharset2\fprq2{\*\panose 05000000000000000000}Wingdings;}
{\f50\fswiss\fcharset0\fprq2{\*\panose 020b0604020202020204}Microsoft Sans Serif;}{\f91\froman\fcharset238\fprq2 Times New Roman CE;}{\f92\froman\fcharset204\fprq2 Times New Roman Cyr;}{\f94\froman\fcharset161\fprq2 Times New Roman Greek;}
{\f95\froman\fcharset162\fprq2 Times New Roman Tur;}{\f96\froman\fcharset177\fprq2 Times New Roman (Hebrew);}{\f97\froman\fcharset178\fprq2 Times New Roman (Arabic);}{\f98\froman\fcharset186\fprq2 Times New Roman Baltic;}
{\f99\froman\fcharset163\fprq2 Times New Roman (Vietnamese);}{\f111\fmodern\fcharset238\fprq1 Courier New CE;}{\f112\fmodern\fcharset204\fprq1 Courier New Cyr;}{\f114\fmodern\fcharset161\fprq1 Courier New Greek;}
{\f115\fmodern\fcharset162\fprq1 Courier New Tur;}{\f116\fmodern\fcharset177\fprq1 Courier New (Hebrew);}{\f117\fmodern\fcharset178\fprq1 Courier New (Arabic);}{\f118\fmodern\fcharset186\fprq1 Courier New Baltic;}
{\f119\fmodern\fcharset163\fprq1 Courier New (Vietnamese);}{\f591\fswiss\fcharset238\fprq2 Microsoft Sans Serif CE;}{\f592\fswiss\fcharset204\fprq2 Microsoft Sans Serif Cyr;}{\f594\fswiss\fcharset161\fprq2 Microsoft Sans Serif Greek;}
{\f595\fswiss\fcharset162\fprq2 Microsoft Sans Serif Tur;}{\f596\fswiss\fcharset177\fprq2 Microsoft Sans Serif (Hebrew);}{\f597\fswiss\fcharset178\fprq2 Microsoft Sans Serif (Arabic);}{\f598\fswiss\fcharset186\fprq2 Microsoft Sans Serif Baltic;}
{\f599\fswiss\fcharset163\fprq2 Microsoft Sans Serif (Vietnamese);}{\f600\fswiss\fcharset222\fprq2 Microsoft Sans Serif (Thai);}}{\colortbl;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;
\red255\green0\blue0;\red255\green255\blue0;\red255\green255\blue255;\red0\green0\blue128;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue192;}
{\stylesheet{\ql \li0\ri0\widctlpar\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0 \fs24\lang1033\langfe1033\cgrid\langnp1033\langfenp1033 \snext0 Normal;}{\*\cs10 \additive \ssemihidden Default Paragraph Font;}{\*
\ts11\tsrowd\trftsWidthB3\trpaddl108\trpaddr108\trpaddfl3\trpaddft3\trpaddfb3\trpaddfr3\trcbpat1\trcfpat1\tscellwidthfts0\tsvertalt\tsbrdrt\tsbrdrl\tsbrdrb\tsbrdrr\tsbrdrdgl\tsbrdrdgr\tsbrdrh\tsbrdrv 
\ql \li0\ri0\widctlpar\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0 \fs20\lang1024\langfe1024\cgrid\langnp1024\langfenp1024 \snext11 \ssemihidden Normal Table;}}{\*\listtable{\list\listtemplateid1437352818\listhybrid{\listlevel\levelnfc23
\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace360\levelindent0{\leveltext\leveltemplateid67698689\'01\u-3913 ?;}{\levelnumbers;}\f3\fbias0 \fi-360\li720\jclisttab\tx720\lin720 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0
\levelfollow0\levelstartat1\levelspace360\levelindent0{\leveltext\leveltemplateid67698691\'01o;}{\levelnumbers;}\f2\fbias0 \fi-360\li1440\jclisttab\tx1440\lin1440 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1
\levelspace360\levelindent0{\leveltext\leveltemplateid67698693\'01\u-3929 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li2160\jclisttab\tx2160\lin2160 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace360\levelindent0
{\leveltext\leveltemplateid67698689\'01\u-3913 ?;}{\levelnumbers;}\f3\fbias0 \fi-360\li2880\jclisttab\tx2880\lin2880 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace360\levelindent0{\leveltext
\leveltemplateid67698691\'01o;}{\levelnumbers;}\f2\fbias0 \fi-360\li3600\jclisttab\tx3600\lin3600 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace360\levelindent0{\leveltext\leveltemplateid67698693
\'01\u-3929 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li4320\jclisttab\tx4320\lin4320 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace360\levelindent0{\leveltext\leveltemplateid67698689\'01\u-3913 ?;}{\levelnumbers
;}\f3\fbias0 \fi-360\li5040\jclisttab\tx5040\lin5040 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace360\levelindent0{\leveltext\leveltemplateid67698691\'01o;}{\levelnumbers;}\f2\fbias0 \fi-360\li5760
\jclisttab\tx5760\lin5760 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace360\levelindent0{\leveltext\leveltemplateid67698693\'01\u-3929 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li6480\jclisttab\tx6480\lin6480 }
{\listname ;}\listid351881218}}{\*\listoverridetable{\listoverride\listid351881218\listoverridecount0\ls1}}{\*\rsidtbl \rsid4791842\rsid9831187}{\*\generator Microsoft Word 10.0.2627;}{\info{\author Shekhar Vaidya}{\operator Shekhar Vaidya}
{\creatim\yr2006\mo7\dy12\hr20\min57}{\revtim\yr2006\mo7\dy12\hr21\min3}{\version3}{\edmins7}{\nofpages1}{\nofwords27}{\nofchars160}{\*\company Idera}{\nofcharsws186}{\vern16437}}
\widowctrl\ftnbj\aenddoc\noxlattoyen\expshrtn\noultrlspc\dntblnsbdb\nospaceforul\hyphcaps0\formshade\horzdoc\dgmargin\dghspace180\dgvspace180\dghorigin1800\dgvorigin1440\dghshow1\dgvshow1
\jexpand\viewkind1\viewscale100\pgbrdrhead\pgbrdrfoot\splytwnine\ftnlytwnine\htmautsp\nolnhtadjtbl\useltbaln\alntblind\lytcalctblwd\lyttblrtgr\lnbrkrule\nobrkwrptbl\snaptogridincell\allowfieldendsel\ApplyBrkRules\wrppunct\asianbrkrule\rsidroot9831187 
\fet0\sectd \linex0\endnhere\sectlinegrid360\sectdefaultcl\sftnbj {\*\pnseclvl1\pnucrm\pnstart1\pnindent720\pnhang {\pntxta .}}{\*\pnseclvl2\pnucltr\pnstart1\pnindent720\pnhang {\pntxta .}}{\*\pnseclvl3\pndec\pnstart1\pnindent720\pnhang {\pntxta .}}
{\*\pnseclvl4\pnlcltr\pnstart1\pnindent720\pnhang {\pntxta )}}{\*\pnseclvl5\pndec\pnstart1\pnindent720\pnhang {\pntxtb (}{\pntxta )}}{\*\pnseclvl6\pnlcltr\pnstart1\pnindent720\pnhang {\pntxtb (}{\pntxta )}}{\*\pnseclvl7\pnlcrm\pnstart1\pnindent720\pnhang 
{\pntxtb (}{\pntxta )}}{\*\pnseclvl8\pnlcltr\pnstart1\pnindent720\pnhang {\pntxtb (}{\pntxta )}}{\*\pnseclvl9\pnlcrm\pnstart1\pnindent720\pnhang {\pntxtb (}{\pntxta )}}\pard\plain \ql \li0\ri0\widctlpar\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0 
\fs24\lang1033\langfe1033\cgrid\langnp1033\langfenp1033 {\f50\fs16\insrsid4791842 This wizard helps you create a new audit filter.   With this wizard you will:}{\f50\fs16\insrsid9831187 
\par }{\f50\fs16\insrsid4791842 
\par {\listtext\pard\plain\f3\fs16\insrsid4791842 \loch\af3\dbch\af0\hich\f3 \'b7\tab}}\pard \ql \fi-360\li720\ri0\widctlpar\jclisttab\tx720\aspalpha\aspnum\faauto\ls1\adjustright\rin0\lin720\itap0\pararsid4791842 {\f50\fs16\insrsid4791842 
Specify the filter name and description.
\par {\listtext\pard\plain\f3\fs16\insrsid4791842 \loch\af3\dbch\af0\hich\f3 \'b7\tab}Select SQL Server objects that you wish to collect permissions data.
\par }\pard \ql \li0\ri0\widctlpar\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0\pararsid4791842 {\f50\fs16\insrsid4791842\charrsid4791842 
\par }}";
        private const string WizardFinishPrefix = @"{\rtf1\ansi\ansicpg1252\deff0{\fonttbl{\f0\fswiss\fprq2\fcharset0 Microsoft Sans Serif;}{\f1\fswiss\fcharset0 Arial;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\lang1033\f0\fs16 You have entered all the neccessary information to create a new snapshot rule.\par
\par
";
        private const string WizardFinishName = @"Name         : ";
        private const string WizardFinishDescription = @"\par
Description : ";
        private const string WizardFinishSuffix = @"\par
\par
\f1\fs20\par
}";
        #endregion

        #region Fields

        private Sql.RegisteredServer m_RegisteredServer;
        private List<string> m_ListOfFiltersInListView;

        #endregion

        #region Helpers

        private string MsgBoxCaption
        {
            get { return Utility.ErrorMsgs.NewFilterRuleCaption; }
        }

 
        #endregion

        #region Ctors

        public Form_WizardCreateFilterRule(
                Sql.RegisteredServer registeredServer,
                List<string> listOfFiltersInListView
            )
        {
            Debug.Assert(registeredServer != null);

            InitializeComponent();

            m_RegisteredServer = registeredServer;
            m_ListOfFiltersInListView = listOfFiltersInListView;
        }

        #endregion

        #region Methods
        private static Sql.RuleScope getScope(
                bool isUser,
                bool isSystem
            )
        {
            Debug.Assert(isUser || isSystem);
            Sql.RuleScope scope = isUser ? Sql.RuleScope.User : Sql.RuleScope.Unknown;
            if (isSystem)
            {
                scope = scope == Sql.RuleScope.User ? Sql.RuleScope.All : Sql.RuleScope.System;
            }
            return scope;
        }

        private Sql.DataCollectionFilter getFilter()
        {
            // Create a new filter.
            Sql.DataCollectionFilter filter = new Sql.DataCollectionFilter(m_RegisteredServer.ConnectionName,
                                                    _txtbx_Name.Text, _txtbx_Description.Text);

            Sql.DataCollectionFilter rules;

            filterSelection1.GetFilter(out rules);

            filter.AddDatabaseRules(rules.Rules);

            return filter;
        }

        public static Sql.DataCollectionFilter Process(
                Sql.RegisteredServer registeredServer,
                List<string> listOfFiltersInListView
            )
        {
            // Create and display the form.
            Sql.DataCollectionFilter filter = null;
            Form_WizardCreateFilterRule form = new Form_WizardCreateFilterRule(registeredServer,listOfFiltersInListView);

            // If the customer has clicked Finish, then
            // create a filter object.
            if (form.ShowDialog() == DialogResult.OK)
            {
                filter = form.getFilter();
            }

            return filter;
        }

        #endregion

        #region Event Handlers

        private void Form_WizardCreateFilterRule_Load(object sender, EventArgs e)
        {
            // Set the intro text.
            _rtbx_Introduction.Rtf = WizardIntro;

            // Setup wizard start page.
            _wizard.SelectedPage = _page_Introduction;

            Idera.SQLsecure.UI.Console.Sql.ServerVersion parsedVersion = Sql.SqlHelper.ParseVersion(m_RegisteredServer.Version);
            ServerInfo serverInfo = new ServerInfo(parsedVersion, m_RegisteredServer.SQLServerAuthType == "W", 
                m_RegisteredServer.SqlLogin, m_RegisteredServer.SqlPassword, m_RegisteredServer.FullConnectionName,Utility.Activity.TypeServerOnPremise);
            filterSelection1.Initialize(null, serverInfo);

            // Others page.
        }

        #region Page NameAndDescription

        private void pageNameAndDescriptionUpdateMoveNext()
        {
            _page_NameAndDescription.AllowMoveNext = _txtbx_Name.Text.Trim().Length != 0;
        }

        private void _page_NameAndDescription_BeforeDisplay(object sender, EventArgs e)
        {
            pageNameAndDescriptionUpdateMoveNext();
        }

        private void _txtbx_Name_TextChanged(object sender, EventArgs e)
        {
            pageNameAndDescriptionUpdateMoveNext();
        }

        private void _page_NameAndDescription_BeforeMoveNext(object sender, CancelEventArgs e)
        {
            // Check if the name is already in the list view.
            string name = _txtbx_Name.Text;
            bool isNameInUse = m_ListOfFiltersInListView.Contains(name);

            // If filter rule name already exists for the server, give a warning.
            if (isNameInUse)
            {
                Utility.MsgBox.ShowError(MsgBoxCaption, Utility.ErrorMsgs.FilterRuleAlreadyExistsMsg);
                e.Cancel = true;
            }
        }

        #endregion
     
        #region Page DbLevel

        private void pageDbLevelUpdateMoveNext()
        {
             _page_DbLevel.AllowMoveNext = true;
        }



        private void _page_DbLevel_BeforeMoveNext(object sender, CancelEventArgs e)
        {
//            p1.NextPage = _page_Finish;
//            _page_Finish.PreviousPage = _page_DbLevel;
        }

        #endregion

        #region Page Finish

        private void _page_Finish_BeforeDisplay(object sender, EventArgs e)
        {
            string summary = WizardFinishPrefix + WizardFinishName + _txtbx_Name.Text + WizardFinishDescription
                                + _txtbx_Description.Text + WizardFinishSuffix;
            _rtbx_FinishSummary.Rtf = summary;
        }

        #endregion

        private void Form_WizardCreateFilterRule_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        #endregion

        private void Form_WizardCreateFilterRule_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            ShowHelpTopic();   
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.AddFilterWizardHelpTopic);
        }


               
    }
}