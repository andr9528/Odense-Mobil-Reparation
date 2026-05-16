namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed class OrderDetailsPageViewModel
    {
        public bool IsEditing { get; set; }
        public ToggleSwitch EditToggle { get; set; } = null!;
    }
}