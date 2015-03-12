using DesignerTool.AppLogic;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.VMTests
{
    public class TestSession : AppSession
    {
        #region Singleton

        public static new TestSession Current
        {
            get
            {
                if (AppSession.Current == null)
                {
                    AppSession.Current = new TestSession();
                }
                return AppSession.Current as TestSession;
            }
        }

        #endregion

        public ViewModelBase CurrentViewModel { get; set; }

        public override void Navigate(ViewModelBase viewModel)
        {
            this.CurrentViewModel = viewModel;
        }

        public Func<UserMessageResults> ShowMessage_UserResponse;

        public string ShowMessage_Message { get; set; }
        public string ShowMessage_Caption { get; set; }
        public ResultType ShowMessage_MessageType { get; set; }
        public UserMessageButtons ShowMessage_Button { get; set; }
        public override UserMessageResults ShowMessage(string message, string caption = null, ResultType msgType = ResultType.Information, UserMessageButtons buttons = UserMessageButtons.OK)
        {
            this.ShowMessage_Message = message;
            this.ShowMessage_Caption = caption;
            this.ShowMessage_MessageType = msgType;
            this.ShowMessage_Button = buttons;

            return ShowMessage_UserResponse();
        }
    }
}
