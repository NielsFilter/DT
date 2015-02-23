using DesignerTool.AppLogic;
using DesignerTool.AppLogic.ViewModels.Panorama;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using DesignerTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DesignerTool.Pages.Shell
{
    public class HomeViewModel : PageViewModel
    {
        #region Constructors

        public HomeViewModel()
            : base()
        {
            this.loadModules();
        }

        #endregion

        #region Properties

        private List<PanoramaGroup> _panoramaItems;
        public List<PanoramaGroup> PanoramaItems
        {
            get { return this._panoramaItems; }

            set
            {
                if (value != this._panoramaItems)
                {
                    this._panoramaItems = value;
                    base.NotifyPropertyChanged("PanoramaItems");
                }
            }
        }

        #endregion

        #region Private Methods

        private void loadModules()
        {
            List<PanoramaGroup> items = new List<PanoramaGroup>();

            // Load the modules below
            items.Add(new PanoramaGroup("User Module", CollectionViewSource.GetDefaultView(this.userModule())));
            items.Add(new PanoramaGroup("Other Module", CollectionViewSource.GetDefaultView(this.otherModule())));

            this.PanoramaItems = new List<PanoramaGroup>(items);
        }

        private List<IPanoramaTile> userModule()
        {
            PanoramaTileViewModel tile = null;
            List<IPanoramaTile> userModuleTiles = new List<IPanoramaTile>();

            //// Users
            //tile = new PanoramaTileViewModel("System users", "Access to the system, usernames and passwords are all here.");
            //tile.WidthInBlocks = 3;
            //tile.TileSelectedAction = () => SessionContext.Current.Navigate(new UserListViewModel());
            //tile.VectorStyleName = "vcUserGroup";
            //userModuleTiles.Add(tile);

            //// People
            //tile = new PanoramaTileViewModel("People", "All people or contacts and their personal are here.");
            //tile.WidthInBlocks = 2;
            //tile.TileSelectedAction = () => SessionContext.Current.Navigate(new PersonListViewModel());
            //tile.VectorStyleName = "vcMultiContact";
            //userModuleTiles.Add(tile);

            //// User Groups
            //tile = new PanoramaTileViewModel("User Groups", "Systems Permissions are ordered into groups. Here is a list of all these groups");
            //tile.WidthInBlocks = 1;
            //tile.TileSelectedAction = () => SessionContext.Current.Navigate(new UserGroupListViewModel());
            //tile.VectorStyleName = "vcGroupCircle";
            //userModuleTiles.Add(tile);

            // Test 
            userModuleTiles.Add(new PanoramaTileViewModel("Test", "Here's another test module which does nothing."));

            //// Search
            //tile = new PanoramaTileViewModel("Calculator", "The calculator module is found here. This is also a test module.");
            //tile.WidthInBlocks = 2;
            //tile.ImageUrl = @"/DesignerTool;component/Images/Search.png";
            userModuleTiles.Add(tile);

            return userModuleTiles;
        }

        private List<IPanoramaTile> otherModule()
        {
            PanoramaTileViewModel tile = null;
            List<IPanoramaTile> otherModuleTiles = new List<IPanoramaTile>();

            //// Lorem ipsum
            //tile = new PanoramaTileViewModel("Lorem ipsum", "Dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore.");
            //tile.ImageUrl = @"/DesignerTool;component/Images/Adobe.png";
            //otherModuleTiles.Add(tile);

            //// Minim veniam
            //tile = new PanoramaTileViewModel("Minim veniam", "Ut enim ad minim veniam, quis nostrud exercitation.");
            //tile.ImageUrl = @"/DesignerTool;component/Images/Blogger.png";
            //otherModuleTiles.Add(tile);

            //// Magna aliqua
            //tile = new PanoramaTileViewModel("Magna aliqua", "Laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor.");
            //tile.WidthInBlocks = 2;
            //tile.ImageUrl = @"/DesignerTool;component/Images/Android.png";
            //otherModuleTiles.Add(tile);

            //// Duis aute
            //tile = new PanoramaTileViewModel("Duis aute", "Dolor in reprehenderit in voluptate velit esse cillum.");
            //tile.ImageUrl = @"/DesignerTool;component/Images/VisualStudio.png";
            //otherModuleTiles.Add(tile);

            return otherModuleTiles;
        }

        #endregion

        #region Menu Selected

        private void tileVM_TileSelected(object context)
        {
            if (context != null && context is Type)
            {
                var type = ((Type)context);
                if (type == typeof(ViewModelBase))
                {
                    var vm = Activator.CreateInstance(type) as ViewModelBase;
                    if (vm != null)
                    {
                        SessionContext.Current.Navigate(context as ViewModelBase);
                    }
                }
            }
        }

        #endregion
    }
}
