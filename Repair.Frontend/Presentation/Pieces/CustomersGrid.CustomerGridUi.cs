using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Text;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;
using static Repair.Frontend.Presentation.Pieces.OrdersGrid.OrdersGridUi;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomersGrid
{
    internal sealed partial class CustomersGridUi(CustomersGridLogic logic, CustomersGridViewModel viewModel)
        : BaseUi<CustomersGridLogic, CustomersGridViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            grid.RowSpacing = 8;

            grid.DefineRows(new GridLength(1, GridUnitType.Star));
            grid.DefineRows(GridLength.Auto, GridLength.Auto, GridLength.Auto);

            grid.DefineColumns(GridUnitType.Star, [1, 1, 1,]);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateCustomerDataGrid().SetRow(0).SetColumn(0, 3));
            grid.Children.Add(SimplePieceFactory.CreateFilterHeader().SetRow(1).SetColumn(0, 3));
            grid.Children.Add(CreateCustomerSearchEditor().SetRow(2).SetColumn(0, 3));
            grid.Children.Add(CreateFuzzySearchGrid().SetRow(3).SetColumn(1));
        }

        private DataGrid CreateCustomerDataGrid()
        {
            ViewModel.DataGrid =
                DataGridFactory.Create<CustomerGridColumns>(ViewModel.Customers, GetColumnBindingPath,
                    GetColumnConverter);

            ViewModel.DataGrid.SetBinding(DataGrid.SelectedItemProperty, new Binding
            {
                Path = new PropertyPath(nameof(CustomersGridViewModel.SelectedCustomer)),
                Mode = BindingMode.TwoWay,
            });

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
            Grid grid = SimplePieceFactory.CreateFuzzySearchGrid(nameof(CustomersGridViewModel.UseFuzzySearch),
                nameof(CustomersGridViewModel.SearchModeText), out CheckBox fuzzySearchCheckBox);

            ViewModel.FuzzySearchToggle = fuzzySearchCheckBox;

            return grid;
        }

        private IValueConverter? GetColumnConverter(CustomerGridColumns column)
        {
            return column switch
            {
                var _ => null,
            };
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

        private enum CustomerGridColumns
        {
            NAME = 0,
            PHONE = 1,
            EMAIL = 2,
        }
    }
}
