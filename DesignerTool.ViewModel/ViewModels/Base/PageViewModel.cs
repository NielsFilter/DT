using DesignerTool.AppLogic;
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

        //private readonly Dictionary<string, IEnumerable<string>> _validationErrors = new Dictionary<string, IEnumerable<string>>();
        //public void AddValidationError(string propertyName, ICollection<string> errors, bool notifyErrorOccurred = true)
        //{
        //    if (_validationErrors == null || string.IsNullOrEmpty(propertyName))
        //    {
        //        return;
        //    }

        //    if (this._validationErrors.ContainsKey(propertyName))
        //    {
        //        _validationErrors[propertyName] = _validationErrors[propertyName].Union(errors);
        //    }
        //    else
        //    {
        //        _validationErrors.Add(propertyName, errors);
        //    }

        //    if (notifyErrorOccurred)
        //    {
        //        this.NotifyErrorsChanged(propertyName);
        //    }
        //}

        //public void ClearValidationErrors(string propertyName = null)
        //{
        //    if (propertyName == null)
        //    {
        //        _validationErrors.Clear();
        //    }
        //    else
        //    {
        //        _validationErrors.Remove(propertyName);
        //    }
        //}

        //private void NotifyErrorsChanged(string propertyName)
        //{
        //    if (this.ErrorsChanged != null)
        //    {
        //        this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        //    }
        //}

        //#region INotifyDataErrorInfo Members

        //public bool HasErrors
        //{
        //    get { return this._validationErrors.Count > 0; }
        //}

        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        //public System.Collections.IEnumerable GetErrors(string propertyName)
        //{
        //    if (string.IsNullOrEmpty(propertyName) || !this._validationErrors.ContainsKey(propertyName))
        //    {
        //        return null;
        //    }

        //    return this._validationErrors[propertyName];
        //}

        //#endregion

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
    }
}
