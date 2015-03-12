using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.DataAccess.Data
{
    public partial class License : BaseModel
    {
        public LicenseInfoXml DecryptedInfo { get; private set; }
        public string CurrentLicenseText { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        public DateTime LastLoginDate
        {
            get
            {
                //TODO: Move to System Settings and check it straight from there.
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

        public LicenseStateTypes State { get; private set; }

        //TODO: Move validation fields to License Manager? Too much logic happening here in the Model...
        #region Validate License

        public bool Validate(bool isDemo)
        {
            bool isValid = true;
            if (ClientInfo.Code == 0)
            {
                // Invalid client code
                isValid = false;
            }

            // 1. Get and decrypt license info stored in the database
            this.getLicenseInfo();

            // 2. checks that no system date manipulation took place.
            this.verifyLicense(ref isValid);

            // 3. set expiry date according to the active license found.
            if(this.DecryptedInfo == null)
            {
                // No valid license found. Expiry date = Min Date
                this.ExpiryDate = DateTime.MinValue;
            }
            this.ExpiryDate = this.DecryptedInfo.ExpiryDate;
            isValid = isValid && this.ExpiryDate >= DateTime.Now;
            
            // 4. Set the other license fields according above results.
            this.calculateState(isValid, isDemo);
            this.calculateDisplayText();

            // Return result
            return isValid && this.ExpiryDate >= DateTime.Now;
        }

        private void getLicenseInfo()
        {
            try
            {
                // Decrypt xml and read license info.
                var xmlCode = Crypto.Decrypt(this.Code, ClientInfo.Code.ToString());
                this.DecryptedInfo = XML.Deserialize<LicenseInfoXml>(xmlCode);
            }
            catch (Exception ex)
            {
                Logger.Log("Could not decrypt and read license info.", ex);
                this.DecryptedInfo = null;
            }
        }

        private void verifyLicense(ref bool isValid)
        {
            if (isValid && !validateTimeManipulation())
            {
                // DateTime was manipulated.
                isValid = false;
            }
        }

        private void calculateState(bool isValid, bool isDemo)
        {
            if (!isValid)
            {
                // Expired
                this.State = LicenseStateTypes.Expired;
            }
            else
            {
                // Valid license. Determine demo / valid / expire soon.
                if (isDemo)
                {
                    this.State = LicenseStateTypes.Demo;
                    return;
                }

                bool isExpireSoon = false; // TODO: Check against setting of how when this should kick in.
                if (isExpireSoon)
                {
                    this.State = LicenseStateTypes.ExpiresSoon;
                }
                else
                {
                    this.State = LicenseStateTypes.Valid;
                }
            }
        }

        private void calculateDisplayText()
        {
            // License Text
            if (this.State == LicenseStateTypes.Valid || this.State == LicenseStateTypes.ExpiresSoon)
            {
                this.CurrentLicenseText = String.Format("Valid License. License expires on {0}", this.ExpiryDate.ToLongDateString());
            }
            else if (this.State == LicenseStateTypes.Demo)
            {
                this.CurrentLicenseText = String.Format("Valid Demo License. License expires on {0}", this.ExpiryDate.ToLongDateString());
            }
            else
            {
                this.CurrentLicenseText = "License has expired";
            }
        }

        /// <summary>
        /// Validates that the user did not manipulate the date and time settings to extend licensed functionality.
        /// </summary>
        /// <returns>True = no tampering / manipulation found. False = manipulation found.</returns>
        private bool validateTimeManipulation()
        {
            // Check that last login date is before the current date.
            return DateTime.Now > this.LastLoginDate && this.DecryptedInfo.CreatedDate <= DateTime.Now;
        }

        #endregion
    }
}
