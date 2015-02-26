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
        private LicenseInfoXml _currentLicense;
        public LicenseInfoXml CurrentLicense
        {
            get
            {
                try
                {
                    if (this._currentLicense == null)
                    {
                        var xmlCode = Crypto.Decrypt(this.Code, SessionContext.Current.ClientCode.ToString());
                        this._currentLicense = XML.Deserialize<LicenseInfoXml>(xmlCode);
                    }
                    return this._currentLicense;
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

        #region Validate License

        public bool Validate()
        {
            if (SessionContext.Current.ClientCode == 0)
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

        /// <summary>
        /// Validates that the user did not manipulate the date and time settings to extend licensed functionality.
        /// </summary>
        /// <returns>True = no tampering / manipulation found. False = manipulation found.</returns>
        private bool validateTimeManipulation()
        {
            // Check that last login date is before the current date.
            return DateTime.Now > this.LastLoginDate && this.CurrentLicense.CreatedDate <= DateTime.Now;
        }

        #endregion
    }
}
