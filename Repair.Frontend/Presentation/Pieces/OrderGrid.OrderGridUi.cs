using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid
{
    internal sealed partial class OrderGridUi : BaseUi<OrderGridLogic, OrderGridViewModel>
    {
        public OrderGridUi(OrderGridLogic logic, OrderGridViewModel viewModel) : base(logic, viewModel)
        {
        }

        /// <inheritdoc />
        protected override void ConfigureGrid(Grid grid)
        {
            grid.RowSpacing = 8;
            grid.DefineRows(new GridLength(1, GridUnitType.Star), GridLength.Auto, GridLength.Auto, GridLength.Auto);
            grid.DefineColumns(GridUnitType.Star, [1, 1, 1, 1,]);
        }

        /// <inheritdoc />
        protected override void AddControlsToGrid(Grid grid)
        {
            DataGrid dataGrid = CreateOrderDataGrid().SetRow(0).SetColumn(0, 4);
            TextBox handInWhatSearchBox = CreateHandInWhatSearchBox().SetRow(1).SetColumn(0);
            TextBox repairWhatSearchBox = CreateRepairWhatSearchBox().SetRow(2).SetColumn(0);
            Grid fuzzyToggle = CreateFuzzySearchGrid().SetRow(3).SetColumn(0);

            grid.Children.Add(dataGrid);
            grid.Children.Add(handInWhatSearchBox);
            grid.Children.Add(repairWhatSearchBox);
            grid.Children.Add(fuzzyToggle);
        }

        private TextBox CreateHandInWhatSearchBox()
        {
            ViewModel.HandInWhatSearchBox = TextBoxFactory.CreateSearchBox("Handed in", "Search handed in...",
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
            Grid grid = GridFactory.CreateDefaultGrid();

            grid.RowSpacing = 4;
            grid.ColumnSpacing = 8;
            grid.Margin = new Thickness(4);

            grid.DefineRows(GridLength.Auto, GridLength.Auto);
            grid.DefineColumns(GridLength.Auto, GridLength.Auto);

            TextBlock header = TextBlockFactory.CreateBlackText("Use fuzzy search").SetRow(0).SetColumn(0, 2);

            header.Margin = new Thickness(4);

            ViewModel.FuzzySearchToggle = CreateFuzzyToggle().SetRow(1).SetColumn(0);

            TextBlock searchModeTextBlock = CreateSearchModeTextBlock().SetRow(1).SetColumn(1);

            grid.Children.Add(header);
            grid.Children.Add(ViewModel.FuzzySearchToggle);
            grid.Children.Add(searchModeTextBlock);

            return grid;
        }

        private CheckBox CreateFuzzyToggle()
        {
            var fuzzySearchToggle = new CheckBox
            {
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(4),
                BorderBrush = new SolidColorBrush(Colors.Black),
            };

            fuzzySearchToggle.SetBinding(ToggleButton.IsCheckedProperty, new Binding
            {
                Path = new PropertyPath(nameof(OrderGridViewModel.UseFuzzySearch)),
                Mode = BindingMode.TwoWay,
            });

            return fuzzySearchToggle;
        }

        private TextBlock CreateSearchModeTextBlock()
        {
            TextBlock textBlock = TextBlockFactory.CreateBlackText();

            textBlock.Margin = new Thickness(4);

            textBlock.SetBinding(TextBlock.TextProperty, new Binding
            {
                Path = new PropertyPath(nameof(OrderGridViewModel.SearchModeText)),
                Mode = BindingMode.OneWay,
            });

            return textBlock;
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
