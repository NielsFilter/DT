using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mapper
{
    /// <summary>
    /// An IMapper takes a series of images, and figures out how these could be combined in a sprite.
    /// It returns the dimensions that the sprite will have, and the locations of each image within that sprite.
    /// 
    /// This object does not create the sprite image itself. It only figures out how it needs to be constructed.
    /// </summary>
    public interface IMapper<S> where S : class, ISheet, new()
    {
        /// <summary>
        /// Works out how to map a series of images into a sprite.
        /// </summary>
        /// <param name="boards">
        /// The list of board to place onto sheets
        /// </param>
        /// <returns>
        /// A List of sheets with the boards mapped onto these sheets.
        /// </returns>
        IEnumerable<S> Mapping(IEnumerable<IBoard> boards);
    }
}
