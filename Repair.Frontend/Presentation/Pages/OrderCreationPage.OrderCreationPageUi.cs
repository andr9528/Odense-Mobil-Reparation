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
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(0);
            grid.Padding = new Thickness(10);

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
            Grid grid = GridFactory.CreateDefaultGrid();

            grid.HorizontalAlignment = HorizontalAlignment.Right;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.ColumnSpacing = 8;
            grid.DefineColumns(new GridLength(1, GridUnitType.Star), GridLength.Auto, GridLength.Auto);

            var saveButton = new Button {Content = "Save",};
            saveButton.Click += Logic.SaveClicked;

            var cancelButton = new Button {Content = "Cancel",};
            cancelButton.Click += Logic.CancelClicked;

            grid.Children.Add(saveButton.SetColumn(1));
            grid.Children.Add(cancelButton.SetColumn(2));

            return grid;
        }
    }
}
