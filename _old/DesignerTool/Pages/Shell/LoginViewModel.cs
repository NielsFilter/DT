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
    public class LoginViewModel : PageViewModel, INotifyDataErrorInfo
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
            validate(null);

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

        #region Validation

        private void validate(string propertyName)
        {
            if (propertyName != null)
            {
                this._validationErrors.Remove(propertyName);
            }

            // Username
            if (propertyName == "Username")
            {
                List<string> errors = new List<string>();

                if (string.IsNullOrEmpty(this.Username))
                {
                    errors.Add("Username is required");
                }

                this._validationErrors.Add(propertyName, errors);
                this.NotifyErrorsChanged(propertyName);
            }

            // Password
            if (propertyName == "Password")
            {
                List<string> errors = new List<string>();
                if (string.IsNullOrEmpty(this.Password))
                {
                    errors.Add("Password is required");
                }

                if (this.Password.Length < 6)
                {
                    errors.Add("Password must be greater than 6 characters");
                }

                this._validationErrors.Add(propertyName, errors);
            }
        }

        #endregion

        #region INotifyDataErrorInfo members
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void NotifyErrorsChanged(string propertyName)
        {
            if (this.ErrorsChanged != null)
            {
                this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !this._validationErrors.ContainsKey(propertyName))
            {
                return null;
            }

            return this._validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return this._validationErrors.Count > 0; }
        }
        #endregion
    }
}
