using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Enums
{
    public enum UserMessageTypes
    {
        [Notification(caption: "Success", message: "Save Successful")]
        SUCCESS,
        [Notification(caption: "Warning", message: "Warning")]
        WARNING,
        [Notification(caption: "Error", message: "An error has occurred")]
        ERROR,
        [Notification(caption: "Please Note", message: "Please Note")]
        NOTIFICATION
    }
}
