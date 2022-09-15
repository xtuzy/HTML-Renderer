﻿using HtmlRendererCore.Skia.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace HtmlRendererCore.Skia.Adapters
{
    public class SKGraphicsAdapter : RGraphics
    {
        SKCanvas canvas;
        /// <summary>
        /// for measure
        /// </summary>
        public SKGraphicsAdapter() : base(SKCanvasAdapter.Instance, new RRect(0, 0, double.MaxValue, double.MaxValue))
        {
        }
        public SKGraphicsAdapter(SKCanvas canvas) : base(SKCanvasAdapter.Instance, new RRect(0, 0, double.MaxValue, double.MaxValue))
        {
            this.canvas = canvas;
        }
        public SKGraphicsAdapter(SKCanvas canvas, RRect initialClip) : base(SKCanvasAdapter.Instance, initialClip)
        {
            this.canvas = canvas;
        }
        public override void DrawImage(IRImage image, RRect destRect, RRect srcRect)
        {
            canvas.DrawBitmap(((SKPaintAdapter)image).Image, Utils.Convert(srcRect), Utils.Convert(destRect));
        }

        public override void DrawImage(IRImage image, RRect destRect)
        {
            canvas.DrawBitmap(((SKPaintAdapter)image).Image, Utils.Convert(destRect));
        }

        public override void DrawLine(IRPen pen, double x1, double y1, double x2, double y2)
        {
            using (var paint = GetPaintFromPen(pen))
            {
                x1 = (int)x1;
                x2 = (int)x2;
                y1 = (int)y1;
                y2 = (int)y2;

                var adj = pen.PenWidth;
                if (Math.Abs(x1 - x2) < .1 && Math.Abs(adj % 2 - 1) < .1)
                {
                    x1 += .5;
                    x2 += .5;
                }
                if (Math.Abs(y1 - y2) < .1 && Math.Abs(adj % 2 - 1) < .1)
                {
                    y1 += .5;
                    y2 += .5;
                }

                canvas.DrawLine(new SKPoint((float)x1, (float)y1), new SKPoint((float)x2, (float)y2), paint);
            }
        }
        SKPaint GetPaintFromPen(IRPen pen)
        {
            var paint = new SKPaint() { IsAntialias = true};

            var penPaintAdapter = (SKPaintAdapter)pen;
            paint.StrokeWidth = (float)penPaintAdapter.PenWidth;
            paint.PathEffect = penPaintAdapter.PathEffect;
            paint.Color = (SKColor)penPaintAdapter.Brush;
            return paint;
        }
        public override void DrawPath(IRPen pen, IRGraphicsPath path)
        {
            using (var paint = GetPaintFromPen(pen))
            {
                canvas.DrawPath(((SKPaintAdapter)path).ClosePath(), paint);
            }
        }

        public override void DrawPath(IRBrush brush, IRGraphicsPath path)
        {
            using (var paint = GetPaintFromBrush(brush))
            {
                canvas.DrawPath(((SKPaintAdapter)path).ClosePath(), paint);
            }
        }

        public override void DrawPolygon(IRBrush brush, RPoint[] points)
        {
            using (SKPath path = new SKPath() { FillType = SKPathFillType.Winding })
            {
                using (var paint = GetPaintFromBrush(brush))
                {
                    path.MoveTo((float)(points[0].X), (float)(points[0].Y));
                    for (var index = 1; index < points.Length; index++)
                    {
                        var point = points[index];
                        path.LineTo((float)point.X, (float)point.Y);
                    }
                    path.Close();
                    canvas.DrawPath(path, paint);
                }
            }
        }
        SKPaint GetPaintFromBrush(IRBrush brush)
        {
            var paint = new SKPaint() { IsAntialias = true };

            var brushPaintAdapter = (SKPaintAdapter)brush;
            switch (brushPaintAdapter.GetBrushType)
            {
                case SKPaintAdapter.BrushType.SolidColor:
                    paint.Color = (SKColor)brushPaintAdapter.Brush;
                    break;
                case SKPaintAdapter.BrushType.LinerGradien:
                case SKPaintAdapter.BrushType.ImageBrush:
                    paint.Shader = (SKShader)brushPaintAdapter.Brush;
                    break;
            }
            return paint;
        }
        public override void DrawRectangle(IRPen pen, double x, double y, double width, double height)
        {
            var adj = pen.PenWidth;
            if (Math.Abs(adj % 2 - 1) < .1)
            {
                x += .5;
                y += .5;
            }

            using (var paint = GetPaintFromPen(pen))
            {
                canvas.DrawRect((float)x, (float)y, (float)width, (float)height, paint);
            }
        }

        public override void DrawRectangle(IRBrush brush, double x, double y, double width, double height)
        {
            using (var paint = GetPaintFromBrush(brush))
            {
                canvas.DrawRect((float)x, (float)y, (float)width, (float)height, paint);
            }
        }

        public override void DrawString(string str, IRFont font, RColor color, RPoint point, RSize size, bool rtl)
        {
            var fontAdapter = ((SKPaintAdapter)font);
            var paint = fontAdapter.Paint;
            var containTextInFont = paint.ContainsGlyphs(str);
            var brushPaintAdapter = ((SKPaintAdapter)_adapter.GetSolidBrush(color));
            switch (brushPaintAdapter.GetBrushType)
            {
                case SKPaintAdapter.BrushType.SolidColor:
                    paint.Color = (SKColor)brushPaintAdapter.Brush;
                    break;
                case SKPaintAdapter.BrushType.LinerGradien:
                case SKPaintAdapter.BrushType.ImageBrush:
                    paint.Shader = (SKShader)brushPaintAdapter.Brush;
                    break;
            }

            //canvas.DrawText(str, (float)point.X, (float)(point.Y + font.FontSize), paint);
            var centerYtoBaseline = (paint.FontMetrics.Descent-paint.FontMetrics.Ascent)/2-paint.FontMetrics.Descent;
            canvas.DrawText(str, (float)point.X, (float)(point.Y+fontAdapter.FontHeight/2+centerYtoBaseline), paint);
        }

        public override IRGraphicsPath GetGraphicsPath()
        {
            return new SKPaintAdapter();
        }

        public override IRBrush GetTextureBrush(IRImage image, RRect dstRect, RPoint translateTransformLocation)
        {
            throw new NotImplementedException();
        }

        public override RSize MeasureString(string str, IRFont font)
        {
            var fontAdapter = ((SKPaintAdapter)font);
            var width = fontAdapter.Paint.MeasureText(str);

            /*if (font.FontHeight < 0)
            {
                var height = fontAdapter.FontSize;
                var descent = fontAdapter.Paint.FontMetrics.Descent;
                fontAdapter.UnderlineOffset = (int)Math.Round((height - descent + 1f));
            }*/
            //return Utils.Convert(new SKSize(width, (float)font.FontHeight));
            return Utils.Convert(new SKSize(width, (float)fontAdapter.FontHeight));
        }

        public override void MeasureString(string str, IRFont font, double maxWidth, out int charFit, out double charFitWidth)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The clipping bound stack as clips are pushed/poped to/from the graphics
        /// </summary>
        protected readonly Stack<RRect> _clipStack = new Stack<RRect>();

        /// <summary>
        /// The suspended clips
        /// </summary>
        private Stack<RRect> _suspendedClips = new Stack<RRect>();

        public override void PopClip()
        {
            canvas.Restore();
            _clipStack.Pop();
        }

        public override void PushClip(RRect rect)
        {
            _clipStack.Push(rect);
            canvas.Save();
            canvas.ClipRect(Utils.Convert(rect));
        }

        public override void PushClipExclude(RRect rect)
        {
            var path = new SKPath();
            path.AddRect(Utils.Convert(_clipStack.Peek()));
            path.AddRect(Utils.Convert(rect));
            path.Close();

            _clipStack.Push(_clipStack.Peek());
            canvas.ClipPath(path);
        }

        public override void ReturnPreviousSmoothingMode(object prevMode)
        {
            //throw new NotImplementedException();
        }

        public override object SetAntiAliasSmoothingMode()
        {
            //Paint.IsAntialias = true;
            return null;
        }

        public override void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}