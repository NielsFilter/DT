using DesignerTool.AppLogic;
using DesignerTool.AppLogic.Security;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Mvvm.Paging;
using DesignerTool.Common.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            if (AppSession.Current.ParentViewModel != null)
            {
                AppSession.Current.ParentViewModel.IsLoading = false;
            }
        }

        #endregion

        #region Navigate

        public void Navigate(ViewModelBase viewModel)
        {
            AppSession.Current.Navigate(viewModel);
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
            if (AppSession.Current.ParentViewModel != null)
            {
                // This is the default Loading Visibility code. Set the IsLoading property on the ParentViewModel (ShellViewModel)
                Action<bool> setLoadingVisibility = (isVisible) =>
                {
                    if (AppSession.Current.ParentViewModel != null)
                    {
                        AppSession.Current.ParentViewModel.IsLoading = isVisible;
                    }
                };

                // Set the loading message.
                AppSession.Current.ParentViewModel.LoadingMessage = loadingMessage;

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
            CancellationTokenSource delayCts = new CancellationTokenSource();
            List<Task> runningTasks = new List<Task>();

            // Check if we need to delay the Loading message.
            if (loadingDelay > 0)
            {
                runningTasks.Add(
                    // This task is simply to provide a delay for the loading message.
                    Task.Factory.StartNew(() => Thread.Sleep((int)loadingDelay), delayCts.Token) // Delay for a period
                        .ContinueWith((prevTask) => controlLoadDel(true), delayCts.Token, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext())); // Once the delay is over, show the loading
            }

            runningTasks.Add(
                // This task executes the actual work.
                Task.Factory.StartNew(action) // Do the actual work on a separate thread.
                    .ContinueWith((prevTask) => delayCts.Cancel())); // We're done, cancel the delay timer incase it's running on a LOOOONG delay (no need to wait for it anymore).

            // We wait for all our tasks to be done, then we make sure the loading is hidden again.
            // This approach is easier than trying to coordinate between tasks if 1 has run an the other not yet etc.
            Task.Factory.ContinueWhenAll(runningTasks.ToArray(), (tsks) => controlLoadDel(false));
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

        #region Notification Panel

        #region Properties

        private string _notificationText;
        public string NotificationText
        {
            get
            {
                return this._notificationText;
            }
            set
            {
                if (value != this._notificationText)
                {
                    this._notificationText = value;
                    base.NotifyPropertyChanged("NotificationText");
                }
            }
        }

        private string _notificationExtra;
        public string NotificationExtra
        {
            get
            {
                return this._notificationExtra;
            }
            set
            {
                if (value != this._notificationExtra)
                {
                    this._notificationExtra = value;
                    base.NotifyPropertyChanged("NotificationExtra");
                }
            }
        }

        private ResultType _notificationType;
        public ResultType NotificationType
        {
            get
            {
                return this._notificationType;
            }
            set
            {
                if (value != this._notificationType)
                {
                    this._notificationType = value;
                    base.NotifyPropertyChanged("NotificationType");
                }
            }
        }

        private bool _isShowNotification = false;
        public bool IsShowNotification
        {
            get
            {
                return this._isShowNotification;
            }
            set
            {
                if (value != this._isShowNotification)
                {
                    this._isShowNotification = value;
                    base.NotifyPropertyChanged("IsShowNotification");
                }
            }
        }

        #endregion

        #region Show & Hide methods

        public void ShowSaved(string savedMessage = null)
        {
            if (string.IsNullOrWhiteSpace(savedMessage))
            {
                savedMessage = "Successfully saved";
            }

            this.ShowNotification(String.Format("{0} {1:HH:mm}", savedMessage, DateTime.Now), null, ResultType.Success);
        }

        public void ShowErrors(string errorMsg, List<string> lstErrors)
        {
            if (lstErrors == null)
            {
                // No extra error info
                this.ShowError(errorMsg, null);
            }
            else
            {
                // Multiple extra info
                StringBuilder extraInfo = new StringBuilder();
                lstErrors.ForEach(err => extraInfo.AppendLine(String.Format(" - {0}", err)));
                this.ShowError(errorMsg, extraInfo.ToString());
            }
        }

        public void ShowError(string errorMsg, string extraInfo = null)
        {
            if (String.IsNullOrWhiteSpace(errorMsg))
            {
                errorMsg = "An error has occurred.";
            }

            this.ShowNotification(errorMsg, extraInfo, ResultType.Error);
        }

        public void ShowNotification(string heading, string extraText, ResultType msgType = ResultType.Information)
        {
            this.NotificationText = heading;
            this.NotificationExtra = extraText;
            this.NotificationType = msgType;
            this.IsShowNotification = true;
        }

        public void HideNotification()
        {
            this.IsShowNotification = false;
        }

        #endregion

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
