using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderCreationPage
{
    private sealed class OrderCreationPageLogic : BaseLogic<OrderCreationPageViewModel>
    {
        private readonly IEntityQueryService<Order, SearchableOrder> queryService;
        private readonly INavigationService navigationService;

        public OrderCreationPageLogic(OrderCreationPageViewModel viewModel) : base(viewModel)
        {
            queryService = ViewModel.Arguments.OrderQueryService;
            navigationService = ViewModel.Arguments.NavigationService;
        }

        internal async void SaveClicked(object sender, RoutedEventArgs e)
        {
            if (!IsUserInputValid())
            {
                return;
            }

            Order newOrder = BuildNewOrder();
            await queryService.AddEntity(newOrder);

            OrderDetailsPage.OrderDetailsPageArguments arguments = GetArgumentsFactory()
                .CreateOrderDetailsPageArguments(newOrder.Id);

            var details = new OrderDetailsPage(arguments);
            navigationService.NavigateTo(details, "Order Details Page");
        }

        private bool IsUserInputValid()
        {
            OrderEditor.OrderEditorViewModel order = ViewModel.OrderEditor.ViewModel;

            return !string.IsNullOrWhiteSpace(order.HandInWhat) && !string.IsNullOrWhiteSpace(order.RepairWhat) &&
                   GetSelectedCustomerId() > 0;
        }

        private Order BuildNewOrder()
        {
            OrderEditor.OrderEditorViewModel order = ViewModel.OrderEditor.ViewModel;

            return new Order
            {
                HandInWhen = order.HandInWhenPicker.ViewModel.SelectedDateTime,
                ReturnedWhen = order.ReturnedWhenPicker.ViewModel.SelectedDateTime,
                IsOrderComplete = order.IsOrderComplete,
                HasBorrowedPhone = order.HasBorrowedPhone,
                HandInWhat = order.HandInWhat,
                RepairWhat = order.RepairWhat,
                CustomerId = GetSelectedCustomerId(),
            };
        }

        private int GetSelectedCustomerId()
        {
            if (ViewModel.OrderEditor.ViewModel.CustomersGrid.ViewModel.DataGrid.SelectedItem is not Customer customer)
            {
                return 0;
            }

            return customer.Id;
        }

        internal void CancelClicked(object sender, RoutedEventArgs e)
        {
            navigationService.NavigateBack();
        }
    }
}
