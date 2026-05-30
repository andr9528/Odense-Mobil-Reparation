using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomerGrid
{
    internal sealed partial class CustomerGridUi(CustomerGridLogic logic, CustomerGridViewModel viewModel)
        : BaseUi<CustomerGridLogic, CustomerGridViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            grid.RowSpacing = 8;

            grid.DefineRows(new GridLength(1, GridUnitType.Star));
            grid.DefineRows(GridLength.Auto, GridLength.Auto);

            grid.DefineColumns(GridUnitType.Star, [1, 1, 1,]);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateCustomerDataGrid().SetRow(0).SetColumn(0, 3));
            grid.Children.Add(CreateCustomerSearchEditor().SetRow(1).SetColumn(0, 3));
            grid.Children.Add(CreateFuzzySearchGrid().SetRow(2).SetColumn(1));
        }

        private DataGrid CreateCustomerDataGrid()
        {
            ViewModel.DataGrid = DataGridFactory.Create<CustomerGridColumns>(ViewModel.Customers, GetColumnBindingPath);

            ViewModel.DataGrid.Margin = new Thickness(4);

            return ViewModel.DataGrid;
        }

        private CustomerEditor CreateCustomerSearchEditor()
        {
            CustomerEditor.CustomerEditorArguments arguments =
                Logic.GetArgumentsFactory().CreateCustomerEditorArguments(true);

            var customerEditor = new CustomerEditor(arguments)
            {
                ViewModel =
                {
                    IsReadOnly = false,
                },
            };

            ViewModel.ConnectCustomerEditor(customerEditor);

            return ViewModel.CustomerEditor;
        }

        private Grid CreateFuzzySearchGrid()
        {
            Grid grid = SearchModeFactory.CreateFuzzySearchGrid(nameof(CustomerGridViewModel.UseFuzzySearch),
                nameof(CustomerGridViewModel.SearchModeText), out CheckBox fuzzySearchCheckBox);

            ViewModel.FuzzySearchToggle = fuzzySearchCheckBox;

            return grid;
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
