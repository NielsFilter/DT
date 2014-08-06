using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mapper
{
    /// <summary>
    /// Represents the contents of a sheet
    /// </summary>
    public interface ISheet
    {
        /// <summary>
        /// Width of the sheet
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of the sheet
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Area of the sheet
        /// </summary>
        int Area { get; }

        /// <summary>
        /// Holds the locations of all the boards within the sheet.
        /// </summary>
        List<IMappedBoard> MappedImages { get; }

        /// <summary>
        /// Adds an image to the SpriteInfo, and updates the width and height of the SpriteInfo.
        /// </summary>
        /// <param name="mappedImage"></param>
        void AddMappedImage(IMappedBoard mappedImage);

    }
}
