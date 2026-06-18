using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Core.Details;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    internal sealed class OrderDetailsPageUi(OrderDetailsPageLogic logic, OrderDetailsPageViewModel viewModel)
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
            grid.Children.Add(CreateDetailsButtonsGrid().SetRow(0));
            grid.Children.Add(CreateOrderEditor().SetRow(1));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Order Details");
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
