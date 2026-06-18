using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Core.Details;
using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed partial class CustomerDetailsPageViewModel(CustomerDetailsPageArguments arguments)
        : BaseDetailsPageViewModel
    {
        public CustomerDetailsPageArguments Arguments { get; } = arguments;

        [ObservableProperty] private Customer customer = null!;

        public CustomerEditor CustomerEditor { get; set; } = null!;
        public OrdersGrid OrdersGrid { get; set; } = null!;
    }
}
