﻿using DesignerTool.AppLogic;
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
            base.ShowLoading(() =>
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                        User user = this.rep.LoginUser(this.Username, this.Password);
                        if (user != null)
                        {
                            LocalSettings.Current.LastLoggedInUsername = user.Username;
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
