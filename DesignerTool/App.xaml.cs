using DesignerTool.AppLogic;
using DesignerTool.AppLogic.Data;
using DesignerTool.AppLogic.Security;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Mvvm;
using DesignerTool.Common.Settings;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            new WpfContext();

            ApplicationPaths.CreateAppDirectories();

            // Test database connection
            if(!this.testDatabaseConnection())
            {
                SessionContext.Current.ShowMessage("Could not establish database connection.", "Database connection failed", Common.Enums.UserMessageType.Error);
            }

            LicenseManager.Evaluate();

            this._isStartUp = false;
        }

        private bool testDatabaseConnection()
        {            
            try
            {
                using (DesignerToolDbEntities ctx = new DesignerToolDbEntities())
                {
                    var test = ctx.SystemSettings.FirstOrDefault();
                    return test != null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Database Test Connection Failed. " + ex.Message));
                return false;
            }
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
            LocalSettings.Current.saveToFile();

            //TODO: LocalDB detach?
        }

        #endregion
    }
}
