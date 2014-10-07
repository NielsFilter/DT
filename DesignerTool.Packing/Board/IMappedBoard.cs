using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mapper
{
    /// <summary>
    /// Defines an board that has been mapped to a specific location, for example onto a sheet.
    /// </summary>
    public interface IMappedBoard
    {
        int X { get; }
        int Y { get; }
        IBoard Board { get; }
    }
}
