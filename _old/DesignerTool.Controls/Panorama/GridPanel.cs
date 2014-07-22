using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace DesignerTool.Controls.Panorama
{
    // Lays out items in a grid where an item is either shown, or not shown
    // the grid stretches horizontally.  It's not hard to add an orientation but I don't need it!
    // the elements MUST be derived from FrameworkElement and not UIElement as we need to set the width and height
    // of each element
    public class GridPanel : Canvas
    {
        #region Dependency Properties

        #region ItemHeight: Height of each item in the grid layout

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(GridPanel), new UIPropertyMetadata(120.0, ItemHeightChanged));
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        private static void ItemHeightChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // same as changing the height of the grid
            ((GridPanel)sender).ActualHeightChanged(sender, EventArgs.Empty);
        }
        #endregion

        #region ItemWidth: Width of each item in the grid layout

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(GridPanel), new UIPropertyMetadata(120.0, ItemWidthChanged));
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        private static void ItemWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // always need to re-layout
            ((GridPanel)sender).ArrangeChildren();
        }

        #endregion

        #region ItemMargin: Margin of each item in the grid layout

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(GridPanel), new UIPropertyMetadata(ItemMarginChanged));
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }
        private static void ItemMarginChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // always need to re-layout
            ((GridPanel)sender).ArrangeChildren();
        }

        #endregion

        public int MinRowWidth
        {
            get { return (int)GetValue(MinRowWidthProperty); }
            set { SetValue(MinRowWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinRowWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinRowWidthProperty =
            DependencyProperty.Register("MinRowWidth", typeof(int), typeof(GridPanel), new PropertyMetadata(MinRowWidthChanged));

        private static void MinRowWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
 	        if(e.OldValue != e.NewValue && d is GridPanel)
            {
                ((GridPanel)d).ArrangeChildren();
            }
        }


        #region TrackItemWidthSpanChanges: Switch on tracking of changes in the attached property ItemWidthSpan - only switch this on if it's going to change when the gridpanel is displayed

        public static readonly DependencyProperty TrackItemWidthSpanChangesProperty = DependencyProperty.Register("TrackItemWidthSpanChanges", typeof(bool), typeof(GridPanel), new UIPropertyMetadata(false));
        public bool TrackItemWidthSpanChanges
        {
            get { return (bool)GetValue(TrackItemWidthSpanChangesProperty); }
            set { SetValue(TrackItemWidthSpanChangesProperty, value); }
        }

        #endregion

        #region Attached property - ItemWidthSpan: How many 'ItemWidth' units a block will occupy.  Be careful not to make this too large or there may not be a solution to the layout algorithm...

        public static readonly DependencyProperty ItemWidthSpanProperty = DependencyProperty.RegisterAttached("ItemWidthSpan", typeof(int), typeof(GridPanel), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender));
        public static void SetItemWidthSpan(FrameworkElement element, int value)
        {
            if (value < 1)
            {
                value = 1;
            }
            element.SetValue(ItemWidthSpanProperty, value);
        }
        public static int GetItemWidthSpan(FrameworkElement element)
        {
            return (int)element.GetValue(ItemWidthSpanProperty);
        }

        #endregion

        #endregion

        #region Methods

        // CTOR
        public GridPanel()
        {
            _actualHeightChangeNotifier = new PropertyChangeNotifier(this, GridPanel.ActualHeightProperty);
            _actualHeightChangeNotifier.ValueChanged += this.ActualHeightChanged;
        }
        // the meat and potatos...lays out the grid
        private void ArrangeChildren()
        {
            // the layout strategy is:
            //	1. Fill all available rows
            //	2. Reduce the amount of free space at the end of each row
            //	3. Fill the top rows completely

            // this can be called before the control has been instantiated so _numberOfRows could be zero
            if (_numberOfRows == 0)
                return;

            // cache these for performance
            double itemWidth = this.ItemWidth;
            double itemHeight = this.ItemHeight;
            Thickness itemMargin = this.ItemMargin;

            int itemsCount = this.Children.Count;

            // no children, no layout
            if (itemsCount == 0)
                return;

            // cache for the children
            List<RowLayoutItem> items = new List<RowLayoutItem>();

            // do we need to attach a notifier to the children?
            bool createItemWidthSpanChangeNotifier = false;
            if (this.TrackItemWidthSpanChanges && _itemWidthSpanChangeNotifier == null)
            {
                createItemWidthSpanChangeNotifier = true;
                _itemWidthSpanChangeNotifier = new PropertyChangeNotifier[itemsCount];
            }

            int totalWidth = 0;
            int maxWidth = 0;
            int calculatedItemWidth;

            // first pass calculate the total width (in item units) and find the maximum item width 
            // as the total width cannot be less than this
            int index = 0;
            foreach (FrameworkElement item in this.Children)
            {
                items.Add(new RowLayoutItem() { ItemWidth = calculatedItemWidth = GridPanel.GetItemWidthSpan(item), Element = item });
                if (calculatedItemWidth > maxWidth)
                    maxWidth = calculatedItemWidth;
                totalWidth += calculatedItemWidth;

                if (createItemWidthSpanChangeNotifier)
                {
                    PropertyChangeNotifier pcn = new PropertyChangeNotifier(item, GridPanel.ItemWidthSpanProperty);
                    pcn.ValueChanged += this.ItemWidthSpanChanged;
                    _itemWidthSpanChangeNotifier[index++] = pcn;
                }
            }

            // second pass layout the items with a width of totalWidth / _numberOfRows
            // we also want to reduce the 'jaggedness' of the right hand side and so
            // an optimal layout is defined where the space on the right hand side is a minimum
            // where all rows have been laid out (otherwise it'll always lay it out in a single line!)

            List<RowLayout> optimalRows = null;
            int minimumSpace = int.MaxValue;
            bool fittingItems = true;

            // width of one row
            int rowWidth = Math.Min(maxWidth, (totalWidth / _numberOfRows) + (totalWidth % _numberOfRows > 0 ? 1 : 0));

            if (rowWidth < this.MinRowWidth)
            {
                rowWidth = this.MinRowWidth;
            }

            // temporary variables held outside the loop to reuce stack allocations
            List<RowLayout> rows;
            RowLayout rowItem;
            RowLayoutItem rowLayoutItem;

            maxWidth = 0;
            int space = 0;

            // do this until all the elements have been laid out
            while (fittingItems)
            {
                // elements in each row
                rows = new List<RowLayout>();

                // index into elements array
                index = 0;

                // lay out the items
                for (int row = 0; row < _numberOfRows; row++)
                {
                    // allocate the row layout
                    rowItem = new RowLayout();

                    // allocate elements to the row layout until they won't fit into the space or we run out of items
                    while (index < itemsCount && rowItem.TotalRowWidth < rowWidth)
                    {
                        rowLayoutItem = items[index];
                        calculatedItemWidth = rowLayoutItem.ItemWidth;

                        // is the item to be added greater than the allowed row width
                        if (rowItem.TotalRowWidth + calculatedItemWidth > rowWidth)
                        {
                            break;
                        }

                        // we can fit the item so add to the row list and increment the width
                        rowItem.RowItems.Add(rowLayoutItem);
                        index++;

                        // keep a copy of the maximum width
                        if ((rowItem.TotalRowWidth += calculatedItemWidth) > maxWidth)
                        {
                            maxWidth = rowItem.TotalRowWidth;
                        }
                    }
                    if (rowItem != null && rowItem.RowItems.Count > 0)
                    {
                        rows.Add(rowItem);
                    }
                }

                // if we couldn't lay out all the elements, increment the row width by 1 and try again.
                if (index < itemsCount)
                {
                    rowWidth++;
                }
                // if all rows filled, check for a minimum of space
                // but also weigh fill from the top so that it'll look neat by multiplying by the inverse row position squared 
                else
                {
                    fittingItems = false;
                    // check the free space
                    space = 0;
                    // calculate the free space and add the weighting
                    for (int i = 0; i < rows.Count; i++)
                    {
                        space += (maxWidth - rows[i].TotalRowWidth) * (rows.Count - i) * (rows.Count - i);
                    }

                    // if the row has less free space, cache it
                    if (space < minimumSpace)
                    {
                        minimumSpace = space;
                        optimalRows = rows;
                    }
                    // increment the row width to test the next solution
                    rowWidth++;
                }
            }

            // get the actual row width (not the guesstimate)
            rowWidth = 0;
            foreach (RowLayout r in optimalRows)
            {
                if (r.TotalRowWidth > rowWidth)
                {
                    rowWidth = r.TotalRowWidth;
                }
            }

            // set the Width of the canvas (obviously, don't set the height!!)
            if (rowWidth > 0)
            {
                this.Width = (rowWidth * itemWidth) + ((itemMargin.Left + itemMargin.Right) * (rowWidth));
            }
            else
            {
                this.Width = 0;
            }
            // layout the items
            double heightOffset = itemMargin.Top, widthOffset;
            FrameworkElement elem;
            foreach (RowLayout row in optimalRows)
            {
                widthOffset = itemMargin.Left;
                foreach (RowLayoutItem item in row.RowItems)
                {
                    Canvas.SetLeft(elem = item.Element, widthOffset);
                    Canvas.SetTop(elem, heightOffset);

                    elem.Width = ((calculatedItemWidth = item.ItemWidth) * itemWidth) + ((itemMargin.Left + itemMargin.Right) * (calculatedItemWidth - 1));
                    widthOffset += (calculatedItemWidth * itemWidth) + ((itemMargin.Left + itemMargin.Right) * calculatedItemWidth);
                    elem.Height = itemHeight;
                }
                heightOffset += itemHeight + itemMargin.Top + itemMargin.Bottom;
            }
        }

        // helper to work out the number of rows in the grid
        private static int GetNumberOfRows(double height, double itemHeight)
        {
            if (itemHeight <= 0)
                return 1;
            int ret = (int)Math.Floor(height / itemHeight);
            if (ret <= 0)
                return 0;
            return ret;
        }

        #endregion

        #region Event Handlers

        // fired when the height changes
        private void ActualHeightChanged(object sender, EventArgs e)
        {
            // check that the number of layout rows changes
            int numRows = GetNumberOfRows(this.ActualHeight, this.ItemHeight);
            if (numRows != _numberOfRows)
            {
                _numberOfRows = numRows;
                ArrangeChildren();
            }
        }
        // fired when an ItemWidthSpan changes in an attached control
        private void ItemWidthSpanChanged(object sender, EventArgs e)
        {
            this.ArrangeChildren();
        }

        #endregion

        #region Weak References

        // WeakReference implementations of AddValueChanged(...) for a dependency object

        // monitor the actual height changing
        private PropertyChangeNotifier _actualHeightChangeNotifier;

        // monitor the ItemWidthSpan in an attached object changing (if TrackItemWidthSpanChanges = true)
        private PropertyChangeNotifier[] _itemWidthSpanChangeNotifier;

        #endregion

        #region Private Fields

        // keep this to ensure small changes in height don't trigger a layout
        private int _numberOfRows;

        #endregion

        #region Layout Classes

        // Data for each row
        internal class RowLayout
        {
            internal int TotalRowWidth;
            internal List<RowLayoutItem> RowItems = new List<RowLayoutItem>();
        }
        // data for each item in a row
        internal class RowLayoutItem
        {
            internal int ItemWidth;
            internal FrameworkElement Element;
        }

        #endregion
    }

    // Courtesy of Andrew Smith - many thanks Andrew
    // http://agsmith.wordpress.com/2008/04/07/propertydescriptor-addvaluechanged-alternative/
    // Provides a weak binding to AddValueChanged
    public sealed class PropertyChangeNotifier : DependencyObject, IDisposable
    {
        #region Member Variables
        private WeakReference _propertySource;
        #endregion // Member Variables

        #region Constructor
        public PropertyChangeNotifier(DependencyObject propertySource, string path)
            : this(propertySource, new PropertyPath(path))
        {
        }
        public PropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
            : this(propertySource, new PropertyPath(property))
        {
        }
        public PropertyChangeNotifier(DependencyObject propertySource, PropertyPath property)
        {
            if (null == propertySource)
                throw new ArgumentNullException("propertySource");
            if (null == property)
                throw new ArgumentNullException("property");
            this._propertySource = new WeakReference(propertySource);
            Binding binding = new Binding();
            binding.Path = property;
            binding.Mode = BindingMode.OneWay;
            binding.Source = propertySource;
            BindingOperations.SetBinding(this, ValueProperty, binding);
        }
        #endregion // Constructor

        #region PropertySource
        public DependencyObject PropertySource
        {
            get
            {
                try
                {
                    // note, it is possible that accessing the target property
                    // will result in an exception so i’ve wrapped this check
                    // in a try catch
                    return this._propertySource.IsAlive
                    ? this._propertySource.Target as DependencyObject
                    : null;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion // PropertySource

        #region Value
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
        typeof(object), typeof(PropertyChangeNotifier), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangeNotifier notifier = (PropertyChangeNotifier)d;
            if (null != notifier.ValueChanged)
                notifier.ValueChanged(notifier, EventArgs.Empty);
        }

        /// <summary>
        /// Returns/sets the value of the property
        /// </summary>
        /// <seealso cref="ValueProperty"/>
        [Description("Returns/sets the value of the property")]
        [Category("Behavior")]
        [Bindable(true)]
        public object Value
        {
            get
            {
                return (object)this.GetValue(PropertyChangeNotifier.ValueProperty);
            }
            set
            {
                this.SetValue(PropertyChangeNotifier.ValueProperty, value);
            }
        }
        #endregion //Value

        #region Events
        public event EventHandler ValueChanged;
        #endregion // Events

        #region IDisposable Members
        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ValueProperty);
        }
        #endregion
    }
}
