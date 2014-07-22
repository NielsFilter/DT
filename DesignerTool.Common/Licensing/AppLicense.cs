using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DesignerTool.Common.Licensing
{
    [XmlRoot("AppLicense")]
    [Serializable]
    public class AppLicense
    {
        #region CreatedDate

        [XmlElement("CreatedDate")]
        public long CreatedDate_Ticks { get; set; }

        public DateTime CreatedDate
        {
            get
            {
                return new DateTime(CreatedDate_Ticks);
            }
        }

        #endregion

        #region ExpiryDate

        [XmlElement("ExpiryDate")]
        public long ExpiryDate_Ticks { get; set; }

        public DateTime ExpiryDate
        {
            get
            {
                return new DateTime(ExpiryDate_Ticks);
            }
        }

        #endregion
    }
}
