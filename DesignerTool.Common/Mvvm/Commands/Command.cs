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
    /// <example>
    /// Example: In a list, you only want the delete to be enabled if there is a selected item:
    /// var DeleteItemCommand = new Command(deleteItem, (p) => SelectedItem != null);
    /// In the above code, the deleteItem method will be called when SelectedItem != null and the command will be Disabled if no item was selected.
    /// </example>
    /// </summary>
    public class Command : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _handler;

        #region Constructors

        public Command(Action handler, Func<bool> canExecute)
            : this(handler)
        {
            this._canExecute = canExecute;
        }

        public Command(Action handler, bool canExecute)
            : this(handler)
        {
            this._canExecute = new Func<bool>(() => canExecute);
        }

        public Command(Action handler)
        {
            this._handler = handler;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Implementation of <see cref="ICommand"/>. Checks whether the command can be excecuted or not.
        /// If it cannot execute, then the control it is bound to will be disabled. Pass null in if it can always execute.
        /// </summary>
        /// <param name="parameter">Not needed currently, <see cref="ICommand"/> forces it.</param>
        /// <returns>True = the command enabled / False = the command is disabled</returns>
        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null ? true : this._canExecute();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Excecutes the Command.
        /// </summary>
        /// <param name="parameter">This parameter is not used, <see cref="ICommand"/> forces it.
        /// If you need to pass a parameter, you must use the CommandGeneric class.</param>
        public void Execute(object parameter)
        {
            this._handler();
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