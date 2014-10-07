using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace Mapper
{
    public class SheetMapper<S> where S : class, ISheet, new()
    {
        protected S TemplateSheet { get; private set; }

        /// <summary>
        /// See MapperIterative_Base
        /// </summary>
        /// <param name="canvas">
        /// Canvas to be used by the Mapping method.
        /// </param>
        public SheetMapper(S templateSheet)
        {
            this.TemplateSheet = templateSheet;
        }

        /// <summary>
        /// See IMapper.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public IEnumerable<S> MapSheets(IEnumerable<IBoard> unorderedBoards, out IEnumerable<IBoard> boardsTooLarge)
        {
            List<IEnumerable<S>> differentPriorities = new List<IEnumerable<S>>()
            {
                mapSheets(unorderedBoards, out boardsTooLarge), // Height priority (standard way)
                mapSheets(unorderedBoards, out boardsTooLarge, true)
            };

            if (!this.TemplateSheet.HasGrain)
            {
                // To optimize placement, re-attempt to place all the boards onto the sheet, flipping the canvas around
                this.TemplateSheet.Flip();

                  differentPriorities.Add(mapSheets(unorderedBoards, out boardsTooLarge)); // Width priority (Uses height priority logic with a flipped sheet)
                  differentPriorities.Add(mapSheets(unorderedBoards, out boardsTooLarge, true));  // Area priority (Height first)
            }

            // return the priority with the least amount of sheets used.
            return differentPriorities.OrderBy(p => p.Count()).First();
        }

        private IEnumerable<S> mapSheets(IEnumerable<IBoard> unorderedBoards, out IEnumerable<IBoard> boardsTooLarge, bool mapByArea = false)
        {
            List<S> lstBoards = new List<S>();
            List<IBoard> unfittedBoards;

            // Find which boards are too large for the sheet. These will be excluded in the fitting / placement of boards
            boardsTooLarge = findBoardTooLarge(unorderedBoards);

            List<IBoard> orderedImages = unorderedBoards
                .Except(boardsTooLarge)
                .OrderByDescending(b => mapByArea ? (b.Height * b.Width) : b.Height)
                .ThenByDescending(b => mapByArea ? b.Height : b.Width)
                .ToList();

            do
            {
                // Create a blank sheet. As many of the remaining boards as possible will be placed here.
                S sheet = new S();
                sheet.Initialize(this.TemplateSheet.Width, this.TemplateSheet.Height, this.TemplateSheet.HasGrain, this.TemplateSheet.IsFlipped);

                // Try fit as many of the boards onto the sheeets as possible (Larger ones take priority)
                unfittedBoards = this.tryFitBoards(sheet, orderedImages);

                if (!sheet.HasGrain && unfittedBoards.Count > 0)
                {
                    // We can flip the blocks since the Sheet has no grain.
                    // It is possible to fit in a few more boards this way.
                    var flippedBoards = unfittedBoards.ToList();
                    flippedBoards.ForEach(b => flipBoard(b));

                    // Let's try again to see if any other boards will still fit after they've been flipped.
                    unfittedBoards = this.tryFitBoards(sheet, flippedBoards);
                }

                lstBoards.Add(sheet);
                orderedImages = unfittedBoards;
            }
            while (unfittedBoards.Count() > 0);

            return lstBoards;
        }

        #region Private Helpers

        /// <summary>
        /// Finds all the boards that are too large to fit into the sheet.
        /// </summary>
        /// <param name="allBoards">The collection of boards to look through</param>
        /// <returns>The boards that are too big to fit on the current sheet</returns>
        private IEnumerable<IBoard> findBoardTooLarge(IEnumerable<IBoard> allBoards)
        {
            if (this.TemplateSheet.HasGrain)
            {
                // Sheet has a grain, simply check height and width are too big
                foreach (var board in allBoards.Where(b => b.Height > this.TemplateSheet.Height || b.Width > this.TemplateSheet.Width))
                {
                    yield return board;
                }
            }
            else
            {
                foreach (var board in allBoards)
                {
                    if (board.Height > this.TemplateSheet.Height || board.Width > this.TemplateSheet.Width)
                    {
                        if (board.Width > this.TemplateSheet.Height || board.Height > this.TemplateSheet.Width)
                        {
                            yield return board;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to fit the passed board onto the sheet.
        /// </summary>
        /// <param name="sheet"><see cref="ISheet"/> instance onto which the boards will be fit.</param>
        /// <param name="board"></param>
        /// <returns>
        /// true = board was able to fit onto the sheet.
        /// false = board did NOT fit onto the sheet.
        /// </returns>
        private List<IBoard> tryFitBoards(S sheet, IEnumerable<IBoard> boardsToFit)
        {
            List<IBoard> unfittedBoards = new List<IBoard>();
            int xOffset;
            int yOffset;

            foreach (IBoard board in boardsToFit)
            {
                IBoard boardToAdd = null;
                if (sheet.AddBoard(board.Width, board.Height, out xOffset, out yOffset))
                {
                    boardToAdd = board;
                }
                else
                {
                    if (!sheet.HasGrain)
                    {
                        // We can flip the board since the Sheet has no grain.
                        // It is possible to fit in a few more boards this way.
                        var flippedBoard = flipBoard(board);

                        if (sheet.AddBoard(flippedBoard.Width, flippedBoard.Height, out xOffset, out yOffset))
                        {
                            boardToAdd = flippedBoard;
                        }
                    }
                }

                if (boardToAdd == null)
                {
                    // Not enough room on the sheet to place board. Return all the unfitted boards.
                    unfittedBoards.Add(board);
                }
                else
                {
                    // Add to the mapped board list on the sheet.
                    sheet.AddMappedBoard(new MappedBoard(xOffset, yOffset, boardToAdd));
                }
            }

            return unfittedBoards;
        }

        /// <summary>
        /// If the board can flip, then flip it (height and width are changed).
        /// </summary>
        /// <remarks>
        /// If the board is too large for the sheet, then it will remain unflipped.
        /// </remarks>
        /// <param name="board"><see cref="IBoard"/> instance to flip.</param>
        /// <returns>Flipped <see cref="IBoard"/> instance.</returns>
        private IBoard flipBoard(IBoard board)
        {
            if (board.Width <= this.TemplateSheet.Height && board.Height <= this.TemplateSheet.Width)
            {
                board.FlipBoard();
            }

            return board;
        }

        #endregion
    }
}
