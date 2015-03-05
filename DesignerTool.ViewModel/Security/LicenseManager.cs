using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.Utils;
using DesignerTool.DataAccess.Data;
using DesignerTool.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Security
{
    public class LicenseManager : NotifyPropertyChangedBase
    {
        public static LicenseManager Current { get; set; }

        #region Ctors

        private LicenseRepository repLic;
        public LicenseManager(IDesignerToolContext ctx)
        {
            this.repLic = new LicenseRepository(ctx);
        }

        #endregion

        #region Properties

        private License _license;
        public License License
        {
            get
            {
                if(this._license == null)
                {
                    this._license = this.repLic.GetFirstActive();
                }
                return this._license;
            }
            set
            {
                if (value != this._license)
                {
                    this._license = value;
                    base.NotifyPropertyChanged("License");
                }
            }
        }

        #endregion

        #region Evaluate License

        public void Evaluate()
        {
            try
            {
                // Set this to nothing to force a fresh license instance load from the db.
                this._license = null;

                if (this.License == null || !this.License.Validate())
                {
                    // Invalid license
                    AppSession.Current.LicenseExpiry = null;
                }
                else
                {
                    // Valid license
                    AppSession.Current.LicenseExpiry = this.License.ExpiryDate;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("LicenseManager.Evaluate() exception.", ex);
                AppSession.Current.LicenseExpiry = null;
            }
        }

        #endregion

        #region Used Licenses

        public IEnumerable<string> GetUsedLicenseCodes()
        {
            return this.repLic.GetUsedLicenseCodes();
        }

        #endregion

        #region Apply Licenses

        public void ApplyLicense(string code)
        {
            // Apply License from code.
            this.applyLicense(LicenseManager.TranslateCode(code));

            // Add license code to the "used code" list.
            this.saveUsedCode(code);

            this.repLic.ValidateAndCommit();

            this.Evaluate();
        }

        public void ApplyDemoLicense()
        {
            // 1. Create a new License instance with 30 days trial and apply it.
            this.applyLicense(this.createDemoLicense());
            this.repLic.ValidateAndCommit();
        }

        private void applyLicense(LicenseInfoXml licToXml)
        {
            string xml = XML.Serialize(licToXml);

            if (this.License == null)
            {
                this.License = new License();
                this.repLic.AddNew(this.License);
            }
            this.License.Code = Crypto.Encrypt(xml, ClientInfo.Code.ToString());
            this.License.IsActive = true;
        }

        private void saveUsedCode(string code)
        {
            var usedLic = new ActiveLicense();
            usedLic.AppliedDate = DateTime.Now;
            usedLic.Code = code;
            this.repLic.AddUsedLicense(usedLic);
        }

        private LicenseInfoXml createDemoLicense()
        {
            LicenseInfoXml appLicense = new LicenseInfoXml();
            appLicense.CreatedDate_Ticks = DateTime.Now.Ticks;
            appLicense.ExpiryDate_Ticks = DateTime.Now.AddDays(30).Ticks;
            return appLicense;
        }

        #endregion

        #region Static Helpers

        public static LicenseInfoXml TranslateCode(string code)
        {
            ActivationCode activationCode;
            try
            {
                // "Un-encrypt" the user entered code into ActivationCode instance which holds license details.
                activationCode = Crypto.ReadCode(code);
                if (activationCode == null)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("LicenseManager.ApplyLicenseCode() exception.", ex);
                return null;
            }

            // Now We create an AppLicense entry which will be encrypted and saved to the database as user's new license entry.
            LicenseInfoXml updatedLicense = new LicenseInfoXml();
            updatedLicense.CreatedDate_Ticks = DateTime.Now.Ticks;

            if (activationCode.IsExpiryMode)
            {
                // An explicit expiry date was given to the user.
                updatedLicense.ExpiryDate_Ticks = activationCode.ExpiryDate.Ticks;
            }
            else
            {
                // "Extension period mode" - Adds period to existing Expiry date.
                var extPeriodAttr = PeriodInfoAttribute.GetAttribute(activationCode.ExtensionPeriod);
                var currentExpiry = AppSession.Current.LicenseExpiry.Value > DateTime.Today ? AppSession.Current.LicenseExpiry.Value : DateTime.Today;

                var addPeriodMethod = typeof(DateTime).GetMethod(extPeriodAttr.AddPeriodMethod); // Get the add period method from the enum
                var newDate = addPeriodMethod.Invoke(currentExpiry, new object[] { activationCode.Extension }); // Invoke the add method, adding a period amount onto the current license expiry date

                // Now we set the updated license expiry date to the new date.
                updatedLicense.ExpiryDate_Ticks = ((DateTime)newDate).Ticks;
            }

            return updatedLicense;
        }

        #endregion
    }
}
