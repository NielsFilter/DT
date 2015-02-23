using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.ViewModels
{
    public interface IParentViewModel
    {
        bool IsLoading { get; set; }
        string LoadingMessage { get; set; }
        //TODO:   void ShowPanelMessage(UserMessageType msgType, string caption, string message);
    }
}
