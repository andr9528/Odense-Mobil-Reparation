namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    private sealed class CustomersPageViewModel
    {
        public string SearchText { get; set; } = string.Empty;
        public TextBox SearchBox { get; set; } = null!;
        public ListView CustomersList { get; set; } = null!;
    }
}
