using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DesignerTool.Pages.Admin
{
    public class UserListViewModel : PageViewModel
    {
        DesignerDbEntities ctx;

        #region Constructors

        public UserListViewModel()
            : base()
        {
            ctx = new DesignerDbEntities();
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

        #endregion

        #region Commands

        public Command RefreshCommand { get; set; }
        public Command ClearSearchCommand { get; set; }

        public Command AddNewCommand { get; set; }
        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public override void OnWireCommands()
        {
            base.OnWireCommands();

            this.RefreshCommand = new Command(refresh);
            this.ClearSearchCommand = new Command(clearSearch);

            this.AddNewCommand = new Command(addNew);
            this.EditCommand = new Command(edit, () => this.SelectedItem != null);
            this.DeleteCommand = new Command(delete, () => this.SelectedItem != null);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// All initialization must happen here.
        /// </summary>
        public override void OnLoaded()
        {
            base.OnLoaded();

            base.Pager.CurrentPageChanged += (e) => this.refresh();

            this.refresh();
        }

        /// <summary>
        /// This will be hit when a parent or "outside source" calls refresh
        /// </summary>
        public override void OnRefresh()
        {
            this.refresh();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Refreshes the List.
        /// </summary>
        private void refresh()
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

        /// <summary>
        /// Clears the search text and refreshes the list
        /// </summary>
        private void clearSearch()
        {
            base.Pager.SearchText = string.Empty;
            this.refresh();
        }

        private void addNew()
        {
            base.ChangeViewModel(new UserDetailViewModel());
        }

        private void edit()
        {
            if (this.SelectedItem != null)
            {
                base.ChangeViewModel(new UserDetailViewModel(this.SelectedItem.UserID));
            }
        }

        private void delete()
        {
            if (this.SelectedItem != null)
            {
                var response = base.DialogService.ShowMessageBox(this, string.Format("Are you sure you want to delete the user '{0}'?", this.SelectedItem), "Confirm delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                if (response == System.Windows.MessageBoxResult.Yes)
                {
                    ctx.DeleteObject(this.SelectedItem);
                    this.ShowSave("Successfully deleted");
                    this.refresh();
                }
            }
        }

        #endregion
    }
}
