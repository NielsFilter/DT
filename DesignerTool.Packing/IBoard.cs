using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mapper
{
    /// <summary>
    /// Describes a board. Boards are "Cuttings" on Sheets.
    /// </summary>
    public interface IBoard
    {
        int Width { get; }
        int Height { get; }
    }
}

