/* Copyright André Spitzner 2003 - 2016 */
using System;
using System.Windows.Input;

namespace bcplanet.IOT.Base.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// The execute method
        /// </summary>
        readonly Action<object> _Execute;

        /// <summary>
        /// The can execute method
        /// </summary>
        readonly Predicate<object> _CanExecute;

        #endregion 

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _Execute = execute;
            _CanExecute = canExecute;
        }
        #endregion

        #region ICommand Members

        /// <summary>
        /// Definiert die Methode, die bestimmt, ob der Befehl im aktuellen Zustand ausgeführt werden kann.
        /// </summary>
        /// <param name="parameter">Vom Befehl verwendete Daten.Wenn der Befehl keine Datenübergabe erfordert, kann das Objekt auf null festgelegt werden.</param>
        /// <returns>
        /// true, wenn der Befehl ausgeführt werden kann, andernfalls false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return _CanExecute == null || _CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Definiert die Methode, die aufgerufen wird, wenn der Befehl aufgerufen wird.
        /// </summary>
        /// <param name="parameter">Vom Befehl verwendete Daten.Wenn der Befehl keine Datenübergabe erfordert, kann das Objekt auf null festgelegt werden.</param>
        public void Execute(object parameter)
        {
            _Execute(parameter);
        }

        #endregion
    }

}
