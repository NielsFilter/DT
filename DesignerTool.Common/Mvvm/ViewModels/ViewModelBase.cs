using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DesignerTool.Common.Mvvm.Interfaces;
using DesignerTool.Common.Mvvm.Mapping;
using DesignerTool.Common.Mvvm.Services;

namespace DesignerTool.Common.Mvvm.ViewModels
{
    /// <summary>
    /// This is the base ViewModel which holds core ViewModel functionality, not specific to a model or a facade.
    /// You will generally use this for the shell ViewModel, and any other viewModels that do not need to communicate with facades.
    /// </summary>
    public class ViewModelBase : NotifyPropertyChangedBase, IViewModel
    {
        #region Constructors

        public ViewModelBase()
            : this(false)
        {
        }

        public ViewModelBase(bool isPopup)
        {
            this.IsPopup = isPopup;
            this.OnWireCommands();
        }

        #endregion

        #region Properties

        public bool IsPopup { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is virtual (Overridable in VB.NET) so that ViewModels implementing this class can simply override this method and initialize 
        /// all the relevant commands they need to use. Reason why overidable is used instead of just allowing initialization to happen anywhere,
        /// is that it is now forced to happen at the same time \ right time as well as base commands can be done here and view models can access them
        /// without having to create them every time.
        /// </summary>
        public virtual void OnWireCommands()
        {
        }

        public void RefreshViewModel()
        {
            this.OnRefresh();
        }

        public virtual void OnRefresh()
        {
        }
        
        public virtual bool CanNavigate()
        {
            return true;
        }

        #endregion 
        
        #region INotifyDataErrorInfo members

        public void AddValidationError(string propertyName, ICollection<string> errors, bool notifyErrorOccurred = true)
        {
            if (_validationErrors == null || string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            if (this._validationErrors.ContainsKey(propertyName))
            {
                _validationErrors[propertyName] = _validationErrors[propertyName].Union(errors);
            }
            else
            {
                _validationErrors.Add(propertyName, errors);
            }

            if (notifyErrorOccurred)
            {
                this.NotifyErrorsChanged(propertyName);
            }
        }

        public void ClearValidationErrors(string propertyName = null)
        {
            if (propertyName == null)
            {
                _validationErrors.Clear();
            }
            else
            {
                _validationErrors.Remove(propertyName);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void NotifyErrorsChanged(string propertyName)
        {
            if (this.ErrorsChanged != null)
            {
                this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private readonly Dictionary<string, IEnumerable<string>> _validationErrors = new Dictionary<string, IEnumerable<string>>();
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !this._validationErrors.ContainsKey(propertyName))
            {
                return null;
            }

            return this._validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return this._validationErrors.Count > 0; }
        }

        #endregion
    }
}
