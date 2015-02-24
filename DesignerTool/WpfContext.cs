using DesignerTool.AppLogic;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Pages.Admin;
using DesignerTool.Pages.Core;
using DesignerTool.Pages.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DesignerTool
{
    public class WpfContext : SessionContext
    {
        public WpfContext()
        {
            if (SessionContext.Current == null)
            {
                SessionContext.Current = this;
            }
        }

        #region Singleton

        public static new WpfContext Current
        {
            get
            {
                if (SessionContext.Current == null)
                {
                    SessionContext.Current = new WpfContext();
                }
                return SessionContext.Current as WpfContext;
            }
        }

        #endregion

        private Dictionary<Type, Type> _pageLookup;
        internal Dictionary<Type, Type> PageLookup
        {
            get
            {
                if (this._pageLookup == null)
                {
                    this._pageLookup = new Dictionary<Type, Type>();

                    // Core
                    this._pageLookup.Add(typeof(HomeViewModel), typeof(HomeView));
                    this._pageLookup.Add(typeof(LoginViewModel), typeof(LoginView));
                    this._pageLookup.Add(typeof(UserActivationViewModel), typeof(UserActivation));
                    this._pageLookup.Add(typeof(UserListViewModel), typeof(UserList));
                    this._pageLookup.Add(typeof(UserDetailViewModel), typeof(UserDetail));

                    // Calculator
                    this._pageLookup.Add(typeof(BestFitCalculatorViewModel), typeof(BestFitCalculator));
                }
                return this._pageLookup;
            }
        }

        #region Navigate

        internal ContentControl MainContent { get; set; }

        public override void Navigate(ViewModelBase viewModel)
        {
            if (this.MainContent != null)
            {
                Type pageType = this.PageLookup.FirstOrDefault(x => x.Key == viewModel.GetType()).Value;
                if (pageType == null)
                {
                    throw new NotImplementedException(String.Format("The ViewModel '{0}' has no corresponding view. You need hook it up to a view in '{1}'", viewModel.GetType().Name, this.GetType().Name));
                }

                // Here's the change page logic. Added it to a delegate, which we'll check for UI Thread access first.
                Action changePageDel = () => changePage(pageType, viewModel);

                if (Application.Current.Dispatcher.CheckAccess())
                {
                    // Normal call.
                    changePageDel.Invoke();
                }
                else
                {
                    // Invoke UI Thread Dispatcher to do work.
                    Application.Current.Dispatcher.Invoke(changePageDel);
                }
            }
        }

        private void changePage(Type pageType, ViewModelBase viewModel)
        {
            FrameworkElement pg = Activator.CreateInstance(pageType) as FrameworkElement;
            if (pg != null)
            {
                this.MainContent.Content = pg;
                pg.DataContext = viewModel;
            }
        }

        #endregion

        #region Dialogs & Messages

        public override UserMessageResults ShowMessage(string message, string caption = null, UserMessageType msgType = UserMessageType.Information, UserMessageButtons buttons = UserMessageButtons.OK)
        {
            // Message
            if (string.IsNullOrWhiteSpace(message))
            {
                message = NotificationAttribute.GetCaption(msgType);
            }

            // Caption
            if (string.IsNullOrWhiteSpace(caption))
            {
                caption = NotificationAttribute.GetCaption(msgType);
            }

            // Buttons
            MessageBoxButton msgBoxBtn = MessageBoxButton.OK;
            switch (buttons)
            {
                case UserMessageButtons.OK:
                    msgBoxBtn = MessageBoxButton.OK;
                    break;
                case UserMessageButtons.OKCancel:
                    msgBoxBtn = MessageBoxButton.OK;
                    break;
                case UserMessageButtons.YesNo:
                    break;
                case UserMessageButtons.YesNoCancel:
                    break;
            }

            // Images
            MessageBoxImage msgBoxImg = MessageBoxImage.Information;
            switch (msgType)
            {
                case UserMessageType.Error:
                    msgBoxImg = MessageBoxImage.Error;
                    break;
                case UserMessageType.Information:
                    msgBoxImg = MessageBoxImage.Information;
                    break;
                case UserMessageType.Success:
                    msgBoxImg = MessageBoxImage.Information;
                    break;
                case UserMessageType.Warning:
                    msgBoxImg = MessageBoxImage.Exclamation;
                    break;
            }

            // Show the message
            MessageBoxResult res = MessageBox.Show(message, caption, msgBoxBtn, msgBoxImg);

            // Return result
            UserMessageResults usrMsgResult = UserMessageResults.None;
            switch (res)
            {
                case MessageBoxResult.Cancel:
                    usrMsgResult = UserMessageResults.Cancel;
                    break;
                case MessageBoxResult.No:
                    usrMsgResult = UserMessageResults.No;
                    break;
                case MessageBoxResult.None:
                    usrMsgResult = UserMessageResults.None;
                    break;
                case MessageBoxResult.OK:
                    usrMsgResult = UserMessageResults.OK;
                    break;
                case MessageBoxResult.Yes:
                    usrMsgResult = UserMessageResults.Yes;
                    break;
            }

            return usrMsgResult;
        }

        //public override void ShowMessage(string message, string caption, Common.Enums.UserMessageType messageType)
        //{
        //    // TODO: Customize this.
        //    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        //}

        #endregion
    }
}
