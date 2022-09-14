using System;
using System.Collections.Generic;
using System.Text;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using SkiaSharp;

namespace HtmlRendererCore.Skia.Adapters
{
    public class SKPaintAdapter : IRBrush, IRPen, IRGraphicsPath, IRImage, IRFontFamily, IRFont
    {
        public SKPaint Paint { get; private set; }

        public SKBitmap Image;
        /// <summary>
        /// 匹配IRImage接口.
        /// for image,don't create paint
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="image"></param>
        public SKPaintAdapter(SKBitmap image)
        {
            this.Image = image;
        }

        public SKPaintAdapter(string fontFamily)
        {
            this.FontName = fontFamily;
        }

        public SKTypeface Font;
        /// <summary>
        /// 匹配IRFont接口.
        /// for font and font color
        /// </summary>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        public SKPaintAdapter(SKTypeface font, int fontSize)
        {
            Paint = new SKPaint()
            {
                IsAntialias = true,
                Typeface = font,
                TextSize = fontSize,
            };
            this.Font = font;
            FontSize = fontSize;
            FontHeight = (float)(Paint.FontMetrics.Descent - Paint.FontMetrics.Ascent);

            //UnderlineOffset = Paint.FontMetrics.UnderlinePosition == null ? -Paint.FontMetrics.Ascent : -Paint.FontMetrics.Ascent + Paint.FontMetrics.UnderlinePosition.Value;
            UnderlineOffset = FontHeight - Paint.FontMetrics.Descent + 0.5f;
            BaselineOffset = -Paint.FontMetrics.Ascent;
        }

        public SKPath Path;
        /// <summary>
        /// 匹配IRGraphicsPath接口
        /// for path,don't create paint
        /// </summary>
        public SKPaintAdapter()
        {
            Path = new SKPath();
        }

        public object Brush { get; private set; }
        public BrushType GetBrushType { get; private set; }
        /// <summary>
        /// 匹配IRBrush接口
        /// for brush and pen,don't create paint
        /// </summary>
        public SKPaintAdapter(object brush, BrushType brushType = BrushType.SolidColor)
        {
            Brush = brush;
            GetBrushType = brushType;
        }

        public enum BrushType
        {
            SolidColor,
            LinerGradien,
            ImageBrush,
        }

        #region IRPen
        double penWidth = -1;
        public double PenWidth
        {
            get => penWidth;
            set => penWidth = value;
        }

        public SKPathEffect PathEffect {get;private set;}
        public RDashStyle DashStyle
        {
            set
            {
                switch (value)
                {
                    case RDashStyle.Solid:
                        //_pen.DashStyle = XDashStyle.Solid;
                        break;
                    case RDashStyle.Dash:
                        //_pen.DashStyle = XDashStyle.Dash;
                        PathEffect = SKPathEffect.CreateDash(new float[] { (float)(PenWidth * 3), (float)PenWidth }, (float)(PenWidth * 2));
                        //if (Width < 2)
                        //    _pen.DashPattern = new[] { 4, 4d }; // better looking
                        break;
                    case RDashStyle.Dot:
                        //_pen.DashStyle = XDashStyle.Dot;
                        PathEffect = SKPathEffect.CreateDash(new float[] { (float)PenWidth, (float)PenWidth }, (float)(PenWidth * 2));
                        break;
                    case RDashStyle.DashDot:
                        //_pen.DashStyle = XDashStyle.DashDot;
                        PathEffect = SKPathEffect.CreateDash(new float[] { (float)(PenWidth * 3), (float)PenWidth, (float)PenWidth, (float)PenWidth }, (float)(PenWidth * 2));
                        break;
                    case RDashStyle.DashDotDot:
                        //_pen.DashStyle = XDashStyle.DashDotDot;
                        PathEffect = SKPathEffect.CreateDash(new float[] { (float)(PenWidth * 3), (float)PenWidth, (float)PenWidth, (float)PenWidth, (float)PenWidth, (float)PenWidth }, (float)(PenWidth * 2));
                        break;
                    case RDashStyle.Custom:
                        //_pen.DashStyle = XDashStyle.Custom;
                        break;
                    default:
                        //_pen.DashStyle = XDashStyle.Solid;
                        break;
                }
            }
        }

        #endregion

        #region IRImage
        public double ImageWidth => Image.Width;

        public double ImageHeight => Image.Height;
        #endregion

        #region IRFontFamily
        public string FontName { get; set; }
        #endregion

        #region IRFont

        public double FontSize { get; set; }

        public double FontHeight { get; set; }

        public double UnderlineOffset { get; set; }

        public double FontLeftPadding => FontHeight / 6f;

        double WhitespaceWidth=-1;
        public double GetWhitespaceWidth(RGraphics graphics)
        {
            if(WhitespaceWidth==-1)
             WhitespaceWidth= graphics.MeasureString(" ", this).Width;
            return WhitespaceWidth;
        }

        public float BaselineOffset { get; set; }

        #endregion

        #region IRGraphicsPath
        float lastX;
        float lastY;
        public void StartPath(double x, double y)
        {
            Path.MoveTo((float)x, (float)y);
            lastX = (float)x;
            lastY = (float)y;
        }
        public void LineTo(double x, double y)
        {
            //Path.MoveTo((float)lastX, (float)lastY);
            Path.LineTo((float)x, (float)y);
            lastY = (float)x;
            lastY = (float)y;
        }
        public void ArcTo(double x, double y, double size, IRGraphicsPath.Corner corner)
        {
           
            float left = (float)(Math.Min(x,lastX) - (corner == IRGraphicsPath.Corner.TopRight || corner == IRGraphicsPath.Corner.BottomRight ? size : 0));
            float top = (float)(Math.Min(y,lastY) - (corner == IRGraphicsPath.Corner.BottomLeft || corner == IRGraphicsPath.Corner.BottomRight ? size : 0));
            //Path.ArcTo(SKRect.Create(left, top, (float)size * 2, (float)size * 2), GetStartAngle(corner),90, true);
            //Path.MoveTo((float)(left+size), (float)(top+size));
            Path.AddArc(SKRect.Create(left, top, (float)size*2, (float)size*2), GetStartAngle(corner),90);
            //Path.ArcTo(left, top,(float)x,(float)y,(float)size);
            //Path.MoveTo((float)x, (float)y);
            lastX = (float)x;
            lastY = (float)y;
        }

        public SKPath ClosePath()
        {
            //Path.Close();
            return Path;
        }
        /// <summary>
        /// Get arc start angle for the given corner.
        /// </summary>
        private static int GetStartAngle(IRGraphicsPath.Corner corner)
        {
            int startAngle;
            switch (corner)
            {
                case IRGraphicsPath.Corner.TopLeft:
                    startAngle = 180;
                    break;
                case IRGraphicsPath.Corner.TopRight:
                    startAngle = 270;
                    break;
                case IRGraphicsPath.Corner.BottomLeft:
                    startAngle = 90;
                    break;
                case IRGraphicsPath.Corner.BottomRight:
                    startAngle = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("corner");
            }
            return startAngle;
        }
        #endregion

        public void Dispose()
        {
            PathEffect?.Dispose();
            Paint?.Dispose();
            Image?.Dispose();
        }
    }
}
