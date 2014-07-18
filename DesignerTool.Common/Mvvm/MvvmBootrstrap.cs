using DesignerTool.Common.Logging;
using DesignerTool.Common.Mvvm.Interfaces;
using DesignerTool.Common.Mvvm.Mapping;
using DesignerTool.Common.Mvvm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesignerTool.Common.Mvvm
{
    public static class MvvmBootstrap
    {
        public static IViewMapper ViewMapper { get; private set; }

        /// <summary>
        /// Gets or sets the master ViewModel which implents the <see cref="IMasterViewModel"/> interface.
        /// <example>
        /// To access properties on the "Master" ViewModel, simply do the following: 
        /// ApplicationGlobal.MasterViewModel.ChangeViewModel(new MyNewViewModel());
        /// </example>
        /// </summary>
        private static IMasterViewModel shellViewModel { get; set; }
        private static IMasterViewModel shellPopupViewModel { get; set; }

        public static TShellInterface GetShell<TShellInterface>()
        {
            try
            {
                return (TShellInterface)shellViewModel;
            }
            catch (Exception)
            {
                return default(TShellInterface);
            }
        }

        public static TShellPopupInterface GetShellPopup<TShellPopupInterface>()
        {
            try
            {
                return (TShellPopupInterface)shellPopupViewModel;
            }
            catch (Exception)
            {
                return default(TShellPopupInterface);
            }
        }

        public static void SetShellPopup(IMasterViewModel value)
        {
            shellPopupViewModel = value as IMasterViewModel;
        }

        public static void BootStrapApplication(IViewMapper mapper)
        {
            ViewMapper = mapper;
            var app = System.Windows.Application.Current;
            app.Activated += app_Activated;

            // Services (dialogs, ViewModel Mapper)
            ServiceLocator.RegisterSingleton<ILogger, FileLogger>();
            ServiceLocator.RegisterSingleton<IDialogService, DialogService>();
            ServiceLocator.RegisterSingleton<IWindowViewModelMappings, WindowViewModelMappings>();
            ServiceLocator.Register<IOpenFileDialog, OpenFileDialogViewModel>();

            mapViews();
        }

        private static void app_Activated(object sender, EventArgs e)
        {
            if (System.Windows.Application.Current.MainWindow == null
                || System.Windows.Application.Current.MainWindow.Name == "temp")
                return;

            // We set the MasterViewModel here, which allows other ViewModels to access methods on the root / Main window i.e. the shell View Model
            shellViewModel = System.Windows.Application.Current.MainWindow.DataContext as IMasterViewModel;
            if (shellViewModel == null)
            {
                throw new ApplicationException(string.Format("The startup window '{0}' must implement the IMasterViewModel interface.", System.Windows.Application.Current.MainWindow.Name));
            }
            var app = System.Windows.Application.Current;
            app.Activated -= app_Activated;
        }

        /// <summary>
        /// Maps the views to the viewModels according to the application's <see cref="IViewMapper"/> implementation
        /// </summary>
        /// <param name="mapper"></param>
        private static void mapViews()
        {
            var map = ServiceLocator.Resolve<IWindowViewModelMappings>();
            foreach (var item in ViewMapper.MappedViews)
            {
                map.Mappings.Add(item);

                DataTemplate template = new DataTemplate(item.Key);
                template.VisualTree = new FrameworkElementFactory(item.Value);

                DataTemplateKey key = new DataTemplateKey(item.Key);
                Application.Current.Resources.Add(key, template);
            }
        }
    }
}
