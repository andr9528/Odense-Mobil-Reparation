namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrdersPage
{
    private sealed class OrdersPageViewModel
    {
        public string SearchText { get; set; } = string.Empty;
        public TextBox SearchBox { get; set; } = null!;
        public ListView OrdersList { get; set; } = null!;
    }
}