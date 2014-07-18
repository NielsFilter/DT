using DesignerTool.Common.Mvvm.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DesignerTool.Controls.Panorama
{
    /// <summary>
    /// The minimum specification that a tile needs to support  
    /// </summary>
    public interface IPanoramaTile
    {
        object Context { get; }

        Command TileClickedCommand { get; }

        // This is not the really the right place for this as this is UI but the logic for determining whether a button is pressed 
        // or if the list is simply scrolling is determined by the Panorama control and not simply by the mouse state.
        // Therefore life is significantly less complicated is we handle the IsPressed processing in the Tile.
        // Sort of like a button does...
        bool IsPressed { get; set; }

        event Action<object> TileSelected;
    }
}
