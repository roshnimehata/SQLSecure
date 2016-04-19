using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_UnderConstruction : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Title = contextIn.Name;
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.UnderContructionHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.UnderConstructionConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion

        #region Ctors

        public View_UnderConstruction() : base()
        {
            InitializeComponent();

            // Initialize base class fields.
            Title = "Yahoo";
            this._label_Summary.Text = "This is a dummy view used for testing and is also used a default view when specific views are under construction.";
        }

        #endregion
    }
}

