using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;

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
            grid.RowSpacing = 8;
            grid.DefineRows(GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star), GridLength.Auto,
                GridLength.Auto);

            grid.DefineColumns(GridUnitType.Star, [1, 1, 1,]);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0).SetColumn(0, 3));
            grid.Children.Add(CreateCreateCustomerButton().SetRow(1));
            grid.Children.Add(CreateCustomersDataGrid().SetRow(2).SetColumn(0, 3));
            grid.Children.Add(CreateCustomerEditor().SetRow(3).SetColumn(0, 3));
            grid.Children.Add(CreateFuzzySearchGrid().SetRow(4).SetColumn(1));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Customers");
        }

        private Button CreateCreateCustomerButton()
        {
            var button = new Button
            {
                Content = "Create customer",
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            button.Click += Logic.CreateCustomerClicked;

            return button;
        }

        private CustomerEditor CreateCustomerEditor()
        {
            var customerEditor = new CustomerEditor(true);

            ViewModel.ConnectCustomerEditor(customerEditor);
            ViewModel.CustomerEditor.ViewModel.IsReadOnly = false;

            return ViewModel.CustomerEditor;
        }


        private Grid CreateFuzzySearchGrid()
        {
            Grid grid = SearchModeFactory.CreateFuzzySearchGrid(nameof(CustomersPageViewModel.UseFuzzySearch),
                nameof(CustomersPageViewModel.SearchModeText), out CheckBox fuzzySearchCheckBox);

            ViewModel.FuzzySearchToggle = fuzzySearchCheckBox;

            return grid;
        }

        private DataGrid CreateCustomersDataGrid()
        {
            ViewModel.DataGrid = DataGridFactory.Create<CustomerGridColumns>(ViewModel.Customers, GetColumnBindingPath);

            ViewModel.DataGrid.Margin = new Thickness(4);

            ViewModel.DataGrid.SelectionChanged += Logic.CustomerClicked;

            return ViewModel.DataGrid;
        }

        private string GetColumnBindingPath(CustomerGridColumns column)
        {
            return column switch
            {
                CustomerGridColumns.NAME => nameof(Customer.Name),
                CustomerGridColumns.PHONE => nameof(Customer.Phone),
                CustomerGridColumns.EMAIL => nameof(Customer.Email),
                var _ => throw new ArgumentOutOfRangeException(nameof(column), column, null),
            };
        }

        internal enum CustomerGridColumns
        {
            NAME = 0,
            PHONE = 1,
            EMAIL = 2,
        }
    }
}
