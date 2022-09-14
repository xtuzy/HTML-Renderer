using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace HtmlRendererCore.Skia.Maui.Utilities
{
    static class Utils
    {
        /// <summary>
        /// Convert from core point to WPF point.
        /// </summary>
        public static SKPoint Convert(RPoint p)
        {
            return new SKPoint((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Convert from core point to WPF point.
        /// </summary>
        public static SKPoint ConvertRound(RPoint p)
        {
            return new SKPoint((int)p.X, (int)p.Y);
        }

        /// <summary>
        /// Convert from WPF size to core size.
        /// </summary>
        public static RSize Convert(SKSize s)
        {
            return new RSize(s.Width, s.Height);
        }

        /// <summary>
        /// Convert from core size to WPF size.
        /// </summary>
        public static SKSize Convert(RSize s)
        {
            return new SKSize((float)s.Width, (float)s.Height);
        }

        /// <summary>
        /// Convert from core point to WPF point.
        /// </summary>
        public static SKSize ConvertRound(RSize s)
        {
            return new SKSize((int)s.Width, (int)s.Height);
        }
    }
}
