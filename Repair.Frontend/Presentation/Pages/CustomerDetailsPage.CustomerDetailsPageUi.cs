using Microsoft.Extensions.DependencyInjection;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Core.Details;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed class CustomerDetailsPageUi(CustomerDetailsPageLogic logic, CustomerDetailsPageViewModel viewModel)
        : BaseDetailsPageUi<CustomerDetailsPageLogic, CustomerDetailsPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            ConfigureDefaultPageGrid(grid);

            grid.DefineRows(GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star));
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateDetailsButtonsGrid(CreateCreateOrderButton()).SetRow(0));
            grid.Children.Add(CreateCustomerEditor().SetRow(1));
            grid.Children.Add(CreateOrdersGrid().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Customer Details");
        }

        private Button CreateCreateOrderButton()
        {
            Button button = new()
            {
                Content = "Create Order",
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(20, 8, 20, 8),
            };

            button.Click += Logic.CreateOrderClicked;

            return button;
        }

        private CustomerEditor CreateCustomerEditor()
        {
            ArgumentsFactory argumentsFactory = Logic.GetArgumentsFactory();

            ViewModel.CustomerEditor = new CustomerEditor(argumentsFactory.CreateCustomerEditorArguments());

            Logic.RegisterCustomerEditorEvents();

            return ViewModel.CustomerEditor;
        }

        private OrdersGrid CreateOrdersGrid()
        {
            ArgumentsFactory argumentsFactory = Logic.GetArgumentsFactory();

            ViewModel.OrdersGrid = new OrdersGrid(
                argumentsFactory.CreateOrderGridArguments(ViewModel.Arguments.CustomerId));

            ViewModel.OrdersGrid.ViewModel.DataGrid.SelectionChanged += Logic.OrderClicked;

            return ViewModel.OrdersGrid;
        }
    }
}
