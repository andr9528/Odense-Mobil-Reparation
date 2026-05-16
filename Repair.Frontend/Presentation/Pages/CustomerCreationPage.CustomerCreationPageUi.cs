using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerCreationPage
{
    private sealed class CustomerCreationPageUi : BaseUi<CustomerCreationPageLogic, CustomerCreationPageViewModel>
    {
        public CustomerCreationPageUi(CustomerCreationPageLogic logic, CustomerCreationPageViewModel viewModel) : base(logic, viewModel)
        {
        }

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
            grid.Children.Add(CreateCustomerDetailsGrid().SetRow(1));
            grid.Children.Add(CreateButtonsGrid().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            return new TextBlock {Text = "Create customer", FontSize = 24,};
        }

        private Grid CreateCustomerDetailsGrid()
        {
            // TODO: Add customer input fields.
            return GridFactory.CreateDefaultGrid();
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
