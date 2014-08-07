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

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void FlipBoard()
        {
            int width = this.Width;
            int height = this.Height;

            this.Height = width;
            this.Width = height;
        }
    }
}