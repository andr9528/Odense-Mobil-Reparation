using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Models.Entity.ComplexSearchable;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;
using Repair.Models.Extensions;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrdersGrid
{
    internal sealed partial class OrdersGridLogic : BaseLogic<OrdersGridViewModel>
    {
        private readonly IEntityQueryService<Order, SearchableOrder> orderQueryService;
        private readonly DispatcherQueue dispatcherQueue;
        private readonly ILogger<OrdersGridLogic> logger;

        public OrdersGridLogic(OrdersGridViewModel viewModel) : base(viewModel)
        {
            orderQueryService = ViewModel.Arguments.OrderQueryService;
            dispatcherQueue = ViewModel.Arguments.DispatcherQueue;
            logger = ViewModel.Arguments.LoggerFactory.CreateLogger<OrdersGridLogic>();

            ViewModel.SearchChanged += SearchChanged;
        }

        internal async void SearchChanged(object? sender, EventArgs e)
        {
            try
            {
                await RefreshOrders();
            }
            catch (Exception exe)
            {
                logger.LogError(exe, "Exception caught during refresh of Orders");
            }
        }

        public async Task RefreshOrders()
        {
            ComplexSearchableOrder searchable = CreateSearchableOrder();

            List<Order> orders = (await orderQueryService.GetEntitiesComplex(searchable)).ToList();
            orders = ViewModel.DataGrid.ApplyCurrentSort(orders).ToList();

            logger.LogDebug("Orders query returned {OrderCount} orders.", orders.Count);

            dispatcherQueue.TryEnqueue(() =>
            {
                logger.LogDebug("Updating Orders collection. Existing count: {ExistingCount}", ViewModel.Orders.Count);

                ViewModel.Orders.ReplaceItems(orders);
                ViewModel.DataGrid.Refresh();

                logger.LogDebug("Orders collection updated. New count: {NewCount}", ViewModel.Orders.Count);
            });
        }

        private ComplexSearchableOrder CreateSearchableOrder()
        {
            var complex = new ComplexSearchableOrder
            {
                Searchable = new SearchableOrder
                {
                    CustomerId = ViewModel.CustomerId,
                },
                UseFuzzy = ViewModel.UseFuzzySearch,
                CustomerName = ViewModel.CustomerNameSearchText,
                IsOrderComplete = ViewModel.IsOrderComplete,
                HasBorrowedPhone = ViewModel.HasBorrowedPhone,
            };

            if (ViewModel.UseFuzzySearch)
            {
                complex.HandInWhat = ViewModel.HandInWhatSearchText;
                complex.RepairWhat = ViewModel.RepairWhatSearchText;
            }
            else
            {
                complex.Searchable.HandInWhat = ViewModel.HandInWhatSearchText;
                complex.Searchable.RepairWhat = ViewModel.RepairWhatSearchText;
            }

            AddDateTimeFilters(complex);

            return complex;
        }

        private void AddDateTimeFilters(ComplexSearchableOrder complex)
        {
            if (ViewModel.UseHandInFromFilter)
            {
                complex.HandInFrom = ViewModel.HandInFromDateTimePicker.ViewModel.SelectedDateTime;
            }

            if (ViewModel.UseHandInToFilter)
            {
                complex.HandInTo = ViewModel.HandInToDateTimePicker.ViewModel.SelectedDateTime;
            }

            if (ViewModel.UseReturnedFromFilter)
            {
                complex.ReturnedFrom = ViewModel.ReturnedFromDateTimePicker.ViewModel.SelectedDateTime;
            }

            if (ViewModel.UseReturnedToFilter)
            {
                complex.ReturnedTo = ViewModel.ReturnedToDateTimePicker.ViewModel.SelectedDateTime;
            }
        }

        public void IsOrderCompleteSelectionChanged(object? sender, EventArgs e)
        {
            ViewModel.IsOrderComplete = ViewModel.IsOrderCompleteOptionBar.ViewModel.SelectedValue;
        }

        public void HasBorrowedPhoneSelectionChanged(object? sender, EventArgs e)
        {
            ViewModel.HasBorrowedPhone = ViewModel.HasBorrowedPhoneOptionBar.ViewModel.SelectedValue;
        }
    }
}
