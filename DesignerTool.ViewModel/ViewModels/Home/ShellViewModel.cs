using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.ViewModels;
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
                if (SessionContext.Current == null || SessionContext.Current.LoggedInUser == null)
                {
                    return false;
                }

                if (this.CurrentViewModel != null)
                {
                    return this.CurrentViewModel.GetType() != typeof(HomeViewModel); // check if already home
                }
                return true;
            }
        }

        #endregion

        #region Load & Refresh

        public override void OnLoad()
        {
            base.OnLoad();

            SessionContext.Current.ParentViewModel = this;

            if (SessionContext.Current.LoggedInUser == null)
            {
                this.GoLogin();
            }
            else
            {
                this.GoHome();
            }

            using (DesignerTool.AppLogic.Data.DesignerToolDbEntities ctx = new DesignerTool.AppLogic.Data.DesignerToolDbEntities())
            {
                var abc = ctx.Users.Local;
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
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

        #region Home

        public void GoHome()
        {
            SessionContext.Current.Navigate(this.HomeViewModel);
        }

        #endregion

        #region Login

        public void GoLogin()
        {
            SessionContext.Current.Navigate(new LoginViewModel());
        }

        #endregion

        #region Admin

        public void GoUsers()
        {
            SessionContext.Current.Navigate(new UserListViewModel());
        }

        #endregion

        #region License & Activation

        public void GoGenerateLicenseKey()
        {
            SessionContext.Current.Navigate(new ActivationKeyGeneratorViewModel());
        }

        public void GoLicenseActivate()
        {
            SessionContext.Current.Navigate(new UserActivationViewModel());
        }

        #endregion

        #region Best Fit Calculator

        public void GoBestFitCalculator()
        {
            SessionContext.Current.Navigate(new BestFitCalculatorViewModel());
        }

        #endregion

        #endregion
    }
}

