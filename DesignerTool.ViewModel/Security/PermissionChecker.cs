using DesignerTool.Common.Enums;
using DesignerTool.Pages.Admin;
using DesignerTool.Pages.Core;
using DesignerTool.Pages.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Security
{
    public class PermissionChecker
    {
        public static IPermission GetPermission(Type permissionType)
        {
            if(AppSession.Current.LoggedInUser == null)
            {
                // Not logged in = no permissions.
                return new Permission(permissionType);
            }

            if (AppSession.Current.LoggedInUser.Role == RoleType.Internal.ToString())
            {
                // Internal has ALL permissions
                return new Permission(permissionType, true, true, true);
            }
            else if (AppSession.Current.LoggedInUser.Role == RoleType.Admin.ToString())
            {
                // Admin
                var adminPermission = AdminPermissions.FirstOrDefault(p => p.PermissionType == permissionType);
                if (adminPermission == null)
                {
                    // No permissions
                    return new Permission(permissionType);
                }
                return adminPermission;
            }
            else
            {
                // User
                var userPermission = UserPermissions.FirstOrDefault(p => p.PermissionType == permissionType);
                if (userPermission == null)
                {
                    // No permissions
                    return new Permission(permissionType, true, false, false);
                }
                return userPermission;
            }
        }

        #region User

        private static List<IPermission> _userPermissions;
        public static List<IPermission> UserPermissions
        {
            get
            {
                if (_userPermissions == null || _userPermissions.Count == 0)
                {
                    _userPermissions = loadUserPermissions();
                }
                return _userPermissions;

            }
        }

        private static List<IPermission> loadUserPermissions()
        {
            return new List<IPermission>()
            {
                new Permission(typeof(UserActivationViewModel), true, false, false),
                new Permission(typeof(BestFitCalculatorViewModel), true, true, true)
            };

        }

        #endregion

        #region Admin

        private static List<IPermission> _adminPermissions;
        public static List<IPermission> AdminPermissions
        {
            get
            {
                if (_adminPermissions == null || _adminPermissions.Count == 0)
                {
                    _adminPermissions = loadAdminPermissions();
                }
                return _adminPermissions;
            }
        }

        private static List<IPermission> loadAdminPermissions()
        {
            return new List<IPermission>()
            {
                new Permission(typeof(UserListViewModel), true, true, true),
                new Permission(typeof(UserDetailViewModel), true, true, true),
                new Permission(typeof(UserActivationViewModel), true, true, true),
                new Permission(typeof(BestFitCalculatorViewModel), true, true, true)
            };
        }

        #endregion
    }
}
