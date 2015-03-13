using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Settings
{
    public class LocalSettings : LocalSettingsBase
    {
        #region Ctors

        public LocalSettings() : base()
        {

        }

        #endregion

        #region Settings Properties

        // Last Logged In Username
        protected string _lastLoggedInUsername = string.Empty;
        [Description("The username of the last user that logged into the system")]
        public string LastLoggedInUsername
        {
            get
            {
                return this._lastLoggedInUsername;
            }
            set
            {
                this._lastLoggedInUsername = value;
                base.NotifyPropertyChanged("LastLoggedInUsername");
            }
        }

        #endregion
    }
}
