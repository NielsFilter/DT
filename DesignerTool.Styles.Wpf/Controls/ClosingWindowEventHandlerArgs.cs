using System;

namespace DesignerTool.Styles.Wpf.Controls
{
    public class ClosingWindowEventHandlerArgs : EventArgs
    {
        public bool Cancelled { get; set; }
    }
}