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
//using PdfSharp.Pdf;
//using System.Drawing;
//using System.Drawing.Text;
using SkiaSharp;
using System.Drawing.Text;
using System.IO;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.PdfSharp.Utilities;

namespace TheArtOfDev.HtmlRenderer.PdfSharp.Adapters
{
    /// <summary>
    /// Adapter for PdfSharp library platform.
    /// </summary>
    internal sealed class PdfSharpAdapter : RAdapter
    {
        #region Fields and Consts

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        private static readonly PdfSharpAdapter _instance = new PdfSharpAdapter();

        #endregion


        /// <summary>
        /// Init color resolve.
        /// </summary>
        private PdfSharpAdapter()
        {
            AddFontFamilyMapping("monospace", "Courier New");
            AddFontFamilyMapping("Helvetica", "Arial");

            var families = new InstalledFontCollection();

            foreach (var family in families.Families)
            {
                AddFontFamily(new FontFamilyAdapter(XFontFamily.FromFamilyName(family.Name)));
            }
        }

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        public static PdfSharpAdapter Instance
        {
            get { return _instance; }
        }

        protected override RColor GetColorInt(string colorName)
        {
            try
            {
                //var color = Color.FromKnownColor((KnownColor)System.Enum.Parse(typeof(KnownColor), colorName, true));
                var color = Utils.FromName(colorName);
                return Utils.Convert(color);
            }
            catch
            {
                return RColor.Empty;
            }
        }

        protected override RPen CreatePen(RColor color)
        {
            return new PenAdapter(new XPen() { Color = Utils.Convert(color) });
        }

        protected override RBrush CreateSolidBrush(RColor color)
        {
            /*XBrush solidBrush;
            if (color == RColor.White)
                solidBrush = XBrushes.White;
            else if (color == RColor.Black)
                solidBrush = XBrushes.Black;
            else if (color.A < 1)
                solidBrush = XBrushes.Transparent;
            else
                solidBrush = new XSolidBrush(Utils.Convert(color));

            return new BrushAdapter(solidBrush);*/
            return new BrushAdapter(null);
        }

        protected override RBrush CreateLinearGradientBrush(RRect rect, RColor color1, RColor color2, double angle)
        {

            //XLinearGradientMode mode;
            //if (angle < 45)
            //    mode = XLinearGradientMode.ForwardDiagonal;
            //else if (angle < 90)
            //    mode = XLinearGradientMode.Vertical;
            //else if (angle < 135)
            //    mode = XLinearGradientMode.BackwardDiagonal;
            //else
            //    mode = XLinearGradientMode.Horizontal;
            //return new BrushAdapter(new XLinearGradientBrush(Utils.Convert(rect), Utils.Convert(color1), Utils.Convert(color2), mode));
            return new BrushAdapter(null);
        }

        protected override RImage ConvertImageInt(object image)
        {
            return image != null ? new ImageAdapter((XImage)image) : null;
        }

        protected override RImage ImageFromStreamInt(Stream memoryStream)
        {
            return new ImageAdapter(SKBitmap.Decode(memoryStream));
        }

        protected override RFont CreateFontInt(string family, double size, RFontStyle style)
        {
            var fontStyle = SKTypefaceStyle.Normal;
            switch (style)
            {
                case RFontStyle.Regular:
                    break;
                case RFontStyle.Bold:
                    fontStyle = SKTypefaceStyle.Bold;
                    break;
                case RFontStyle.Italic:
                    fontStyle = SKTypefaceStyle.Italic;
                    break;
                case RFontStyle.Underline:
                    break;
                case RFontStyle.Strikeout:
                    break;
            }
           
            var xFont = XFont.FromFamilyName(family, fontStyle);
            return new FontAdapter(xFont, (int)size);
        }

        protected override RFont CreateFontInt(RFontFamily family, double size, RFontStyle style)
        {
            return CreateFontInt(family.Name, size, style);
        }
    }
}