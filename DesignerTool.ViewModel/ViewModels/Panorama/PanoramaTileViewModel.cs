using DesignerTool.AppLogic.ViewModels.Base;
using System;

namespace DesignerTool.AppLogic.ViewModels.Panorama
{
    public class PanoramaTileViewModel : ViewModelBase, IPanoramaTile
    {
        #region Constructors

        public PanoramaTileViewModel(string groupHeading)
            : base()
        {
            this.WidthInBlocks = 1; // Default
            this.GroupHeading = groupHeading;
        }

        public PanoramaTileViewModel(string groupHeading, string description)
            : this(groupHeading)
        {
            this.Description = description;
        }

        public PanoramaTileViewModel(string groupHeading, string description, string vectorStyleName, string imageUrl, object context)
            : this(groupHeading, description)
        {
            this.VectorStyleName = vectorStyleName;
            this.ImageUrl = imageUrl;
            this.Context = context;
        }

        public PanoramaTileViewModel(string groupHeading, string description, string vectorStyleName, string imageUrl, Action tileSelectedAction)
            : this(groupHeading, description)
        {
            this.VectorStyleName = vectorStyleName;
            this.ImageUrl = imageUrl;
            this.TileSelectedAction = tileSelectedAction;
        }

        #endregion

        #region Events

        public event Action<object> TileSelected;

        #endregion

        #region Properties

        public Action TileSelectedAction { get; set; }
        public object Context { get; private set; }

        private string _groupHeading;
        public string GroupHeading
        {
            get
            {
                return this._groupHeading;
            }
            set
            {
                if (value != this._groupHeading)
                {
                    this._groupHeading = value;
                    base.NotifyPropertyChanged("GroupHeading");
                }
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (value != this._description)
                {
                    this._description = value;
                    base.NotifyPropertyChanged("Description");
                }
            }
        }

        private string _vectorStyleName;
        public string VectorStyleName
        {
            get
            {
                return this._vectorStyleName;
            }
            set
            {
                if (value != this._vectorStyleName)
                {
                    this._vectorStyleName = value;
                    base.NotifyPropertyChanged("VectorStyleName");
                }
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get
            {
                return this._imageUrl;
            }
            set
            {
                if (value != this._imageUrl)
                {
                    this._imageUrl = value;
                    base.NotifyPropertyChanged("ImageUrl");
                }
            }
        }

        private int _widthInBlocks;
        public int WidthInBlocks
        {
            get
            {
                return this._widthInBlocks;
            }
            set
            {
                if (value != this._widthInBlocks)
                {
                    this._widthInBlocks = value;
                    base.NotifyPropertyChanged("WidthInBlocks");
                }
            }
        }

        private bool _isPressed;
        public bool IsPressed
        {
            get { return this._isPressed; }
            set
            {
                if (this._isPressed != value)
                {
                    this._isPressed = value;
                    this.NotifyPropertyChanged("IsPressed");
                }
            }
        }

        #endregion

        public void TileClicked()
        {
            if (this.TileSelectedAction != null)
            {
                this.TileSelectedAction.Invoke();
            }

            if (this.TileSelected != null)
            {
                this.TileSelected(this.Context);
            }
        }
    }
}
