using Microsoft.UI.Dispatching;
using Repair.Abstractions.Persistence;
using Repair.Abstractions.Services;
using Repair.Frontend.Abstraction;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows order details and information about the related customer.
/// </summary>
internal sealed partial class OrderDetailsPage : Border
{
    public OrderDetailsPage(OrderDetailsPageArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new OrderDetailsPageViewModel(arguments);
        Margin = new Thickness(0);

        var viewModel = (OrderDetailsPageViewModel) DataContext;
        var logic = new OrderDetailsPageLogic(viewModel);
        var ui = new OrderDetailsPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();

        _ = logic.RefreshOrder();
    }

    internal sealed record OrderDetailsPageArguments(
        int OrderId,
        IEntityQueryService<Order, SearchableOrder> OrderQueryService,
        DispatcherQueue DispatcherQueue,
        ILoggerFactory LoggerFactory,
        INavigationService NavigationService,
        IReportService ReportService);
}
