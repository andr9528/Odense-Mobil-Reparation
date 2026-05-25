using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Pages;
using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Services;

internal class ArgumentsFactory
{
    private readonly IEntityQueryService<Customer, SearchableCustomer> customerQueryService;
    private readonly IEntityQueryService<Order, SearchableOrder> orderQueryService;
    private readonly DispatcherQueue dispatcherQueue;
    private readonly ILoggerFactory loggerFactory;
    private readonly INavigationService navigationService;

    public ArgumentsFactory(
        IEntityQueryService<Customer, SearchableCustomer> customerQueryService,
        IEntityQueryService<Order, SearchableOrder> orderQueryService, DispatcherQueue dispatcherQueue,
        ILoggerFactory loggerFactory, INavigationService navigationService)
    {
        this.customerQueryService = customerQueryService;
        this.orderQueryService = orderQueryService;
        this.dispatcherQueue = dispatcherQueue;
        this.loggerFactory = loggerFactory;
        this.navigationService = navigationService;
    }

    internal CustomerDetailsPage.CustomerDetailsPageArguments CreateCustomerDetailsPageArguments(int customerId)
    {
        return new CustomerDetailsPage.CustomerDetailsPageArguments(customerId, customerQueryService, orderQueryService,
            dispatcherQueue, loggerFactory, navigationService);
    }

    internal OrderGrid.OrderGridArguments CreateOrderGridArguments(int customerId = 0)
    {
        return new OrderGrid.OrderGridArguments(orderQueryService, dispatcherQueue, loggerFactory, customerId);
    }

    internal CustomersPage.CustomersPageArguments CreateCustomersPageArguments()
    {
        return new CustomersPage.CustomersPageArguments(customerQueryService, dispatcherQueue, loggerFactory,
            navigationService);
    }

    internal OrdersPage.OrdersPageArguments CreateOrdersPageArguments()
    {
        return new OrdersPage.OrdersPageArguments(orderQueryService, dispatcherQueue, loggerFactory);
    }

    internal CustomerCreationPage.CustomerCreationPageArguments CreateCustomerCreationPageArguments()
    {
        return new CustomerCreationPage.CustomerCreationPageArguments(customerQueryService, navigationService);
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

    public OrderCreationPage.OrderCreationPageArguments CreateOrderCreationPageArguments()
    {
        return new OrderCreationPage.OrderCreationPageArguments();
    }
}
