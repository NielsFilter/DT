using DesignerTool.Common.Enums;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.Data
{
    public partial class License
    {
        public AppLicense CurrentLicense
        {
            get
            {
                try
                {
                    var xmlCode = Security.Decrypt(this.Code, SessionContext.ClientCode);
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
                    using (DesignerDbEntities ctx = new DesignerDbEntities())
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
            if (!validClientCode())
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
        /// Validates the Registry matches the installed Client Code.
        /// </summary>
        /// <returns>True = valid client Code & registry, False = invalid client code or missing registry</returns>
        private bool validClientCode()
        {
            // TODO: Validate Client Code - Check if we need to do this here?
            return true;
            //try
            //{
            //    var clientCode = Microsoft.Win32.Registry.GetValue(REGISTRY_PATH, CLIENT_CODE_VALUE, null);
            //    if (clientCode == null)
            //    {
            //        // TODO: Logging
            //        return false;
            //    }

            //    return this.ClientCode == clientCode.ToString();
            //}
            //catch (Exception)
            //{
            //    // TODO: Logging
            //    return false;
            //}
        }

        /// <summary>
        /// Validates that the user did not manipulate the date and time settings to extend licensed functionality.
        /// </summary>
        /// <returns>True = no tampering / manipulation found. False = manipulation found.</returns>
        private bool validateTimeManipulation()
        {
            // Check that last login date is before the current date.
            return DateTime.Now > this.LastLoginDate && this.CurrentLicense.CreatedDate <= DateTime.Now; // TODO: CurrentLicense should be cached.
        }

        public static AppLicense ApplyLicenseCode(string code)
        {
            var activationCode = Security.ReadCode(code);
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
                var currentExpiry = SessionContext.LicenseExpiry.Value > DateTime.Today ? SessionContext.LicenseExpiry.Value : DateTime.Today;

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
                using (DesignerDbEntities ctx = new DesignerDbEntities())
                {
                    var lic = ctx.Licenses.FirstOrDefault();
                    if (lic == null || !lic.Validate())
                    {
                        // Invalid license
                       SessionContext.LicenseExpiry = null;
                    }
                    else
                    {
                        // Valid license
                        SessionContext.LicenseExpiry = lic.ExpiryDate;
                    }
                }
            }
            catch (Exception)
            {
                // TODO: Logging
                SessionContext.LicenseExpiry = null;
            }
        }

        #endregion
    }
}
