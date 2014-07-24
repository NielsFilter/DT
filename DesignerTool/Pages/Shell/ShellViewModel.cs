using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.Interfaces;
using DesignerTool.Pages.Admin;
using DesignerTool.Pages.Tools;
using DesignerTool.ViewModels;

namespace DesignerTool.Pages.Shell
{
    public class ShellViewModel : ShellBase, IShell
    {
        #region Constructors

        public ShellViewModel()
            : base()
        {            
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

        #endregion

        #region Commands

        public Command LoadedCommand { get; set; }
        public Command ShowMenuCommand { get; set; }
        public Command HideMenuCommand { get; set; }

        public Command HomeCommand { get; set; }
        public Command UsersCommand { get; set; }
        public Command ActivationKeyCommand { get; set; }
        public Command LicenseActivationCommand { get; set; }        

        public override void OnWireCommands()
        {
            base.OnWireCommands();

            this.LoadedCommand = new Command(this.load, null);
            this.ShowMenuCommand = new Command(this.showMenu, this.canShowMenu);
            this.HideMenuCommand = new Command(this.hideMenu, this.canHideMenu);

            this.HomeCommand = new Command(this.home, this.canGoHome);
            this.UsersCommand = new Command(this.users, this.canGoUsers);
            this.ActivationKeyCommand = new Command(this.generateLicenseKey, this.canGenerateLicenseKey);
            this.LicenseActivationCommand = new Command(this.licenseActivate);
        }

        #endregion

        public override void OnRefresh()
        {
            base.OnRefresh();
            this.load();
        }

        #region Private Methods

        private void load()
        {
            if (SessionContext.LoggedInUser == null)
            {
                base.ChangeViewModel(new LoginViewModel()); // Log in window
            }
            else
            {
                base.ChangeViewModel(new HomeViewModel()); // Default view model
            }
        }

        #endregion

        #region Menu

        private void showMenu()
        {
            this.IsMenuVisible = true;
        }

        private bool canShowMenu()
        {
            return !this.IsMenuVisible;
        }

        private void hideMenu()
        {
            this.IsMenuVisible = false;
        }

        private bool canHideMenu()
        {
            return this.IsMenuVisible;
        }

        #endregion

        #region Navigation

        #region Home

        private void home()
        {
            base.ChangeViewModel(this.HomeViewModel);
        }

        private bool canGoHome()
        {
            if (this.CurrentViewModel != null)
            {
                return this.CurrentViewModel.GetType() != typeof(HomeViewModel); // check if already home
            }
            return true;
        }

        #endregion

        #region Users

        private bool canGoUsers()
        {
            return true;
        }

        private void users()
        {
            base.ChangeViewModel(new UserListViewModel());
        }

        #endregion

        #region Activation

        private bool canGenerateLicenseKey()
        {
            return true;
        }

        private void generateLicenseKey()
        {
            base.ChangeViewModel(new ActivationKeyGeneratorViewModel());
        }

        private void licenseActivate()
        {
            base.ChangeViewModel(new UserActivationViewModel());
        }
        
        #endregion

        #endregion
    }
}

