using DesignerTool.AppLogic.Data;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Security
{
    public class LicenseManager
    {
        private static License _currentLicense;
        public static License CurrentLicense
        {
            get
            {
                if (_currentLicense == null)
                {
                    License lic = null;
                    using (DesignerToolDbEntities ctx = new DesignerToolDbEntities())
                    {
                        // Get the license.
                        lic = ctx.Licenses.FirstOrDefault((l) => l.IsActive);

                        if (lic == null && SessionContext.Current.ClientCode == 0)
                        {
                            //This is a new Installation. So we need to create a new license (demo license).
                            // 1. Create and Save a ClientCode for the new user.
                            int clientCode = generateNewClientCode();
                            ctx.SystemSettings.First(ss => ss.Setting == "ClientCode").Value = clientCode.ToString();

                            // 2. Create a new License instance with 30 days trial.
                            LicenseInfoXml appLicense = new LicenseInfoXml();
                            appLicense.CreatedDate_Ticks = DateTime.Now.Ticks;
                            appLicense.ExpiryDate_Ticks = DateTime.Now.AddDays(30).Ticks;
                            string xml = XML.Serialize(appLicense);

                            // 3. Save the encrypted license in the db.
                            lic = new License();
                            lic.Code = Crypto.Encrypt(xml, clientCode.ToString());
                            lic.IsActive = true;
                            ctx.Licenses.Add(lic);

                            // Save the client code & license.
                            ctx.SaveChanges();
                        }
                    }
                    _currentLicense = lic;
                }

                return _currentLicense;
            }
        }

        #region Evaluate License

        public static void Evaluate()
        {
            try
            {
                var lic = CurrentLicense;
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
            catch (Exception)
            {
                // TODO: Logging
                SessionContext.Current.LicenseExpiry = null;
            }
        }

        private static int generateNewClientCode()
        {
            // Client code is 9 digits long {000 000 000}
            return new Random().Next(100000000, 999999999);
        }

        #endregion

        #region Apply License

        public static LicenseInfoXml ApplyLicenseCode(string code)
        {
            ActivationCode activationCode;
            try
            {
                // "Unencrypt" the user entered code into ActivationCode instance which holds license details.
                activationCode = Crypto.ReadCode(code);
                if (activationCode == null)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                //TODO: Logging
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
                var currentExpiry = SessionContext.Current.LicenseExpiry.Value > DateTime.Today ? SessionContext.Current.LicenseExpiry.Value : DateTime.Today;

                updatedLicense.ExpiryDate_Ticks = ((DateTime)typeof(DateTime).GetMethod(extPeriodAttr.AddPeriodMethod) // Get the add period method from the enum 
                    .Invoke(
                        currentExpiry, new object[] { activationCode.Extension })).Ticks; // Invoke the add method, adding a period amount onto the current license expiry date
            }

            return updatedLicense;
        }

        #endregion
    }
}
