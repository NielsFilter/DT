using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using DesignerTool.AppLogic.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DesignerTool.AppLogic;

namespace DesignerTool.Pages.Admin
{
    public class UserListViewModel : PageViewModel
    {
        DesignerToolDbEntities ctx;

        #region Constructors

        public UserListViewModel()
            : base()
        {
            ctx = new DesignerToolDbEntities();
        }

        #endregion

        #region Properties

        private User _selectedItem;
        public User SelectedItem
        {
            get
            {
                return this._selectedItem;
            }
            set
            {
                if (value != this._selectedItem)
                {
                    this._selectedItem = value;
                    base.NotifyPropertyChanged("SelectedItem");
                    base.NotifyPropertyChanged("CanEdit");
                    base.NotifyPropertyChanged("CanDelete");
                }
            }
        }

        private ObservableCollection<User> _list;
        public ObservableCollection<User> List
        {
            get
            {
                if (this._list == null)
                {
                    this._list = new ObservableCollection<User>();
                }
                return this._list;
            }
            set
            {
                if (value != this._list)
                {
                    this._list = value;
                    base.NotifyPropertyChanged("List");
                    base.NotifyPropertyChanged("ListPaged"); // Notifies that the Paged collection has changed
                }
            }
        }

        public System.ComponentModel.ICollectionView ListPaged
        {
            get { return System.Windows.Data.CollectionViewSource.GetDefaultView(this.List); }
        }

        //private string _searchText;
        //public string SearchText
        //{
        //    get
        //    {
        //        return this._searchText;
        //    }
        //    set
        //    {
        //        if (value != this._searchText)
        //        {
        //            this._searchText = value;
        //            base.NotifyPropertyChanged("SearchText");
        //        }
        //    }
        //}

        public bool CanDelete
        {
            get { return this.SelectedItem != null; }
        }

        public bool CanEdit
        {
            get { return this.SelectedItem != null; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// All initialization must happen here.
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();

            base.Pager.CurrentPageChanged += (e) => this.Refresh();

            this.Refresh();
        }

        /// <summary>
        /// This will be hit when a parent or "outside source" calls refresh
        /// </summary>
        public override void OnRefresh()
        {
            base.ShowLoading(() =>
            {
                string searchText = base.Pager.SearchText;

                var data = ctx.Users.Where(u => u.Username.Contains(searchText))
                    .OrderBy(u => u.Username)
                    .Skip(base.Pager.CurrentPageStartIndex)
                    .Take(base.Pager.PageSize);

                if (data != null)
                {
                    base.Pager.TotalRecords = ctx.Users.Count();
                    this.List = data.ToObservableCollection();
                }
            }, "Loading list of users");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Clears the search text and refreshes the list
        /// </summary>
        public void ClearSearch()
        {
            base.Pager.SearchText = string.Empty;
            this.OnRefresh();
        }

        public void AddNew()
        {
            SessionContext.Current.Navigate(new UserDetailViewModel());
        }

        public void Edit()
        {
            if (this.SelectedItem != null)
            {
                SessionContext.Current.Navigate(new UserDetailViewModel(this.SelectedItem.UserID));
            }
        }

        public void Delete()
        {
            if (this.SelectedItem != null)
            {
                //TODO:
                //var response = base.DialogService.ShowMessageBox(this, string.Format("Are you sure you want to delete the user '{0}'?", this.SelectedItem), "Confirm delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                //if (response == System.Windows.MessageBoxResult.Yes)
                //{
                //    ctx.DeleteObject(this.SelectedItem);
                //    this.ShowSave("Successfully deleted");
                //    this.refresh();
                //}
            }
        }

        #endregion
    }
}