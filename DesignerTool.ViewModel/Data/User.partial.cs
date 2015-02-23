using DesignerTool.Common.Data;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Data
{
    public partial class User : IDataErrorInfo
    {
        #region Password

        private const int MIN_PASSWORD_LENGTH = 6;

        public bool ValidatePassword(string password)
        {
            try
            {
                // Decrypt and validate
                string userPwd = Security.Decrypt(this.Password, "filterniels");
                return userPwd.CompareTo(password) == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SetPassword(string pwd)
        {
            if (pwd.Length < MIN_PASSWORD_LENGTH)
            {
                throw new ValidationException(String.Format("Password must be at least {0} chars", MIN_PASSWORD_LENGTH));
            }
            // Encrypt the password
            this.Password = Security.Encrypt(pwd, "filterniels");
        }

        #endregion

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.Password))
            {
                throw new ValidationException("A Password is required.");
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Username":
                        if (string.IsNullOrWhiteSpace(this.Username))
                        {
                            return "A username is required.";
                        }
                        break;
                    case "Password":
                        if (string.IsNullOrEmpty(this.Password))
                        {
                            return "A password is required.";
                        }
                        else if (this.Password.Length < 5)
                        {
                            return "The password must be at least 5 characters long.";
                        }
                        break;
                }

                return string.Empty; // No validation exceptions
            }
        }
    }
}
