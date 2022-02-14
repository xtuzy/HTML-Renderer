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

namespace TheArtOfDev.HtmlRenderer.PdfSharp.Adapters
{
    /// <summary>
    /// Because PdfSharp doesn't support texture brush we need to implement it ourselves.
    /// </summary>
    internal sealed class XTextureBrush
    {
        #region Fields/Consts

        /// <summary>
        /// The image to draw in the brush
        /// </summary>
        private readonly XImage _image;

        /// <summary>
        /// the
        /// </summary>
        private readonly XRect _dstRect;

        /// <summary>
        /// the transform the location of the image to handle center alignment
        /// </summary>
        private readonly XPoint _translateTransformLocation;

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        public XTextureBrush(XImage image, XRect dstRect, XPoint translateTransformLocation)
        {
            _image = image;
            _dstRect = dstRect;
            _translateTransformLocation = translateTransformLocation;
        }

        /// <summary>
        /// Draw the texture image in the given graphics at the given location.
        /// </summary>
        public void DrawRectangle(XGraphics g, float x, float y, float width, float height)
        {
            var prevState = g.Save();
            //g.IntersectClip(new XRect(x, y, width, height));
            g.ClipRect(XRect.Create(x, y, width, height));

            var rx = _translateTransformLocation.X;
            var w = _image.Width; var h = _image.Height;
            while (rx < x + width)
            {
                var ry = _translateTransformLocation.Y;
                while (ry < y + height)
                {
                    g.DrawBitmap(_image, XRect.Create(rx, ry, w, h));
                    ry += h;
                }
                rx += w;
            }

            //g.Restore(prevState);
            g.Restore();
        }
    }
}