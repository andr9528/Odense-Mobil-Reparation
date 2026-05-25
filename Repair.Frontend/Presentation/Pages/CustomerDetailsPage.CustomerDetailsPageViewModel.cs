using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed partial class CustomerDetailsPageViewModel(CustomerDetailsPageArguments arguments) : ObservableObject
    {
        public CustomerDetailsPageArguments Arguments { get; } = arguments;

        [ObservableProperty] private Customer customer = null!;

        [ObservableProperty] private bool isEditing;

        [ObservableProperty] private bool hasChanges;
        [ObservableProperty] private string saveButtonText = "Okay";

        [ObservableProperty] private string cancelButtonText = "Back";

        public CustomerEditor CustomerEditor { get; set; } = null!;

        public OrderGrid OrderGrid { get; set; } = null!;

        public CheckBox EditCheckBox { get; set; } = null!;

        public Button SaveButton { get; set; } = null!;

        public Button CancelButton { get; set; } = null!;
    }
}
