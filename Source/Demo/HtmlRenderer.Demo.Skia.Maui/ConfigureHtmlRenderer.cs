using HtmlRendererCore.Skia.Maui;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlRenderer.Demo.Skia.Maui
{
    internal class ConfigureHtmlRenderer
    {
        /// <summary>
        /// Load custom fonts to be used by renderer HTMLs
        /// </summary>
        internal static async void LoadCustomFonts()
        {
            // load custom font font into private fonts collection
            Dictionary<string,string> fontFiles =new Dictionary<string, string>()
            {
                { "CustomFont.ttf","1 Smoothy DNA"},
                {"OpenSans-Regular.ttf", "OpenSansRegular"},
                {"OpenSans-Semibold.ttf", "OpenSansSemibold"},
            };
            foreach (var fontFile in fontFiles)
            {
                // add the fonts to renderer
                HtmlRender.AddMauiFont(fontFile.Key, fontFile.Value);
                HtmlRender.AddFontFamily(fontFile.Value);
            }
        }
    }
}
