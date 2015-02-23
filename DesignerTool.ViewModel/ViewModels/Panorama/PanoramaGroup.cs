using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace DesignerTool.AppLogic.ViewModels.Panorama
{
    /// <summary>
    /// Represents a groupig of tiles
    /// </summary>
    public class PanoramaGroup
    {
        public PanoramaGroup(string header, ICollectionView tiles)
        {
            this.Header = header;
            this.Tiles = tiles;
        }

        public string Header { get; private set; }
        public ICollectionView Tiles { get; private set; }
    }
}
