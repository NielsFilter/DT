using DesignerTool.AppLogic.ViewModels.Base;
using DesignerTool.Common.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.ViewModels.Paging
{
    /// <summary>
    /// Utility class that helps coordinate paged access to a data store.
    /// </summary>
    public sealed class PagingViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingController"/> class.
        /// </summary>
        /// <param name="pageSize">The size of each page.</param>
        public PagingViewModel(int pageSize)
        {
            this._pageSize = pageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingController"/> class.
        /// </summary>
        /// <param name="totalRecords">The item count.</param>
        /// <param name="pageSize">The size of each page.</param>
        public PagingViewModel(int totalRecords, int pageSize)
            : this(pageSize)
        {
            this._totalRecords = totalRecords;
            this._currentPage = this._totalRecords == 0 ? 0 : 1;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Occurs when the value of <see cref="CurrentPage"/> changes.
        /// </summary>
        public event Action<CurrentPageChangedEventArgs> CurrentPageChanged;

        public System.Windows.Input.ICommand GotoFirstPageCommand { get; private set; }
        public System.Windows.Input.ICommand GotoPreviousPageCommand { get; private set; }
        public System.Windows.Input.ICommand GotoNextPageCommand { get; private set; }
        public System.Windows.Input.ICommand GotoLastPageCommand { get; private set; }

        #endregion

        #region Properties

        private int _totalRecords;
        /// <summary>
        /// Gets or sets the total number of items to be divided into pages.
        /// </summary>
        /// <value>The item count.</value>
        public int TotalRecords
        {
            get
            {
                return this._totalRecords;
            }
            set
            {
                this._totalRecords = value;
                this.NotifyPropertyChanged("TotalRecords");
                this.NotifyPropertyChanged("PageCount");
                this.setNavigationEnabledState();

                if (this.CurrentPage > this.PageCount)
                {
                    this.CurrentPage = this.PageCount;
                }
            }
        }

        private int _pageSize;
        /// <summary>
        /// Gets or sets the number of items that each page contains.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get { return this._pageSize; }
            set
            {
                var oldStartIndex = this.CurrentPageStartIndex;
                this._pageSize = value;
                this.NotifyPropertyChanged("PageSize");
                this.NotifyPropertyChanged("PageCount");
                this.NotifyPropertyChanged("CurrentPageStartIndex");

                if (oldStartIndex >= 0)
                {
                    this.CurrentPage = this.GetPageFromIndex(oldStartIndex);
                }
            }
        }

        /// <summary>
        /// Gets the number of pages required to contain all items.
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount
        {
            get
            {
                if (this._totalRecords == 0)
                {
                    return 0;
                }
                return (int)Math.Ceiling((double)this._totalRecords / this._pageSize);
            }
        }

        private int _currentPage = 1;
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            get
            {
                return this._currentPage;
            }
            set
            {
                this._currentPage = value;
                this.NotifyPropertyChanged("CurrentPage");
                this.NotifyPropertyChanged("CurrentPageStartIndex");
                this.setNavigationEnabledState();

                var handler = this.CurrentPageChanged;
                if (handler != null)
                {
                    handler(new CurrentPageChangedEventArgs(this.CurrentPageStartIndex, this.PageSize));
                }
            }
        }

        /// <summary>
        /// Gets the index of the first item belonging to the current page.
        /// </summary>
        /// <value>The index of the first item belonging to the current page.</value>
        public int CurrentPageStartIndex
        {
            get
            {
                if (this.CurrentPage <= 0)
                {
                    return 0;
                }

                return (this.CurrentPage - 1) * this.PageSize;
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if (this._searchText != value)
                {
                    _searchText = value;
                    base.NotifyPropertyChanged("SearchText");
                }
            }
        }

        /// <summary>
        /// Gets the number of the page to which the item with the specified index belongs.
        /// </summary>
        /// <param name="itemIndex">The index of the item in question.</param>
        /// <returns>The number of the page in which the item with the specified index belongs.</returns>
        private int GetPageFromIndex(int itemIndex)
        {
            return (int)Math.Floor((double)itemIndex / this.PageSize) + 1;
        }

        public bool CanGoFirstPage
        {
            get { return this.TotalRecords != 0 && this.CurrentPage > 1; }
        }

        public bool CanGoLastPage
        {
            get { return this.TotalRecords != 0 && this.CurrentPage < this.PageCount; }
        }

        public bool CanGoNextPage
        {
            get { return this.TotalRecords != 0 && this.CurrentPage < this.PageCount; }
        }

        public bool CanGoPreviousPage
        {
            get { return this.TotalRecords != 0 && this.CurrentPage > 1; }
        }

        #endregion

        #region Page Navigation

        public void GoToFirstPage()
        {
            if (this.CanGoFirstPage)
            {
                this.CurrentPage = 1;
            }
        }

        public void GoToLastPage()
        {
            if (this.CanGoLastPage)
            {
                this.CurrentPage = this.PageCount;
            }
        }

        public void GoToNextPage()
        {
            if (this.CanGoNextPage)
            {
                ++this.CurrentPage;
            }
        }

        public void GoToPreviousPage()
        {
            if (this.CanGoPreviousPage)
            {
                --this.CurrentPage;
            }
        }

        #endregion

        private void setNavigationEnabledState()
        {
            this.NotifyPropertyChanged("CanGoFirstPage");
            this.NotifyPropertyChanged("CanGoLastPage");
            this.NotifyPropertyChanged("CanGoNextPage");
            this.NotifyPropertyChanged("CanGoPreviousPage");
        }
    }
}
