using DesignerTool.Common.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool
{
    //TODO: Where must this go
    public class Boardx : NotifyPropertyChangedBase
    {
        #region Constructors

        public Boardx()
        {
            // Defaults
            this.Height = 0D;
            this.Width = 0D;
            this.Qty = 1;
            this.IsFollowsGrain = true;
        }

        #endregion

        #region Properties

        private double _height;
        public double Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if (value != this._height)
                {
                    this._height = value;
                    base.NotifyPropertyChanged("Height");
                }
            }
        }

        private double _width;
        public double Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (value != this._width)
                {
                    this._width = value;
                    base.NotifyPropertyChanged("Width");
                }
            }
        }

        private int _qty;
        public int Qty
        {
            get
            {
                return this._qty;
            }
            set
            {
                if (value != this._qty)
                {
                    this._qty = value;
                    base.NotifyPropertyChanged("Qty");
                }
            }
        }

        private bool _isFollowsGrain;
        public bool IsFollowsGrain
        {
            get
            {
                return this._isFollowsGrain;
            }
            set
            {
                if (value != this._isFollowsGrain)
                {
                    this._isFollowsGrain = value;
                    base.NotifyPropertyChanged("IsFollowsGrain");
                }
            }
        }

        #endregion
    }
}
