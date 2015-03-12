using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Enums
{
    public enum ResultType
    {
        [Notification(caption: "Success", message: "Save Successful.")]
        Success,
        [Notification(caption: "Warning", message: "Warning.")]
        Warning,
        [Notification(caption: "Error", message: "An error has occurred.")]
        Error,
        [Notification(caption: "Please Note", message: "Please Note.")]
        Information
    }

    public enum UserMessageButtons
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel
    }

    public enum UserMessageResults
    {
        Cancel,
        No,
        None,
        OK,
        Yes
    }

    public enum RoleType
    {
        Admin,
        User,
        Internal
    }

    public enum PeriodType
    {
        [PeriodInfo("Year", "Years", "AddYears")]
        Year,
        [PeriodInfo("Month", "Months", "AddMonths")]
        Month,
        [PeriodInfo("Day", "Days", "AddDays")]
        Day
    }

    public enum LicenseStateTypes
    {
        Valid,
        Demo,
        ExpiresSoon,
        Expired
    }
}
