using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;

namespace DesignerTool.Common.Mvvm.Paging
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
            Contract.Requires(pageSize > 0);

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

        public Command GotoFirstPageCommand { get; private set; }
        public Command GotoPreviousPageCommand { get; private set; }
        public Command GotoNextPageCommand { get; private set; }
        public Command GotoLastPageCommand { get; private set; }

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
                Contract.Requires(value >= 0);

                this._totalRecords = value;
                this.NotifyPropertyChanged("TotalRecords");
                this.NotifyPropertyChanged("PageCount");

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
            get
            {
                return this._pageSize;
            }

            set
            {
                Contract.Requires(value > 0);

                var oldStartIndex = this.CurrentPageStartIndex;
                this._pageSize = value;
                this.NotifyPropertyChanged("PageSize");
                this.NotifyPropertyChanged("this.PageCount");
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
                Contract.Ensures(Contract.Result<int>() == 0 || this._totalRecords > 0);
                Contract.Ensures(Contract.Result<int>() > 0 || this._totalRecords == 0);

                if (this._totalRecords == 0)
                {
                    return 0;
                }

                var ceil = (int)Math.Ceiling((double)this._totalRecords / this._pageSize);

                Contract.Assume(ceil > 0); // Math.Ceiling makes the static checker unable to prove the postcondition without help
                return ceil;
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
                Contract.Requires(value == 0 || this.PageCount != 0);
                Contract.Requires(value > 0 || this.PageCount == 0);
                Contract.Requires(value <= this.PageCount);

                this._currentPage = value;
                this.NotifyPropertyChanged("CurrentPage");
                this.NotifyPropertyChanged("CurrentPageStartIndex");

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
                Contract.Ensures(Contract.Result<int>() == -1 || this.PageCount != 0);
                Contract.Ensures(Contract.Result<int>() >= 0 || this.PageCount == 0);
                Contract.Ensures(Contract.Result<int>() < this.TotalRecords);
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
            Contract.Requires(itemIndex >= 0);
            Contract.Requires(itemIndex < this._totalRecords);
            Contract.Ensures(Contract.Result<int>() >= 1);
            Contract.Ensures(Contract.Result<int>() <= this.PageCount);

            var result = (int)Math.Floor((double)itemIndex / this.PageSize) + 1;
            Contract.Assume(result >= 1); // Math.Floor makes the static checker unable to prove the postcondition without help
            Contract.Assume(result <= this.PageCount); // Ditto
            return result;
        }

        #endregion

        #region Methods

        public override void OnLoad()
        {
            // Hook up commands
            this.GotoFirstPageCommand = new Command(() => this.CurrentPage = 1, () => this.TotalRecords != 0 && this.CurrentPage > 1);
            this.GotoLastPageCommand = new Command(() => this.CurrentPage = this.PageCount, () => this.TotalRecords != 0 && this.CurrentPage < this.PageCount);
            this.GotoNextPageCommand = new Command(() => ++this.CurrentPage, () => this.TotalRecords != 0 && this.CurrentPage < this.PageCount);
            this.GotoPreviousPageCommand = new Command(() => --this.CurrentPage, () => this.TotalRecords != 0 && this.CurrentPage > 1);
        }

        #endregion
    }
}
