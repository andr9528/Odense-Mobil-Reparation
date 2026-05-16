using Repair.Frontend.Presentation.Pieces;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrdersPage
{
    private sealed class OrdersPageViewModel
    {
        public OrderGrid OrderGrid { get; set; } = null!;
    }
}
