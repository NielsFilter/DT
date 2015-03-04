using DesignerTool.Common.Enums;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.Utils;
using DesignerTool.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DesignerTool.Pages.Tools
{
    public class ActivationKeyGeneratorViewModel : PageViewModel
    {
        #region Constructors

        public ActivationKeyGeneratorViewModel()
            : base()
        {

        }

        #endregion

        #region Properties

        private string _activationKey;
        public string ActivationKey
        {
            get
            {
                return this._activationKey;
            }
            set
            {
                if (value != this._activationKey)
                {
                    this._activationKey = value;
                    base.NotifyPropertyChanged("ActivationKey");
                    base.NotifyPropertyChanged("IsActivationKeyGenerated");
                }
            }
        }

        private ActivationCode _activation;
        public ActivationCode Activation
        {
            get
            {
                return this._activation;
            }
            set
            {
                if (value != this._activation)
                {
                    this._activation = value;
                    base.NotifyPropertyChanged("Activation");
                }
            }
        }

        private IEnumerable<PeriodType> _periods;
        public IEnumerable<PeriodType> Periods
        {
            get
            {
                return this._periods;
            }
            set
            {
                if (value != this._periods)
                {
                    this._periods = value;
                    base.NotifyPropertyChanged("Periods");
                }
            }
        }

        public bool IsActivationKeyGenerated
        {
            get { return !String.IsNullOrWhiteSpace(ActivationKey); }
        }

        #endregion

        #region Load & Refresh

        public override void OnLoad()
        {
            base.OnLoad();

            this.Activation = new ActivationCode();
            this.Activation.IsExpiryMode = true;
            this.Periods = Enum.GetValues(typeof(PeriodType)).Cast<PeriodType>();
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
        }

        #endregion

        #region Generate Key

        public void GenerateCode()
        {
            if(!this.validate())
            {
                return;
            }

            // Done with validation
            this.ActivationKey = Crypto.CreateCode(this.Activation);
            base.ShowSaved("License generated. See code below.");

            //TODO: TESTING PURPOSES - var test = Crypto.ReadCode(this.ActivationKey);
        }

        private bool validate()
        {
            // Validation
            if (this.Activation == null)
            {
                base.ShowError("Could not generate license code.");
                return false;
            }

            List<string> validationMsgs = new List<string>();
            int clientCode;
            if (String.IsNullOrWhiteSpace(this.Activation.ClientCode) ||
                !Int32.TryParse(this.Activation.ClientCode.Replace(" ", ""), out clientCode))
            {
                validationMsgs.Add("Client code entered is invalid.");
            }

            if (this.Activation.IsExpiryMode)
            {
                if (this.Activation.ExpiryDate < DateTime.Today)
                {
                    validationMsgs.Add("The expiry date chosen is in the past. Please select a valid expiry date.");
                }
            }
            else
            {
                if (this.Activation.Extension <= 0)
                {
                    validationMsgs.Add("Please choose an extension amount that is positive.");
                }
            }

            if (validationMsgs.Count > 0)
            {
                base.ShowErrors("Could not generate license code.", validationMsgs);
                return false;
            }
            return true;
        }

        #endregion
    }
}
