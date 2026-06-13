using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Converters;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrdersGrid
{
    internal sealed partial class OrdersGridUi(OrdersGridLogic logic, OrdersGridViewModel viewModel)
        : BaseUi<OrdersGridLogic, OrdersGridViewModel>(logic, viewModel)
    {
        /// <inheritdoc />
        protected override void ConfigureGrid(Grid grid)
        {
            const double searchColumnMinWidth = 290;
            const double searchRowMinHeight = 70;

            grid.RowSpacing = 8;
            grid.ColumnSpacing = 4;

            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star),
            });

            for (var i = 0; i < 3; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto,
                    MinHeight = searchRowMinHeight,
                });
            }

            for (var i = 0; i < 4; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star),
                    MinWidth = searchColumnMinWidth,
                });
            }
        }

        /// <inheritdoc />
        protected override void AddControlsToGrid(Grid grid)
        {
            DataGrid dataGrid = CreateOrderDataGrid().SetRow(0).SetColumn(0, 4);

            AddTextSearchFilterControls(grid);
            AddDateTimeFilterControls(grid);

            NullableBooleanOptionBar isOrderCompleteBar = CreateIsOrderCompleteOptionBar().SetRow(3).SetColumn(1);

            grid.Children.Add(dataGrid);
            grid.Children.Add(isOrderCompleteBar);
        }

        private void AddTextSearchFilterControls(Grid grid)
        {
            TextBox handInWhatSearchBox = CreateHandInWhatSearchBox().SetRow(1).SetColumn(0);
            TextBox repairWhatSearchBox = CreateRepairWhatSearchBox().SetRow(2).SetColumn(0);
            TextBox borrowedPhoneSearchBox = CreateBorrowedPhoneSearchBox().SetRow(2).SetColumn(1);
            TextBox customerNameSearchBox = CreateCustomerNameSearchBox().SetRow(1).SetColumn(1);
            Grid fuzzyToggle = CreateFuzzySearchGrid().SetRow(3).SetColumn(0);

            grid.Children.Add(handInWhatSearchBox);
            grid.Children.Add(repairWhatSearchBox);
            grid.Children.Add(borrowedPhoneSearchBox);
            grid.Children.Add(customerNameSearchBox);

            grid.Children.Add(fuzzyToggle);
        }

        private void AddDateTimeFilterControls(Grid grid)
        {
            Grid handInFromGrid = CreateDateTimeFilterGrid(nameof(OrdersGridViewModel.UseHandInFromFilter),
                    DateTime.Now, "From - Hand In", out CheckBox handInFromCheckBox,
                    out DateTimePicker handInFromPicker)
                .SetRow(2).SetColumn(2);

            ViewModel.UseHandInFromFilterCheckBox = handInFromCheckBox;
            ViewModel.HandInFromDateTimePicker = handInFromPicker;

            Grid handInToGrid = CreateDateTimeFilterGrid(nameof(OrdersGridViewModel.UseHandInToFilter), DateTime.Now,
                "To - Hand In", out CheckBox handInToCheckBox,
                out DateTimePicker handInToPicker).SetRow(3).SetColumn(2);

            ViewModel.UseHandInToFilterCheckBox = handInToCheckBox;
            ViewModel.HandInToDateTimePicker = handInToPicker;

            Grid returnedFromGrid = CreateDateTimeFilterGrid(nameof(OrdersGridViewModel.UseReturnedFromFilter),
                DateTime.Now, "From - Returned", out CheckBox returnedFromCheckBox,
                out DateTimePicker returnedFromPicker).SetRow(2).SetColumn(3);

            ViewModel.UseReturnedFromFilterCheckBox = returnedFromCheckBox;
            ViewModel.ReturnedFromDateTimePicker = returnedFromPicker;

            Grid returnedToGrid = CreateDateTimeFilterGrid(nameof(OrdersGridViewModel.UseReturnedToFilter),
                    DateTime.Now, "To - Returned", out CheckBox returnedToCheckBox, out DateTimePicker returnedToPicker)
                .SetRow(3).SetColumn(3);

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
                Logic.GetArgumentsFactory().CreateDateTimePickerArguments(header, initialValue);

            dateTimePicker = new DateTimePicker(arguments).SetColumn(1);
            dateTimePicker.ViewModel.SelectedDateTimeChanged += Logic.SearchChanged;

            grid.Children.Add(checkBox);
            grid.Children.Add(dateTimePicker);

            return grid;
        }

        private TextBox CreateCustomerNameSearchBox()
        {
            ViewModel.CustomerNameSearchBox = TextBoxFactory.CreateSearchBox("Customer name", "Search customer...",
                nameof(OrdersGridViewModel.CustomerNameSearchText));

            return ViewModel.CustomerNameSearchBox;
        }

        private NullableBooleanOptionBar CreateIsOrderCompleteOptionBar()
        {
            NullableBooleanOptionBar.NullableBooleanOptionBarArguments arguments =
                Logic.GetArgumentsFactory().CreateNullableBooleanOptionBarArguments("Order complete");
            ViewModel.IsOrderCompleteOptionBar = new NullableBooleanOptionBar(arguments);

            ViewModel.IsOrderCompleteOptionBar.ViewModel.SelectionChanged += Logic.IsOrderCompleteSelectionChanged;

            return ViewModel.IsOrderCompleteOptionBar;
        }

        private TextBox CreateBorrowedPhoneSearchBox()
        {
            ViewModel.BorrowedPhoneSearchBox = TextBoxFactory.CreateSearchBox("Borrowed phone",
                "Search borrowed phone...", nameof(OrdersGridViewModel.BorrowedPhoneSearchText));

            return ViewModel.BorrowedPhoneSearchBox;
        }

        private TextBox CreateHandInWhatSearchBox()
        {
            ViewModel.HandInWhatSearchBox = TextBoxFactory.CreateSearchBox("Hand in What", "Search handed in...",
                nameof(OrdersGridViewModel.HandInWhatSearchText));

            return ViewModel.HandInWhatSearchBox;
        }

        private TextBox CreateRepairWhatSearchBox()
        {
            ViewModel.RepairWhatSearchBox = TextBoxFactory.CreateSearchBox("Repair", "Search repair...",
                nameof(OrdersGridViewModel.RepairWhatSearchText));

            return ViewModel.RepairWhatSearchBox;
        }

        private Grid CreateFuzzySearchGrid()
        {
            Grid grid = SimplePieceFactory.CreateFuzzySearchGrid(nameof(OrdersGridViewModel.UseFuzzySearch),
                nameof(OrdersGridViewModel.SearchModeText), out CheckBox fuzzySearchCheckBox);

            ViewModel.FuzzySearchToggle = fuzzySearchCheckBox;

            return grid;
        }

        private DataGrid CreateOrderDataGrid()
        {
            ViewModel.DataGrid =
                DataGridFactory.Create<OrderGridColumns>(ViewModel.Orders, GetColumnBindingPath, GetColumnConverter);
            ViewModel.DataGrid.Margin = new Thickness(4);

            return ViewModel.DataGrid;
        }

        private string GetColumnBindingPath(OrderGridColumns column)
        {
            return column switch
            {
                OrderGridColumns.HAND_IN_WHAT => nameof(Order.HandInWhat),
                OrderGridColumns.REPAIR_WHAT => nameof(Order.RepairWhat),
                OrderGridColumns.HANDED_IN_WHEN => nameof(Order.HandInWhen),
                OrderGridColumns.RETURNED_WHEN => nameof(Order.ReturnedWhen),
                OrderGridColumns.IS_ORDER_COMPLETE => nameof(Order.IsOrderComplete),
                OrderGridColumns.BORROWED_PHONE => nameof(Order.BorrowedPhone),
                OrderGridColumns.CUSTOMER_NAME => $"{nameof(Order.Customer)}.{nameof(Customer.Name)}",
                var _ => throw new ArgumentOutOfRangeException(nameof(column), column, null),
            };
        }

        private IValueConverter? GetColumnConverter(OrderGridColumns column)
        {
            return column switch
            {
                OrderGridColumns.RETURNED_WHEN => new NullableDateTimeConverter($"No Date/Time Set"),
                OrderGridColumns.IS_ORDER_COMPLETE => new BooleanConverter(),
                OrderGridColumns.BORROWED_PHONE => new NullableStringConverter("No Borrowed Phone"),
                var _ => null,
            };
        }

        private enum OrderGridColumns
        {
            HAND_IN_WHAT = 0,
            REPAIR_WHAT = 1,
            HANDED_IN_WHEN = 2,
            RETURNED_WHEN = 3,
            IS_ORDER_COMPLETE = 4,
            BORROWED_PHONE = 5,
            CUSTOMER_NAME = 6,
        }
    }
}
