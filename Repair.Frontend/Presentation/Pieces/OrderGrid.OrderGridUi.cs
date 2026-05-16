using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Factory;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid
{
    internal sealed partial class OrderGridUi
    {
        internal DataGrid DataGrid { get; private set; } = null!;

        private readonly OrderGridLogic logic;
        private readonly OrderGridViewModel viewModel;

        public OrderGridUi(OrderGridLogic logic, OrderGridViewModel viewModel)
        {
            this.logic = logic;
            this.viewModel = viewModel;
        }

        public Grid CreateContentGrid()
        {
            var grid = new Grid
            {
                RowSpacing = 8,
            };

            grid.DefineRows(GridUnitType.Auto, [1, 1, 1,]);
            grid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star),});

            TextBox handInWhatSearchBox = CreateHandInWhatSearchBox();
            TextBox repairWhatSearchBox = CreateRepairWhatSearchBox();
            ToggleSwitch fuzzyToggle = CreateFuzzyToggle();
            var dataGrid = CreateOrderDataGrid();

            Grid.SetRow(handInWhatSearchBox, 0);
            Grid.SetRow(repairWhatSearchBox, 1);
            Grid.SetRow(fuzzyToggle, 2);
            Grid.SetRow(dataGrid, 3);

            grid.Children.Add(handInWhatSearchBox);
            grid.Children.Add(repairWhatSearchBox);
            grid.Children.Add(fuzzyToggle);
            grid.Children.Add(dataGrid);

            return grid;
        }

        private TextBox CreateHandInWhatSearchBox()
        {
            return CreateSearchBox("Search handed in...", nameof(OrderGridViewModel.HandInWhatSearchText));
        }

        private TextBox CreateRepairWhatSearchBox()
        {
            return CreateSearchBox("Search repair...", nameof(OrderGridViewModel.RepairWhatSearchText));
        }

        private TextBox CreateSearchBox(string placeholderText, string bindingPath)
        {
            var textBox = new TextBox
            {
                PlaceholderText = placeholderText,
            };

            textBox.SetBinding(TextBox.TextProperty, new Binding
            {
                Path = new PropertyPath(bindingPath),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

            return textBox;
        }

        private ToggleSwitch CreateFuzzyToggle()
        {
            var toggleSwitch = new ToggleSwitch
            {
                Header = "Fuzzy search",
            };

            toggleSwitch.SetBinding(ToggleSwitch.IsOnProperty, new Binding
            {
                Path = new PropertyPath(nameof(OrderGridViewModel.UseFuzzySearch)),
                Mode = BindingMode.TwoWay,
            });

            return toggleSwitch;
        }

        private DataGrid CreateOrderDataGrid()
        {
            DataGrid = DataGridFactory.Create<OrderGridColumns>(viewModel.Orders, GetColumnBindingPath);

            return DataGrid;
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
                var _ => throw new ArgumentOutOfRangeException(nameof(column), column, null),
            };
        }

        internal enum OrderGridColumns
        {
            HAND_IN_WHAT = 0,
            REPAIR_WHAT = 1,
            HAND_IN_WHEN = 2,
            RETURNED_WHEN = 3,
            IS_ORDER_COMPLETE = 4,
            HAS_BORROWED_PHONE = 5,
        }
    }
}
