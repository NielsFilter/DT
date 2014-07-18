using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.Services;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DesignerTool.Pages.Shell
{
    public class LoginViewModel : PageViewModel
    {
        #region Properties

        private IDialogService _dlgSvc;
        private IDialogService DialogService
        {
            get
            {
                if (this._dlgSvc == null)
                {
                    this._dlgSvc = ServiceLocator.Resolve<IDialogService>();
                }
                return this._dlgSvc;
            }
        }

        private string _username;
        [Required(ErrorMessage = "Username is required.")]
        public string Username
        {
            get
            {
                return this._username;
            }
            set
            {
                if (value != this._username)
                {
                    this._username = value;
                    base.NotifyPropertyChanged("Username");
                }
            }
        }

        private string _password;
        [Required(ErrorMessage = "Password is required.")]
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (value != this._password)
                {
                    this._password = value;
                    base.NotifyPropertyChanged("Password");
                }
            }
        }

        #endregion

        #region Commands

        public Command LoginCommand { get; set; }

        public override void OnWireCommands()
        {
            base.OnWireCommands();

            this.LoginCommand = new Command(login, () => true);
        }

        #endregion

        #region Login

        private void login()
        {

            base.ShowLoading(() =>
                {
                    bool validLogin = false;
                    using (DesignerDbEntities ctx = new DesignerDbEntities())
                    {
                        var user = ctx.Users.FirstOrDefault(u => u.Username == this.Username);
                        if (user != null)
                        {
                            validLogin = user.ValidatePassword(this.Password);
                        }
                    }

                    System.Threading.Thread.Sleep(8000);

                    if (validLogin)
                    {
                        base.ChangeViewModel(new HomeViewModel());
                    }
                    else
                    {
                        this.DialogService.ShowMessageBox(this, "Invalid username or password", "Login failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }, "Logging in...");
        }

        #endregion
    }
}
