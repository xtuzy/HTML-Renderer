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
                {"DroidSans.ttf", "DroidSans"},
                {"YouYuan.ttf", "YouYuan"},
            };
            foreach (var fontFile in fontFiles)
            {
                // add the fonts to renderer
                HtmlRender.AddMauiFont(fontFile.Key, fontFile.Value);
                HtmlRender.AddFontFamily(fontFile.Value);
            }

            var defaultChineseFont = SKFontManager.Default.MatchCharacter('中');
            var defaultEnglishFont = SKFontManager.Default.MatchCharacter('A');
            var emojiChar = StringUtilities.GetUnicodeCharacterCode("🚀", SKTextEncoding.Utf32);
            var defaultEmojiFont = SKFontManager.Default.MatchCharacter(emojiChar);
            //字体替代
            HtmlRender.AddFontFamilyMapping("monospace", "Courier New");
            HtmlRender.AddFontFamilyMapping("Helvetica", "Arial");
            if (System.OperatingSystem.IsAndroid())
                HtmlRender.AddFontFamilyMapping("Segoe UI", defaultChineseFont.FamilyName);
            if (System.OperatingSystem.IsIOS() || System.OperatingSystem.IsMacCatalyst() || System.OperatingSystem.IsMacOS())
                HtmlRender.AddFontFamilyMapping("Segoe UI", defaultChineseFont.FamilyName);
        }

    }
}
