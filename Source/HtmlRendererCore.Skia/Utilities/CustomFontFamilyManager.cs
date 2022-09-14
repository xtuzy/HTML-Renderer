using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlRendererCore.Skia.Utilities
{
    public static class CustomFontFamilyManager
    {
        //private static readonly ConcurrentDictionary<string, WeakReference<SKTypeface>> cache =
        //   new ConcurrentDictionary<string, WeakReference<SKTypeface>>();
        private static readonly ConcurrentDictionary<string, SKTypeface> cache =
           new ConcurrentDictionary<string, SKTypeface>();

        public static SKTypeface GetFont(string fontFamily)
        {
            //if (cache.TryGetValue(fontFamily, out var weak) && weak.TryGetTarget(out var tf))
            if (cache.TryGetValue(fontFamily, out var weak))
                return weak;
            return null;
        }

        /// <summary>
        /// 给Maui的接口
        /// </summary>
        public static void AddFont(SKTypeface typeface, string fontFamily)
        {
            //cache[fontFamily] = new WeakReference<SKTypeface>(typeface);
            cache[fontFamily] = typeface;
        }
    }
}
