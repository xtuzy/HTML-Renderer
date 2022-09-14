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
using System.Collections.Generic;
using System.Reflection;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace HtmlRendererCore.Skia.Utilities
{
    /// <summary>
    /// Utilities for converting WinForms entities to HtmlRenderer core entities.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Convert from WinForms point to core point.
        /// </summary>
        public static RPoint Convert(SKPoint p)
        {
            return new RPoint(p.X, p.Y);
        }

        /// <summary>
        /// Convert from WinForms point to core point.
        /// </summary>
        public static SKPoint[] Convert(RPoint[] points)
        {
            SKPoint[] myPoints = new SKPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
                myPoints[i] = Convert(points[i]);
            return myPoints;
        }

        /// <summary>
        /// Convert from core point to WinForms point.
        /// </summary>
        public static SKPoint Convert(RPoint p)
        {
            return new SKPoint((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Convert from WinForms size to core size.
        /// </summary>
        public static RSize Convert(SKSize s)
        {
            return new RSize(s.Width, s.Height);
        }

        /// <summary>
        /// Convert from core size to WinForms size.
        /// </summary>
        public static SKSize Convert(RSize s)
        {
            return new SKSize((float)s.Width, (float)s.Height);
        }

        /// <summary>
        /// Convert from WinForms rectangle to core rectangle.
        /// </summary>
        public static RRect Convert(SKRect r)
        {
            return new RRect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from core rectangle to WinForms rectangle.
        /// </summary>
        public static SKRect Convert(RRect r)
        {
            return SKRect.Create((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
        }

        /// <summary>
        /// Convert from core color to WinForms color.
        /// </summary>
        public static SKColor Convert(RColor c)
        {
            return new SKColor(c.R, c.G, c.B, c.A);
        }

        /// <summary>
        /// Convert from  color to WinForms color.
        /// </summary>
        public static RColor Convert(SKColor c)
        {
            return RColor.FromArgb(c.Alpha, c.Red, c.Green, c.Blue);
        }

        static Dictionary<string,SKColor> ValidColorNamesLc;
        public static SKColor FromName(string colorName)
        {
            if(ValidColorNamesLc == null)
            {
                ValidColorNamesLc = new Dictionary<string, SKColor>();
                var colorList = new List<FieldInfo>(typeof(SKColors).GetFields());
                foreach (var colorProp in colorList)
                {
         
                    ValidColorNamesLc.Add(colorProp.Name.ToLower(), (SKColor)colorProp.GetValue(SKColor.Empty));
                }
            }

            // check if color name is valid to avoid ColorConverter throwing an exception
            if (!ValidColorNamesLc.ContainsKey(colorName.ToLower()))
                return SKColor.Empty;
            return ValidColorNamesLc[colorName];
        }
    }
}