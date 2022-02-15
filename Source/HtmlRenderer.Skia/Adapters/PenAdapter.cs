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
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
//using PdfSharp.Drawing;

namespace TheArtOfDev.HtmlRenderer.PdfSharp.Adapters
{
    /// <summary>
    /// Adapter for WinForms pens objects for core.
    /// </summary>
    internal sealed class PenAdapter : IRPen
    {
        /// <summary>
        /// The actual WinForms brush instance.
        /// </summary>
        private readonly SKPaint _pen;

        /// <summary>
        /// Init.
        /// </summary>
        public PenAdapter(SKPaint pen)
        {
            _pen = pen;
        }

        /// <summary>
        /// The actual WinForms brush instance.
        /// </summary>
        public SKPaint Pen
        {
            get { return _pen; }
        }

        public  double PenWidth
        {
            get { return _pen.StrokeWidth; }
            set { _pen.StrokeWidth = (float)value; }
        }

        public  RDashStyle DashStyle
        {
            set
            {
                switch (value)
                {
                    case RDashStyle.Solid:
                        //_pen.DashStyle = XDashStyle.Solid;

                        break;
                    case RDashStyle.Dash:
                        //_pen.DashStyle = XDashStyle.Dash;
                        _pen.PathEffect = SKPathEffect.CreateDash(new float[] { _pen.StrokeWidth * 3, _pen.StrokeWidth }, _pen.StrokeWidth*2);
                        //if (Width < 2)
                        //    _pen.DashPattern = new[] { 4, 4d }; // better looking
                        break;
                    case RDashStyle.Dot:
                        //_pen.DashStyle = XDashStyle.Dot;
                        _pen.PathEffect = SKPathEffect.CreateDash(new float[] { _pen.StrokeWidth, _pen.StrokeWidth }, _pen.StrokeWidth*2);
                        break;
                    case RDashStyle.DashDot:
                        //_pen.DashStyle = XDashStyle.DashDot;
                        _pen.PathEffect = SKPathEffect.CreateDash(new float[] { _pen.StrokeWidth * 3, _pen.StrokeWidth, _pen.StrokeWidth, _pen.StrokeWidth }, _pen.StrokeWidth*2);
                        break;
                    case RDashStyle.DashDotDot:
                        //_pen.DashStyle = XDashStyle.DashDotDot;
                        _pen.PathEffect = SKPathEffect.CreateDash(new float[] {_pen.StrokeWidth*3,_pen.StrokeWidth,_pen.StrokeWidth,_pen.StrokeWidth,_pen.StrokeWidth,_pen.StrokeWidth}, _pen.StrokeWidth*2);
                        break;
                    case RDashStyle.Custom:
                        //_pen.DashStyle = XDashStyle.Custom;
                        break;
                    default:
                        //_pen.DashStyle = XDashStyle.Solid;
                        break;
                }
            }
        }
    }
}