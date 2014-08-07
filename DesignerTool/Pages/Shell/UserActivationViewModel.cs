﻿using DesignerTool.Common.Enums;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.Utils;
using DesignerTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.Pages.Shell
{
    public class UserActivationViewModel : PageViewModel
    {
        #region Constructors

        public UserActivationViewModel()
        {

        }

        #endregion

        #region Properties

        private string _code;
        public string Code
        {
            get
            {
                if (this._code == null)
                {
                    return null;
                }
                return this._code.ToUpper();
            }
            set
            {
                if (value != this._code)
                {
                    this._code = value;
                    base.NotifyPropertyChanged("Code");
                }
            }
        }

        private string _currentLicense;
        public string CurrentLicense
        {
            get
            {
                return this._currentLicense;
            }
            set
            {
                if (value != this._currentLicense)
                {
                    this._currentLicense = value;
                    base.NotifyPropertyChanged("CurrentLicense");
                }
            }
        }

        public IEnumerable<string> UsedLicenseCodes
        {
            get
            {
                return this.Db.ActiveLicenses.Select(al => al.Code.ToUpper());
            }
        }

        #endregion

        #region Commands

        public Command ActivateLicenseCommand { get; set; }

        public override void OnWireCommands()
        {
            base.OnWireCommands();

            this.ActivateLicenseCommand = new Command(this.activateLicense, this.canActivateLicense);
        }

        #endregion

        #region Load

        public override void OnLoaded()
        {
            base.OnLoaded();

            this.readCurrentLicense();
        }

        private void readCurrentLicense()
        {
            var lic = this.Db.Licenses.FirstOrDefault();
            if (lic == null)
            {
                this.CurrentLicense = "No license";
            }

            this.CurrentLicense = lic.CurrentLicenseText;
        }

        #endregion

        #region Activate License

        private bool canActivateLicense()
        {
            return !string.IsNullOrEmpty(this.Code) && !this.UsedLicenseCodes.Contains(this.Code);
        }

        private void activateLicense()
        {
            string invalidCodeMsg = "The code you have entered is invalid. Please make sure that you have entered it correctly.";
            try
            {
                var updatedLicense = License.ApplyLicenseCode(this.Code);
                string xml = XML.Serialize(updatedLicense);

                var lic = this.Db.Licenses.FirstOrDefault();
                if (lic == null)
                {
                    lic = new Data.License();
                }

                lic.Code = Security.Encrypt(xml, SessionContext.ClientCode);
                lic.IsActive = true;

                var usedLic = new ActiveLicense();
                usedLic.AppliedDate = DateTime.Now;
                usedLic.Code = this.Code;
                this.Db.ActiveLicenses.AddObject(usedLic); // Add the license Keys to the used list. Prevents reuse.
                this.Db.SaveChanges();

                // License updated successfully
                License.Evaluate();
                readCurrentLicense();
            }
            catch (Exception)
            {
                base.DialogService.ShowMessageBox(this, invalidCodeMsg, "Invalid Code", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }

        #endregion
    }
}