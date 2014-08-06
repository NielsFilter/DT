using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mapper;

/// <summary>
/// Represents the contents of a sprite image.
/// </summary>
public class Sheet : ISheet
{
    private List<IMappedBoard> _mappedImages = null;
    private int _width = 0;
    private int _height = 0;

    /// <summary>
    /// Holds the locations of all the individual images within the sprite image.
    /// </summary>
    public List<IMappedBoard> MappedImages { get { return _mappedImages; } }

    /// <summary>
    /// Width of the sprite image
    /// </summary>
    public int Width { get { return _width; } }

    /// <summary>
    /// Height of the sprite image
    /// </summary>
    public int Height { get { return _height; } }

    /// <summary>
    /// Area of the sprite image
    /// </summary>
    public int Area { get { return _width * _height; } }

    public Sheet()
    {
        _mappedImages = new List<IMappedBoard>();
        _width = 0;
        _height = 0;
    }

    /// <summary>
    /// Adds a Rectangle to the SpriteInfo, and updates the width and height of the SpriteInfo.
    /// </summary>
    /// <param name="imageLocation"></param>
    public void AddMappedImage(IMappedBoard imageLocation)
    {
        _mappedImages.Add(imageLocation);

        IBoard newImage = imageLocation.Board;

        int highestY = imageLocation.Y + newImage.Height;
        int rightMostX = imageLocation.X + newImage.Width;

        if (_height < highestY) { _height = highestY; }
        if (_width < rightMostX) { _width = rightMostX; }
    }

}
