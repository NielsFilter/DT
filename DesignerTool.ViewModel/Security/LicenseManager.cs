using DesignerTool.AppLogic.Settings;
using DesignerTool.Common.Base;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Exceptions;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Logging;
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

        #region Events

        public event Action LicenseChanged;

        #endregion

        #region Properties

        private License _license;
        public License License
        {
            get
            {
                if (this._license == null)
                {
                    this._license = this.repLic.GetFirstActive();
                    base.NotifyPropertyChanged("License");
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
                bool isDemo = GetUsedLicenseCodes().Count() == 0 || AppSession.Current.IsNewInstallation;
                bool isValid = this.validate(isDemo);

                if (this.License == null || !isValid)
                {
                    // Invalid license
                    AppSession.Current.LicenseExpiry = null;
                }
                else
                {
                    // Valid license
                    AppSession.Current.LicenseExpiry = this.License.ExpiryDate;
                }

                // 4. Set the other license fields according above results.
                this.calculateState(isValid, isDemo);
                this.calculateDisplayText();

                // Notify that the license has been re-evaluated and changed. This allows any VM's to update action regarding licenses.
                if (this.LicenseChanged != null)
                {
                    this.LicenseChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("LicenseManager.Evaluate() exception.", ex);
                AppSession.Current.LicenseExpiry = null;
            }
        }

        private bool validate(bool isDemo)
        {
            // 1. Check that a valid ClientCode exists.
            if (SettingsManager.Database.ClientCode == 0)
            {
                // Invalid client code
                this.License.ExpiryDate = DateTime.MinValue;
                return false;
            }

            // 2. Get and decrypt license info stored in the database
            var licInfo = this.getLicenseInfo();
            if (licInfo == null)
            {
                // No valid license found. Expiry date = Min Date
                this.License.ExpiryDate = DateTime.MinValue;
                return false;
            }

            // 3. Verify whether date manipulation took place.
            if (!validateTimeManipulation(licInfo.CreatedDate))
            {
                // DateTime was manipulated.
                return false;
            }

            // 4. Set expiry date according to the active license found.
            this.License.ExpiryDate = licInfo.ExpiryDate;

            // If we got here and the license expiry date is in the future, then license is valid.
            return this.License.ExpiryDate >= DateTime.Now;
        }

        private LicenseInfoXml getLicenseInfo()
        {
            try
            {
                // Decrypt xml and read license info.
                var xmlCode = Crypto.Decrypt(this.License.Code, SettingsManager.Database.ClientCode.ToString());
                return XML.Deserialize<LicenseInfoXml>(xmlCode);
            }
            catch (Exception ex)
            {
                Logger.Log("Could not decrypt and read license info.", ex);
                return null;
            }
        }

        private void calculateState(bool isValid, bool isDemo)
        {
            if (!isValid)
            {
                // Expired
                this.License.State = LicenseStateTypes.Expired;
            }
            else
            {
                // Valid license. Determine demo / valid / expire soon.
                if (isDemo)
                {
                    this.License.State = LicenseStateTypes.Demo;
                    return;
                }

                if (DateTime.Today.AddDays(SettingsManager.Database.LicenseExpiryWarningDays) >= this.License.ExpiryDate.Date)
                {
                    // License is expiring soon (according to setting)
                    this.License.State = LicenseStateTypes.ExpiresSoon;
                }
                else
                {
                    this.License.State = LicenseStateTypes.Valid;
                }
            }
        }

        private void calculateDisplayText()
        {
            // License Text
            if (this.License.State == LicenseStateTypes.Valid)
            {
                this.License.LicenseDisplay = String.Format("License is valid until {0}", this.License.ExpiryDate.ToLongDateString());
            }
            else if (this.License.State == LicenseStateTypes.ExpiresSoon)
            {
                this.License.LicenseDisplay = String.Format("License is valid but expires soon, on {0}", this.License.ExpiryDate.ToLongDateString());
            }
            else if (this.License.State == LicenseStateTypes.Demo)
            {
                this.License.LicenseDisplay = String.Format("This is a demo License which expires on {0}", this.License.ExpiryDate.ToLongDateString());
            }
            else
            {
                this.License.LicenseDisplay = "This license is either invalid or has expired.";
            }
        }

        /// <summary>
        /// Validates that the user did not manipulate the date and time settings to extend licensed functionality.
        /// </summary>
        /// <returns>True = no tampering / manipulation found. False = manipulation found.</returns>
        private bool validateTimeManipulation(DateTime createdDate)
        {
            // Check that last login date is before the current date.
            return DateTime.Now > SettingsManager.Database.LastLoginDateTime && createdDate <= DateTime.Now;
        }

        #endregion

        #region Used Licenses

        public IEnumerable<string> GetUsedLicenseCodes()
        {
            return this.repLic.GetUsedLicenseCodes();
        }

        #endregion

        #region Apply Licenses

        public void ApplyLicense(string code, int clientCode)
        {
            if (this.GetUsedLicenseCodes().Contains(code))
            {
                // Code already been used.
                throw new LicenseCodeUsedException(String.Format("The license code '{0}' has already been used and activated.", code));
            }

            // Apply License from code.
            this.applyLicense(LicenseManager.TranslateCode(code), clientCode);

            // Add license code to the "used code" list.
            this.saveUsedCode(code);

            this.repLic.ValidateAndCommit();

            this.Evaluate();
        }

        public void ApplyDemoLicense(int clientCode)
        {
            // 1. Create a new License instance with 30 days trial and apply it.
            this.applyLicense(this.createDemoLicense(), clientCode);
            this.repLic.ValidateAndCommit();
        }

        private void applyLicense(LicenseInfoXml licToXml, int clientCode)
        {
            string xml = XML.Serialize(licToXml);

            if (this.License == null)
            {
                this.License = new License();
                this.repLic.AddNew(this.License);
            }
            this.License.Code = Crypto.Encrypt(xml, clientCode.ToString());
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
