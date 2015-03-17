using DesignerTool.AppLogic.ViewModels.Base;
using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.ViewModels.Core
{
    public class ConfigurationViewModel : PageViewModel
    {
        #region Constructors

        public ConfigurationViewModel(IDesignerToolContext ctx)
            : base()
        {
        }

        #endregion

        #region Properties

        public override string Heading
        {
            get { return "Settings"; }
        }

        #endregion

        #region Load & Refresh

        public override void Load()
        {
            base.Load();
        }

        public override void Refresh()
        {
            base.Refresh();
        }

        #endregion
    }
}
