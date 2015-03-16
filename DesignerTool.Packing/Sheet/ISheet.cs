using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Packing.Sheet
{
    /// <summary>
    /// A canvas is a rectangle of a given size that lets you add smaller rectangle.
    /// The canvas will place each rectangle so that it doesn't overlap with any other rectangle that is already 
    /// on the canvas.
    /// </summary>
    public interface ISheet 
    {
        int Width { get; }
        int Height { get; }
        bool IsFlipped { get; }
        bool HasGrain { get; }

        /// <summary>
        /// Sets the dimensions of the canvas.
        /// If there were already rectangles on the canvas when this is called, those rectangles will be removed.
        /// 
        /// Be sure to call this method before you call AddRectangle for the first time.
        /// </summary>
        /// <param name="canvasWidth">New width of the canvas</param>
        /// <param name="canvasHeight">New height of the canvas</param>
        void SetCanvasDimensions(int canvasWidth, int canvasHeight);

        void Initialize(int canvasWidth, int canvasHeight, bool hasGrain, bool isFlipped);

        /// <summary>
        /// Adds a board to the sheet
        /// </summary>
        /// <param name="boardWidth">Width of the board</param>
        /// <param name="boardHeight">Height of the board</param>
        /// <param name="boardXOffset">X position where board has been placed</param>
        /// <param name="boardYOffset">Y position where board has been placed</param>
        /// <returns>
        /// true: board placed
        /// false: board not placed because there was no room
        /// </returns>
        bool AddBoard(int boardWidth, int boardHeight, out int boardXOffset, out int boardYOffset);

        /// <summary>
        /// Clears the canvas, removing all currently places items.
        /// </summary>
        void ClearCanvas();

        /// <summary>
        /// Holds the locations of all the individual images within the sprite image.
        /// </summary>
        List<IMappedBoard> MappedImages { get; }

        /// <summary>
        /// Holds the locations of all the individual images within the sprite image.
        /// </summary>
        void AddMappedBoard(IMappedBoard mappedBoard);

        /// <summary>
        /// Holds the locations of all the individual images within the sprite image.
        /// </summary>
        void Flip();
    }
}
