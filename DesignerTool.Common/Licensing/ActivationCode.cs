using DesignerTool.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.Common.Licensing
{
    [Serializable]
    public class ActivationCode
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
        
        public string ClientCode { get; set; }
        public bool IsExpiryMode { get; set; }
        public int Extension { get; set; }
        public PeriodType ExtensionPeriod { get; set; }
        public DateTime ExpiryDate { get; set; }

        #endregion
    }
}
