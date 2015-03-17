using DesignerTool.AppLogic.Security;
using DesignerTool.AppLogic.ViewModels.Admin;
using DesignerTool.AppLogic.ViewModels.Base;
using DesignerTool.AppLogic.ViewModels.Core;
using DesignerTool.AppLogic.ViewModels.Tools;
using DesignerTool.Common.Enums;
using DesignerTool.DataAccess.Data;
using System;

namespace DesignerTool.AppLogic.ViewModels.Shell
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

        private LicenseStateTypes _currentLicenseState;
        public LicenseStateTypes CurrentLicenseState
        {
            get
            {
                return this._currentLicenseState;
            }
            set
            {
                if (value != this._currentLicenseState)
                {
                    this._currentLicenseState = value;
                    base.NotifyPropertyChanged("CurrentLicenseState");
                    base.NotifyPropertyChanged("CurrentLicenseInfo");
                }
            }
        }

        public string CurrentLicenseInfo
        {
            get
            {
                string info = string.Empty;
                switch (this.CurrentLicenseState)
                {
                    case LicenseStateTypes.Valid:
                        info = "License is valid.";
                        break;
                    case LicenseStateTypes.Demo:
                        info = "Demo License.";
                        break;
                    case LicenseStateTypes.ExpiresSoon:
                        info = "License is valid but expires soon.";
                        break;
                    case LicenseStateTypes.Expired:
                        info = "License is invalid or has expired.";
                        break;
                }
                return info;
            }
        }

        #endregion

        #region Events

        public event Action LicenseChanged;

        #endregion

        #region Load & Refresh

        public override void Load()
        {
            LicenseManager.Current.LicenseChanged += Current_LicenseChanged;
            this.CurrentLicenseState = LicenseManager.Current.License.State;

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

        public void GoHome()
        {
            AppSession.Current.Navigate(this.HomeViewModel);
        }

        public void GoLogin()
        {
            AppSession.Current.Navigate(new LoginViewModel(AppSession.Current.CreateContext()));
        }

        public void GoUsers()
        {
            AppSession.Current.Navigate(new UserListViewModel(AppSession.Current.CreateContext()));
        }

        public void GoGenerateLicenseKey()
        {
            AppSession.Current.Navigate(new ActivationKeyGeneratorViewModel());
        }

        public void GoUserLicense()
        {
            AppSession.Current.Navigate(new UserActivationViewModel(AppSession.Current.CreateContext()));
        }

        public void GoBestFitCalculator()
        {
            AppSession.Current.Navigate(new BestFitCalculatorViewModel());
        }

        public void GoUserProfile()
        {
            //TODO:
        }

        public void GoDebtors()
        {
            AppSession.Current.Navigate(new DebtorListViewModel(AppSession.Current.CreateContext()));
        }

        public void GoSuppliers()
        {
            AppSession.Current.Navigate(new SupplierListViewModel(AppSession.Current.CreateContext()));
        }

        public void GoUnitTypes()
        {
            AppSession.Current.Navigate(new UnitTypeListViewModel(AppSession.Current.CreateContext()));
        }

        #endregion

        public void LogOut()
        {
            // Log user out of the system.
            AppSession.Current.LoggedInUser = null;
            AppSession.Current.Navigate(new LoginViewModel(AppSession.Current.CreateContext()));
        }

        public void Current_LicenseChanged()
        {
            if (this.LicenseChanged != null)
            {
                this.LicenseChanged();
            }
            this.CurrentLicenseState = LicenseManager.Current.License.State;
        }
    }
}

