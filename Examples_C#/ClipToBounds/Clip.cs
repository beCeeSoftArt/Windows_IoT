/* Copyright André Spitzner 2003 - 2016 */
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace bcplanet.IOT.Base.Base
{
    /// <summary>
    /// Append a property local:Clip.ToBounds="True" 
    /// </summary>
    public class Clip
    {
        /// <summary>
        /// Gets to bounds.
        /// </summary>
        /// <param name="depObj">The dep object.</param>
        /// <returns></returns>
        public static bool GetToBounds(DependencyObject depObj)
        {
            return (bool)depObj.GetValue(ToBoundsProperty);
        }

        /// <summary>
        /// Sets to bounds.
        /// </summary>
        /// <param name="depObj">The dep object.</param>
        /// <param name="clipToBounds">if set to <c>true</c> [clip to bounds].</param>
        public static void SetToBounds(DependencyObject depObj, bool clipToBounds)
        {
            depObj.SetValue(ToBoundsProperty, clipToBounds);
        }

        /// <summary>
        /// To bounds property
        /// </summary>
        public static readonly DependencyProperty ToBoundsProperty =
            DependencyProperty.RegisterAttached("ToBounds", typeof(bool),
            typeof(Clip), new PropertyMetadata(false, OnToBoundsPropertyChanged));

        /// <summary>
        /// Called when [to bounds property changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnToBoundsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;
            if (frameworkElement != null)
            {
                ClipToBounds(frameworkElement);

                // whenever the element which this property is attached to is loaded
                // or re-sizes, we need to update its clipping geometry
                frameworkElement.Loaded += FrameworkElementLoaded;
                frameworkElement.SizeChanged += FrameworkElementSizeChanged;
            }
        }

        /// <summary>
        /// Creates a rectangular clipping geometry which matches the geometry of the
        /// passed element
        /// </summary>
        /// <param name="frameworkElement">The frameworkElement.</param>
        private static void ClipToBounds(FrameworkElement frameworkElement)
        {
            if (GetToBounds(frameworkElement))
            {
                frameworkElement.Clip = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, frameworkElement.ActualWidth, frameworkElement.ActualHeight)
                };
            }
            else
                frameworkElement.Clip = null;
        }

        /// <summary>
        /// Handles the SizeChanged event of the frameworkElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private static void FrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClipToBounds(sender as FrameworkElement);
        }

        /// <summary>
        /// Handles the Loaded event of the frameworkElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private static void FrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            ClipToBounds(sender as FrameworkElement);
        }
    }
}
