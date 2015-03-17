using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesignerTool.Controls
{
    /// <summary>
    /// Interaction logic for SearchTextBox.xaml
    /// </summary>
    public partial class SearchTextBox : UserControl, INotifyPropertyChanged
    {
        #region Constructors

        public SearchTextBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency Properties

        #region SearchText

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchTextBox), new PropertyMetadata(string.Empty, SearchTextChanged));

        private static void SearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null && d is SearchTextBox && e.OldValue != e.NewValue)
            {
                var searchTextBox = ((SearchTextBox)d);
                searchTextBox.checkEnabled();
            }
        }

        #endregion

        #region SearchCommand

        //TODO: Remove Command
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchTextBox), new UIPropertyMetadata(null));

        #endregion

        #endregion

        #region Properties

        public bool CanSearch
        {
            get { return this.IsEnabled; }
        }

        public bool CanClear
        {
            get { return this.IsEnabled && !string.IsNullOrWhiteSpace(this.SearchText); }
        }

        #endregion

        #region Custom Events

        public delegate void SearchRequestEventHandler(string searchText);
        public event SearchRequestEventHandler SearchRequested;

        #endregion

        #region Click Events

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (this.CanSearch)
            {
                if (this.SearchRequested != null)
                {
                    // Child is hooked to the event, so we raise it.
                    this.SearchRequested(this.SearchText);
                }

                if (this.SearchCommand != null)
                {
                    // Able to Execute the search command.
                    this.SearchCommand.Execute(null);
                }
            }
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            if (this.CanClear)
            {
                this.SearchText = string.Empty;

                if (this.SearchRequested != null)
                {
                    // Child is hooked to the event, so we raise it.
                    this.SearchRequested(this.SearchText);
                }

                if (this.CanSearch && this.SearchCommand != null)
                {
                    // Able to Execute the search command.
                    this.SearchCommand.Execute(null);
                }
            }
        }

        #endregion

        #region Private Methods

        private void checkEnabled()
        {
            this.btnSearch.IsEnabled = this.CanSearch;
            this.btnClear.IsEnabled = this.CanClear;
        }

        #endregion

        #region INotifyPropertyChanged

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
