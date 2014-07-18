using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Mvvm.Paging
{
    /// <summary>
    /// Provides context for the <see cref="PagingController.CurrentPageChanged"/> event.
    /// </summary>
    public class CurrentPageChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentPageChangedEventArgs"/> class.
        /// </summary>
        /// <param name="startIndex">The index of the first item in the current page..</param>
        /// <param name="totalRecords">The count of items in the current page.</param>
        public CurrentPageChangedEventArgs(int startIndex, int totalRecords)
        {
            this.StartIndex = startIndex;
            this.TotalRecords = totalRecords;
        }

        /// <summary>
        /// Gets the index of the first item in the current page.
        /// </summary>
        /// <value>The index of the first item.</value>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Gets the count of items in the current page.
        /// </summary>
        /// <value>The item count.</value>
        public int TotalRecords { get; private set; }
    }
}
