using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace Mapper
{
    public class MapperOptimalEfficiency<S> : MapperOptimalEfficiency_Base<S> where S : class, ISprite, new()
    {
        /// <summary>
        /// See MapperIterative_Base
        /// </summary>
        /// <param name="canvas">
        /// Canvas to be used by the Mapping method.
        /// </param>
        public MapperOptimalEfficiency(ICanvas canvas)
            : base(canvas)
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
            : base(canvas, cutoffEfficiency, maxNbrCandidateSprites)
        {
        }

        /// <summary>
        /// See IMapper.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public override IEnumerable<S> Mapping(IEnumerable<IImageInfo> images, IMapperStats mapperStats)
        {
            int candidateSpriteFails = 0;
            int candidateSpritesGenerated = 0;
            int canvasRectangleAddAttempts = 0;
            int canvasNbrCellsGenerated = 0;

            // Sort the images by height descending
            IOrderedEnumerable<IImageInfo> imageInfosHighestFirst = images.OrderByDescending(p => p.Height);

            int totalAreaAllImages = imageInfosHighestFirst.Sum(a => a.Width * a.Height);
            int widthWidestImage = imageInfosHighestFirst.Max(a => a.Width);
            int heightHighestImage = imageInfosHighestFirst.First().Height;

            int canvasMaxWidth = 600;//Canvas.UnlimitedSize;
            int canvasMaxHeight = 300;//TODO: Get from canvas height heightHighestImage;

            return MappingRestrictedBox(imageInfosHighestFirst, canvasMaxWidth, canvasMaxHeight);
        }

        /// <summary>
        /// Works out whether there is any point in trying to fit the images on a canvas
        /// with the given width and height.
        /// </summary>
        /// <param name="canvasMaxWidth">Candidate canvas width</param>
        /// <param name="canvasMaxHeight">Candidate canvas height</param>
        /// <param name="bestSpriteArea">Area of the smallest sprite produces so far</param>
        /// <param name="totalAreaAllImages">Total area of all images</param>
        /// <param name="candidateBiggerThanBestSprite">true if the candidate canvas is bigger than the best sprite so far</param>
        /// <param name="candidateSmallerThanCombinedImages">true if the candidate canvas is smaller than the combined images</param>
        /// <returns></returns>
        protected virtual bool CandidateCanvasFeasable(
            int canvasMaxWidth, int canvasMaxHeight, int bestSpriteArea, int totalAreaAllImages,
            out bool candidateBiggerThanBestSprite, out bool candidateSmallerThanCombinedImages)
        {
            int candidateArea = canvasMaxWidth * canvasMaxHeight;
            candidateBiggerThanBestSprite = (candidateArea > bestSpriteArea);
            candidateSmallerThanCombinedImages = (candidateArea < totalAreaAllImages);

            return !(candidateBiggerThanBestSprite || candidateSmallerThanCombinedImages);
        }
    }
}
