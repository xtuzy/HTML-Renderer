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

using SkiaSharp;
using System;
using TheArtOfDev.HtmlRenderer.Adapters;
//using PdfSharp.Drawing;

namespace TheArtOfDev.HtmlRenderer.PdfSharp.Adapters
{
    /// <summary>
    /// Adapter for WinForms brushes objects for core.
    /// </summary>
    internal sealed class BrushAdapter : IRBrush
    {
        /// <summary>
        /// The actual PdfSharp brush instance.<br/>
        /// Should be <see cref="SKShader"/> but there is some fucking issue inheriting from it =/
        /// </summary>
        private readonly object _brush;

        /// <summary>
        /// Init.
        /// </summary>
        public BrushAdapter(object brush)
        {
            _brush = brush;
        }

        /// <summary>
        /// The actual WinForms brush instance.
        /// </summary>
        public object Brush
        {
            get { return _brush; }
        }

        public  void Dispose()
        { }
    }
}