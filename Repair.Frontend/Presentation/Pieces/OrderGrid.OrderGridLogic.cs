using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.ComplexSearchable;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid
{
    internal sealed partial class OrderGridLogic
    {
        private readonly IEntityQueryService<Order, SearchableOrder> orderQueryService;
        private readonly OrderGridViewModel viewModel;
        private readonly DispatcherQueue dispatcherQueue;

        public OrderGridLogic(
            IEntityQueryService<Order, SearchableOrder> orderQueryService, OrderGridViewModel viewModel,
            DispatcherQueue dispatcherQueue)
        {
            this.orderQueryService = orderQueryService;
            this.viewModel = viewModel;
            this.dispatcherQueue = dispatcherQueue;

            this.viewModel.PropertyChanged += async (_, args) =>
            {
                if (args.PropertyName is nameof(OrderGridViewModel.HandInWhatSearchText)
                    or nameof(OrderGridViewModel.RepairWhatSearchText) or nameof(OrderGridViewModel.UseFuzzySearch))
                {
                    await RefreshOrders();
                }
            };
        }

        public async Task RefreshOrders()
        {
            var orders = await orderQueryService.GetEntitiesComplex(CreateSearchableOrder());

            dispatcherQueue.TryEnqueue(() =>
            {
                viewModel.Orders.Clear();

                foreach (Order order in orders)
                {
                    viewModel.Orders.Add(order);
                }
            });
        }

        private ComplexSearchableOrder CreateSearchableOrder()
        {
            var complex = new ComplexSearchableOrder
            {
                Searchable = new SearchableOrder
                {
                    CustomerId = viewModel.CustomerId,
                },
            };

            if (viewModel.UseFuzzySearch)
            {
                complex.HandInWhat = viewModel.HandInWhatSearchText;
                complex.RepairWhat = viewModel.RepairWhatSearchText;
            }
            else
            {
                complex.Searchable.HandInWhat = viewModel.HandInWhatSearchText;
                complex.Searchable.RepairWhat = viewModel.RepairWhatSearchText;
            }

            return complex;
        }
    }
}
