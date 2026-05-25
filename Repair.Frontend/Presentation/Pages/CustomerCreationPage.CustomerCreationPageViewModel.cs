using Repair.Frontend.Presentation.Pieces;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerCreationPage
{
    private sealed class CustomerCreationPageViewModel(CustomerCreationPageArguments arguments)
    {
        public CustomerCreationPageArguments Arguments { get; } = arguments;

        public CustomerEditor CustomerEditor { get; set; } = null!;
    }
}
