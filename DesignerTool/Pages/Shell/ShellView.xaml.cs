using DesignerTool.AppLogic.ViewModels.Shell;
using DesignerTool.Common.Enums;
using DesignerTool.Styles.Wpf;
using DesignerTool.Styles.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesignerTool.Pages.Shell
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : MetroWindow
    {

        #region ViewModel

        private ShellViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is ShellViewModel))
                {
                    return null;
                }
                return (ShellViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load

        public ShellView()
        {
            InitializeComponent();

            //TODO: TEMP: Testing THEMES
            WpfSession.Current.MainContent = this.contentMain;

            var themes = new[] { "BaseLight", "BaseDark" };
            var accents = new[] {
                                    "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt",
                                    "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna"
                               };

            var rnd = new Random();

            var randomTheme = themes[rnd.Next(0, themes.Length)];
            var randomAccent = accents[rnd.Next(0, accents.Length)];

            var theme = ThemeManager.GetAppTheme(randomTheme);
            var accent = ThemeManager.GetAccent(randomAccent);

            ThemeManager.ChangeAppStyle(Application.Current, accent, theme);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.Page_Loaded;
            this.ViewModel.Load();
        }
        
        #endregion

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.GoHome();
        }

        private void ShowMenu_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.ShowMenu();
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.GoUsers();
        }

        private void GenerateActivationKey_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.GoGenerateLicenseKey();
        }

        private void ActivateLicense_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.GoUserLicense();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.GoBack();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.LogOut();
        }

        private void ViewProfile_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.GoUserProfile();
        }

        private void ViewProfileMenu_Click(object sender, RoutedEventArgs e)
        {
            this.ctxViewProfile.IsOpen = true;
        }

        private void License_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.GoUserLicense();
        }

        private void Debtors_Click(object sender, MouseButtonEventArgs e)
        {
            this.ViewModel.GoDebtors();
        }

        private void Suppliers_Click(object sender, MouseButtonEventArgs e)
        {
            this.ViewModel.GoSuppliers();
        }

        private void UnitTypes_Click(object sender, MouseButtonEventArgs e)
        {
            this.ViewModel.GoUnitTypes();
        }
    }
}
