using DesignerTool.AppLogic.ViewModels.Base;
using DesignerTool.Packing.Board;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.AppLogic.ViewModels.Tools
{
    public class BestFitCalculatorViewModel : PageViewModel
    {
        private const int SHEET_HEIGHT = 100;
        private const int SHEET_WIDTH = 60;

        #region Constructors

        public BestFitCalculatorViewModel()
            : base()
        {
        }

        #endregion

        #region Properties

        public override string Heading
        {
            get { return "Best Fit Calculator"; }
        }

        private ObservableCollection<Board> _boards;
        public ObservableCollection<Board> Boards
        {
            get { return this._boards; }
            set
            {
                if (value != this._boards)
                {
                    this._boards = value;
                    base.NotifyPropertyChanged("Boards");
                }
            }
        }        

        #endregion

        //#region Commands

        //public Command CalculateCommand { get; set; }

        //public override void OnWireCommands()
        //{
        //    base.OnWireCommands();

        //    this.CalculateCommand = new Command(caclulate, () => true);
        //}

        //#endregion

        #region Load

        /// <summary>
        /// All initialization must happen here.
        /// </summary>
        public override void Load()
        {
            base.Load();

            this.Boards = new ObservableCollection<Board>();
        }

        #endregion

        #region Calculate

        private void caclulate()
        {
        }

        #endregion
    }
}
