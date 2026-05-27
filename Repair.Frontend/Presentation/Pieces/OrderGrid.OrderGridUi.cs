using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid
{
    internal sealed partial class OrderGridUi(OrderGridLogic logic, OrderGridViewModel viewModel)
        : BaseUi<OrderGridLogic, OrderGridViewModel>(logic, viewModel)
    {
        /// <inheritdoc />
        protected override void ConfigureGrid(Grid grid)
        {
            const int totalHeight = 200;
            const int searchRowsHeight = 18;
            const int dataRowHeight = totalHeight - 3 * searchRowsHeight;

            grid.RowSpacing = 8;
            grid.DefineRows(new GridLength(dataRowHeight, GridUnitType.Star));
            grid.DefineRows(GridUnitType.Star, [searchRowsHeight, searchRowsHeight, searchRowsHeight,]);
            grid.DefineColumns(GridUnitType.Star, [1, 1, 1, 1,]);
        }

        /// <inheritdoc />
        protected override void AddControlsToGrid(Grid grid)
        {
            DataGrid dataGrid = CreateOrderDataGrid().SetRow(0).SetColumn(0, 4);

            AddTextSearchFilterControls(grid);
            AddDateTimeFilterControls(grid);

            NullableBooleanOptionBar isOrderCompleteBar = CreateIsOrderCompleteOptionBar().SetRow(1).SetColumn(1);
            NullableBooleanOptionBar hasBorrowedPhoneBar = CreateHasBorrowedPhoneOptionBar().SetRow(2).SetColumn(1);

            grid.Children.Add(dataGrid);
            grid.Children.Add(isOrderCompleteBar);
            grid.Children.Add(hasBorrowedPhoneBar);
        }

        private void AddTextSearchFilterControls(Grid grid)
        {
            TextBox handInWhatSearchBox = CreateHandInWhatSearchBox().SetRow(1).SetColumn(0);
            TextBox repairWhatSearchBox = CreateRepairWhatSearchBox().SetRow(2).SetColumn(0);
            TextBox customerNameSearchBox = CreateCustomerNameSearchBox().SetRow(3).SetColumn(0);

            Grid fuzzyToggle = CreateFuzzySearchGrid().SetRow(3).SetColumn(1);

            grid.Children.Add(handInWhatSearchBox);
            grid.Children.Add(repairWhatSearchBox);
            grid.Children.Add(customerNameSearchBox);

            grid.Children.Add(fuzzyToggle);
        }

        private void AddDateTimeFilterControls(Grid grid)
        {
            Grid handInFromGrid = CreateDateTimeFilterGrid(nameof(OrderGridViewModel.UseHandInFromFilter), DateTime.Now,
                    "From - Hand In", out CheckBox handInFromCheckBox, out DateTimePicker handInFromPicker).SetRow(2)
                .SetColumn(2);

            ViewModel.UseHandInFromFilterCheckBox = handInFromCheckBox;
            ViewModel.HandInFromDateTimePicker = handInFromPicker;

            Grid handInToGrid = CreateDateTimeFilterGrid(nameof(OrderGridViewModel.UseHandInToFilter), DateTime.Now,
                "To - Hand In", out CheckBox handInToCheckBox,
                out DateTimePicker handInToPicker).SetRow(3).SetColumn(2);

            ViewModel.UseHandInToFilterCheckBox = handInToCheckBox;
            ViewModel.HandInToDateTimePicker = handInToPicker;

            Grid returnedFromGrid = CreateDateTimeFilterGrid(nameof(OrderGridViewModel.UseReturnedFromFilter),
                DateTime.Now, "From - Returned", out CheckBox returnedFromCheckBox,
                out DateTimePicker returnedFromPicker).SetRow(2).SetColumn(3);

            ViewModel.UseReturnedFromFilterCheckBox = returnedFromCheckBox;
            ViewModel.ReturnedFromDateTimePicker = returnedFromPicker;

            Grid returnedToGrid = CreateDateTimeFilterGrid(nameof(OrderGridViewModel.UseReturnedToFilter), DateTime.Now,
                    "To - Returned", out CheckBox returnedToCheckBox, out DateTimePicker returnedToPicker).SetRow(3)
                .SetColumn(3);

            ViewModel.UseReturnedToFilterCheckBox = returnedToCheckBox;
            ViewModel.ReturnedToDateTimePicker = returnedToPicker;

            grid.Children.Add(handInFromGrid);
            grid.Children.Add(handInToGrid);
            grid.Children.Add(returnedFromGrid);
            grid.Children.Add(returnedToGrid);
        }

        private Grid CreateDateTimeFilterGrid(
            string useFilterBindingPath, DateTime initialValue, string header, out CheckBox checkBox,
            out DateTimePicker dateTimePicker)
        {
            Grid grid = GridFactory.CreateDefaultGrid();

            grid.ColumnSpacing = 4;
            grid.DefineColumns(GridUnitType.Auto, [1,]);
            grid.DefineColumns(GridUnitType.Star, [1,]);

            checkBox = CheckBoxFactory.CreateLightCheckBox(useFilterBindingPath).SetColumn(0);

            DateTimePicker.DateTimePickerArguments arguments =
                GetArgumentsFactory().CreateDateTimePickerArguments(initialValue, header);

            dateTimePicker = new DateTimePicker(arguments).SetColumn(1);
            dateTimePicker.ViewModel.SelectedDateTimeChanged += Logic.SearchChanged;

            grid.Children.Add(checkBox);
            grid.Children.Add(dateTimePicker);

            return grid;
        }

        private TextBox CreateCustomerNameSearchBox()
        {
            ViewModel.CustomerNameSearchBox = TextBoxFactory.CreateSearchBox("Customer name", "Search customer...",
                nameof(OrderGridViewModel.CustomerNameSearchText));

            return ViewModel.CustomerNameSearchBox;
        }

        private NullableBooleanOptionBar CreateIsOrderCompleteOptionBar()
        {
            NullableBooleanOptionBar.NullableBooleanOptionBarArguments arguments =
                GetArgumentsFactory().CreateNullableBooleanOptionBarArguments("Order complete");
            ViewModel.IsOrderCompleteOptionBar = new NullableBooleanOptionBar(arguments);

            ViewModel.IsOrderCompleteOptionBar.ViewModel.SelectionChanged += Logic.IsOrderCompleteSelectionChanged;

            return ViewModel.IsOrderCompleteOptionBar;
        }

        private NullableBooleanOptionBar CreateHasBorrowedPhoneOptionBar()
        {
            NullableBooleanOptionBar.NullableBooleanOptionBarArguments arguments =
                GetArgumentsFactory().CreateNullableBooleanOptionBarArguments("Borrowed phone");
            ViewModel.HasBorrowedPhoneOptionBar = new NullableBooleanOptionBar(arguments);

            ViewModel.HasBorrowedPhoneOptionBar.ViewModel.SelectionChanged += Logic.HasBorrowedPhoneSelectionChanged;

            return ViewModel.HasBorrowedPhoneOptionBar;
        }

        private TextBox CreateHandInWhatSearchBox()
        {
            ViewModel.HandInWhatSearchBox = TextBoxFactory.CreateSearchBox("Hand in What", "Search handed in...",
                nameof(OrderGridViewModel.HandInWhatSearchText));

            return ViewModel.HandInWhatSearchBox;
        }

        private TextBox CreateRepairWhatSearchBox()
        {
            ViewModel.RepairWhatSearchBox = TextBoxFactory.CreateSearchBox("Repair", "Search repair...",
                nameof(OrderGridViewModel.RepairWhatSearchText));

            return ViewModel.RepairWhatSearchBox;
        }

        private Grid CreateFuzzySearchGrid()
        {
            Grid grid = SearchModeFactory.CreateFuzzySearchGrid(nameof(OrderGridViewModel.UseFuzzySearch),
                nameof(OrderGridViewModel.SearchModeText), out CheckBox fuzzySearchCheckBox);

            ViewModel.FuzzySearchToggle = fuzzySearchCheckBox;

            return grid;
        }

        private DataGrid CreateOrderDataGrid()
        {
            ViewModel.DataGrid = DataGridFactory.Create<OrderGridColumns>(ViewModel.Orders, GetColumnBindingPath);
            ViewModel.DataGrid.Margin = new Thickness(4);

            return ViewModel.DataGrid;
        }

        private string GetColumnBindingPath(OrderGridColumns column)
        {
            return column switch
            {
                OrderGridColumns.HAND_IN_WHAT => nameof(Order.HandInWhat),
                OrderGridColumns.REPAIR_WHAT => nameof(Order.RepairWhat),
                OrderGridColumns.HAND_IN_WHEN => nameof(Order.HandInWhen),
                OrderGridColumns.RETURNED_WHEN => nameof(Order.ReturnedWhen),
                OrderGridColumns.IS_ORDER_COMPLETE => nameof(Order.IsOrderComplete),
                OrderGridColumns.HAS_BORROWED_PHONE => nameof(Order.HasBorrowedPhone),
                OrderGridColumns.CUSTOMER_NAME => $"{nameof(Order.Customer)}.{nameof(Customer.Name)}",
                var _ => throw new ArgumentOutOfRangeException(nameof(column), column, null),
            };
        }

        private ArgumentsFactory GetArgumentsFactory()
        {
            return App.Startup.ServiceProvider.GetRequiredService<ArgumentsFactory>();
        }

        internal enum OrderGridColumns
        {
            HAND_IN_WHAT = 0,
            REPAIR_WHAT = 1,
            HAND_IN_WHEN = 2,
            RETURNED_WHEN = 3,
            IS_ORDER_COMPLETE = 4,
            HAS_BORROWED_PHONE = 5,
            CUSTOMER_NAME = 6,
        }
    }
}
