using DesignerTool.AppLogic.ViewModels.Tools;
using DesignerTool.Controls;
using DesignerTool.Packing.Board;
using DesignerTool.Packing.Sheet;
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

namespace DesignerTool.Pages.Tools
{
    //TODO: Move all this logic to the ViewModel
    /// <summary>
    /// Interaction logic for BestFitCalculator.xaml
    /// </summary>
    public partial class BestFitCalculator : BaseView
    {
        #region ViewModel

        private BestFitCalculatorViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is BestFitCalculatorViewModel))
                {
                    return null;
                }
                return (BestFitCalculatorViewModel)this.DataContext;
            }
        }

        #endregion

        private List<Color> colours = new List<Color>()
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
            Colors.LightSeaGreen,            
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
            Colors.LightSeaGreen,  
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
            Colors.LightSeaGreen,  
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

        #region Load

        public BestFitCalculator()
        {
            InitializeComponent();
        }

        public override void PageLoaded()
        {
            this.ViewModel.Load();
        }

        #endregion

        private List<IBoard> GetBoards()
        {
            return new List<IBoard>()
            {
                new Board(100, 200),
                new Board(200, 100),
                new Board(300, 100),
                //new Board(150, 150),
                //new Board(150, 150),
                //new Board(150, 150),
                new Board(150, 150),
                new Board(140, 80),
                new Board(180, 300)

                //new Board(100, 200),
                //new Board(200, 200),
                //new Board(300, 200),
                //new Board(100, 300),
                //new Board(10, 30),
                //new Board(30, 45),
                //new Board(10, 10),
                //new Board(10, 30),
                //new Board(70, 100),
                //new Board(80, 40),
                //new Board(100, 100),
                //new Board(100, 200),
                //new Board(200, 200),
                //new Board(300, 200),
                //new Board(100, 300),
                //new Board(300, 100),
                //new Board(300, 100),
                //new Board(300, 100),
                //new Board(300, 100),
                //new Board(300, 100),
                //new Board(400, 400),
                //new Board(300, 100),
                //new Board(10, 30),
                //new Board(30, 45),
                //new Board(10, 10),
                //new Board(10, 30),
                //new Board(70, 100),
                //new Board(80, 40),
                //new Board(100, 100),                
                //new Board(100, 50),             
                //new Board(700, 300),
            };
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            stackBoards.Children.Clear();

            Sheet _sheet = new Sheet(600, 300, chkHasGrain.IsChecked.Value);
            SheetMapper<Sheet> mapper = new SheetMapper<Sheet>(_sheet);
            IEnumerable<IBoard> boardsTooLarge;

            var mappedSheets = mapper.MapSheets(GetBoards(), out boardsTooLarge);

            this.drawBoards(mappedSheets);
        }

        private void drawBoards(IEnumerable<Sheet> mappedSheets)
        {
            try
            {
                foreach (var sheet in mappedSheets)
                {
                    System.Windows.Controls.Canvas canvas = new System.Windows.Controls.Canvas();
                    canvas.Background = new SolidColorBrush(Colors.Gray);
                    canvas.Margin = new Thickness(10);
                    canvas.Height = sheet.Height;
                    canvas.Width = sheet.Width;

                    if (sheet.IsFlipped)
                    {
                        var rotate = new RotateTransform(90d);
                        canvas.LayoutTransform = rotate;
                    }

                    stackBoards.Children.Add(canvas);

                    foreach (var item in sheet.MappedImages)
                    {
                        int index;
                        try
                        {
                            index = sheet.MappedImages.IndexOf(item);
                            Rectangle r = new Rectangle();
                            //   r.StrokeDashArray = new DoubleCollection() { 4 };
                            //r.StrokeThickness = 1d;
                            r.Height = item.Board.Height;
                            r.Width = item.Board.Width;
                            r.ToolTip = index + 1;
                            r.Fill = new SolidColorBrush(colours[index]);

                            canvas.Children.Add(r);
                            System.Windows.Controls.Canvas.SetLeft(r, item.X);
                            System.Windows.Controls.Canvas.SetTop(r, item.Y);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
