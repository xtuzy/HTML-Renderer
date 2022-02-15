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
    /// Adapter for WinForms Image object for core.
    /// </summary>
    internal sealed class ImageAdapter : IRImage
    {
        /// <summary>
        /// the underline win-forms image.
        /// </summary>
        private readonly SKBitmap _image;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ImageAdapter(SKBitmap image)
        {
            _image = image;
        }

        /// <summary>
        /// the underline win-forms image.
        /// </summary>
        public SKBitmap Image
        {
            get { return _image; }
        }

        public  double ImageWidth
        {
            get { return _image.Width; }
        }

        public  double ImageHeight
        {
            get { return _image.Height; }
        }

        public  void Dispose()
        {
            _image.Dispose();
        }
    }
}