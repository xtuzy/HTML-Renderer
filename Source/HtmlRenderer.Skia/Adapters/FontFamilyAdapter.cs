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

using TheArtOfDev.HtmlRenderer.Adapters;
//using PdfSharp.Drawing;

namespace TheArtOfDev.HtmlRenderer.PdfSharp.Adapters
{
    /// <summary>
    /// Adapter for WinForms Font object for core.
    /// </summary>
    internal sealed class FontFamilyAdapter : IRFontFamily
    {
        /// <summary>
        /// the underline win-forms font.
        /// </summary>
        private readonly SKTypeface _fontFamily;

        /// <summary>
        /// Init.
        /// </summary>
        public FontFamilyAdapter(SKTypeface fontFamily)
        {
            _fontFamily = fontFamily;
        }

        /// <summary>
        /// the underline win-forms font family.
        /// </summary>
        public SKTypeface FontFamily
        {
            get { return _fontFamily; }
        }

        public  string FontName
        {
            get { return _fontFamily.FamilyName; }
        }
    }
}