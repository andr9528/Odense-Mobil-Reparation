using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Core
{
    public abstract class BaseUi<TLogic, TViewModel> where TLogic : class where TViewModel : class
    {
        protected BaseUi(TLogic logic, TViewModel viewModel)
        {
            Logic = logic;
            ViewModel = viewModel;
        }

        protected TLogic Logic { get; }
        protected TViewModel ViewModel { get; }

        public Grid CreateContentGrid()
        {
            Grid grid = GridFactory.CreateDefaultGrid();

            ConfigureGrid(grid);
            AddControlsToGrid(grid);

            return grid;
        }

        protected abstract void ConfigureGrid(Grid grid);

        protected abstract void AddControlsToGrid(Grid grid);
    }
}
