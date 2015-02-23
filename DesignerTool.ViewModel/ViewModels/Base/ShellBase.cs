using DesignerTool.Common.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.Enums;
using DesignerTool.Pages.Shell;

namespace DesignerTool.ViewModels
{
    public class ShellBase : ViewModelBase
    {
        #region Constructors

        public ShellBase()
            : base()
        {
        }

        #endregion

        #region Properties

        private ViewModelBase _currentViewModel;
        /// <summary>
        /// The Current View is bound to the frame. So changing by changing this property you can "navigate" between pages.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (value != this._currentViewModel)
                {
                    this.hideNotification();
                    this._currentViewModel = value;
                    base.NotifyPropertyChanged("CurrentViewModel");
                }
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return this._isLoading;
            }
            set
            {
                this._isLoading = value;
                base.NotifyPropertyChanged("IsLoading");
            }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get
            {
                return this._loadingMessage;
            }
            set
            {
                if (value != this._loadingMessage)
                {
                    this._loadingMessage = value;
                    base.NotifyPropertyChanged("LoadingMessage");
                }
            }
        }

        public ViewModelBase PreviousViewModel { get; private set; }

        private bool _isSaveShow = false;
        public bool IsSaveShow
        {
            get
            {
                return this._isSaveShow;
            }
            set
            {
                this._isSaveShow = value;
                base.NotifyPropertyChanged("IsSaveShow");
            }
        }

        private bool _isErrorShow = false;
        public bool IsErrorShow
        {
            get
            {
                return this._isErrorShow;
            }
            set
            {
                this._isErrorShow = value;
                base.NotifyPropertyChanged("IsErrorShow");
            }
        }

        private string _errorMessageHeader;
        public string ErrorMessageHeader
        {
            get
            {
                return this._errorMessageHeader;
            }
            set
            {
                if (value != this._errorMessageHeader)
                {
                    this._errorMessageHeader = value;
                    base.NotifyPropertyChanged("ErrorMessageHeader");
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return this._errorMessage;
            }
            set
            {
                if (value != this._errorMessage)
                {
                    this._errorMessage = value;
                    base.NotifyPropertyChanged("ErrorMessage");
                }
            }
        }

        private string _saveMessageHeader;
        public string SaveMessageHeader
        {
            get
            {
                return this._saveMessageHeader;
            }
            set
            {
                if (value != this._saveMessageHeader)
                {
                    this._saveMessageHeader = value;
                    base.NotifyPropertyChanged("SaveMessageHeader");
                }
            }
        }

        private string _saveMessage;
        public string SaveMessage
        {
            get
            {
                return this._saveMessage;
            }
            set
            {
                if (value != this._saveMessage)
                {
                    this._saveMessage = value;
                    base.NotifyPropertyChanged("SaveMessage");
                }
            }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return this._isEnabled;
            }
            set
            {
                if (value != this._isEnabled)
                {
                    this._isEnabled = value;
                    base.NotifyPropertyChanged("IsEnabled");
                }
            }
        }

        #endregion

        #region Private Methods

        private void hideNotification()
        {
            this.IsSaveShow = false;
            this.IsErrorShow = false;
        }

        #endregion

        #region Public Methods

        public void ChangeViewModel(ViewModelBase vm)
        {
            if (vm != null)
            {
                //TODO:
                //if (vm.IsPopup)
                //{
                //    // Popup
                //    this.PopupDialog = new ShellPopupViewModel(this);
                //    MvvmBootstrap.SetShellPopup(this.PopupDialog);

                //    this.PopupDialog.CurrentViewModel = vm;

                //    this.IsEnabled = false;
                //    this.DialogService.ShowDialog(this, this.PopupDialog);
                //    this.IsEnabled = true;
                //}
                //else
                //{
                //}


                // Navigate (same page)
                this.PreviousViewModel = this.CurrentViewModel;
                this.CurrentViewModel = vm;
            }
        }

        public void GoBack()
        {
            if (this.PreviousViewModel != null)
            {
                this.CurrentViewModel = this.PreviousViewModel;
                this.PreviousViewModel = null;

                base.Refresh();
            }
        }

        public bool CanGoBack()
        {
            return this.PreviousViewModel != null;
        }

        public void ShowUserMessage(UserMessageType msgType, string message, string caption)
        {
            this.hideNotification();

            if (caption == null)
            {
                caption = NotificationAttribute.GetCaption(msgType);
            }

            if (message == null)
            {
                message = NotificationAttribute.GetMessage(msgType);
            }

            if (msgType == UserMessageType.SUCCESS)
            {
                // Saved
                this.SaveMessageHeader = caption;
                this.SaveMessage = string.Format("{0} - {1}", message, DateTime.Now.ToString("HH:mm:ss"));
                this.IsSaveShow = true;
            }
            else if (msgType == UserMessageType.ERROR)
            {
                // Error
                this.ErrorMessageHeader = caption;
                this.ErrorMessage = message;
                this.IsErrorShow = true;
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            // Refresh the ViewModel to reflect db changes
            this.CurrentViewModel.Refresh();
        }

        #endregion
    }
}
