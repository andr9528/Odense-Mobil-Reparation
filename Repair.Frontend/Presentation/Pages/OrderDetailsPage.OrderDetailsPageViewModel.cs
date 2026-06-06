using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;
using BaseDetailsPageViewModel = Repair.Frontend.Presentation.Core.Details.BaseDetailsPageViewModel;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed partial class OrderDetailsPageViewModel(OrderDetailsPageArguments arguments)
        : BaseDetailsPageViewModel
    {
        public OrderDetailsPageArguments Arguments { get; } = arguments;

        [ObservableProperty] private Order order = null!;
        [ObservableProperty] private bool isPrinting;
        [ObservableProperty] private string printButtonText = "Print";

        public OrderEditor OrderEditor { get; set; } = null!;
        public Button PrintButton { get; set; } = null!;
    }
}
