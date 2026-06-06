using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Core.Details;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed class OrderDetailsPageUi(OrderDetailsPageLogic logic, OrderDetailsPageViewModel viewModel)
        : BaseDetailsPageUi<OrderDetailsPageLogic, OrderDetailsPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            ConfigureDefaultPageGrid(grid);

            grid.DefineRows(GridLength.Auto, new GridLength(1, GridUnitType.Star));
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateDetailsButtonsGrid(CreatePrintButton()).SetRow(0));
            grid.Children.Add(CreateOrderEditor().SetRow(1));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Order Details");
        }

        private Button CreatePrintButton()
        {
            ViewModel.PrintButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 120,
                Padding = new Thickness(20, 8, 20, 8),
            };

            ViewModel.PrintButton.SetBinding(ContentControl.ContentProperty, new Binding
            {
                Path = new PropertyPath(nameof(OrderDetailsPageViewModel.PrintButtonText)),
                Mode = BindingMode.OneWay,
            });

            ViewModel.PrintButton.Click += Logic.PrintClicked;

            return ViewModel.PrintButton;
        }

        private OrderEditor CreateOrderEditor()
        {
            ArgumentsFactory argumentsFactory = Logic.GetArgumentsFactory();

            ViewModel.OrderEditor = new OrderEditor(argumentsFactory.CreateOrderEditorArguments());

            Logic.RegisterOrderEditorEvents();

            return ViewModel.OrderEditor;
        }
    }
}
