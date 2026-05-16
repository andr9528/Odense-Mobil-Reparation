using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderCreationPage
{
    private sealed class OrderCreationPageUi : BaseUi<OrderCreationPageLogic, OrderCreationPageViewModel>
    {
        public OrderCreationPageUi(OrderCreationPageLogic logic, OrderCreationPageViewModel viewModel) : base(logic, viewModel)
        {
        }

        protected override void ConfigureGrid(Grid grid)
        {
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(0);
            grid.Padding = new Thickness(10);
            grid.DefineRows(GridLength.Auto, GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star), GridLength.Auto);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateOrderDetailsGrid().SetRow(1));
            grid.Children.Add(CreateCustomerSearchBox().SetRow(2));
            grid.Children.Add(CreateCustomersList().SetRow(3));
            grid.Children.Add(CreateButtonsGrid().SetRow(4));
        }

        private UIElement CreateHeader()
        {
            return new TextBlock {Text = "Create order", FontSize = 24,};
        }

        private Grid CreateOrderDetailsGrid()
        {
            // TODO: Add order input fields.
            return GridFactory.CreateDefaultGrid();
        }

        private TextBox CreateCustomerSearchBox()
        {
            ViewModel.CustomerSearchBox = new TextBox {PlaceholderText = "Search customers",};
            ViewModel.CustomerSearchBox.TextChanged += Logic.CustomerSearchTextChanged;
            return ViewModel.CustomerSearchBox;
        }

        private ListView CreateCustomersList()
        {
            ViewModel.CustomersList = new ListView {IsItemClickEnabled = true,};
            ViewModel.CustomersList.ItemClick += Logic.CustomerClicked;
            return ViewModel.CustomersList;
        }

        private Grid CreateButtonsGrid()
        {
            Grid grid = GridFactory.CreateDefaultGrid().DefineColumns(GridLength.Auto, GridLength.Auto);

            var saveButton = new Button {Content = "Save",};
            saveButton.Click += Logic.SaveClicked;

            var cancelButton = new Button {Content = "Cancel",};
            cancelButton.Click += Logic.CancelClicked;

            grid.Children.Add(saveButton.SetColumn(0));
            grid.Children.Add(cancelButton.SetColumn(1));

            return grid;
        }
    }
}
