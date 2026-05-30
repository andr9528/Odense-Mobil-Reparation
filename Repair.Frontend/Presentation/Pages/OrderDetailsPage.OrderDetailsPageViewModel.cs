namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed class OrderDetailsPageViewModel(OrderDetailsPageArguments arguments)
    {
        public OrderDetailsPageArguments Arguments { get; } = arguments;
        public bool IsEditing { get; set; }
        public ToggleSwitch EditToggle { get; set; } = null!;
    }
}
