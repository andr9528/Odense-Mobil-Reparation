namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed class CustomerDetailsPageViewModel
    {
        public bool IsEditing { get; set; }
        public string OrderSearchText { get; set; } = string.Empty;
        public ToggleSwitch EditToggle { get; set; } = null!;
        public TextBox OrderSearchBox { get; set; } = null!;
        public ListView OrdersList { get; set; } = null!;
    }
}
