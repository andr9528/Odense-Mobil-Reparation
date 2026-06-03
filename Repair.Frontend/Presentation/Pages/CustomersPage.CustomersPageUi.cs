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
            grid.RowSpacing = 8;

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
            Grid buttonGrid = GridFactory.CreateDefaultGrid()
                .DefineColumns(GridLength.Auto, new GridLength(1, GridUnitType.Star));

            buttonGrid.Children.Add(CreateCreateCustomerButton().SetColumn(0));

            return buttonGrid;
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Customers");
        }

        private Button CreateCreateCustomerButton()
        {
            var button = new Button
            {
                Content = "Create Customer",
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            button.Click += Logic.CreateCustomerClicked;

            return button;
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
