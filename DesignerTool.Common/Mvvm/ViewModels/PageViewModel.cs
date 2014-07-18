using System;
using System.ComponentModel;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.Interfaces;
using DesignerTool.Common.Mvvm.Paging;
using DesignerTool.Common.Mvvm.Services;
using DesignerTool.Common.Enums;

namespace DesignerTool.Common.Mvvm.ViewModels
{
    public class PageViewModel : ViewModelBase
    {
        private const int PAGE_SIZE = 10;
        System.Timers.Timer _showLoadingDelayTimer;
        #region Constructor

        public PageViewModel(bool isPopup = false)
            : base()
        {
            this.IsPopup = isPopup;

            this._showLoadingDelayTimer = new System.Timers.Timer();
            this._showLoadingDelayTimer.Elapsed += this.showLoadingDelayTimer_Elapsed;
        }

        #endregion

        #region Properties

        public IDialogService DialogService
        {
            get { return ServiceLocator.Resolve<IDialogService>(); }
        }

        private IShell _shellViewModel;
        public IShell ShellViewModel
        {
            get
            {
                if (this._shellViewModel == null)
                {
                    this._shellViewModel = MvvmBootstrap.GetShell<IShell>();
                }
                return this._shellViewModel;
            }
        }

        private IShellPopup _shellPopupViewModel;
        public IShellPopup ShellPopupViewModel
        {
            get
            {
                if (this._shellPopupViewModel == null)
                {
                    this._shellPopupViewModel = MvvmBootstrap.GetShellPopup<IShellPopup>();
                }
                return this._shellPopupViewModel;
            }
        }

        public ViewModelBase ParentViewModel
        {
            get
            {
                if (this.ShellPopupViewModel != null)
                {
                    return this.ShellPopupViewModel.ParentViewModel;
                }
                return null;
            }
        }

        public IMasterViewModel CurrentMaster
        {
            get
            {
                return this.IsPopup ? this.ShellPopupViewModel as IMasterViewModel : this.ShellViewModel as IMasterViewModel;
            }
        }

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

        //private bool _canCreate = true;
        //public bool CanCreate
        //{
        //    get
        //    {
        //        return this._canCreate;
        //    }
        //    set
        //    {
        //        if (value != this._canCreate)
        //        {
        //            this._canCreate = value;
        //            base.NotifyPropertyChanged("CanCreate");
        //        }
        //    }
        //}

        //private bool _canRead = true;
        //public bool CanRead
        //{
        //    get
        //    {
        //        return this._canRead;
        //    }
        //    set
        //    {
        //        if (value != this._canRead)
        //        {
        //            this._canRead = value;
        //            base.NotifyPropertyChanged("CanRead");
        //        }
        //    }
        //}

        //private bool _canUpdate = true;
        //public bool CanUpdate
        //{
        //    get
        //    {
        //        return this._canUpdate;
        //    }
        //    set
        //    {
        //        if (value != this._canUpdate)
        //        {
        //            this._canUpdate = value;
        //            base.NotifyPropertyChanged("CanUpdate");
        //        }
        //    }
        //}

        //private bool _canDelete = true;
        //public bool CanDelete
        //{
        //    get
        //    {
        //        return this._canDelete;
        //    }
        //    set
        //    {
        //        if (value != this._canDelete)
        //        {
        //            this._canDelete = value;
        //            base.NotifyPropertyChanged("CanDelete");
        //        }
        //    }
        //}

        #endregion

        #region Commands

        /// <summary>
        /// This command handles the Navigation of ViewModels. Note that Navigation is based on the ViewModel first approach and NOT the View first approach.
        /// </summary>
        public Command<ViewModelBase> NavigateVMCommand { get; set; }

        public Command LoadedCommand { get; set; }
        public Command GoBackCommand { get; set; }
        public Command CloseCommand { get; set; }

        /// <summary>
        /// The base commands that are required by all ViewModels inheriting from this class must be instantiated here.
        /// See the base class comments about this method if you wonder why this is an override.
        /// </summary>
        public override void OnWireCommands()
        {
            base.OnWireCommands();

            this.GoBackCommand = new Command(this.GoBack, () => this.CanGoBack());
            this.CloseCommand = new Command(() => this.ClosePopup(), () => this.CanClosePopup());
            this.NavigateVMCommand = new Command<ViewModelBase>(p => this.ChangeViewModel(p));
            this.LoadedCommand = new Command(this.OnLoaded);
        }

        #endregion

        #region Public Methods

        public virtual void OnLoaded()
        {
        }

        private bool CanGoBack()
        {
            if (this.CurrentMaster == null)
            {
                return false;
            }
            return this.CurrentMaster.CanGoBack();
        }

        private void GoBack()
        {
            if (this.CanGoBack())
            {
                if (this.CurrentMaster != null)
                {
                    this.CurrentMaster.GoBack();
                }
            }
        }

        public bool CanClosePopup()
        {
            if (this.ShellPopupViewModel == null)
            {
                return false;
            }
            return this.ShellPopupViewModel.CanClose();
        }

        public void ClosePopup(bool refreshParent = true)
        {
            if (this.CanClosePopup())
            {
                this.ShellPopupViewModel.Close();
                if (refreshParent)
                {
                    // refresh the parent viewModel
                    this.ShellPopupViewModel.ParentViewModel.RefreshViewModel();
                }
            }
        }

        /// <summary>
        /// This will tell the shell (<see cref="IShell"/>) to change the current active ViewModel (i.e. Navigation).
        /// Note that a ViewModel must be passed in and not a View.
        /// </summary>
        /// <param name="vm"></param>
        public void ChangeViewModel(ViewModelBase vm)
        {
            if (vm != null)
            {
                this.CurrentMaster.ChangeViewModel(vm);
            }
        }

        public void ShowSave(string text = null)
        {
            this.CurrentMaster.ShowUserMessage(UserMessageTypes.SUCCESS, text, null);
        }

        public void ShowError(string caption = null, string text = null)
        {
            this.CurrentMaster.ShowUserMessage(UserMessageTypes.ERROR, text, caption);
        }

        public void RefreshParent()
        {
            var parent = this.ParentViewModel;
            if (parent != null)
            {
                parent.RefreshViewModel();
            }
        }

        /// <summary>
        /// To show the loading bar while doing work, use this method. The loading will show when the work starts and hide when the work completes or fails.
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
        /// </summary>
        /// <param name="action">The work that needs to be done while loading is shown.</param>
        public void ShowLoading(Action action, string loadingMessage = "Loading...", double loadingDelay = 400)
        {
            this.CurrentMaster.LoadingMessage = loadingMessage;
            this._showLoadingDelayTimer.Interval = loadingDelay;
            this._showLoadingDelayTimer.Start(); // Shows "is busy / loading" after a delay

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                // Invokes your work to be done on a background thread.
                action.Invoke();
            };
            worker.RunWorkerAsync();
            worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                // Work is complete. Hides Loading again. This will even be called when the work throws an exception.
                this._showLoadingDelayTimer.Stop();
                this.CurrentMaster.IsLoading = false;
            };
        }

        #endregion

        #region Private Methods

        private void showLoadingDelayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this._showLoadingDelayTimer.Stop();
            if (this.CurrentMaster != null)
            {
                this.CurrentMaster.IsLoading = true;
            }
        }

        #endregion 
    }
}
