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
using System.Drawing;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace TheArtOfDev.HtmlRenderer.PdfSharp.Utilities
{
    /// <summary>
    /// Utilities for converting WinForms entities to HtmlRenderer core entities.
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Convert from WinForms point to core point.
        /// </summary>
        public static RPoint Convert(XPoint p)
        {
            return new RPoint(p.X, p.Y);
        }

        /// <summary>
        /// Convert from WinForms point to core point.
        /// </summary>
        public static XPoint[] Convert(RPoint[] points)
        {
            XPoint[] myPoints = new XPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
                myPoints[i] = Convert(points[i]);
            return myPoints;
        }

        /// <summary>
        /// Convert from core point to WinForms point.
        /// </summary>
        public static XPoint Convert(RPoint p)
        {
            return new XPoint((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Convert from WinForms size to core size.
        /// </summary>
        public static RSize Convert(XSize s)
        {
            return new RSize(s.Width, s.Height);
        }

        /// <summary>
        /// Convert from core size to WinForms size.
        /// </summary>
        public static XSize Convert(RSize s)
        {
            return new XSize((float)s.Width, (float)s.Height);
        }

        /// <summary>
        /// Convert from WinForms rectangle to core rectangle.
        /// </summary>
        public static RRect Convert(XRect r)
        {
            return new RRect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from core rectangle to WinForms rectangle.
        /// </summary>
        public static XRect Convert(RRect r)
        {
            return XRect.Create((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
        }

        /// <summary>
        /// Convert from core color to WinForms color.
        /// </summary>
        public static XColor Convert(RColor c)
        {
            return new XColor(c.R, c.G, c.B, c.A);
        }

        /// <summary>
        /// Convert from  color to WinForms color.
        /// </summary>
        public static RColor Convert(Color c)
        {
            return RColor.FromArgb(c.Alpha, c.Red, c.Green, c.Blue);
        }

        public static Color FromName(string colorName)
        {
            switch (colorName)
            {
                case nameof(SkiaSharp.SKColors.CadetBlue):
                    return SkiaSharp.SKColors.CadetBlue;
                case nameof(SkiaSharp.SKColors.Aqua):
                    return SkiaSharp.SKColors.Aqua;
                case nameof(SkiaSharp.SKColors.Blue):
                    return SkiaSharp.SKColors.Blue;
                default:
                    return SkiaSharp.SKColors.Empty;
            }
        }
    }
}