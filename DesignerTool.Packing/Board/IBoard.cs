using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Packing.Board
{
    /// <summary>
    /// Describes a board. Boards are "Cuttings" on Sheets.
    /// </summary>
    public interface IBoard
    {
        int Width { get; }
        int Height { get; }

        void FlipBoard();
    }
}

