using DesignerTool.Common.Mvvm.Mapping;
using DesignerTool.Pages.Admin;
using DesignerTool.Pages.Core;
using DesignerTool.Pages.Shell;
using DesignerTool.Pages.Tools;
using DesignerTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool
{
    public class ViewMapper : IViewMapper
    {
        public IDictionary<Type, Type> MappedViews
        {
            get
            {
                // Register Views with ViewModels
                IDictionary<Type, Type> views = new Dictionary<Type, Type>();

                mapShell(ref views);
                mapAdmin(ref views);
                mapCore(ref views);
                mapTools(ref views);

                return views;
            }
        }

        #region Shell Module

        private void mapShell(ref IDictionary<Type, Type> views)
        {
            views.Add(typeof(LoginViewModel), typeof(LoginView));
            views.Add(typeof(ShellViewModel), typeof(ShellView));
            views.Add(typeof(ShellPopupViewModel), typeof(ShellPopupView));

            views.Add(typeof(HomeViewModel), typeof(HomeView));
            views.Add(typeof(UserActivationViewModel), typeof(UserActivation));
        }

        #endregion

        #region Admin Module

        private void mapAdmin(ref IDictionary<Type, Type> views)
        {
            // System User
            views.Add(typeof(UserListViewModel), typeof(UserList));
            views.Add(typeof(UserDetailViewModel), typeof(UserDetail));
        }

        #endregion

        #region Core Module

        private void mapCore(ref IDictionary<Type, Type> views)
        {
            // Best Fit Calculator
            views.Add(typeof(BestFitCalculatorViewModel), typeof(BestFitCalculator));
        }

        #endregion

        #region Tools Module

        private void mapTools(ref IDictionary<Type, Type> views)
        {
            // Activation Key Generator (Licensing)
            views.Add(typeof(ActivationKeyGeneratorViewModel), typeof(ActivationKeyGenerator));
        }

        #endregion
    }
}
