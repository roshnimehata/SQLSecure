using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    [Designer(typeof(ViewSectionDesigner))]
    public partial class ViewSection : UserControl
    {
        public enum ViewSectionHeaderStyle
        {
            Main,
            Subsection
        }

        public ViewSection()
        {
            InitializeComponent();
        }

        private ViewSectionHeaderStyle m_HeaderStyle = ViewSectionHeaderStyle.Main;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientPanel ViewPanel
        {
            get { return _gradientPanel_Body; }
        }
        public String Title
        {
            get { return _label_Title.Text; }
            set { _label_Title.Text = value; }
        }
        [DefaultValue(ViewSectionHeaderStyle.Main)]
        public ViewSectionHeaderStyle HeaderStyle
        {
            get { return m_HeaderStyle; }
            set
            {
                // This property is really only designed to be used in the designer, so it forces these values when set
                if (value == ViewSectionHeaderStyle.Main)
                {
                    _gradientPanel_Header.Height = 20;
                    _gradientPanel_Header.GradientBorderMode = GradientPanel.GradientBorderStyle.Fixed3DOut;
                    _gradientPanel_Header.GradientCornerMode = GradientPanel.GradientCornerStyle.RoundTop;
                    _label_Title.Font =
                        new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
                }
                else if (value == ViewSectionHeaderStyle.Subsection)
                {
                    _gradientPanel_Header.Height = 16;
                    _gradientPanel_Header.GradientBorderMode = GradientPanel.GradientBorderStyle.None;
                    _gradientPanel_Header.GradientCornerMode = GradientPanel.GradientCornerStyle.Square;
                    _label_Title.Font =
                        new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
                }
                m_HeaderStyle = value;
            }
        }
        public Color HeaderTextColor
        {
            get { return _gradientPanel_Header.ForeColor; }
            set { _gradientPanel_Header.ForeColor = value; }
        }
        public Color HeaderGradientColor
        {
            get { return _gradientPanel_Header.GradientColor; }
            set { _gradientPanel_Header.GradientColor = value; }
        }
        public GradientPanel.GradientBorderStyle HeaderGradientBorderStyle
        {
            get { return _gradientPanel_Header.GradientBorderMode; }
            set { _gradientPanel_Header.GradientBorderMode = value; }
        }
        public GradientPanel.GradientCornerStyle HeaderGradientCornerStyle
        {
            get { return _gradientPanel_Header.GradientCornerMode; }
            set { _gradientPanel_Header.GradientCornerMode = value; }
        }
    }
    class ViewSectionDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        public override void Initialize(IComponent comp)
        {
            base.Initialize(comp);

            ViewSection uc = (ViewSection)comp;
            EnableDesignMode(uc.ViewPanel, "Panel");

        }
    }
}
