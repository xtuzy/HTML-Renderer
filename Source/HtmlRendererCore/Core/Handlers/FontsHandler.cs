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

using System;
using System.Collections.Generic;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.Core.Utils;

namespace TheArtOfDev.HtmlRenderer.Core.Handlers
{
    /// <summary>
    /// Utilities for fonts and fonts families handling.
    /// </summary>
    internal sealed class FontsHandler
    {
        #region Fields and Consts

        /// <summary>
        /// 
        /// </summary>
        private readonly RAdapter _adapter;

        /// <summary>
        /// Allow to map not installed fonts to different
        /// </summary>
        private readonly Dictionary<string, string> _fontsMapping = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// collection of all installed and added font families to check if font exists
        /// </summary>
        private readonly Dictionary<string, IRFontFamily> _existingFontFamilies = new Dictionary<string, IRFontFamily>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// cache of all the font used not to create same font again and again
        /// </summary>
        private readonly Dictionary<string, Dictionary<double, Dictionary<RFontStyle, IRFont>>> _fontsCache = new Dictionary<string, Dictionary<double, Dictionary<RFontStyle, IRFont>>>(StringComparer.InvariantCultureIgnoreCase);

        #endregion

        /// <summary>
        /// Init.
        /// </summary>
        public FontsHandler(RAdapter adapter)
        {
            ArgChecker.AssertArgNotNull(adapter, "global");

            _adapter = adapter;
        }

        /// <summary>
        /// Check if the given font family exists by name
        /// </summary>
        /// <param name="family">the font to check</param>
        /// <returns>true - font exists by given family name, false - otherwise</returns>
        public bool IsFontExists(string family)
        {
            bool exists = _existingFontFamilies.ContainsKey(family);
            if (!exists)
            {
                string mappedFamily;
                if (_fontsMapping.TryGetValue(family, out mappedFamily))
                {
                    exists = _existingFontFamilies.ContainsKey(mappedFamily);
                }
            }
            return exists;
        }

        /// <summary>
        /// Adds a font family to be used.
        /// </summary>
        /// <param name="fontFamily">The font family to add.</param>
        public void AddFontFamily(IRFontFamily fontFamily)
        {
            ArgChecker.AssertArgNotNull(fontFamily, "family");

            _existingFontFamilies[fontFamily.FontName] = fontFamily;
        }

        /// <summary>
        /// Adds a font mapping from <paramref name="fromFamily"/> to <paramref name="toFamily"/> iff the <paramref name="fromFamily"/> is not found.<br/>
        /// When the <paramref name="fromFamily"/> font is used in rendered html and is not found in existing 
        /// fonts (installed or added) it will be replaced by <paramref name="toFamily"/>.<br/>
        /// </summary>
        /// <param name="fromFamily">the font family to replace</param>
        /// <param name="toFamily">the font family to replace with</param>
        public void AddFontFamilyMapping(string fromFamily, string toFamily)
        {
            ArgChecker.AssertArgNotNullOrEmpty(fromFamily, "fromFamily");
            ArgChecker.AssertArgNotNullOrEmpty(toFamily, "toFamily");

            _fontsMapping[fromFamily] = toFamily;
        }

        /// <summary>
        /// Get cached font instance for the given font properties.<br/>
        /// Improve performance not to create same font multiple times.
        /// </summary>
        /// <returns>cached font instance</returns>
        public IRFont GetCachedFont(string family, double size, RFontStyle style)
        {
            //先尝试从缓存获取字体
            var font = TryGetFont(family, size, style);
            if (font == null)
            {
                //如果没有这个字体的记录
                if (!_existingFontFamilies.ContainsKey(family))
                {
                    //从可替换字体中获取
                    string mappedFamily;
                    if (_fontsMapping.TryGetValue(family, out mappedFamily))
                    {
                        font = TryGetFont(mappedFamily, size, style);
                        if (font == null)
                        {
                            font = CreateFont(mappedFamily, size, style);
                            _fontsCache[mappedFamily][size][style] = font;
                        }
                    }
                }

                if (font == null)
                {
                    font = CreateFont(family, size, style);
                }

                _fontsCache[family][size][style] = font;
            }
            return font;
        }

        #region Private methods

        /// <summary>
        /// Get cached font if it exists in cache or null if it is not.
        /// </summary>
        private IRFont TryGetFont(string family, double size, RFontStyle style)
        {
            IRFont font = null;
            if (_fontsCache.ContainsKey(family))
            {
                var a = _fontsCache[family];
                if (a.ContainsKey(size))
                {
                    var b = a[size];
                    if (b.ContainsKey(style))
                    {
                        font = b[style];
                    }
                }
                else
                {
                    _fontsCache[family][size] = new Dictionary<RFontStyle, IRFont>();
                }
            }
            else
            {
                _fontsCache[family] = new Dictionary<double, Dictionary<RFontStyle, IRFont>>();
                _fontsCache[family][size] = new Dictionary<RFontStyle, IRFont>();
            }
            return font;
        }

        /// <summary>
        // create font (try using existing font family to support custom fonts)
        /// </summary>
        private IRFont CreateFont(string family, double size, RFontStyle style)
        {
            IRFontFamily fontFamily;
            try
            {
                return _existingFontFamilies.TryGetValue(family, out fontFamily)
                    ? _adapter.CreateFont(fontFamily, size, style)
                    : _adapter.CreateFont(family, size, style);
            }
            catch
            {
                // handle possibility of no requested style exists for the font, use regular then
                return _existingFontFamilies.TryGetValue(family, out fontFamily)
                    ? _adapter.CreateFont(fontFamily, size, RFontStyle.Regular)
                    : _adapter.CreateFont(family, size, RFontStyle.Regular);
            }
        }

        #endregion
    }
}