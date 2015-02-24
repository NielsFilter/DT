using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.AppLogic.Data
{
    public partial class License
    {
        public AppLicense CurrentLicense
        {
            get
            {
                try
                {
                    var xmlCode = Crypto.Decrypt(this.Code, SessionContext.Current.ClientCode);
                    return XML.Deserialize<AppLicense>(xmlCode);
                }
                catch (Exception)
                {
                    // TODO: Logging
                    return null;
                }
            }
        }

        public string CurrentLicenseText
        {
            get
            {
                try
                {
                    bool validLic = this.Validate();
                    if (validLic)
                    {
                        return String.Format("Valid License. License expires on {0}", this.ExpiryDate.ToLongDateString());
                    }
                    else
                    {
                        return "License has expired";
                    }
                }
                catch (Exception)
                {
                    return "License has expired";
                }
            }
        }

        public DateTime ExpiryDate
        {
            get
            {
                var lic = this.CurrentLicense;
                if (lic == null)
                {
                    return DateTime.MinValue;
                }

                return lic.ExpiryDate;
            }
        }

        public DateTime LastLoginDate
        {
            get
            {
                try
                {
                    using (DesignerToolDbEntities ctx = new DesignerToolDbEntities())
                    {
                        return new DateTime(
                                    Int64.Parse(ctx.SystemSettings.FirstOrDefault(ss => ss.Setting == "LastLoginDateTime").Value));
                    }
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
        }

        public bool Validate()
        {
            if (!string.IsNullOrEmpty(SessionContext.Current.ClientCode))
            {
                // Invalid client code
                return false;
            }

            if (!validateTimeManipulation())
            {
                // DateTime was manipulated.
                return false;
            }

            return this.ExpiryDate >= DateTime.Today;
        }

        #region Private Methods

        /// <summary>
        /// Validates that the user did not manipulate the date and time settings to extend licensed functionality.
        /// </summary>
        /// <returns>True = no tampering / manipulation found. False = manipulation found.</returns>
        private bool validateTimeManipulation()
        {
            // Check that last login date is before the current date.
            return DateTime.Now > this.LastLoginDate && this.CurrentLicense.CreatedDate <= DateTime.Now; // TODO: CurrentLicense should be cached.
        }

        #region Static Methods

        public static AppLicense ApplyLicenseCode(string code)
        {
            var activationCode = Crypto.ReadCode(code);
            if (activationCode == null)
            {
                return null;
            }

            AppLicense updatedLicense = new AppLicense();
            updatedLicense.CreatedDate_Ticks = DateTime.Now.Ticks;

            if (activationCode.IsExpiryMode)
            {
                // Set explicit expiry date
                updatedLicense.ExpiryDate_Ticks = activationCode.ExpiryDate.Ticks;
            }
            else
            {
                var extPeriodAttr = PeriodInfoAttribute.GetAttribute(activationCode.ExtensionPeriod);
                var currentExpiry = SessionContext.Current.LicenseExpiry.Value > DateTime.Today ? SessionContext.Current.LicenseExpiry.Value : DateTime.Today;

                updatedLicense.ExpiryDate_Ticks = ((DateTime)typeof(DateTime).GetMethod(extPeriodAttr.AddPeriodMethod) // Get the add period method from the enum 
                    .Invoke(
                        currentExpiry, new object[] { activationCode.Extension })).Ticks; // Invoke the add method, adding a period amount onto the current license expiry date
            }

            return updatedLicense;
        }

        public static void Evaluate()
        {
            try
            {
                using (DesignerToolDbEntities ctx = new DesignerToolDbEntities())
                {
                    var lic = ctx.Licenses.FirstOrDefault();
                    if (lic == null || !lic.Validate())
                    {
                        // Invalid license
                        SessionContext.Current.LicenseExpiry = null;
                    }
                    else
                    {
                        // Valid license
                        SessionContext.Current.LicenseExpiry = lic.ExpiryDate;
                    }
                }
            }
            catch (Exception)
            {
                // TODO: Logging
                SessionContext.Current.LicenseExpiry = null;
            }
        }

        #endregion

        #endregion
    }
}
