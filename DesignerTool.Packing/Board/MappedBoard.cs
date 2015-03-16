using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Packing.Board
{
    public class MappedBoard : IMappedBoard
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public IBoard Board { get; private set; }

        public MappedBoard(int x, int y, IBoard imageInfo)
        {
            X = x;
            Y = y;
            Board = imageInfo;
        }
    }
}
