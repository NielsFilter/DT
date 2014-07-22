using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DesignerTool.Common.Mvvm.Mapping;
using DialogResult = System.Windows.Forms.DialogResult;

namespace DesignerTool.Common.Mvvm.Services
{
    /// <summary>
    /// Class responsible for abstracting ViewModels from Views.
    /// </summary>
    public class DialogService : IDialogService
    {
        private readonly HashSet<FrameworkElement> views;
        private readonly IWindowViewModelMappings windowViewModelMappings;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        /// <param name="windowViewModelMappings">
        /// The window ViewModel mappings. Default value is null.
        /// </param>
        public DialogService(IWindowViewModelMappings windowViewModelMappings = null)
        {
            this.windowViewModelMappings = windowViewModelMappings;
            views = new HashSet<FrameworkElement>();
        }

        #endregion

        #region IDialogService Members

        /// <summary>
        /// Gets the registered views.
        /// </summary>
        public ReadOnlyCollection<FrameworkElement> Views
        {
            get { return new ReadOnlyCollection<FrameworkElement>(views.ToList()); }
        }

        /// <summary>
        /// Registers a View.
        /// </summary>
        /// <param name="view">The registered View.</param>
        public void Register(FrameworkElement view)
        {
            // Get owner window
            Window owner = getOwner(view);
            if (owner == null)
            {
                // Perform a late register when the View hasn't been loaded yet.
                // This will happen if e.g. the View is contained in a Frame.
                view.Loaded += this.lateRegister;
                return;
            }

            // Register for owner window closing, since we then should unregister View reference,
            // preventing memory leaks
            owner.Closed += this.ownerClosed;

            views.Add(view);
        }

        /// <summary>
        /// Unregisters a View.
        /// </summary>
        /// <param name="view">The unregistered View.</param>
        public void Unregister(FrameworkElement view)
        {
            views.Remove(view);
        }

        /// <summary>
        /// Shows a dialog.
        /// </summary>
        /// <remarks>
        /// The dialog used to represent the ViewModel is retrieved from the registered mappings.
        /// </remarks>
        /// <param name="ownerViewModel">
        /// A ViewModel that represents the owner window of the dialog.
        /// </param>
        /// <param name="viewModel">The ViewModel of the new dialog.</param>
        /// <returns>
        /// A nullable value of type bool that signifies how a window was closed by the user.
        /// </returns>
        public bool? ShowDialog(object ownerViewModel, object viewModel)
        {
            Type dialogType = windowViewModelMappings.GetViewType(viewModel.GetType());

            // Create dialog and set properties
            var dialog = (Window)Activator.CreateInstance(dialogType);
            dialog.Owner = this.findOwnerWindow(ownerViewModel);
            dialog.DataContext = viewModel;

            // Show dialog
            return dialog.ShowDialog();
        }

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A ViewModel that represents the owner window of the message box.
        /// </param>
        /// <param name="messageBoxText">A string that specifies the text to display.</param>
        /// <param name="caption">A string that specifies the title bar caption to display.</param>
        /// <param name="button">
        /// A MessageBoxButton value that specifies which button or buttons to display.
        /// </param>
        /// <param name="icon">A MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>
        /// A MessageBoxResult value that specifies which message box button is clicked by the user.
        /// </returns>
        public MessageBoxResult ShowMessageBox(
            object ownerViewModel,
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon)
        {
            return MessageBox.Show(this.findOwnerWindow(ownerViewModel), messageBoxText, caption, button, icon);
        }

        /// <summary>
        /// Shows the OpenFileDialog.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A ViewModel that represents the owner window of the dialog.
        /// </param>
        /// <param name="openFileDialog">The interface of a open file dialog.</param>
        /// <returns>DialogResult.OK if successful; otherwise DialogResult.Cancel.</returns>
        public bool? ShowOpenFileDialog(object ownerViewModel, IOpenFileDialog openFileDialog)
        {
            // Create OpenFileDialog with specified ViewModel
            OpenFileDialog dialog = new OpenFileDialog(openFileDialog);

            // Show dialog
            return dialog.ShowDialog(this.findOwnerWindow(ownerViewModel));
        }

        /// <summary>
        /// Shows the FolderBrowserDialog.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A ViewModel that represents the owner window of the dialog.
        /// </param>
        /// <param name="folderBrowserDialog">The interface of a folder browser dialog.</param>
        /// <returns>The DialogResult.OK if successful; otherwise DialogResult.Cancel.</returns>
        public DialogResult ShowFolderBrowserDialog(object ownerViewModel, IFolderBrowserDialog folderBrowserDialog)
        {
            // Create FolderBrowserDialog with specified ViewModel
            FolderBrowserDialog dialog = new FolderBrowserDialog(folderBrowserDialog);

            // Show dialog
            return dialog.ShowDialog(new WindowWrapper(this.findOwnerWindow(ownerViewModel)));
        }

        #endregion

        #region Attached properties

        #region IsRegisteredView

        /// <summary>
        /// Attached property describing whether a FrameworkElement is acting as a View in MVVM.
        /// </summary>
        public static readonly DependencyProperty IsRegisteredViewProperty =
            DependencyProperty.RegisterAttached(
            "IsRegisteredView",
            typeof(bool),
            typeof(DialogService),
            new UIPropertyMetadata(IsRegisteredViewPropertyChanged));

        /// <summary>
        /// Gets value describing whether FrameworkElement is acting as View in MVVM.
        /// </summary>
        public static bool GetIsRegisteredView(FrameworkElement target)
        {
            return (bool)target.GetValue(IsRegisteredViewProperty);
        }

        /// <summary>
        /// Sets value describing whether FrameworkElement is acting as View in MVVM.
        /// </summary>
        public static void SetIsRegisteredView(FrameworkElement target, bool value)
        {
            target.SetValue(IsRegisteredViewProperty, value);
        }

        /// <summary>
        /// Is responsible for handling IsRegisteredViewProperty changes, i.e. whether
        /// FrameworkElement is acting as View in MVVM or not.
        /// </summary>
        private static void IsRegisteredViewPropertyChanged(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            // The Visual Studio Designer or Blend will run this code when setting the attached
            // property, however at that point there is no IDialogService registered
            // in the ServiceLocator which will cause the Resolve method to throw a ArgumentException.
            if (DesignerProperties.GetIsInDesignMode(target)) return;

            FrameworkElement view = target as FrameworkElement;
            if (view != null)
            {
                if ((bool)e.NewValue)
                {
                    ServiceLocator.Resolve<IDialogService>().Register(view);
                }
                else
                {
                    ServiceLocator.Resolve<IDialogService>().Unregister(view);
                }
            }
        }

        public static bool? GetDialogResult(DependencyObject obj)
        {
            return (bool?)obj.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(DependencyObject obj, bool? value)
        {
            obj.SetValue(DialogResultProperty, value);
        }

        #endregion

        #region DialogResult

        // Using a DependencyProperty as the backing store for DialogResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(DialogService), new UIPropertyMetadata(DialogResultPropertyChanged));

        private static void DialogResultPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            // The Visual Studio Designer or Blend will run this code when setting the attached
            // property, however at that point there is no IDialogService registered
            // in the ServiceLocator which will cause the Resolve method to throw a ArgumentException.
            if (DesignerProperties.GetIsInDesignMode(target)) return;

            Window wnd = getOwner(target as FrameworkElement);
            if (wnd != null)
            {
                wnd.DialogResult = e.NewValue as bool?;
            }
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Finds window corresponding to specified ViewModel.
        /// </summary>
        private Window findOwnerWindow(object viewModel)
        {
            FrameworkElement view = views.SingleOrDefault(v => ReferenceEquals(v.DataContext, viewModel));
            if (view == null)
            {
                if (views.Count == 1)
                {
                    // Shell view
                    view = views.First();
                }
                else
                {
                    throw new ArgumentException("Viewmodel is not referenced by any registered View.");
                }
            }

            // Get owner window
            Window owner = view as Window;
            if (owner == null)
            {
                owner = Window.GetWindow(view);
            }

            // Make sure owner window was found
            if (owner == null)
            {
                throw new InvalidOperationException("View is not contained within a Window.");
            }

            return owner;
        }

        /// <summary>
        /// Callback for late View register. It wasn't possible to do a instant register since the
        /// View wasn't at that point part of the logical nor visual tree.
        /// </summary>
        private void lateRegister(object sender, RoutedEventArgs e)
        {
            FrameworkElement view = sender as FrameworkElement;
            if (view != null)
            {
                // Unregister loaded event
                view.Loaded -= this.lateRegister;

                // Register the view
                Register(view);
            }
        }

        /// <summary>
        /// Handles owner window closed, View service should then unregister all Views acting
        /// within the closed window.
        /// </summary>
        private void ownerClosed(object sender, EventArgs e)
        {
            Window owner = sender as Window;
            if (owner != null)
            {
                // Find Views acting within closed window
                foreach (var view in views.Where(v => Window.GetWindow(v) == owner).ToArray())
                {
                    Unregister(view);
                }
            }
        }

        /// <summary>
        /// Gets the owning Window of a view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The owning Window if found; otherwise null.</returns>
        private static Window getOwner(FrameworkElement view)
        {
            if (view == null)
            {
                return null;
            }

            var wnd = view as Window;
            if (wnd == null)
            {
                wnd = Window.GetWindow(view);
            }
            return wnd;
        }

        #endregion
    }
}