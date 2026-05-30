using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrdersPage
{
    private sealed class OrdersPageUi : BaseUi<OrdersPageLogic, OrdersPageViewModel>
    {
        private readonly IEntityQueryService<Order, SearchableOrder> queryService;
        private readonly DispatcherQueue dispatcherQueue;
        private readonly ILoggerFactory loggerFactory;

        public OrdersPageUi(OrdersPageLogic logic, OrdersPageViewModel viewModel) : base(logic, viewModel)
        {
            queryService = ViewModel.Arguments.OrderQueryService;
            dispatcherQueue = ViewModel.Arguments.DispatcherQueue;
            loggerFactory = ViewModel.Arguments.LoggerFactory;
        }

        protected override void ConfigureGrid(Grid grid)
        {
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(0);
            grid.DefineRows(GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star));
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateCreateOrderButton().SetRow(1));
            grid.Children.Add(CreateOrdersGrid().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Orders");
        }

        private Button CreateCreateOrderButton()
        {
            var button = new Button
            {
                Content = "Create order",
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            button.Click += Logic.CreateOrderClicked;

            return button;
        }

        private OrdersGrid CreateOrdersGrid()
        {
            OrdersGrid.OrdersGridArguments arguments = Logic.GetArgumentsFactory().CreateOrderGridArguments();

            ViewModel.OrdersGrid = new OrdersGrid(arguments);

            ViewModel.OrdersGrid.ViewModel.DataGrid.SelectionChanged += Logic.OrderClicked;

            return ViewModel.OrdersGrid;
        }
    }
}
