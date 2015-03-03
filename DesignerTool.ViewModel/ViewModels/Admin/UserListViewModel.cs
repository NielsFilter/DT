using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DesignerTool.AppLogic;
using System.Security.Authentication;
using DesignerTool.DataAccess.Data;
using DesignerTool.DataAccess.Repositories;
using DesignerTool.Common.Enums;

namespace DesignerTool.Pages.Admin
{
    public class UserListViewModel : PageViewModel
    {
        private UserRepository rep;

        #region Constructors

        public UserListViewModel(IDesignerToolContext ctx)
            : base()
        {
            if (!base.PagePermissions.CanRead)
            {
                throw new AuthenticationException();
            }
            rep = new UserRepository(ctx);
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
            get { return base.PagePermissions.CanDelete && this.SelectedItem != null; }
        }

        public bool CanEdit
        {
            get
            {
                return this.SelectedItem != null &&
                    (this.PagePermissions.CanModify || this.PagePermissions.CanRead);
            }
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

                var data = rep.Search_Paged(base.Pager.SearchText, base.Pager.CurrentPageStartIndex, base.Pager.PageSize);
                if (data != null)
                {
                    base.Pager.TotalRecords = this.rep.ListAll().Count();
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
            AppSession.Current.Navigate(new UserDetailViewModel(AppSession.Current.CreateContext()));
        }

        public void Edit()
        {
            if (this.SelectedItem != null)
            {
                AppSession.Current.Navigate(new UserDetailViewModel(AppSession.Current.CreateContext(), this.SelectedItem.UserID));
            }
        }

        public void Delete()
        {
            if (this.SelectedItem != null)
            {
                var response = AppSession.Current.ShowMessage(
                    string.Format("Are you sure you want to delete the user '{0}'?", this.SelectedItem),
                    "Confirm delete",
                    UserMessageType.Information,
                    UserMessageButtons.YesNo);

                if (response == UserMessageResults.Yes)
                {
                    this.rep.Delete(this.SelectedItem);
                    base.ShowSaved(String.Format("Successfully deleted"));
                    this.OnRefresh();
                }
            }
        }

        #endregion
    }
}