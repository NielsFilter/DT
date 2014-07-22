using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public enum Periods
    {
        [Description("Year")]
        YEAR,
        [Description("Month")]
        MONTH,
        [Description("Day")]
        DAY
    }
}
