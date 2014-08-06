using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapper;

namespace Mapper
{
    public class Board : IBoard
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool CanFlip { get; private set; }

        public Board(int width, int height)
            : this(width, height, false)
        {
        }

        public Board(int width, int height, bool canFlip)
        {
            Width = width;
            Height = height;
            CanFlip = canFlip;
        }
    }
}