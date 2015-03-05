using DesignerTool.AppLogic.Security;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.Utils;
using DesignerTool.Common.ViewModels;
using DesignerTool.DataAccess.Data;
using DesignerTool.DataAccess.Repositories;
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

        public UserActivationViewModel(IDesignerToolContext ctx)
            : base()
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
                    base.NotifyPropertyChanged("CanActivateLicense");
                }
            }
        }

        private IEnumerable<string> _usedLicenseCodes;
        public IEnumerable<string> UsedLicenseCodes
        {
            get
            {
                return this._usedLicenseCodes;
            }
            set
            {
                if (value != this._usedLicenseCodes)
                {
                    this._usedLicenseCodes = value;
                    base.NotifyPropertyChanged("UsedLicenseCodes");
                    base.NotifyPropertyChanged("CanActivateLicense");
                }
            }
        }

        private License _myLicense;
        public License MyLicense
        {
            get
            {
                return this._myLicense;
            }
            set
            {
                if (value != this._myLicense)
                {
                    this._myLicense = value;
                    base.NotifyPropertyChanged("MyLicense");
                    base.NotifyPropertyChanged("CurrentLicenseDisplay");
                }
            }
        }

        public string CurrentLicenseDisplay
        {
            get
            {
                if (this.MyLicense == null)
                {
                    return "No license";
                }
                return this.MyLicense.CurrentLicenseText;
            }
        }

        public bool CanActivateLicense
        {
            get
            {
                return !string.IsNullOrEmpty(this.Code) && !this.UsedLicenseCodes.Contains(this.Code);
            }
        }

        #endregion

        #region Load & Refresh

        public override void OnLoad()
        {
            base.OnLoad();
            this.Refresh();
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            // Get current active license
            this.MyLicense = LicenseManager.Current.License;

            // Get previously used license codes.
            this.UsedLicenseCodes = LicenseManager.Current.GetUsedLicenseCodes();
        }

        #endregion

        #region Activate License

        public void ActivateLicense()
        {
            try
            {
                LicenseManager.Current.ApplyNewLicense(this.Code);

                var updatedLicense = XML.Serialize(LicenseManager.TranslateCode(this.Code));
                string xml = XML.Serialize(updatedLicense);

                if(this.MyLicense == null)
                {
                    this.MyLicense = new License();
                }

                this.MyLicense.Code = Crypto.Encrypt(xml, ClientInfo.Code.ToString());
                this.MyLicense.IsActive = true;

                var usedLic = new ActiveLicense();
                usedLic.AppliedDate = DateTime.Now;
                usedLic.Code = this.Code;

                LicenseManager.AddUsedLicense(usedLic);
                this.rep.ValidateAndCommit();

                // License updated successfully
                LicenseManager.Current.Evaluate();
                this.Refresh();
                base.ShowSaved("License successfully applied");
            }
            catch (Exception ex)
            {
                Logger.Log("Could not apply License code", ex);
                base.ShowError("Could not apply License code.", "The code you have entered is invalid. Please make sure that you have entered it correctly.");
            }
        }

        #endregion
    }
}
