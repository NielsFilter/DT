using DesignerTool.AppLogic;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using DesignerTool.Pages.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DesignerTool.AppLogic.ViewModels.Home;
using DesignerTool.Common.Settings;
using DesignerTool.DataAccess.Repositories;
using DesignerTool.DataAccess.Data;
using DesignerTool.AppLogic.Settings;

namespace DesignerTool.Pages.Shell
{
    public class LoginViewModel : PageViewModel
    {
        private UserRepository rep;

        #region Constructors

        public LoginViewModel(IDesignerToolContext ctx)
            : base()
        {
            this.rep = new UserRepository(ctx);
        }

        #endregion

        #region Properties

        public override string Heading
        {
            get { return "Login"; }
        }

        private string _username;
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
                    base.NotifyPropertyChanged("CanLogin");
                }
            }
        }

        private string _password;
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
                    base.NotifyPropertyChanged("CanLogin");
                }
            }
        }

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Username) && !string.IsNullOrWhiteSpace(this.Password);
            }
        }

        #endregion

        #region Load & Refresh

        public override void Load()
        {
            base.Load();

            this.Username = SettingsManager.Local.LastLoggedInUsername;
        }

        public override void Refresh()
        {
            base.Refresh();
        }

        #endregion

        #region Login

        public void Login()
        {
            base.ShowLoading(() =>
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                        User user = this.rep.LoginUser(this.Username, this.Password);
                        if (user != null)
                        {
                            SettingsManager.Local.LastLoggedInUsername = user.Username;
                            SettingsManager.Database.LastLoginDateTime = DateTime.Now;

                            AppSession.Current.LoggedInUser = user;
                            AppSession.Current.Navigate(new HomeViewModel());
                        }
                        else
                        {
                            base.ShowError("Login Failed", "Invalid Username or Password");
                        }
                    }
                    catch (Exception ex)
                    {
                        base.ShowError("Login Failed", ex.Message);
                    }
                }, "Logging in...");
        }

        #endregion
    }
}
