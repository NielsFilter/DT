using DesignerTool.AppLogic;
using DesignerTool.AppLogic.Data;
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

namespace DesignerTool.Pages.Shell
{
    public class LoginViewModel : PageViewModel
    {
        #region Properties

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
                    this.validate("Username");
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
                    this.validate("Password");
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

        public override void OnLoad()
        {
            base.OnLoad();

            this.Username = LocalSettings.Current.LastLoggedInUsername;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
        }

        #endregion

        #region Login

        public void Login()
        {
            this.validate(null);

            base.ShowLoading(() =>
                {
                    bool validLogin = false;
                    using (DesignerToolDbEntities ctx = new DesignerToolDbEntities())
                    {
                        var user = ctx.Users.FirstOrDefault(u => u.Username == this.Username);
                        if (user != null)
                        {
                            validLogin = user.ValidatePassword(this.Password);
                            if (validLogin)
                            {
                                LocalSettings.Current.LastLoggedInUsername = user.Username;
                                SessionContext.Current.LoggedInUser = user;
                                SessionContext.Current.Navigate(new HomeViewModel());
                                return;
                            }
                        }
                    }

                    //TODO: this.DialogService.ShowMessageBox(this, "Invalid username or password", "Login failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }, "Logging in...");
        }

        #endregion

        #region Validation

        private void validate(string propertyName)
        {
            //TODO:
            //base.ClearValidationErrors(propertyName);

            //// Username
            //if (propertyName == "Username")
            //{
            //    List<string> errors = new List<string>();
            //    if (string.IsNullOrEmpty(this.Username))
            //    {
            //        errors.Add("Username is required");
            //    }

            //    base.AddValidationError(propertyName, errors);
            //}

            //// Password
            //if (propertyName == "Password")
            //{
            //    List<string> errors = new List<string>();
            //    if (string.IsNullOrEmpty(this.Password))
            //    {
            //        errors.Add("Password is required");
            //    }

            //    if (this.Password.Length < 6)
            //    {
            //        errors.Add("Password must be greater than 6 characters");
            //    }

            //    base.AddValidationError(propertyName, errors);
            //}
        }

        #endregion
    }
}
