using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrdersPage
{
    private sealed class OrdersPageUi : BaseUi<OrdersPageLogic, OrdersPageViewModel>
    {
        public OrdersPageUi(OrdersPageLogic logic, OrdersPageViewModel viewModel) : base(logic, viewModel)
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
            grid.Children.Add(CreateOrdersList().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            return new TextBlock {Text = "Orders", FontSize = 24,};
        }

        private TextBox CreateSearchBox()
        {
            ViewModel.SearchBox = new TextBox {PlaceholderText = "Search orders",};
            ViewModel.SearchBox.TextChanged += Logic.SearchTextChanged;
            return ViewModel.SearchBox;
        }

        private ListView CreateOrdersList()
        {
            ViewModel.OrdersList = new ListView {IsItemClickEnabled = true,};
            ViewModel.OrdersList.ItemClick += Logic.OrderClicked;
            return ViewModel.OrdersList;
        }
    }
}