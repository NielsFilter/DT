using DesignerTool.Common.Data;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesignerTool.Data
{
    public partial class User : IValidatable
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

        public void Validate(System.Data.EntityState entityState)
        {
            if (this.Password.Length < 20)
            {
                throw new ValidationException("Password must be atleast 20 chars");
            }
        }
    }
}
