﻿// "Therefore those skilled at the unorthodox
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
        private readonly SKBitmap _image;

        /// <summary>
        /// the
        /// </summary>
        private readonly SKRect _dstRect;

        /// <summary>
        /// the transform the location of the image to handle center alignment
        /// </summary>
        private readonly SKPoint _translateTransformLocation;

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        public XTextureBrush(SKBitmap image, SKRect dstRect, SKPoint translateTransformLocation)
        {
            _image = image;
            _dstRect = dstRect;
            _translateTransformLocation = translateTransformLocation;
        }

        /// <summary>
        /// Draw the texture image in the given graphics at the given location.
        /// </summary>
        public void DrawRectangle(SKCanvas g, float x, float y, float width, float height)
        {
            var prevState = g.Save();
            //g.IntersectClip(new SKRect(x, y, width, height));
            g.ClipRect(SKRect.Create(x, y, width, height));

            var rx = _translateTransformLocation.X;
            var w = _image.Width; var h = _image.Height;
            while (rx < x + width)
            {
                var ry = _translateTransformLocation.Y;
                while (ry < y + height)
                {
                    g.DrawBitmap(_image, SKRect.Create(rx, ry, w, h));
                    ry += h;
                }
                rx += w;
            }

            //g.Restore(prevState);
            g.Restore();
        }
    }
}