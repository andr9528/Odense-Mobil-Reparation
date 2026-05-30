using Repair.Frontend.Presentation.Pieces;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderCreationPage
{
    private sealed class OrderCreationPageViewModel(OrderCreationPageArguments arguments)
    {
        public OrderCreationPageArguments Arguments { get; } = arguments;
        internal OrderEditor OrderEditor { get; set; } = null!;
    }
}
