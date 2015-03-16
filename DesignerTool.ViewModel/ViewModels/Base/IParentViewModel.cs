using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.ViewModels.Base
{
    public interface IParentViewModel
    {
        bool IsLoading { get; set; }
        string LoadingMessage { get; set; }
        //TODO:   void ShowPanelMessage(UserMessageType msgType, string caption, string message);
    }
}
