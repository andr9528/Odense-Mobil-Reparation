namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderCreationPage
{
    private sealed class OrderCreationPageViewModel
    {
        public bool IsEditing { get; set; }
        public string CustomerSearchText { get; set; } = string.Empty;
        public TextBox CustomerSearchBox { get; set; } = null!;
        public ListView CustomersList { get; set; } = null!;
    }
}
