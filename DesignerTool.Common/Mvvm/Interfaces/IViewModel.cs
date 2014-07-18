using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Mvvm.Interfaces
{
    /// <summary>
    /// Interface used to indicate / tag that a class is a ViewModel. It forces any ViewModel to implement the <see cref="INotifyPropertyChanged"/> interface.
    /// This is important to easily find / use all ViewModels via this interface and control which members them must implement to be a ViewModel
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        void NotifyPropertyChanged(string propertyName);
    }
}
