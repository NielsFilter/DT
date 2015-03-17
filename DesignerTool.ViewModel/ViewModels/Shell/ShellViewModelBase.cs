using DesignerTool.Common.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.AppLogic;
using DesignerTool.AppLogic.ViewModels.Base;

namespace DesignerTool.AppLogic.ViewModels.Shell
{
    public class ShellBase : ViewModelBase, IParentViewModel
    {
        #region Constructors

        public ShellBase()
            : base()
        {
        }

        #endregion

        #region Properties

        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return this._isLoading;
            }
            set
            {
                this._isLoading = value;
                base.NotifyPropertyChanged("IsLoading");
            }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get
            {
                return this._loadingMessage;
            }
            set
            {
                if (value != this._loadingMessage)
                {
                    this._loadingMessage = value;
                    base.NotifyPropertyChanged("LoadingMessage");
                }
            }
        }

        #endregion
    }
}
