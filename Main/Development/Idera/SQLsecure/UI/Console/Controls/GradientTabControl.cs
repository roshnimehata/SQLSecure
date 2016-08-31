using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Infragistics.Win.UltraWinTabControl;

namespace Idera.SQLsecure.UI.Console.Controls
{
    [ToolboxBitmap(typeof(UltraTabControl))]
    public partial class GradientTabControl : UltraTabControl
    {
        public enum GradientBorderStyle
        {
            None,
            Fixed3DOut,
            Fixed3DIn
        }

        //public enum GradientCornerStyle
        //{
        //    Square,
        //    BulletEnds,
        //    RoundTop,
        //    RoundCorners
        //}

        private Color gradientColor;
        private float rotation;
        //private GradientBorderStyle m_borderMode;
        //private GradientCornerStyle m_CornerStyle;
        private Padding m_padding = new Padding(0);

        public GradientTabControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        /// <summary>
        /// Property GradientColor (Color)
        /// </summary>
        public Color GradientColor
        {
            get { return gradientColor; }
            set { gradientColor = value; }
        }

        /// <summary>
        /// Property Rotation (float)
        /// </summary>        
        [DefaultValueAttribute(0)]
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        //[DefaultValueAttribute(GradientBorderStyle.None)]
        //public GradientBorderStyle GradientBorderMode
        //{
        //    get { return m_borderMode; }
        //    set { m_borderMode = value; }
        //}

        //[DefaultValueAttribute(GradientCornerStyle.Square)]
        //public GradientCornerStyle GradientCornerMode
        //{
        //    get { return m_CornerStyle; }
        //    set { m_CornerStyle = value; }
        //}

        /// <summary>
        /// Padding GradientTabControl.Padding
        /// Specifies the interior spacing of the tab control from the edge of the background
        /// </summary>        
        [DefaultValueAttribute(3)]
        public new Padding Padding
        {
            get { return m_padding; }
            set { m_padding = value; }
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    int left = Padding.Left;
        //    int top = Padding.Top;
        //    int width = Width - Padding.Left - Padding.Right;
        //    int height = Height - Padding.Top - Padding.Bottom;

        //    Rectangle rect = new Rectangle(left,
        //                                    top,
        //                                    width,
        //                                    height);

        //    ClientRectangle.Intersect(rect);

        //    base.OnPaint(e);

        //    e.Graphics.
        //}

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (ClientRectangle.Width != 0 && ClientRectangle.Height != 0)
            {
                // Create a GraphicsPath object.
                GraphicsPath borderPathOutSide = new GraphicsPath();

                borderPathOutSide.StartFigure();
                Rectangle rect = ClientRectangle;
                borderPathOutSide.AddRectangle(rect);
                borderPathOutSide.CloseFigure();

                using (
                    LinearGradientBrush gradientBrush =
                        new LinearGradientBrush(ClientRectangle, BackColor, GradientColor, Rotation))
                {
                    e.Graphics.FillRectangle(gradientBrush, ClientRectangle);
                }
                // FillRectangle only fills inside now color top line
                using (Pen pen = new Pen(GradientColor))
                {
                    e.Graphics.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Right,
                                        ClientRectangle.Top);
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            int left = Padding.Left;
            int top = Padding.Top;
            int width = Width - Padding.Left - Padding.Right;
            int height = Height - Padding.Top - Padding.Bottom;

            Rectangle rect = new Rectangle(left,
                                            top,
                                            width,
                                            height);

            ClientRectangle.Intersect(rect);

            Invalidate();
        }
    }
}
