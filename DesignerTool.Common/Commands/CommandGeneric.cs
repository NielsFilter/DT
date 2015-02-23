using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DesignerTool.Common.Mvvm.Commands
{
    /// <summary>
    /// This base command is a wrapper of the <see cref="ICommand"/> interface.
    /// The two main implementations here are the Execute (handles work) and CanExcecute (determines whether a command is allowed or not).
    /// This is similar to <see cref="Command"/>, except that it carries a generic type. The generic allows you to pass in a parameter to the command.
    /// <example>
    /// Example: In a list, you only want the delete to be enabled if there is a selected item:
    /// var DeleteItemCommand = new Command&lt;MyItem&gt;(deleteItem, (p) => SelectedItem != null);
    /// In the above code, the deleteItem method will be called with a parameter of type 'MyItem' when SelectedItem != null and the command will be Disabled if no item was selected.
    /// </example>
    /// <typeparam name="T">The type of parameter that is passed to it when the command executes.</typeparam>
    /// </summary>
    public class Command<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _handler;

        #region Constructors

        public Command(Action<T> handler, Func<T, bool> canExecute)
            : this(handler)
        {
            this._canExecute = canExecute;
        }

        public Command(Action<T> handler, bool canExecute)
            : this(handler)
        {
            this._canExecute = new Func<T, bool>((parameter) => canExecute);
        }

        public Command(Action<T> handler)
        {
            this._handler = handler;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Implementation of <see cref="ICommand"/>. Checks whether the command can be excecuted or not.
        /// If it cannot execute, then the control it is bound to will be disabled. Pass null in if it can always execute.
        /// </summary>
        /// <param name="parameter">Not needed currently, but <see cref="ICommand"/> forces it.</param>
        /// <returns>True = the command enabled / False = the command is disabled</returns>
        public bool CanExecute(object parameter)
        {
            if (parameter != null && parameter.GetType() != typeof(T))
            {
                throw new ArgumentException("parameter must be of type " + typeof(T).ToString());
            }
            return _canExecute == null ? true : this._canExecute((T)parameter);
        }

        /// <summary>
        /// Excecutes the Command.
        /// </summary>
        /// <param name="parameter">This parameter is of type <typeparamref name="T"/>, it is converted to <typeparamref name="T"/> and the command is executed.</param>
        public void Execute(object parameter)
        {
            if (parameter != null && parameter.GetType() != typeof(T))
            {
                throw new ArgumentException("parameter must be of type " + typeof(T).ToString());
            }
            this._handler((T)parameter);
        }

        /// <summary>
        /// First checks if the command can execute and then executes it.
        /// </summary>
        /// <param name="parameter"></param>
        public void ExecuteIfAllowed(T parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.Execute(parameter);
            }
        }

        #endregion

        #region Public Events

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
