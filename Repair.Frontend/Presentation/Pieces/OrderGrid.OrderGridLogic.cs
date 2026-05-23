using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Presentation.Core;
using Repair.Models.Entity.ComplexSearchable;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid
{
    internal sealed partial class OrderGridLogic : BaseLogic<OrderGridViewModel>
    {
        private readonly IEntityQueryService<Order, SearchableOrder> orderQueryService;
        private readonly DispatcherQueue dispatcherQueue;
        private readonly ILogger<OrderGridLogic> logger;

        public OrderGridLogic(
            IEntityQueryService<Order, SearchableOrder> orderQueryService, OrderGridViewModel viewModel,
            DispatcherQueue dispatcherQueue, ILogger<OrderGridLogic> logger) : base(viewModel)
        {
            this.orderQueryService = orderQueryService;
            this.dispatcherQueue = dispatcherQueue;
            this.logger = logger;

            ViewModel.SearchChanged += SearchChanged;
        }

        private async void SearchChanged(object? sender, EventArgs e)
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
            var orders = await orderQueryService.GetEntitiesComplex(CreateSearchableOrder());

            dispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Orders.Clear();

                foreach (Order order in orders)
                {
                    ViewModel.Orders.Add(order);
                }
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

            return complex;
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
