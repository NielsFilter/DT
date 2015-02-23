using DesignerTool.AppLogic;
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

        //TODO:
        #region Dialogs, Messages...

        public override void ShowError(string errorMessage, string caption = "Error")
        {
            MessageBox.Show(errorMessage, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public override void ShowMessage(string message, string caption = "Information")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //public override void ShowMessage(string message, string caption, Common.Enums.UserMessageType messageType)
        //{
        //    // TODO: Customize this.
        //    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        //}

        #endregion
    }
}
