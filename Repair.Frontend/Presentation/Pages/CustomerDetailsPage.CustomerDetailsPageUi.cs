using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed class CustomerDetailsPageUi(CustomerDetailsPageLogic logic, CustomerDetailsPageViewModel viewModel)
        : BaseUi<CustomerDetailsPageLogic, CustomerDetailsPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(0);
            grid.Padding = new Thickness(10);
            grid.DefineRows(GridLength.Auto, GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star));
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateCustomerDetailsGrid().SetRow(1));
            grid.Children.Add(CreateOrderSearchBox().SetRow(2));
            grid.Children.Add(CreateOrdersList().SetRow(3));
        }

        private UIElement CreateHeader()
        {
            Grid header = GridFactory.CreateDefaultGrid().DefineColumns(new GridLength(1, GridUnitType.Star), GridLength.Auto);
            header.Children.Add(new TextBlock {Text = "Customer details", FontSize = 24,}.SetColumn(0));
            header.Children.Add(CreateEditToggle().SetColumn(1));
            return header;
        }

        private ToggleSwitch CreateEditToggle()
        {
            ViewModel.EditToggle = new ToggleSwitch {Header = "Edit", IsOn = false,};
            ViewModel.EditToggle.Toggled += Logic.EditToggleChanged;
            return ViewModel.EditToggle;
        }

        private Grid CreateCustomerDetailsGrid()
        {
            // TODO: Add customer fields.
            return GridFactory.CreateDefaultGrid();
        }

        private TextBox CreateOrderSearchBox()
        {
            ViewModel.OrderSearchBox = new TextBox {PlaceholderText = "Search customer orders",};
            ViewModel.OrderSearchBox.TextChanged += Logic.OrderSearchTextChanged;
            return ViewModel.OrderSearchBox;
        }

        private ListView CreateOrdersList()
        {
            ViewModel.OrdersList = new ListView {IsItemClickEnabled = true,};
            ViewModel.OrdersList.ItemClick += Logic.OrderClicked;
            return ViewModel.OrdersList;
        }
    }
}
