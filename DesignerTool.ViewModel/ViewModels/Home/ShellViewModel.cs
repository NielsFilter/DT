using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using DesignerTool.DataAccess.Data;
using DesignerTool.Pages.Admin;
using DesignerTool.Pages.Core;
using DesignerTool.Pages.Shell;
using DesignerTool.Pages.Tools;
using DesignerTool.ViewModels;

namespace DesignerTool.AppLogic.ViewModels.Home
{
    public class ShellViewModel : ShellBase
    {
        #region Constructors

        public ShellViewModel()
            : base()
        {
            this.IsMenuVisible = true;
            AppSession.Current.ViewModelNavigated += Current_ViewModelNavigated;
        }

        #endregion

        #region Properties

        private bool _isMenuVisible;
        public bool IsMenuVisible
        {
            get
            {
                return this._isMenuVisible;
            }
            set
            {
                if (value != this._isMenuVisible)
                {
                    this._isMenuVisible = value;
                    base.NotifyPropertyChanged("IsMenuVisible");
                }
            }
        }

        private HomeViewModel _homeViewModel;
        /// <summary>
        /// This is for caching purposes
        /// </summary>
        public HomeViewModel HomeViewModel
        {
            get
            {
                if (this._homeViewModel == null)
                {
                    this._homeViewModel = new HomeViewModel();
                }
                return this._homeViewModel;
            }
        }

        public bool CanGoHome
        {
            get
            {
                if (AppSession.Current == null || AppSession.Current.LoggedInUser == null)
                {
                    return false;
                }

                if (AppSession.Current.CurrentViewModel != null)
                {
                    return AppSession.Current.CurrentViewModel.GetType() != typeof(HomeViewModel); // check if already home
                }
                return true;
            }
        }

        public override string Heading
        {
            get
            {
                if(AppSession.Current.CurrentViewModel != null)
                {
                    return AppSession.Current.CurrentViewModel.Heading;
                }
                return base.Heading;
            }
        }

        private bool _isProfileMenuOpen;
        public bool IsProfileMenuOpen
        {
            get
            {
                return this._isProfileMenuOpen;
            }
            set
            {
                if (value != this._isProfileMenuOpen)
                {
                    this._isProfileMenuOpen = value;
                    base.NotifyPropertyChanged("IsProfileMenuOpen");
                }
            }
        }

        #endregion

        #region Load & Refresh

        public override void Load()
        {
            base.Load();

            AppSession.Current.ParentViewModel = this;

            if (AppSession.Current.LoggedInUser == null)
            {
                this.GoLogin();
            }
            else
            {
                this.GoHome();
            }

            using (DesignerTool.DataAccess.Data.DesignerToolDbEntities ctx = new DesignerTool.DataAccess.Data.DesignerToolDbEntities())
            {
                var abc = ctx.Users.Local;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
        }

        #endregion

        #region Menu

        public void ShowMenu()
        {
            this.IsMenuVisible = true;
        }

        public void HideMenu()
        {
            this.IsMenuVisible = false;
        }

        #endregion

        #region Navigation

        #region Back

        public override bool CanGoBack
        {
            get
            {
                if (AppSession.Current.CurrentViewModel != null)
                {
                    return AppSession.Current.CurrentViewModel.CanGoBack;
                }
                return base.CanGoBack;
            }
        }

        public override void GoBack()
        {
            if (AppSession.Current.CurrentViewModel != null)
            {
                AppSession.Current.CurrentViewModel.GoBack();
            }
        }

        private void Current_ViewModelNavigated(ViewModelBase obj)
        {
            // Notify anyone checking on these properties.
            base.NotifyPropertyChanged("CanGoHome");
            base.NotifyPropertyChanged("CanGoBack");
            base.NotifyPropertyChanged("Heading");
        }

        #endregion

        #region Home

        public void GoHome()
        {
            AppSession.Current.Navigate(this.HomeViewModel);
        }

        #endregion

        #region Login

        public void GoLogin()
        {
            AppSession.Current.Navigate(new LoginViewModel(AppSession.Current.CreateContext()));
        }

        #endregion

        #region Admin

        public void GoUsers()
        {
            AppSession.Current.Navigate(new UserListViewModel(AppSession.Current.CreateContext()));
        }

        #endregion

        #region License & Activation

        public void GoGenerateLicenseKey()
        {
            AppSession.Current.Navigate(new ActivationKeyGeneratorViewModel());
        }

        public void GoLicenseActivate()
        {
            AppSession.Current.Navigate(new UserActivationViewModel(AppSession.Current.CreateContext()));
        }

        #endregion

        #region Best Fit Calculator

        public void GoBestFitCalculator()
        {
            AppSession.Current.Navigate(new BestFitCalculatorViewModel());
        }

        #endregion

        public void GoUserProfile()
        {
            //TODO:
        }

        #endregion

        public void LogOut()
        {
            // Log user out of the system.
            AppSession.Current.LoggedInUser = null;
            AppSession.Current.Navigate(new LoginViewModel(AppSession.Current.CreateContext()));
        }
    }
}

