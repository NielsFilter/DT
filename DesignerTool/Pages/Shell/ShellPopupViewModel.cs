using DesignerTool.Common.Mvvm.Interfaces;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.ViewModels;

namespace DesignerTool.Pages.Shell
{
    public class ShellPopupViewModel : ShellBase, IShellPopup
    {
        #region Constructors

        public ShellPopupViewModel()
            : base()
        {
            this.IsPopup = true;
        }

        public ShellPopupViewModel(ViewModelBase parentViewModel)
            : this()
        {
            this.ParentViewModel = parentViewModel;
        }

        #endregion

        #region Commands

        public override void OnWireCommands()
        {
            base.OnWireCommands();
        }

        #endregion

        #region Properties

        public ViewModelBase ParentViewModel { get; set; }

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get
            {
                return this._dialogResult;
            }
            set
            {
                if (value != this._dialogResult)
                {
                    this._dialogResult = value;
                    base.NotifyPropertyChanged("DialogResult");
                }
            }
        }

        #endregion

        #region Private Methods

        private void hideNotification()
        {
            this.IsSaveShow = false;
            this.IsErrorShow = false;
        }

        #endregion

        #region Public Methods

        public void Close()
        {
            this.DialogResult = false;
        }

        public bool CanClose()
        {
            return this.IsPopup;
        }

        #endregion
    }
}
