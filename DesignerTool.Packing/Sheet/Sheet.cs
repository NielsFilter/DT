using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DesignerTool.Packing.Board;

namespace DesignerTool.Packing.Sheet
{
    /// <summary>
    /// This type of canvas places rectangles as far to the left as possible (lowest X).
    /// If there is a choice between locations with the same X, it will pick the one with the 
    /// lowest Y.
    /// </summary>
    public class Sheet : ISheet
    {
        public struct CanvasCell
        {
            public bool Occupied;

            public CanvasCell(bool occupied)
            {
                this.Occupied = occupied;
            }

            public override string ToString()
            {
                return Occupied ? "x" : ".";
            }
        }

        private DynamicTwoDimensionalArray<CanvasCell> _canvasCells;

        // Make _canvasCells available to canvas classes derived from this class.
        protected DynamicTwoDimensionalArray<CanvasCell> CanvasCells { get { return _canvasCells; } }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsFlipped { get; private set; }
        public bool HasGrain { get; private set; }

        private List<IMappedBoard> _mappedImages = null;
        /// <summary>
        /// Holds the locations of all the individual images within the sprite image.
        /// </summary>
        public List<IMappedBoard> MappedImages
        {
            get
            {
                if (this._mappedImages == null)
                {
                    this._mappedImages = new List<IMappedBoard>();
                }
                return this._mappedImages;
            }
        }

        // Lowest free height deficit found since the last call to SetCanvasDimension
        private int _lowestFreeHeightDeficitSinceLastRedim;

        #region Constructors

        public Sheet()
        {
            this.ClearCanvas();
        }

        public Sheet(bool hasGrain)
            : this()
        {
            this.HasGrain = hasGrain;
        }

        public Sheet(int height, int width, bool hasGrain)
            : this(hasGrain)
        {
            this.SetCanvasDimensions(height, width);
        }

        public Sheet(int height, int width)
            : this(height, width, false)
        {

        }

        #endregion

        /// <summary>
        /// See ICanvas
        /// </summary>
        public virtual void SetCanvasDimensions(int canvasWidth, int canvasHeight)
        {
            // Right now, it is unknown how many rectangles need to be placed.
            // So guess that a 100 by 100 capacity will be enough.
            const int initialCapacityX = 100;
            const int initialCapacityY = 100;

            // Initially, there is one free cell, which covers the entire canvas.
            _canvasCells.Initialize(initialCapacityX, initialCapacityY, canvasWidth, canvasHeight, new CanvasCell(false));

            _lowestFreeHeightDeficitSinceLastRedim = Int32.MaxValue;

            Width = canvasWidth;
            Height = canvasHeight;
        }

        public void Initialize(int canvasWidth, int canvasHeight, bool hasGrain, bool isFlipped)
        {
            this.ClearCanvas();
            this.SetCanvasDimensions(canvasWidth, canvasHeight);
            this.HasGrain = hasGrain;
            this.IsFlipped = isFlipped;
        }

        /// <summary>
        /// See ICanvas.
        /// </summary>
        public virtual bool AddBoard(int boardWidth, int boardHeight, out int boardXOffset, out int boardYOffset)
        {
            boardXOffset = 0;
            boardYOffset = 0;

            int requiredWidth = boardWidth;
            int requiredHeight = boardHeight;

            int x = 0;
            int y = 0;
            int offsetX = 0;
            int offsetY = 0;
            bool boardWasPlaced = false;
            int nbrRows = _canvasCells.NbrRows;

            do
            {
                int nbrRequiredCellsHorizontally;
                int nbrRequiredCellsVertically;
                int leftOverWidth;
                int leftOverHeight;

                // First move upwards until we find an unoccupied cell. 
                // If we're already at an unoccupied cell, no need to do anything.
                // Important to clear all occupied cells to get 
                // the lowest free height deficit. This must be taken from the top of the highest 
                // occupied cell.

                while ((y < nbrRows) && (_canvasCells.Item(x, y).Occupied))
                {
                    offsetY += _canvasCells.RowHeight(y);
                    y += 1;
                }

                // If we found an unoccupied cell, than see if we can place a board there.
                // If not, than y popped out of the top of the canvas.

                if ((y < nbrRows) && (FreeHeightDeficit(Height, offsetY, requiredHeight) <= 0))
                {
                    if (IsAvailable(
                        x, y, requiredWidth, requiredHeight,
                        out nbrRequiredCellsHorizontally, out nbrRequiredCellsVertically,
                        out leftOverWidth, out leftOverHeight))
                    {
                        PlaceBoard(
                            x, y, requiredWidth, requiredHeight,
                            nbrRequiredCellsHorizontally, nbrRequiredCellsVertically,
                            leftOverWidth, leftOverHeight);

                        boardXOffset = offsetX;
                        boardYOffset = offsetY;

                        boardWasPlaced = true;
                        break;
                    }

                    // Go to the next cell
                    offsetY += _canvasCells.RowHeight(y);
                    y += 1;
                }

                // If we've come so close to the top of the canvas that there is no space for the
                // board, go to the next column. This automatically also checks whether we've popped out of the top
                // of the canvas (in that case, _canvasHeight == offsetY).

                int freeHeightDeficit = FreeHeightDeficit(Height, offsetY, requiredHeight);
                if (freeHeightDeficit > 0)
                {
                    offsetY = 0;
                    y = 0;

                    offsetX += _canvasCells.ColumnWidth(x);
                    x += 1;

                    // This update is far from perfect, because if the board could not be placed at this column
                    // because of insufficient horizontal space, than this update should not be made (because it may lower
                    // _lowestFreeHeightDeficitSinceLastRedim while in raising the height of the canvas by this lowered amount
                    // may not result in the board being placed here after all.
                    //
                    // However, checking for sufficient horizontal width takes a lot of CPU ticks. Tests have shown that this
                    // far outstrips the gains through having fewer failed sprite generations.
                    if (_lowestFreeHeightDeficitSinceLastRedim > freeHeightDeficit) { _lowestFreeHeightDeficitSinceLastRedim = freeHeightDeficit; }
                }

                // If we've come so close to the right edge of the canvas that there is no space for
                // the board, return false now.
                if ((Width - offsetX) < requiredWidth)
                {
                    boardWasPlaced = false;
                    break;
                }
            } while (true);

            return boardWasPlaced;
        }

        /// <summary>
        /// Works out the free height deficit when placing a rectangle with a required height at a given offset.
        /// 
        /// If the free height deficit is 0 or negative, there may be room to place the rectangle (still need to check for blocking
        /// occupied cells).
        /// 
        /// If the free height deficit is greater than 0, you're too close to the top edge of the canvas to place the rectangle.
        /// </summary>
        /// <param name="canvasHeight"></param>
        /// <param name="offsetY"></param>
        /// <param name="requiredHeight"></param>
        /// <returns></returns>
        private int FreeHeightDeficit(int canvasHeight, int offsetY, int requiredHeight)
        {
            int spaceLeftVertically = canvasHeight - offsetY;
            int freeHeightDeficit = requiredHeight - spaceLeftVertically;

            return freeHeightDeficit;
        }

        /// <summary>
        /// Sets the cell at x,y to occupied, and also its top and right neighbours, as needed
        /// to place a rectangle with the given width and height.
        /// 
        /// If the rectangle takes only part of a row or column, they are split.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="requiredWidth"></param>
        /// <param name="requiredHeight"></param>
        /// <param name="nbrRequiredCellsHorizontally">
        /// Number of cells that the rectangle requires horizontally
        /// </param>
        /// <param name="nbrRequiredCellsVertically">
        /// Number of cells that the rectangle requires vertically
        /// </param>
        /// <param name="leftOverWidth">
        /// The amount of horizontal space left in the right most cells that could be used for the rectangle
        /// </param>
        /// <param name="leftOverHeight">
        /// The amount of vertical space left in the bottom most cells that could be used for the rectangle
        /// </param>
        private void PlaceBoard(
            int x, int y,
            int requiredWidth, int requiredHeight,
            int nbrRequiredCellsHorizontally, int nbrRequiredCellsVertically,
            int leftOverWidth,
            int leftOverHeight)
        {
            // Split the far most row and column if needed.

            if (leftOverWidth > 0)
            {
                int xFarRightColumn = x + nbrRequiredCellsHorizontally - 1;
                _canvasCells.InsertColumn(xFarRightColumn, leftOverWidth);
            }

            if (leftOverHeight > 0)
            {
                int yFarBottomColumn = y + nbrRequiredCellsVertically - 1;
                _canvasCells.InsertRow(yFarBottomColumn, leftOverHeight);
            }

            for (int i = x + nbrRequiredCellsHorizontally - 1; i >= x; i--)
            {
                for (int j = y + nbrRequiredCellsVertically - 1; j >= y; j--)
                {
                    _canvasCells.SetItem(i, j, new CanvasCell(true));
                }
            }
        }

        /// <summary>
        /// Returns true if a rectangle with the given width and height can be placed
        /// in the cell with the given x and y, and its right and top neighbours.
        /// 
        /// This method assumes that x,y is far away enough from the edges of the canvas
        /// that the rectangle could actually fit. So this method only looks at whether cells
        /// are occupied or not.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="requiredWidth"></param>
        /// <param name="requiredHeight"></param>
        /// <param name="nbrRequiredCellsHorizontally">
        /// Number of cells that the rectangle requires horizontally
        /// </param>
        /// <param name="nbrRequiredCellsVertically">
        /// Number of cells that the rectangle requires vertically
        /// </param>
        /// <param name="leftOverWidth">
        /// The amount of horizontal space left in the right most cells that could be used for the rectangle
        /// </param>
        /// <param name="leftOverHeight">
        /// The amount of vertical space left in the bottom most cells that could be used for the rectangle
        /// </param>
        /// <returns></returns>
        private bool IsAvailable(
            int x, int y, int requiredWidth, int requiredHeight,
            out int nbrRequiredCellsHorizontally,
            out int nbrRequiredCellsVertically,
            out int leftOverWidth,
            out int leftOverHeight)
        {
            nbrRequiredCellsHorizontally = 0;
            nbrRequiredCellsVertically = 0;
            leftOverWidth = 0;
            leftOverHeight = 0;

            int foundWidth = 0;
            int foundHeight = 0;
            int trialX = x;
            int trialY = y;

            // Check all cells that need to be unoccupied for there to be room for the rectangle.

            while (foundHeight < requiredHeight)
            {
                trialX = x;
                foundWidth = 0;

                while (foundWidth < requiredWidth)
                {
                    if (_canvasCells.Item(trialX, trialY).Occupied)
                    {
                        return false;
                    }

                    foundWidth += _canvasCells.ColumnWidth(trialX);
                    trialX++;
                }

                foundHeight += _canvasCells.RowHeight(trialY);
                trialY++;
            }

            // Visited all cells that we'll need to place the rectangle,
            // and none were occupied. So the space is available here.

            nbrRequiredCellsHorizontally = trialX - x;
            nbrRequiredCellsVertically = trialY - y;

            leftOverWidth = (foundWidth - requiredWidth);
            leftOverHeight = (foundHeight - requiredHeight);

            return true;
        }

        public void ClearCanvas()
        {
            Width = 0;
            Height = 0;

            _lowestFreeHeightDeficitSinceLastRedim = Int32.MaxValue;

            _canvasCells = new DynamicTwoDimensionalArray<CanvasCell>();
        }

        /// <summary>
        /// Adds a Rectangle to the SpriteInfo, and updates the width and height of the SpriteInfo.
        /// </summary>
        /// <param name="imageLocation"></param>
        public void AddMappedBoard(IMappedBoard imageLocation)
        {
            this.MappedImages.Add(imageLocation);

            IBoard newImage = imageLocation.Board;

            int highestY = imageLocation.Y + newImage.Height;
            int rightMostX = imageLocation.X + newImage.Width;

            if (this.Height < highestY) { this.Height = highestY; }
            if (this.Width < rightMostX) { this.Width = rightMostX; }
        }

        public void Flip()
        {
            if (!this.HasGrain)
            {
                int height = this.Height;
                int width = this.Width;

                this.SetCanvasDimensions(height, width);
                this.IsFlipped = !this.IsFlipped;
            }
        }
    }
}
