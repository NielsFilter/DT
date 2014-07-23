using DesignerTool.Common.Mvvm.Mapping;
using DesignerTool.Pages.Admin;
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
            // Home screen
            views.Add(typeof(HomeViewModel), typeof(HomeView));
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
            //// User
            //views.Add(typeof(UserListViewModel), typeof(UserListView));
            //views.Add(typeof(UserDetailViewModel), typeof(UserDetailView));

            //// Person
            //views.Add(typeof(PersonListViewModel), typeof(PersonListView));
            //views.Add(typeof(PersonDetailViewModel), typeof(PersonDetailView));
            //views.Add(typeof(PersonSearchViewModel), typeof(PersonSearchView));

            //// User Groups
            //views.Add(typeof(UserGroupListViewModel), typeof(UserGroupListView));
            //views.Add(typeof(UserGroupDetailViewModel), typeof(UserGroupDetailView));
            //views.Add(typeof(UserGroupSearchViewModel), typeof(UserGroupSearchView));

            //// User Group Permission
            ////views.Add(typeof(UserGroupPermissionListViewModel), typeof(UserGroupPermissionListView));
            //views.Add(typeof(UserGroupPermissionDetailViewModel), typeof(UserGroupPermissionDetailView));
            //views.Add(typeof(UserGroupPermissionSearchViewModel), typeof(UserGroupPermissionSearchView));
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
