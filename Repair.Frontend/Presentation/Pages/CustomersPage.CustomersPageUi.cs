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

            grid.DefineRows(GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star));

            grid.DefineColumns(GridUnitType.Star, [1,]);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateCreateCustomerButton().SetRow(1));
            grid.Children.Add(CreateCustomerGrid().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Customers");
        }

        private Button CreateCreateCustomerButton()
        {
            var button = new Button
            {
                Content = "Create customer",
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            button.Click += Logic.CreateCustomerClicked;

            return button;
        }

        private CustomerGrid CreateCustomerGrid()
        {
            CustomerGrid.CustomerGridArguments arguments = Logic.GetArgumentsFactory().CreateCustomerGridArguments();

            var customerGrid = new CustomerGrid(arguments);

            ViewModel.CustomersGrid = customerGrid;

            customerGrid.ViewModel.DataGrid.SelectionChanged += Logic.CustomerClicked;

            return customerGrid;
        }
    }
}
