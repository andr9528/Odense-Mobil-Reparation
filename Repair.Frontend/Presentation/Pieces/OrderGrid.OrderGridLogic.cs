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

        public OrderGridLogic(OrderGridViewModel viewModel) : base(viewModel)
        {
            orderQueryService = ViewModel.Arguments.OrderQueryService;
            dispatcherQueue = ViewModel.Arguments.DispatcherQueue;
            logger = ViewModel.Arguments.LoggerFactory.CreateLogger<OrderGridLogic>();

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
