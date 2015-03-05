using DesignerTool.Common.Enums;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Data
{
    public partial class User : BaseModel
    {
        private const string ENCRYPT_PWD = "filterniels";

        #region Password

        private const int MIN_PASSWORD_LENGTH = 6;

        public bool ValidatePassword(string password)
        {
            try
            {
                // Decrypt and validate
                string userPwd = Crypto.Decrypt(this.Password, ENCRYPT_PWD);
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
            this.Password = Crypto.Encrypt(pwd, ENCRYPT_PWD);
        }

        #endregion

        #region Validation

        public override string Validation(string columnName)
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

        #endregion

        public static User New()
        {
            // Set all the defaults.
            User newUser = new User();
            newUser.IsActive = true;
            newUser.Role = RoleType.User.ToString();

            return newUser;
        }
    }
}
