using DesignerTool.Classes;
using DesignerTool.Common.Mvvm.Views;
using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesignerTool.Pages.Core
{
    /// <summary>
    /// Interaction logic for BestFitCalculator.xaml
    /// </summary>
    public partial class BestFitCalculator : BaseView
    {
        public BestFitCalculator()
        {
            InitializeComponent();

            this.Test_Click(null, null);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            int canvasHeight = 300;
            int canvasWidth = 600;

            var rectangles = new List<IImageInfo>()
            {
                new ImageInfo(100, 200),
                new ImageInfo(200, 200),
                new ImageInfo(300, 200),
                new ImageInfo(100, 300),
                new ImageInfo(10, 30),
                new ImageInfo(30, 45),
                new ImageInfo(10, 10),
                new ImageInfo(10, 30),
                new ImageInfo(70, 100),
                new ImageInfo(80, 40),
                new ImageInfo(100, 100),
                new ImageInfo(100, 200),
                new ImageInfo(200, 200),
                new ImageInfo(300, 200),
                new ImageInfo(100, 300),
                new ImageInfo(10, 30),
                new ImageInfo(30, 45),
                new ImageInfo(10, 10),
                new ImageInfo(10, 30),
                new ImageInfo(70, 100),
                new ImageInfo(80, 40),
                new ImageInfo(100, 100)
            };

            Mapper.Canvas _canvas = new Mapper.Canvas();
            _canvas.SetCanvasDimensions(canvasWidth, canvasHeight);
            
            MapperOptimalEfficiency<Sprite> mapper = new MapperOptimalEfficiency<Sprite>(_canvas);

            var sprites = mapper.Mapping(rectangles);

            List<Color> colours = new List<Color>()
            {
                Colors.Blue,
                Colors.Green,
                Colors.Red,
                Colors.Orange,
                Colors.Pink,
                Colors.Purple,
                Colors.LightBlue,
                Colors.Yellow,
                Colors.Black,
                Colors.Silver,
                Colors.LimeGreen,
                Colors.Fuchsia,
                Colors.Firebrick,
                Colors.DarkBlue,
                Colors.DarkRed,
                Colors.DarkOrange,
                Colors.DarkCyan,
                Colors.DarkGoldenrod,
                Colors.DarkKhaki,
                Colors.LightSalmon,
                Colors.LightSlateGray,
                Colors.SteelBlue,
                Colors.LightSeaGreen
            };

            foreach (var sprite in sprites)
            {
                System.Windows.Controls.Canvas canvas = new System.Windows.Controls.Canvas();
                canvas.Background = new SolidColorBrush(Colors.Gray);
                canvas.Margin = new Thickness(10);
                canvas.Height = canvasHeight;
                canvas.Width = canvasWidth;
                stackBoards.Children.Add(canvas);

                foreach (var item in sprite.MappedImages)
                {
                    int index = sprite.MappedImages.IndexOf(item);

                    Rectangle r = new Rectangle();
                    r.Height = item.ImageInfo.Height;
                    r.Width = item.ImageInfo.Width;
                    r.ToolTip = index + 1;
                    r.Fill = new SolidColorBrush(colours[index]);

                    canvas.Children.Add(r);
                    System.Windows.Controls.Canvas.SetLeft(r, item.X);
                    System.Windows.Controls.Canvas.SetTop(r, item.Y);
                }
            }
        }
    }
}
