using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Abstractions.Services;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Pages;
using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Services;

internal class ArgumentsFactory(
    IEntityQueryService<Customer, SearchableCustomer> customerQueryService,
    IEntityQueryService<Order, SearchableOrder> orderQueryService,
    DispatcherQueue dispatcherQueue,
    ILoggerFactory loggerFactory,
    INavigationService navigationService,
    IReportService reportService)
{
    internal CustomerDetailsPage.CustomerDetailsPageArguments CreateCustomerDetailsPageArguments(int customerId)
    {
        return new CustomerDetailsPage.CustomerDetailsPageArguments(customerId, customerQueryService, orderQueryService,
            dispatcherQueue, loggerFactory, navigationService);
    }

    internal OrdersGrid.OrdersGridArguments CreateOrderGridArguments(int customerId = 0)
    {
        return new OrdersGrid.OrdersGridArguments(orderQueryService, dispatcherQueue, loggerFactory, customerId);
    }

    internal CustomersPage.CustomersPageArguments CreateCustomersPageArguments()
    {
        return new CustomersPage.CustomersPageArguments(navigationService);
    }

    internal OrdersPage.OrdersPageArguments CreateOrdersPageArguments()
    {
        return new OrdersPage.OrdersPageArguments(navigationService);
    }

    internal CustomerCreationPage.CustomerCreationPageArguments CreateCustomerCreationPageArguments()
    {
        return new CustomerCreationPage.CustomerCreationPageArguments(customerQueryService, navigationService,
            loggerFactory);
    }

    internal CustomerEditor.CustomerEditorArguments CreateCustomerEditorArguments(bool isSearchMode = false)
    {
        return new CustomerEditor.CustomerEditorArguments(isSearchMode);
    }

    internal NullableBooleanOptionBar.NullableBooleanOptionBarArguments CreateNullableBooleanOptionBarArguments(
        string header)
    {
        return new NullableBooleanOptionBar.NullableBooleanOptionBarArguments(header);
    }

    internal OrderCreationPage.OrderCreationPageArguments CreateOrderCreationPageArguments(int selectedCustomerId = 0)
    {
        return new OrderCreationPage.OrderCreationPageArguments(orderQueryService, navigationService, loggerFactory,
            selectedCustomerId);
    }

    internal DateTimePicker.DateTimePickerArguments CreateDateTimePickerArguments(
        string header, DateTime? initialValue = null, int minuteIncrement = 5)
    {
        return new DateTimePicker.DateTimePickerArguments(header, loggerFactory, initialValue, minuteIncrement);
    }

    internal CustomersGrid.CustomersGridArguments CreateCustomersGridArguments(int selectedCustomerId = 0)
    {
        return new CustomersGrid.CustomersGridArguments(customerQueryService, dispatcherQueue, loggerFactory,
            selectedCustomerId);
    }

    internal OrderEditor.OrderEditorArguments CreateOrderEditorArguments(
        Order? order = null, int selectedCustomerId = 0)
    {
        return new OrderEditor.OrderEditorArguments(order, selectedCustomerId);
    }

    internal OrderDetailsPage.OrderDetailsPageArguments CreateOrderDetailsPageArguments(int orderId)
    {
        return new OrderDetailsPage.OrderDetailsPageArguments(orderId, orderQueryService, dispatcherQueue,
            loggerFactory, navigationService, reportService);
    }
}
    
