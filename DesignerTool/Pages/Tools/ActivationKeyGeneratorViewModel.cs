using DesignerTool.Common.Enums;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.Utils;
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

        #endregion

        #region Commands

        public Command GenerateCommand { get; set; }

        public override void OnWireCommands()
        {
            base.OnWireCommands();

            this.GenerateCommand = new Command(this.generate, this.canGenerate);
        }

        #endregion

        #region Load

        public override void OnLoaded()
        {
            base.OnLoaded();

            this.Activation = new ActivationCode();
            this.Periods = Enum.GetValues(typeof(PeriodType)).Cast<PeriodType>();
        }

        #endregion

        #region Generate Key

        private bool canGenerate()
        {
            if (string.IsNullOrEmpty(this.Activation.ClientCode))
                return false;

            if (this.Activation.IsExpiryMode)
            {
                return this.Activation.ExpiryDate > DateTime.Today;
            }
            else
            {
                return this.Activation.Extension > 0;
            }
        }

        private void generate()
        {
            //TODO: Continue HERE - Add Apply license functionality
            this.ActivationKey = Security.CreateCode(this.Activation);

            var test = Security.ReadCode(this.ActivationKey);
        }

        #endregion
    }
}
