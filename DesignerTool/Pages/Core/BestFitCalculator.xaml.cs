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
            stackBoards.Children.Clear();

            int canvasHeight = 600;
            int canvasWidth = 300;

            var rectangles = new List<IBoard>()
            {
                new Board(100, 200, true),
                new Board(200, 200, true),
                new Board(300, 200, true),
                new Board(100, 300, true),
                new Board(10, 30, true),
                new Board(30, 45, true),
                new Board(10, 10, true),
                new Board(10, 30, true),
                new Board(70, 100, true),
                new Board(80, 40, true),
                new Board(100, 100, true),
                new Board(100, 200, true),
                new Board(200, 200, true),
                new Board(300, 200, true),
                new Board(100, 300, true),
                new Board(10, 30, true),
                new Board(30, 45, true),
                new Board(10, 10, true),
                new Board(10, 30, true),
                new Board(70, 100, true),
                new Board(80, 40, true),
                new Board(100, 100, true)
            };

            Mapper.Canvas _canvas = new Mapper.Canvas();
            _canvas.SetCanvasDimensions(canvasWidth, canvasHeight);
            
            MapperOptimalEfficiency<Sheet> mapper = new MapperOptimalEfficiency<Sheet>(_canvas);

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
                    r.Height = item.Board.Height;
                    r.Width = item.Board.Width;
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
