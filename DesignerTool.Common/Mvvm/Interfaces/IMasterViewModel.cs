using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DesignerTool.Common.Mvvm.Services;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.Enums;

namespace DesignerTool.Common.Mvvm.Interfaces
{
    /// <summary>
    /// This interface marks the ViewModel which is the "Shell" or "Master" ViewModel for the application.
    /// Any ViewModel "child" will then use this interface to communicate this "Parent" View Model.
    /// </summary>
    public interface IMasterViewModel
    {
        /// <summary>
        /// Hides or Shows Loading UI Display
        /// </summary>
        bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets the message showing while loading / busy
        /// </summary>
        string LoadingMessage { get; set; }

        /// <summary>
        /// Gets the current / active ViewModel. The "Master" ViewModel only ever loads one ViewModel at a time.
        /// </summary>
        ViewModelBase CurrentViewModel { get; set; }

        /// <summary>
        /// Gets the previous ViewModel. null if there was no previous ViewModel
        /// </summary>
        ViewModelBase PreviousViewModel { get; }

        /// <summary>
        /// Navigation. Changes the Active ViewModel to the one passed.
        /// </summary>
        /// <param name="vm">The new viewModel that the "Master" will show as the Active one</param>
        void ChangeViewModel(ViewModelBase vm);

        /// <summary>
        /// Refreshes the active view model (CurrentViewModel)
        /// </summary>
        void RefreshViewModel();

        /// <summary>
        /// Go back to the previous page (Loads the previous ViewModel)
        /// </summary>
        void GoBack();

        /// <summary>
        /// Checks if there is a previous page to go back to
        /// </summary>
        /// <returns>True = Can go Back, False = Cannot go back</returns>
        bool CanGoBack();

        /// <summary>
        /// Gets the <see cref="IDialogService"/> instance, which handles MessageBoxes, Dialogs, Popups etc.
        /// </summary>
        IDialogService DialogService { get; }

        /// <summary>
        /// Shows the user a message. The way that this message is displayed depends on the shell implementation and the <see cref="DesignerTool.Common.Enums.UserMessageType"/>
        /// </summary>
        /// <param name="msgType">Type of message from the <see cref="DesignerTool.Common.Enums.UserMessageType"/> enum</param>
        /// <param name="message">Message text</param>
        /// <param name="caption">Message caption / title</param>
        void ShowUserMessage(UserMessageType msgType, string message, string caption);
    }
}
