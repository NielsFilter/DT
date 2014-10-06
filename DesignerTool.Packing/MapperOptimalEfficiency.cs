using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace Mapper
{
    public class MapperOptimalEfficiency<S> where S : class, ISheet, new()
    {
        private ICanvas _canvas = null;
        protected ICanvas Canvas { get { return _canvas; } }

        protected float CutoffEfficiency { get; private set; }
        protected int MaxNbrCandidateSprites { get; private set; }

        /// <summary>
        /// See MapperIterative_Base
        /// </summary>
        /// <param name="canvas">
        /// Canvas to be used by the Mapping method.
        /// </param>
        public MapperOptimalEfficiency(ICanvas canvas)
            : this(canvas, 1.0f, Int32.MaxValue)
        {
        }

        /// <summary>
        /// See MapperIterative_Base
        /// </summary>
        /// <param name="canvas">
        /// Canvas to be used by the Mapping method.
        /// </param>
        /// <param name="cutoffEfficiency">
        /// The Mapping method will stop trying to get a better solution once it has found a solution
        /// with this efficiency.
        /// </param>
        /// <param name="maxNbrCandidateSprites">
        /// The Mapping method will stop trying to get a better solution once it has generated this many
        /// candidate sprites.
        /// </param>
        public MapperOptimalEfficiency(ICanvas canvas, float cutoffEfficiency, int maxNbrCandidateSprites)
        {
            _canvas = canvas;
            CutoffEfficiency = cutoffEfficiency;
            MaxNbrCandidateSprites = maxNbrCandidateSprites;
        }

        /// <summary>
        /// See IMapper.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public IEnumerable<S> Mapping(IEnumerable<IBoard> images)
        {
            var lstBoards = MappingRestrictedBox(images);

            return lstBoards;
        }

        /// <summary>
        /// Produces a mapping to a sprite that has given maximum dimensions.
        /// If the mapping can not be done inside those dimensions, returns null.
        /// </summary>
        /// <param name="images">
        /// List of image infos. 
        /// 
        /// This method will not sort this list. 
        /// All images in this collection will be used, regardless of size.
        /// </param>
        /// <param name="maxWidth">
        /// The sprite won't be wider than this.
        /// </param>
        /// <param name="maxHeight">
        /// The generated sprite won't be higher than this.
        /// </param>
        /// <param name="canvasStats">
        /// The statistics produced by the canvas. These numbers are since the last call to its SetCanvasDimensions method.
        /// </param>
        /// <param name="lowestFreeHeightDeficitTallestRightFlushedImage">
        /// The lowest free height deficit for the images up to and including the tallest rectangle whose right hand border sits furthest to the right
        /// of all images.
        /// 
        /// This is the minimum amount by which the height of the canvas needs to be increased to accommodate that rectangle.
        /// if the width of the canvas is decreased to one less than the width now taken by images.
        /// 
        /// Note that providing the additional height might get some other (not right flushed) image to be placed higher, thereby
        /// making room for the flushed right image.
        /// 
        /// This will be set to Int32.MaxValue if there was never any free height deficit.
        /// </param>
        /// <returns>
        /// The generated sprite.
        /// 
        /// null if not all the images could be placed within the size limitations.
        /// </returns>
        protected virtual IEnumerable<S> MappingRestrictedBox(IEnumerable<IBoard> unorderedImages)
        {
            int maxWidth = Canvas.Width;
            int maxHeight = Canvas.Height;

            List<S> lstBoards = new List<S>();
            List<IBoard> unfittedImages = new List<IBoard>();

            List<IBoard> orderedImages = unorderedImages
                .OrderByDescending(b => b.Height)
                .ThenByDescending(b => b.Width)
                .ToList();

            do
            {
                _canvas.ClearCanvas();
                _canvas.SetCanvasDimensions(maxWidth, maxHeight);

                int heightHighestRightFlushedImage = 0;
                int furthestRightEdge = 0;

                S spriteInfo = new S();

                unfittedImages = new List<IBoard>();

                foreach (IBoard board in orderedImages)
                {
                    tryFit(spriteInfo, board, ref unfittedImages, ref furthestRightEdge, ref heightHighestRightFlushedImage);
                }

                if (!Canvas.HasGrain)
                {
                    // We can flip the blocks since the Canvas has no grain.
                    // It is possible to fit in a few more boards this way.
                    var flippedBoards = unfittedImages.ToList();
                    flippedBoards.ForEach(b => flipBoard(b));

                    // Clear the unfitted list as it will be rebuilt again in this "fitting session"
                    unfittedImages = new List<IBoard>();
                    foreach (IBoard board in flippedBoards)
                    {
                        tryFit(spriteInfo, board, ref unfittedImages, ref furthestRightEdge, ref heightHighestRightFlushedImage);
                    }
                }

                lstBoards.Add(spriteInfo);
                orderedImages = unfittedImages;
            }
            while (unfittedImages.Count() > 0);

            return lstBoards;
        }

        private IBoard flipBoard(IBoard board)
        {
            if (board.Width <= Canvas.Height && board.Height <= board.Width)
            {
                board.FlipBoard();
            }

            return board;
        }

        private void tryFit(S spriteInfo, IBoard image, ref List<IBoard> unfittedImages, ref int furthestRightEdge, ref int heightHighestRightFlushedImage)
        {
            int xOffset;
            int yOffset;
            int lowestFreeHeightDeficit;

            if (!_canvas.AddRectangle(image.Width, image.Height, out xOffset, out yOffset, out lowestFreeHeightDeficit))
            {
                // Not enough room on the canvas to place the rectangle
                unfittedImages.Add(image);
                return;
            }

            MappedBoard imageLocation = new MappedBoard(xOffset, yOffset, image);
            spriteInfo.AddMappedImage(imageLocation);

            // Update the lowestFreeHeightDeficitTallestRightFlushedImage
            int rightEdge = image.Width + xOffset;
            if ((rightEdge > furthestRightEdge) ||
                ((rightEdge == furthestRightEdge) && (image.Height > heightHighestRightFlushedImage)))
            {
                // The image is flushed the furthest right of all images, or it is flushed equally far to the right
                // as the furthest flushed image but it is taller. 
                heightHighestRightFlushedImage = image.Height;
                furthestRightEdge = rightEdge;
            }
        }
    }
}
