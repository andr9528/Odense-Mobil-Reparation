using Repair.Frontend.Presentation.Pieces;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerCreationPage
{
    private sealed class CustomerCreationPageViewModel
    {
        public CustomerEditor CustomerEditor { get; set; } = null!;
    }
}
