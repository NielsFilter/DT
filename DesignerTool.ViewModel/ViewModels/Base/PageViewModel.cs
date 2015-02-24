using DesignerTool.AppLogic;
using DesignerTool.AppLogic.Data;
using DesignerTool.AppLogic.Security;
using DesignerTool.Common.Global;
using DesignerTool.Common.Mvvm.Paging;
using DesignerTool.Common.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;

namespace DesignerTool.Common.ViewModels
{
    public class PageViewModel : ViewModelBase
    {
        private const int PAGE_SIZE = 20;

        #region ctors

        public PageViewModel()
            : base()
        {
            this.SetPagePermissions();
            if (SessionContext.Current.ParentViewModel != null)
            {
                SessionContext.Current.ParentViewModel.IsLoading = false;
            }
        }

        #endregion

        #region Navigate

        public void Navigate(ViewModelBase viewModel)
        {
            SessionContext.Current.Navigate(viewModel);
        }

        #endregion

        #region Validation

        #endregion

        #region Loading

        /// <summary>
        /// To show the loading bar on the entire page while doing work, use this method. The loading will show when the work starts and hide when the work completes or fails.
        /// Your work (passed in the action parameter) will run on a Background thread while the Loading UI will continue on the UI Thread.
        /// <example>
        /// Example: 
        /// ShowLoading(() => 
        /// {
        ///     // Do the heavy work here.
        /// });
        /// 
        /// Alternatively you could call it like this
        /// ShowLoading(MyHeavyWorkMethod);  // All the heavy work will then be in a method called 'MyHeavyWorkMethod'
        /// </example>
        /// <remarks>This method uses the IParentViewModel's <c>IsLoading</c> and <c>LoadingMessage</c> methods.</remarks>
        /// </summary>
        /// <param name="action">The work that needs to be done while loading is shown.</param>
        public void ShowLoading(Action action, string loadingMessage = "Loading...", double loadingDelay = 400)
        {
            if (SessionContext.Current.ParentViewModel != null)
            {
                Action<bool> setLoadingVisibility = (isVisible) =>
                {
                    if (SessionContext.Current.ParentViewModel != null)
                    {
                        SessionContext.Current.ParentViewModel.IsLoading = isVisible;
                    }
                };

                SessionContext.Current.ParentViewModel.LoadingMessage = loadingMessage;

                this.ShowLoading(setLoadingVisibility, action, loadingDelay);
            }
            else
            {
                // Just do the work without showing loading.
                action.Invoke();
            }
        }

        /// <summary>
        /// To show the loading bar in a specific region while doing work, use this method. The loading will show when the work starts and hide when the work completes or fails.
        /// Your work (passed in the action parameter) will run on a Background thread while the Loading UI will continue on the UI Thread.
        /// <example>
        /// Example: 
        /// ShowLoading(SetLoadingVisibility, () => 
        /// {
        ///     // Do the heavy work here.
        /// });
        /// </example>
        /// </summary>
        /// <param name="action">The work that needs to be done while loading is shown.</param>
        public void ShowLoading(Action<bool> controlLoadDel, Action action, double loadingDelay = 400)
        {
            Timer loadingDelayTimer = null;
            if (loadingDelay <= 0)
            {
                controlLoadDel(true);
            }
            else
            {
                // Shows "loading" after a delay
                loadingDelayTimer = new Timer(loadingDelay);
                loadingDelayTimer.Elapsed += (s, e) =>
                {
                    loadingDelayTimer.Stop();
                    controlLoadDel(true);
                };
                loadingDelayTimer.Start();
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, e) => action.Invoke(); // Invokes your work to be done on a background thread.
            worker.RunWorkerCompleted += (o, e) =>
            {
                // Work is complete. Hides Loading again. This will even be called when the work throws an exception.
                controlLoadDel(false);

                if (loadingDelayTimer != null)
                {
                    loadingDelayTimer.Stop();
                    loadingDelayTimer.Dispose();
                }

                if (e.Error != null)
                {
                    throw e.Error;
                }
            };

            worker.RunWorkerAsync();
        }

        #endregion

        #region Paging

        private PagingViewModel _pager = null;
        public PagingViewModel Pager
        {
            get
            {
                if (this._pager == null)
                {
                    this._pager = new PagingViewModel(PAGE_SIZE);
                }
                return this._pager;
            }
            set
            {
                if (value != this._pager)
                {
                    this._pager = value;
                    base.NotifyPropertyChanged("Pager");
                }
            }
        }

        #endregion

        #region Notifications

        private string _successText;
        public string SuccessText
        {
            get
            {
                return this._successText;
            }
            set
            {
                if (value != this._successText)
                {
                    this._successText = value;
                    base.NotifyPropertyChanged("SuccessText");
                }
            }
        }

        private bool _isShowSuccess = false;
        public bool IsShowSuccess
        {
            get
            {
                return this._isShowSuccess;
            }
            set
            {
                if (value != this._isShowSuccess)
                {
                    if (value)
                    {
                        this.IsShowError = false;
                        this.IsShowWarning = false;
                    }

                    this._isShowSuccess = value;
                    base.NotifyPropertyChanged("IsShowSuccess");
                }
            }
        }

        private string _warningText;
        public string WarningText
        {
            get
            {
                return this._warningText;
            }
            set
            {
                if (value != this._warningText)
                {
                    this._warningText = value;
                    base.NotifyPropertyChanged("WarningText");
                }
            }
        }

        private bool _isShowWarning;
        public bool IsShowWarning
        {
            get
            {
                return this._isShowWarning;
            }
            set
            {
                if (value != this._isShowWarning)
                {
                    if(value)
                    {
                        this.IsShowSuccess = false;
                        this.IsShowError = false;
                    }

                    this._isShowWarning = value;
                    base.NotifyPropertyChanged("IsShowWarning");
                }
            }
        }

        private string _errorText;
        public string ErrorText
        {
            get
            {
                return this._errorText;
            }
            set
            {
                if (value != this._errorText)
                {
                    this._errorText = value;
                    base.NotifyPropertyChanged("ErrorText");
                }
            }
        }

        private bool _isShowError;
        public bool IsShowError
        {
            get
            {
                return this._isShowError;
            }
            set
            {
                if (value != this._isShowError)
                {
                    if (value)
                    {
                        this.IsShowSuccess = false;
                        this.IsShowWarning = false;
                    }

                    this._isShowError = value;
                    base.NotifyPropertyChanged("IsShowError");
                }
            }
        }

        public void ShowSaved(string savedText = null)
        {
            if (string.IsNullOrWhiteSpace(savedText))
            {
                savedText = String.Format("Saved Successful {0:HH:mm}", DateTime.Now);
            }

            this.SuccessText = savedText;
            this.IsShowSuccess = true;
        }

        #endregion

        #region Permissions

        private IPermission _pagePermissions;
        public IPermission PagePermissions
        {
            get
            {
                return this._pagePermissions;
            }
            set
            {
                if (value != this._pagePermissions)
                {
                    this._pagePermissions = value;
                    base.NotifyPropertyChanged("PagePermissions");
                }
            }
        }

        public void SetPagePermissions()
        {
            this.PagePermissions = PermissionChecker.GetPermission(this.GetType());
        }

        #endregion
    }
}
