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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesignerTool.Controls
{
    /// <summary>
    /// Interaction logic for LoadingLabel.xaml
    /// </summary>
    public partial class LoadingLabel : UserControl
    {
        public LoadingLabel()
        {
            InitializeComponent();

            var sb = (Storyboard)this.FindResource("Rotate");
            this.BeginStoryboard(sb);
        }

        public string LoadingMessage
        {
            get { return (string)GetValue(LoadingMessageProperty); }
            set { SetValue(LoadingMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadingMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingMessageProperty =
            DependencyProperty.Register("LoadingMessage", typeof(string), typeof(LoadingLabel), new UIPropertyMetadata("Loading..."));
    }
}
