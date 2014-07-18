using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Controls.Panorama;
using System;

namespace DesignerTool.ViewModels
{
    public class PanoramaTileViewModel : ViewModelBase, IPanoramaTile
    {
        #region Constructors

        public PanoramaTileViewModel(string heading)
            : base()
        {
            this.WidthInBlocks = 1; // Default
            this.Heading = heading;
        }

        public PanoramaTileViewModel(string heading, string description)
            : this(heading)
        {
            this.Description = description;
        }

        public PanoramaTileViewModel(string heading, string description, string vectorStyleName, string imageUrl, object context)
            : this(heading, description)
        {
            this.VectorStyleName = vectorStyleName;
            this.ImageUrl = imageUrl;
            this.Context = context;
        }

        public PanoramaTileViewModel(string heading, string description, string vectorStyleName, string imageUrl, Action tileSelectedAction)
            : this(heading, description)
        {
            this.VectorStyleName = vectorStyleName;
            this.ImageUrl = imageUrl;
            this.TileSelectedAction = tileSelectedAction;
        }

        #endregion

        #region Events

        public event Action<object> TileSelected;

        #endregion

        #region Commands

        public Command TileClickedCommand { get; set; }

        public override void OnWireCommands()
        {
            this.TileClickedCommand = new Command(this.tileClicked);
        }

        #endregion

        #region Properties

        public Action TileSelectedAction { get; set; }
        public object Context { get; private set; }

        private string _heading;
        public string Heading
        {
            get
            {
                return this._heading;
            }
            set
            {
                if (value != this._heading)
                {
                    this._heading = value;
                    base.NotifyPropertyChanged("Heading");
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

        #region Private Methods

        public void tileClicked()
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

        #endregion
    }
}
