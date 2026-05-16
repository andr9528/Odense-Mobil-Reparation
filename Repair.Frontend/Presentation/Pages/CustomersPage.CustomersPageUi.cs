using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    private sealed class CustomersPageUi : BaseUi<CustomersPageLogic, CustomersPageViewModel>
    {
        public CustomersPageUi(CustomersPageLogic logic, CustomersPageViewModel viewModel) : base(logic, viewModel)
        {
        }

        protected override void ConfigureGrid(Grid grid)
        {
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(0);
            grid.Padding = new Thickness(10);
            grid.DefineRows(GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star));
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateSearchBox().SetRow(1));
            grid.Children.Add(CreateCustomersList().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            // TODO: Replace with localized header / command bar.
            return new TextBlock {Text = "Customers", FontSize = 24,};
        }

        private TextBox CreateSearchBox()
        {
            ViewModel.SearchBox = new TextBox {PlaceholderText = "Search customers",};
            ViewModel.SearchBox.TextChanged += Logic.SearchTextChanged;
            return ViewModel.SearchBox;
        }

        private ListView CreateCustomersList()
        {
            ViewModel.CustomersList = new ListView {IsItemClickEnabled = true,};
            ViewModel.CustomersList.ItemClick += Logic.CustomerClicked;
            return ViewModel.CustomersList;
        }
    }
}
