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
        private const string CLIENT_CODE_VALUE = "ClientCode";
        private const string REGISTRY_PATH = "HKEY_LOCAL_MACHINE\\Software\\DT";

        public string ClientCode
        {
            get
            {
                var clientCode = Microsoft.Win32.Registry.GetValue(REGISTRY_PATH, CLIENT_CODE_VALUE, null);
                if (clientCode == null)
                {
                    // TODO: Logging
                    throw new UnauthorizedAccessException("Application has not been registered with a valid client code.");
                }

                return clientCode.ToString();
            }
        }

        public AppLicense CurrentLicense
        {
            get
            {
                try
                {
                    var xmlCode = Security.Decrypt(this.Code, this.ClientCode);
                    return XML.Deserialize<AppLicense>(xmlCode);
                }
                catch (Exception)
                {
                    // TODO: Logging
                    return null;
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
            try
            {
                var clientCode = Microsoft.Win32.Registry.GetValue(REGISTRY_PATH, CLIENT_CODE_VALUE, null);
                if (clientCode == null)
                {
                    // TODO: Logging
                    return false;
                }

                return this.ClientCode == clientCode.ToString();
            }
            catch (Exception)
            {
                // TODO: Logging
                return false;
            }
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

        #endregion
    }
}
