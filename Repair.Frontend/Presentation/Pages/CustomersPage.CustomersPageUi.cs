using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    private sealed class CustomersPageUi(CustomersPageLogic logic, CustomersPageViewModel viewModel)
        : BaseUi<CustomersPageLogic, CustomersPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            ConfigureDefaultPageGrid(grid);

            grid.DefineRows(GridLength.Auto, new GridLength(1, GridUnitType.Star));
            grid.DefineColumns(GridUnitType.Star, [1,]);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateButtonGrid().SetRow(0));
            grid.Children.Add(CreateCustomersGrid().SetRow(1));
        }

        private UIElement CreateButtonGrid()
        {
            return SimplePieceFactory.CreateLeftButtonGrid("Create Customer", Logic.CreateCustomerClicked);
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Customers");
        }

        private CustomersGrid CreateCustomersGrid()
        {
            CustomersGrid.CustomersGridArguments arguments = Logic.GetArgumentsFactory().CreateCustomersGridArguments();

            var customersGrid = new CustomersGrid(arguments);

            ViewModel.CustomersGrid = customersGrid;

            customersGrid.ViewModel.DataGrid.SelectionChanged += Logic.CustomerClicked;

            return customersGrid;
        }
    }
}
