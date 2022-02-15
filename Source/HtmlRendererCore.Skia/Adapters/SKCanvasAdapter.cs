using HtmlRendererCore.Skia.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace HtmlRendererCore.Skia.Adapters
{
    internal class SKCanvasAdapter : RAdapter
    {
        #region Fields and Consts

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        private static readonly SKCanvasAdapter _instance = new SKCanvasAdapter();

        #endregion

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        public static SKCanvasAdapter Instance
        {
            get { return _instance; }
        }

        protected override IRImage ConvertImageInt(object image)
        {
            return image != null ? new SKPaintAdapter((SKBitmap)image) : null;
        }

        protected override IRFont CreateFontInt(string family, double size, RFontStyle style)
        {
            var fontStyle = SKFontStyle.Normal;
            switch (style)
            {
                case RFontStyle.Regular:
                    break;
                case RFontStyle.Bold:
                    fontStyle = SKFontStyle.Bold;
                    break;
                case RFontStyle.Italic:
                    fontStyle = SKFontStyle.Italic;
                    break;
                case RFontStyle.Underline:
                    break;
                case RFontStyle.Strikeout:
                    break;
            }

            var typeface = SKTypeface.FromFamilyName(family, fontStyle);
            return new SKPaintAdapter(typeface, (int)size);
        }

        protected override IRFont CreateFontInt(IRFontFamily family, double size, RFontStyle style)
        {
            return CreateFontInt(family.FontName, size, style);
        }

        protected override IRBrush CreateLinearGradientBrush(RRect rect, RColor color1, RColor color2, double angle)
        {
            return new SKPaintAdapter(Utils.Convert(color1));
        }

        protected override IRPen CreatePen(RColor color)
        {
            return new SKPaintAdapter(Utils.Convert(color));
        }

        protected override IRBrush CreateSolidBrush(RColor color)
        {
            return new SKPaintAdapter(Utils.Convert(color));
        }

        protected override RColor GetColorInt(string colorName)
        {
            try
            {
                var color = Utils.FromName(colorName);
                return Utils.Convert(color);
            }
            catch
            {
                return RColor.Empty;
            }
        }

        protected override IRImage ImageFromStreamInt(Stream memoryStream)
        {
            return new SKPaintAdapter(SKBitmap.Decode(memoryStream));
        }
    }
}
