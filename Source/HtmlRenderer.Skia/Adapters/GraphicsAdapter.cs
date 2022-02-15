// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

//using PdfSharp.Drawing;
using SkiaSharp;
using System;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.Core.Utils;
using TheArtOfDev.HtmlRenderer.PdfSharp.Utilities;

namespace TheArtOfDev.HtmlRenderer.PdfSharp.Adapters
{
    /// <summary>
    /// Adapter for WinForms Graphics for core.
    /// </summary>
    internal sealed class GraphicsAdapter : RGraphics
    {
        #region Fields and Consts

        /// <summary>
        /// The wrapped WinForms graphics object
        /// </summary>
        private readonly SKCanvas _g;

        /// <summary>
        /// if to release the graphics object on dispose
        /// </summary>
        private readonly bool _releaseGraphics;

        /// <summary>
        /// Used to measure and draw strings
        /// </summary>
        //private static readonly XStringFormat _stringFormat;
        private static readonly SKTextAlign _stringFormat;

        #endregion


        static GraphicsAdapter()
        {
            //_stringFormat = new XStringFormat();
            //_stringFormat.Alignment = XStringAlignment.Near;
            //_stringFormat.LineAlignment = XLineAlignment.Near;
            _stringFormat = SKTextAlign.Left;
        }

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="g">the win forms graphics object to use</param>
        /// <param name="releaseGraphics">optional: if to release the graphics object on dispose (default - false)</param>
        public GraphicsAdapter(SKCanvas g, bool releaseGraphics = false)
            : base(PdfSharpAdapter.Instance, new RRect(0, 0, double.MaxValue, double.MaxValue))
        {
            ArgChecker.AssertArgNotNull(g, "g");

            _g = g;
            _releaseGraphics = releaseGraphics;
        }

        public override void PopClip()
        {
            _clipStack.Pop();
            _g.Restore();
        }

        public override void PushClip(RRect rect)
        {
            _clipStack.Push(rect);
            _g.Save();
            //_g.IntersectClip(Utils.Convert(rect));
            _g.ClipRect(Utils.Convert(rect));
        }

        public override void PushClipExclude(RRect rect)
        { }

        public override Object SetAntiAliasSmoothingMode()
        {
            //var prevMode = _g.SmoothingMode;
            var prevMode = textPaint.IsAntialias;
            //_g.SmoothingMode = XSmoothingMode.AntiAlias;
            textPaint.IsAntialias= true;
            return prevMode;
        }

        public override void ReturnPreviousSmoothingMode(Object prevMode)
        {
            if (prevMode != null)
            {
                //_g.SmoothingMode = (XSmoothingMode)prevMode;
                textPaint.IsAntialias = (bool)prevMode;
            }
        }

        static SKPaint textPaint = new SKPaint();
        public override RSize MeasureString(string str, IRFont font)
        {
            var fontAdapter = (FontAdapter)font;
            var realFont = fontAdapter.Font;
            textPaint.Typeface = realFont;
            textPaint.TextSize = (float)fontAdapter.FontSize;
            textPaint.TextAlign = _stringFormat;
            //var size = _g.MeasureString(str, realFont, _stringFormat);
            var size = textPaint.MeasureText(str);

            if (font.FontHeight < 0)
            {
                //var height = realFont.Height;
                var height = fontAdapter.FontSize;
                //var descent = realFont.Size * realFont.FontFamily.GetCellDescent(realFont.Style) / realFont.FontFamily.GetEmHeight(realFont.Style);
                var descent = textPaint.FontMetrics.Descent;
                fontAdapter.SetMetrics((int)height, (int)Math.Round((height - descent + 1f)));
            }

            //return Utils.Convert(size);
            return Utils.Convert(new SKSize(size, (float)fontAdapter.FontSize));
        }

        public override void MeasureString(string str, IRFont font, double maxWidth, out int charFit, out double charFitWidth)
        {
            // there is no need for it - used for text selection
            throw new NotSupportedException();
        }

        public override void DrawString(string str, IRFont font, RColor color, RPoint point, RSize size, bool rtl)
        {
            //var SKShader = ((BrushAdapter)_adapter.GetSolidBrush(color)).Brush;
            textPaint.Typeface = ((FontAdapter)font).Font;
            textPaint.Color = Utils.Convert(color);
            textPaint.TextAlign = _stringFormat;
            _g.DrawText(str, (float)point.X, (float)point.Y, textPaint);
        }

        public override IRBrush GetTextureBrush(IRImage image, RRect dstRect, RPoint translateTransformLocation)
        {
            return new BrushAdapter(new XTextureBrush(((ImageAdapter)image).Image, Utils.Convert(dstRect), Utils.Convert(translateTransformLocation)));
        }

        public override IRGraphicsPath GetGraphicsPath()
        {
            return new GraphicsPathAdapter();
        }

        public override void Dispose()
        {
            if (_releaseGraphics)
                _g.Dispose();
        }


        #region Delegate graphics methods

        public override void DrawLine(IRPen pen, double x1, double y1, double x2, double y2)
        {
            //_g.DrawLine(((PenAdapter)pen).Pen, x1, y1, x2, y2);
            _g.DrawLine((float)x1, (float)y1, (float)x2, (float)y2, ((PenAdapter)pen).Pen);
        }

        public override void DrawRectangle(IRPen pen, double x, double y, double width, double height)
        {
            _g.DrawRect((float)x, (float)y, (float)width, (float)height, ((PenAdapter)pen).Pen);
        }

        public override void DrawRectangle(IRBrush brush, double x, double y, double width, double height)
        {
            var SKShader = ((BrushAdapter)brush).Brush;
            var xTextureBrush = SKShader as XTextureBrush;
            if (xTextureBrush != null)
            {
                xTextureBrush.DrawRectangle(_g, (float)x, (float)y, (float)width, (float)height);
            }
            else
            {
                //_g.DrawRect((SKShader)SKShader, x, y, width, height);
                textPaint.Shader = (SKShader)SKShader;
                _g.DrawRect((float)x, (float)y, (float)width, (float)height, textPaint);

                // handle bug in PdfSharp that keeps the brush color for next string draw
                //if (SKShader is XLinearGradientBrush)
                //    _g.DrawRect(0, 0, 0.1, 0.1,new SKPaint() { Color=SKColors.White});
            }
        }

        public override void DrawImage(IRImage image, RRect destRect, RRect srcRect)
        {
            //_g.DrawImage(((ImageAdapter)image).Image, Utils.Convert(destRect), Utils.Convert(srcRect), SKCanvasUnit.Point);
            _g.DrawBitmap(((ImageAdapter)image).Image, Utils.Convert(destRect), Utils.Convert(srcRect), textPaint);
        }

        public override void DrawImage(IRImage image, RRect destRect)
        {
            _g.DrawBitmap(((ImageAdapter)image).Image, Utils.Convert(destRect));
        }

        public override void DrawPath(IRPen pen, IRGraphicsPath path)
        {
            _g.DrawPath(((GraphicsPathAdapter)path).GraphicsPath, ((PenAdapter)pen).Pen);
        }

        public override void DrawPath(IRBrush brush, IRGraphicsPath path)
        {
            textPaint.Shader = (SKShader)((BrushAdapter)brush).Brush;
            _g.DrawPath(((GraphicsPathAdapter)path).GraphicsPath,textPaint);
        }

        public override void DrawPolygon(IRBrush brush, RPoint[] points)
        {
            SKPath path = new SKPath() { FillType=SKPathFillType.Winding};
            foreach(var point in points)
            {
                path.LineTo((float)point.X, (float)point.Y);
            }
            textPaint.Shader = (SKShader)((BrushAdapter)brush).Brush;
            _g.DrawPath(path, textPaint);
            //if (points != null && points.Length > 0)
            //{
            //    _g.DrawPolygon((SKShader)((BrushAdapter)brush).Brush, Utils.Convert(points), XFillMode.Winding);
            //}
        }

        #endregion
    }
}