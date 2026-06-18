using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Repair.Abstractions.Persistence;
using Repair.Abstractions.Services;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Pages;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;
using MockFactory = Repair.Tests.Frontend.Core.MockFactory;

namespace Repair.Tests.Frontend.Harness;

internal sealed class OrderDetailsPageHarness
{
    private OrderDetailsPageHarness(
        OrderDetailsPage.OrderDetailsPageArguments arguments, OrderDetailsPage.OrderDetailsPageViewModel viewModel,
        OrderDetailsPage.OrderDetailsPageLogic logic, OrderDetailsPage.OrderDetailsPageUi ui,
        Mock<IEntityQueryService<Order, SearchableOrder>> orderQueryServiceMock,
        Mock<INavigationService> navigationServiceMock, Mock<IReportService> reportServiceMock,
        Mock<IUiDispatcher> uiDispatcherMock)
    {
        Arguments = arguments;
        ViewModel = viewModel;
        Logic = logic;
        Ui = ui;
        OrderQueryServiceMock = orderQueryServiceMock;
        NavigationServiceMock = navigationServiceMock;
        ReportServiceMock = reportServiceMock;
        UiDispatcherMock = uiDispatcherMock;
    }

    internal OrderDetailsPage.OrderDetailsPageArguments Arguments { get; }
    internal OrderDetailsPage.OrderDetailsPageViewModel ViewModel { get; }
    internal OrderDetailsPage.OrderDetailsPageLogic Logic { get; }
    internal OrderDetailsPage.OrderDetailsPageUi Ui { get; }

    internal Mock<IEntityQueryService<Order, SearchableOrder>> OrderQueryServiceMock { get; }
    internal Mock<INavigationService> NavigationServiceMock { get; }
    internal Mock<IReportService> ReportServiceMock { get; }
    internal Mock<IUiDispatcher> UiDispatcherMock { get; }

    internal static OrderDetailsPageHarness Create(Order order)
    {
        Mock<IEntityQueryService<Order, SearchableOrder>> orderQueryServiceMock =
            MockFactory.CreateOrderQueryService(order);
        Mock<INavigationService> navigationServiceMock = MockFactory.CreateNavigationService();
        Mock<IReportService> reportServiceMock = MockFactory.CreateReportService();
        Mock<IUiDispatcher> uiDispatcherMock = MockFactory.CreateUiDispatcher();

        OrderDetailsPage.OrderDetailsPageArguments arguments = new(order.Id, orderQueryServiceMock.Object,
            uiDispatcherMock.Object, NullLoggerFactory.Instance, navigationServiceMock.Object,
            reportServiceMock.Object);

        OrderDetailsPage.OrderDetailsPageViewModel viewModel = new(arguments);
        OrderDetailsPage.OrderDetailsPageLogic logic = new(viewModel);
        OrderDetailsPage.OrderDetailsPageUi ui = new(logic, viewModel);

        ui.CreateContentGrid();

        return new OrderDetailsPageHarness(arguments, viewModel, logic, ui, orderQueryServiceMock,
            navigationServiceMock, reportServiceMock, uiDispatcherMock);
    }
}
