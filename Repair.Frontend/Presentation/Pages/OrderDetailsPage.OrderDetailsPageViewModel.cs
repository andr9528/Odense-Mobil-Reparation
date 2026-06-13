using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Core.Details;
using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed partial class OrderDetailsPageViewModel(OrderDetailsPageArguments arguments)
        : BaseDetailsPageViewModel
    {
        public OrderDetailsPageArguments Arguments { get; } = arguments;

        [ObservableProperty] private Order order = null!;

        public OrderEditor OrderEditor { get; set; } = null!;
    }
}
