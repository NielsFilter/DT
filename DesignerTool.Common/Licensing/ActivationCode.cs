using DesignerTool.Common.Enums;
using DesignerTool.Common.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.Common.Licensing
{
    [Serializable]
    public class ActivationCode : NotifyPropertyChangedBase
    {
        #region Contructors
        
        public ActivationCode()
        {
            this.ExpiryDate = DateTime.Today.AddMonths(1);

            this.Extension = 1;
            this.ExtensionPeriod = PeriodType.Month;
        }

        public ActivationCode(string clientCode, bool isExpiryMode, int extension, PeriodType extensionPeriod, DateTime expiryDate)
        {
            this.ClientCode = clientCode;
            this.IsExpiryMode = isExpiryMode;
            this.Extension = extension;
            this.ExtensionPeriod = extensionPeriod;
            this.ExpiryDate = expiryDate;
        }

        #endregion

        #region Properties

        private string _clientCode;
        public string ClientCode
        {
            get
            {
                return this._clientCode;
            }
            set
            {
                if (value != this._clientCode)
                {
                    this._clientCode = value;
                    base.NotifyPropertyChanged("ClientCode");
                }
            }
        }

        private bool _isExpiryMode;
        public bool IsExpiryMode
        {
            get
            {
                return this._isExpiryMode;
            }
            set
            {
                if (value != this._isExpiryMode)
                {
                    this._isExpiryMode = value;
                    base.NotifyPropertyChanged("IsExpiryMode");
                }
            }
        }

        private int _extension;
        public int Extension
        {
            get
            {
                return this._extension;
            }
            set
            {
                if (value != this._extension)
                {
                    this._extension = value;
                    base.NotifyPropertyChanged("Extension");
                }
            }
        }

        private PeriodType _extensionPeriod;
        public PeriodType ExtensionPeriod
        {
            get
            {
                return this._extensionPeriod;
            }
            set
            {
                if (value != this._extensionPeriod)
                {
                    this._extensionPeriod = value;
                    base.NotifyPropertyChanged("ExtensionPeriod");
                }
            }
        }

        private DateTime _expiryDate;
        public DateTime ExpiryDate
        {
            get
            {
                return this._expiryDate;
            }
            set
            {
                if (value != this._expiryDate)
                {
                    this._expiryDate = value;
                    base.NotifyPropertyChanged("ExpiryDate");
                }
            }
        }

        #endregion
    }
}
