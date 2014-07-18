using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesignerTool.Common.Mvvm.ViewModels;

namespace DesignerTool.Common.Mvvm.Interfaces
{
    public interface IShellPopup : IMasterViewModel
    {
        ViewModelBase ParentViewModel { get; set; }
        void Close();
        bool CanClose();
    }
}
