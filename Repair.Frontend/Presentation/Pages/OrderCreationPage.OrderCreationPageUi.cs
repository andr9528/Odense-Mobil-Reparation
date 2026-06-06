using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderCreationPage
{
    private sealed class OrderCreationPageUi(OrderCreationPageLogic logic, OrderCreationPageViewModel viewModel)
        : BaseUi<OrderCreationPageLogic, OrderCreationPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            ConfigureDefaultPageGrid(grid);

            grid.DefineRows(GridLength.Auto, new GridLength(1, GridUnitType.Star));
            grid.DefineColumns(new GridLength(1, GridUnitType.Star));
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateButtonsGrid().SetRow(0));
            grid.Children.Add(CreateOrderEditor().SetRow(1));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Create order");
        }

        private OrderEditor CreateOrderEditor()
        {
            OrderEditor.OrderEditorArguments arguments = Logic.GetArgumentsFactory()
                .CreateOrderEditorArguments(selectedCustomerId: ViewModel.Arguments.SelectedCustomerId);

            ViewModel.OrderEditor = new OrderEditor(arguments)
            {
                ViewModel =
                {
                    IsReadOnly = false,
                },
            };

            return ViewModel.OrderEditor;
        }

        private Grid CreateButtonsGrid()
        {
            return SimplePieceFactory.CreateSaveCancelButtonGrid(Logic.SaveClicked, Logic.CancelClicked);
        }
    }
}
