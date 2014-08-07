using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mapper
{
    /// <summary>
    /// A canvas is a rectangle of a given size that lets you add smaller rectangle.
    /// The canvas will place each rectangle so that it doesn't overlap with any other rectangle that is already 
    /// on the canvas.
    /// </summary>
    public interface ICanvas
    {
        int Width { get; }
        int Height { get; }
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

        /// <summary>
        /// Adds a rectangle
        /// </summary>
        /// <param name="rectangleWidth">Width of the rectangle</param>
        /// <param name="rectangleHeight">Height of the rectangle</param>
        /// <param name="rectangleXOffset">X position where rectangle has been placed</param>
        /// <param name="rectangleYOffset">Y position where rectangle has been placed</param>
        /// <param name="lowestFreeHeightDeficit">
        /// Lowest free height deficit for all the rectangles placed since the last call to SetCanvasDimensions.
        /// 
        /// This will be set to Int32.MaxValue if there was never any free height deficit.
        /// </param>
        /// <returns>
        /// true: rectangle placed
        /// false: rectangle not placed because there was no room
        /// </returns>
        bool AddRectangle(
            int rectangleWidth, int rectangleHeight, out int rectangleXOffset, out int rectangleYOffset,
            out int lowestFreeHeightDeficit);

        /// <summary>
        /// Clears the canvas, removing all currently places items.
        /// </summary>
        void ClearCanvas();
    }
}
