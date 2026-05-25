using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed class OrderDetailsPageUi(OrderDetailsPageLogic logic, OrderDetailsPageViewModel viewModel)
        : BaseUi<OrderDetailsPageLogic, OrderDetailsPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(0);
            grid.Padding = new Thickness(10);
            grid.DefineRows(GridLength.Auto, GridLength.Auto, GridLength.Auto);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateOrderDetailsGrid().SetRow(1));
            grid.Children.Add(CreateCustomerInformationGrid().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            Grid header = GridFactory.CreateDefaultGrid().DefineColumns(new GridLength(1, GridUnitType.Star), GridLength.Auto);
            header.Children.Add(new TextBlock {Text = "Order details", FontSize = 24,}.SetColumn(0));
            header.Children.Add(CreateEditToggle().SetColumn(1));
            return header;
        }

        private ToggleSwitch CreateEditToggle()
        {
            ViewModel.EditToggle = new ToggleSwitch {Header = "Edit", IsOn = false,};
            ViewModel.EditToggle.Toggled += Logic.EditToggleChanged;
            return ViewModel.EditToggle;
        }

        private Grid CreateOrderDetailsGrid()
        {
            // TODO: Add order fields.
            return GridFactory.CreateDefaultGrid();
        }

        private Grid CreateCustomerInformationGrid()
        {
            // TODO: Add customer information for the order.
            return GridFactory.CreateDefaultGrid();
        }
    }
}