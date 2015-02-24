using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Security
{
    public class Permission : IPermission
    {
        #region Ctors

        public Permission(Type permissionType)
        {
            this._permissionType = permissionType;
        }

        public Permission(Type permissionType, bool canRead, bool canModify, bool canDelete)
            : this(permissionType)
        {
            this._canRead = canRead;
            this._canModify = canModify;
            this._canDelete = canDelete;
        }

        #endregion

        #region Properties

        private Type _permissionType;
        public Type PermissionType
        {
            get { return this._permissionType; }
        }

        private bool _canRead = false;
        public bool CanRead
        {
            get { return this._canRead; }
        }

        private bool _canModify = false;
        public bool CanModify
        {
            get { return this._canModify; }

        }

        private bool _canDelete = false;
        public bool CanDelete
        {
            get { return this._canDelete; }
        }

        #endregion

        #region Methods

        internal void SetPermission(bool canRead, bool canModify, bool canDelete)
        {
            this._canRead = canRead;
            this._canModify = canModify;
            this._canDelete = canDelete;
        }

        #endregion
    }
}
