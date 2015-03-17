using DesignerTool.AppLogic;
using DesignerTool.AppLogic.Settings;
using DesignerTool.AppLogic.ViewModels;
using DesignerTool.Common.Global;
using DesignerTool.Common.Logging;
using DesignerTool.DataAccess.Connection;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DesignerTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool _isStartUp = true;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Need to start up the Context used in this Session.
            new WpfSession();
            WpfSession.Current.UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();

            // 1. Set up application paths.
            ApplicationPaths.Initialize();

            // 2. Create Database Connection string
            AppSession.Current.ConnectionString = ConnectionManager.GetEFConnectionString(ApplicationPaths.DatabaseFilePath);
            
            // 3. Start the application.
            var appVM = new AppViewModel(WpfSession.Current.CreateContext());
            appVM.Start();

            this._isStartUp = false;
        }

        #region Unhandled Exceptions

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log the exception
            Logger.Log(e.Exception.Message);

            // TODO: What's the right thing to do here.
            // Unhandled exception
            string message = e.Exception.Message;
            if (e.Exception.InnerException != null)
            {
                message += String.Format("{0}{1}", Environment.NewLine, e.Exception.InnerException.Message);
            }
            message += String.Format("{0}{0}{1}", Environment.NewLine, e.Exception.StackTrace);

            if (this._isStartUp)
            {
                // HACK: NF - Workaround for errors happening on App Startup. http://connect.microsoft.com/VisualStudio/feedback/details/600197/wpf-splash-screen-dismisses-dialog-box
                Window temp = new Window() { Visibility = Visibility.Hidden };
                temp.Name = "temp";
                temp.Show();
                MessageBox.Show(temp, message, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Stop);
                App.Current.Shutdown(1);

                this._isStartUp = false;
            }
            else
            {
                MessageBox.Show(message, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

            e.Handled = true;
        }

        #endregion

        #region Exit

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Save settings to local XML file.
            SettingsManager.Local.SaveToFile();
            
            //TODO: LocalDB detach?
        }

        #endregion
    }
}
