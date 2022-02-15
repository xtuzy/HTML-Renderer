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

using System.Windows.Media.Imaging;
using TheArtOfDev.HtmlRenderer.Adapters;

namespace TheArtOfDev.HtmlRenderer.WPF.Adapters
{
    /// <summary>
    /// Adapter for WPF Image object for core.
    /// </summary>
    internal sealed class ImageAdapter : IRImage
    {
        /// <summary>
        /// the underline WPF image.
        /// </summary>
        private readonly BitmapImage _image;

        /// <summary>
        /// Init.
        /// </summary>
        public ImageAdapter(BitmapImage image)
        {
            _image = image;
        }

        /// <summary>
        /// the underline WPF image.
        /// </summary>
        public BitmapImage Image
        {
            get { return _image; }
        }
        #region interface
        public double ImageWidth
        {
            get { return _image.PixelWidth; }
        }

        public  double ImageHeight
        {
            get { return _image.PixelHeight; }
        }

        public  void Dispose()
        {
            if (_image.StreamSource != null)
                _image.StreamSource.Dispose();
        }
        #endregion
    }
}