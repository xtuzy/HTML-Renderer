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

using HtmlRendererCore.Skia.Adapters;
using System.ComponentModel;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.Core;

namespace HtmlRendererCore.Skia.Maui
{
    /// <summary>
    /// Provides HTML rendering using the text property.<br/>
    /// WPF control that will render html content in it's client rectangle.<br/>
    /// Using <see cref="AutoSize"/> and <see cref="AutoSizeHeightOnly"/> client can control how the html content effects the
    /// size of the label. Either case scrollbars are never shown and html content outside of client bounds will be clipped.
    /// MaxWidth/MaxHeight and MinWidth/MinHeight with AutoSize can limit the max/min size of the control<br/>
    /// The control will handle mouse and keyboard events on it to support html text selection, copy-paste and mouse clicks.<br/>
    /// </summary>
    /// <remarks>
    /// See <see cref="HtmlControl"/> for more info.
    /// </remarks>
    public class HtmlLabel : HtmlControl
    {
        #region Dependency properties

        public static readonly BindableProperty AutoSizeProperty = BindableProperty.Create("AutoSize", typeof(bool), typeof(HtmlLabel), true,
            propertyChanged: (bindable,oldValue,newValue)=> OnDependencyProperty_ValueChanged(bindable, oldValue, newValue, AutoSizeProperty));
        public static readonly BindableProperty AutoSizeHeightOnlyProperty = BindableProperty.Create("AutoSizeHeightOnly", typeof(bool), typeof(HtmlLabel), false,
            propertyChanged: (bindable, oldValue, newValue) => OnDependencyProperty_ValueChanged(bindable, oldValue, newValue, AutoSizeHeightOnlyProperty));

        #endregion

        /// <summary>
        /// Init.
        /// </summary>
        static HtmlLabel()
        {
            //BackgroundProperty.OverrideMetadata(typeof(HtmlLabel), new FrameworkPropertyMetadata(Brushes.Transparent));
        }

        /// <summary>
        /// Automatically sets the size of the label by content size
        /// </summary>
        [Category("Layout")]
        [Description("Automatically sets the size of the label by content size.")]
        public bool AutoSize
        {
            get { return (bool)GetValue(AutoSizeProperty); }
            set { SetValue(AutoSizeProperty, value); }
        }

        /// <summary>
        /// Automatically sets the height of the label by content height (width is not effected).
        /// </summary>
        [Category("Layout")]
        [Description("Automatically sets the height of the label by content height (width is not effected)")]
        public virtual bool AutoSizeHeightOnly
        {
            get { return (bool)GetValue(AutoSizeHeightOnlyProperty); }
            set { SetValue(AutoSizeHeightOnlyProperty, value); }
        }

        #region Private methods
        /// <summary>
        /// Perform the layout of the html in the control.
        /// </summary>
        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            //调用基类实现获得从WidthRequest等计算出的大小
            var constraint = base.MeasureOverride(widthConstraint, heightConstraint);
            if (constraint.Width == 0)//没有设置宽时
                constraint.Width = widthConstraint;
            if(constraint.Height == 0)//没有设置高时
                constraint.Height = heightConstraint;
            if (_htmlContainer != null)
            {
                using (var ig = new SKGraphicsAdapter())
                {
                    //var horizontal = Padding.Left + Padding.Right + BorderThickness.Left + BorderThickness.Right;
                    var horizontal = 0;
                    //var vertical = Padding.Top + Padding.Bottom + BorderThickness.Top + BorderThickness.Bottom;
                    var vertical = 0;

                    var size = new RSize(constraint.Width < Double.PositiveInfinity ? constraint.Width - horizontal : 0, constraint.Height < Double.PositiveInfinity ? constraint.Height - vertical : 0);
                    var minSize = new RSize(MinimumWidthRequest < Double.PositiveInfinity ? MinimumWidthRequest - horizontal : 0, MinimumHeightRequest < Double.PositiveInfinity ? MinimumHeightRequest - vertical : 0);
                    var maxSize = new RSize(MaximumWidthRequest < Double.PositiveInfinity ? MaximumWidthRequest- horizontal : 0, MaximumHeightRequest < Double.PositiveInfinity ? MaximumHeightRequest - vertical : 0);

                    var newSize = HtmlRendererUtils.Layout(ig, _htmlContainer.HtmlContainerInt, size, minSize, maxSize, AutoSize, AutoSizeHeightOnly);

                    constraint = new Size(newSize.Width + horizontal, newSize.Height + vertical);
                }
            }

            if (double.IsPositiveInfinity(constraint.Width) || double.IsPositiveInfinity(constraint.Height))
                constraint = Size.Zero;
            //参考:https://stackoverflow.com/questions/71886088/customcontrol-calculate-own-width-and-height-arrangeoverride-gets-called-with
            this.DesiredSize = constraint;
            return constraint;
        }

        /// <summary>
        /// Handle when dependency property value changes to update the underline HtmlContainer with the new value.
        /// </summary>
        private static void OnDependencyProperty_ValueChanged(BindableObject bindable, object oldValue, object newValue, BindableProperty property)
        {
            var control = bindable as HtmlLabel;
            if (control != null)
            {
                if (property == AutoSizeProperty)
                {
                    if ((bool)newValue)
                    {
                        bindable.SetValue(AutoSizeHeightOnlyProperty, false);
                        control.InvalidateMeasure();
                        control.InvalidateSurface();
                    }
                }
                else if (property == AutoSizeHeightOnlyProperty)
                {
                    if ((bool)newValue)
                    {
                        bindable.SetValue(AutoSizeProperty, false);
                        control.InvalidateMeasure();
                        control.InvalidateSurface();
                    }
                }
            }
        }

        #endregion
    }
}