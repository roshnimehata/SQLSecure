using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Idera.SQLsecure.UI.Console.Controls
{
    /// <summary>
    /// GradientPanel is just like a regular panel except it optionally shows a gradient.
    /// </summary>
    [ToolboxBitmap(typeof(Panel))]
    public class GradientPanel : Panel
    {
        public enum GradientBorderStyle
        {
            None,
            Fixed3DOut,
            Fixed3DIn
        }

        public enum GradientCornerStyle
        {
            Square,
            BulletEnds,
            RoundTop,
            RoundCorners
        }

        private Color gradientColor;
        private float rotation;
        private GradientBorderStyle m_borderMode;
        private GradientCornerStyle m_CornerStyle;
        
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

        [DefaultValueAttribute(GradientBorderStyle.None)]
        public GradientBorderStyle GradientBorderMode
        {
            get { return m_borderMode; }
            set { m_borderMode = value; }
        }

        [DefaultValueAttribute(GradientCornerStyle.Square)]
        public GradientCornerStyle GradientCornerMode
        {
            get { return m_CornerStyle; }
            set { m_CornerStyle = value; }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.OnPaintBackground(e);

            if (ClientRectangle.Width != 0 && ClientRectangle.Height != 0)
            {
                // Create a GraphicsPath object.
                GraphicsPath clipPath = new GraphicsPath();
                GraphicsPath borderPathOutSide = new GraphicsPath();
                GraphicsPath borderPathBottomOutside = new GraphicsPath();
                GraphicsPath borderPathInSide = new GraphicsPath();
                GraphicsPath borderPathBottomInside = new GraphicsPath();

                if (m_CornerStyle == GradientCornerStyle.BulletEnds)
                {
                    int width1 = ClientRectangle.Height;
                    int x1 = ClientRectangle.X + width1;
                    int x2 = ClientRectangle.X + ClientRectangle.Width - width1;

                    Rectangle rect = new Rectangle(ClientRectangle.X,
                                                    ClientRectangle.Y,
                                                    width1,
                                                    ClientRectangle.Height);
                    Rectangle rect2 = new Rectangle(x1,
                                                    ClientRectangle.Y,
                                                    x2 - x1,
                                                    ClientRectangle.Height);
                    Rectangle rect3 = new Rectangle(x2,
                                                    ClientRectangle.Y,
                                                    width1,
                                                    ClientRectangle.Height);

                    clipPath.StartFigure();
                    clipPath.AddArc(rect, 90, 180);
                    clipPath.AddArc(rect3, 270, 180);
                    clipPath.CloseFigure();
                    borderPathOutSide.StartFigure();
                    borderPathOutSide.AddArc(rect, 90, 180);
                    borderPathOutSide.AddArc(rect3, 270, 180);
                    borderPathOutSide.CloseFigure();
                    borderPathBottomOutside = borderPathOutSide;
                    e.Graphics.Clip = new Region(clipPath);
                }
                else if (m_CornerStyle == GradientCornerStyle.Square)
                {
                    borderPathOutSide.StartFigure();
                    Rectangle rect = ClientRectangle;
                    borderPathOutSide.AddRectangle(rect);
                    borderPathOutSide.CloseFigure();
//                    borderPath2.AddLine(ClientRectangle.Right, ClientRectangle
                }
                else if (m_CornerStyle == GradientCornerStyle.RoundCorners)
                {
                    int width1 = ClientRectangle.Height /2;
                    int x1 = ClientRectangle.X + width1;
                    int x2 = ClientRectangle.X + ClientRectangle.Width - width1 - 1;
                    int height1 = ClientRectangle.Height / 2;
                    int y1 = ClientRectangle.Y + height1 - 1;

                    Rectangle rectLeftTop = new Rectangle(ClientRectangle.X,
                                                    ClientRectangle.Y,
                                                    width1,
                                                    height1);
                    Rectangle rectLeftBottom = new Rectangle(ClientRectangle.X,
                                                    y1,
                                                    width1,
                                                    height1);
                    Rectangle rectRightBottom = new Rectangle(x2,
                                                    y1,
                                                    width1,
                                                    height1);
                    Rectangle rectRightTop = new Rectangle(x2,
                                                    ClientRectangle.Y,
                                                    width1,
                                                    height1);

                    clipPath.StartFigure();
                    clipPath.AddArc(rectRightTop, 270, 90);
                    clipPath.AddArc(rectRightBottom, 0, 90);
                    clipPath.AddArc(rectLeftBottom, 90, 90);
                    clipPath.AddArc(rectLeftTop, 180, 90);
                    clipPath.CloseFigure();
                    borderPathOutSide = clipPath;
                    borderPathOutSide.Flatten();

                    borderPathBottomOutside.StartFigure();
                    borderPathBottomOutside.AddArc(rectRightTop, 300, 60);
                    borderPathBottomOutside.AddArc(rectRightBottom, 0, 90);
                    borderPathBottomOutside.AddArc(rectLeftBottom, 90, 60);

                    rectLeftTop.Inflate(-1, -1);
                    rectLeftBottom.Inflate(-1, -1);
                    rectRightTop.Inflate(-1, -1);
                    rectRightBottom.Inflate(-1, -1);

                    borderPathInSide.StartFigure();
                    borderPathInSide.AddArc(rectRightTop, 270, 90);
                    borderPathInSide.AddArc(rectRightBottom, 0, 90);
                    borderPathInSide.AddArc(rectLeftBottom, 90, 90);
                    borderPathInSide.AddArc(rectLeftTop, 180, 90);
                    borderPathInSide.CloseFigure();

                    borderPathBottomInside.StartFigure();
                    borderPathBottomInside.AddArc(rectRightTop, 330, 30);
                    borderPathBottomInside.AddArc(rectRightBottom, 0, 90);
                    borderPathBottomInside.AddArc(rectLeftBottom, 90, 30);



                    e.Graphics.Clip = new Region(clipPath);
                }

                else if (m_CornerStyle == GradientCornerStyle.RoundTop)
                {
                    int width1 = ClientRectangle.Height / 2;
                    int x1 = ClientRectangle.X + width1;
                    int x2 = ClientRectangle.X + ClientRectangle.Width - width1 - 1;
                    int height1 = ClientRectangle.Height / 2;
                    int y1 = ClientRectangle.Y + height1 - 1;

                    Rectangle rectLeftTop = new Rectangle(ClientRectangle.X,
                                                    ClientRectangle.Y,
                                                    width1,
                                                    height1);
                    Rectangle rectRightTop = new Rectangle(x2,
                                                    ClientRectangle.Y,
                                                    width1,
                                                    height1);

                    clipPath.StartFigure();
                    clipPath.AddLine(ClientRectangle.Right, ClientRectangle.Bottom, ClientRectangle.Left, ClientRectangle.Bottom);
                    clipPath.AddArc(rectLeftTop, 180, 90);
                    clipPath.AddArc(rectRightTop, 270, 90);
                    clipPath.CloseFigure();
                    borderPathOutSide = clipPath;
                    borderPathOutSide.Flatten();
                    e.Graphics.Clip = new Region(clipPath);
                }

                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(ClientRectangle, BackColor, GradientColor, Rotation))
                {
                    e.Graphics.FillRectangle(gradientBrush, ClientRectangle);
                }
                // FillRectangle only fills inside now color top line
                using(Pen pen = new Pen(GradientColor))
                {
                    e.Graphics.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Right, ClientRectangle.Top);
                }

                e.Graphics.ResetClip();
                Point BottomLeft = new Point(ClientRectangle.Left, ClientRectangle.Bottom);
                Point BottomRight = new Point(ClientRectangle.Right, ClientRectangle.Bottom);
                Point TopLeft = new Point(ClientRectangle.Left, ClientRectangle.Top);
                Point TopRight = new Point(ClientRectangle.Right, ClientRectangle.Top);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                if (m_borderMode == GradientBorderStyle.Fixed3DIn)
                {
                    if (m_CornerStyle == GradientCornerStyle.Square)
                    {
                        using (Pen pen = new Pen(Color.DarkGray, 4))
                        {
                            e.Graphics.DrawLine(pen, BottomLeft, TopLeft);
                            e.Graphics.DrawLine(pen, TopLeft, TopRight);
                        }
                        using (Pen pen = new Pen(Color.Silver, 4))
                        {
                            e.Graphics.DrawLine(pen, BottomLeft, BottomRight);
                            e.Graphics.DrawLine(pen, BottomRight, TopRight);
                        }
                    }
                    else 
                    {

                        using (Pen pen = new Pen(Color.Gray, 1))
                        {
                            pen.EndCap = LineCap.Triangle;
                            pen.StartCap = LineCap.Triangle;
                            pen.LineJoin = LineJoin.Bevel;
                            e.Graphics.DrawPath(pen, borderPathOutSide);
                            e.Graphics.DrawPath(pen, borderPathInSide);
                        }
                        using (Pen pen = new Pen(Color.Silver, 1))
                        {
                            pen.EndCap = LineCap.Triangle;
                            pen.StartCap = LineCap.Triangle;
                            pen.LineJoin = LineJoin.Bevel;
                            e.Graphics.DrawPath(pen, borderPathBottomOutside);
                            e.Graphics.DrawPath(pen, borderPathBottomInside);
                        }
                    }

                }
                if (m_borderMode == GradientBorderStyle.Fixed3DOut)
                {
                    if (m_CornerStyle == GradientCornerStyle.Square)
                    {
                        using (Pen pen = new Pen(Color.Silver, 4))
                        {
                            e.Graphics.DrawLine(pen, BottomLeft, TopLeft);
                            e.Graphics.DrawLine(pen, TopLeft, TopRight);
                        }
                        using (Pen pen = new Pen(Color.Gray, 4))
                        {
                            e.Graphics.DrawLine(pen, BottomLeft, BottomRight);
                            e.Graphics.DrawLine(pen, BottomRight, TopRight);
                        }
                    }
                    else
                    {
                        using (Pen pen = new Pen(Color.LightGray, 1))
                        {
                            pen.EndCap = LineCap.Triangle;
                            pen.StartCap = LineCap.Triangle;
                            pen.LineJoin = LineJoin.Bevel;
                            e.Graphics.DrawPath(pen, borderPathInSide);
                            pen.Color = Color.Gainsboro;
                            e.Graphics.DrawPath(pen, borderPathOutSide);
                        }
                        using (Pen pen = new Pen(Color.Gray, 1))
                        {
                            pen.EndCap = LineCap.Triangle;
                            pen.StartCap = LineCap.Triangle;
                            pen.LineJoin = LineJoin.Bevel;
                            e.Graphics.DrawPath(pen, borderPathBottomInside);
                            pen.Color = Color.DimGray;
                            e.Graphics.DrawPath(pen, borderPathBottomOutside);
                        }
                    }

                }



            }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate();
        }
    }
}
