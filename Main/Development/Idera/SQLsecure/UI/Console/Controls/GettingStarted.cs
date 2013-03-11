using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class GettingStarted : ViewSection
    {
        public GettingStarted()
        {
            InitializeComponent();

            // Set the Title.
            this.Title = "Getting Started";

            _rtfbx_GettingStarted.Rtf = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033\deflangfe1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Microsoft Sans Serif;}}
{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\li360\f0\fs16 To begin auditing your SQL Server instances, perform the following steps.\par
\pard\li360\sb120\sa120\b Add a SQL Server to SQLsecure\par
\pard\fi-360\li1440\sb60\sa60\tx1440\b0 a.\tab Select \b File > Audit a SQL Server\b0  to open the Audit a SQL Server Wizard.\par
\pard\fi-360\li1440\sb60\sa60 b.\tab Enter the Server Name and Credentials for SQLsecure to use to access permissions information.\par
\pard\li360\sb120\sa120\b Add a Filter for your Snapshot\par
\pard\fi-360\li1440\sb60\sa60\tx1440\b0 a.\tab Right-click your server from the list in the left pane and select \b Properties\b0  from the list.\par
\pard\fi-360\li1440\sb60\sa60 b.\tab Select the \b Filters \b0 tab and click\b  Add\b0  to open the Add New Filter Wizard.\par
c.\tab Enter your Filter criteria.\par
d.\tab Click \b OK\b0  to save your filter.\par
\pard\li360\sb120\sa120\b Schedule your Snapshot\par
\pard\fi-360\li1440\sb60\sa60\tx1440\b0 a.\tab Right-click your server from the list in the left pane and select \b Properties\b0  from the list.\par
\pard\fi-360\li1440\sb60\sa60 b.\tab Select the \b Schedule \b0 tab.\par
c.\tab Click \b Change\b0  and enter the times you want your snapshot collection to occur.\par
d.\tab Click \b OK\b0  to save your snapshot schedule.\par
}";
        }
    }
}
