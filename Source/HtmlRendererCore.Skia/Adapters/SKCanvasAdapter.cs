using HtmlRendererCore.Skia.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace HtmlRendererCore.Skia.Adapters
{
    public class SKCanvasAdapter : RAdapter
    {
        #region Fields and Consts

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        private static readonly SKCanvasAdapter _instance = new SKCanvasAdapter();

        internal SKCanvasAdapter()
        {
            
            AddFontFamilyMapping("monospace", "Courier New");
            AddFontFamilyMapping("Helvetica", "Arial");
            if (System.OperatingSystem.IsAndroid())
                AddFontFamilyMapping("Segue UI", "Robote");
            if (System.OperatingSystem.IsIOS() || System.OperatingSystem.IsMacCatalyst() || System.OperatingSystem.IsMacOS())
                AddFontFamilyMapping("Segue UI", "Helvetica");

            //Add font
            //if (!System.OperatingSystem.IsIOS())//seems ios can't get font
            //{
            //    string fontsfolder =Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            //    var fontFiles = Directory.EnumerateFiles(fontsfolder);
            //    foreach (var fontFile in fontFiles)
            //    {
            //        try
            //        {
            //            if (!fontFile.Contains(".ttf"))
            //                continue;
            //            AddFontFamily(new SKPaintAdapter(SKTypeface.FromFile(fontFile),12));
            //        }
            //        catch
            //        {
            //        }
            //    }
            //}
            //使用Skia读取系统字体
            foreach (var fontName in SKFontManager.Default.FontFamilies)
            {
                //var skTypeface = SKFontManager.Default.MatchFamily(fontName);
                AddFontFamily(new SKPaintAdapter(fontName));
            }
        }

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
                    fontStyle = SKFontStyle.Normal;
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
            if(typeface == null || typeface.FamilyName != family)//Skia会使用默认的字体替代找不到的,所以得到的字体不一致
            {
                var customTypeface = CustomFontFamilyManager.GetFont(family);
                if (customTypeface != null)
                    typeface = customTypeface;
                else
                    typeface = null;
            }
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
