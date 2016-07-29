/* Copyright André Spitzner 2003 - 2016 */

using bcplanet.IOT.Base.Base;
using bcplanet.IOT.Base.Enums;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace bcplanet
{
    /// <summary>
    /// With this class you can watch dependency property changes which 
    /// not react over INotifyPropertyChanged event
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Watches the property.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        public static IDisposable WatchProperty(this DependencyObject target,
                                                string propertyPath,
                                                DependencyPropertyChangedEventHandler handler)
        {
            return new DependencyPropertyWatcher(target, propertyPath, handler);
        }

        /// <summary>
        /// Dependency property watcher
        /// </summary>
        /// <seealso cref="Windows.UI.Xaml.DependencyObject" />
        /// <seealso cref="System.IDisposable" />
        public class DependencyPropertyWatcher : DependencyObject, IDisposable
        {
            /// <summary>
            /// The _ handler
            /// </summary>
            private DependencyPropertyChangedEventHandler _Handler;

            /// <summary>
            /// Initializes a new instance of the <see cref="DependencyPropertyWatcher"/> class.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <param name="propertyPath">The property path.</param>
            /// <param name="handler">The handler.</param>
            /// <exception cref="System.ArgumentNullException">
            /// </exception>
            public DependencyPropertyWatcher(DependencyObject target,
                                             string propertyPath,
                                             DependencyPropertyChangedEventHandler handler)
            {
                if (target == null) throw new ArgumentNullException(nameof(target));
                if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));
                if (handler == null) throw new ArgumentNullException(nameof(handler));

                _Handler = handler;

                var binding = new Binding
                {
                    Source = target,
                    Path = new PropertyPath(propertyPath),
                    Mode = BindingMode.OneWay
                };
                BindingOperations.SetBinding(this, ValueProperty, binding);
            }

            /// <summary>
            /// The value property
            /// </summary>
            private static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register(
                    "Value",
                    typeof(object),
                    typeof(DependencyPropertyWatcher),
                    new PropertyMetadata(null, ValuePropertyChanged));

            /// <summary>
            /// Values the property changed.
            /// </summary>
            /// <param name="d">The dependency object.</param>
            /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
            private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                try
                {
                    var watcher = d as DependencyPropertyWatcher;

                    watcher?.OnValueChanged(e);
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(ELogTypes.Error, ex);
                }

            }

            /// <summary>
            /// Raises the <see cref="E:ValueChanged" /> event.
            /// </summary>
            /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
            private void OnValueChanged(DependencyPropertyChangedEventArgs e)
            {
                var handler = _Handler;
                handler?.Invoke(this, e);
            }

            /// <summary>
            /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
            /// </summary>
            public void Dispose()
            {
                _Handler = null;
                // There is no ClearBinding method, so set a empty binding instead to disconnect memory refrerence
                BindingOperations.SetBinding(this, ValueProperty, new Binding());
            }
        }
    }

}
