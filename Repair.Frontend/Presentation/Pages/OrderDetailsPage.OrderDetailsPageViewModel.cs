using Repair.Frontend.Presentation.Pieces;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed partial class OrderDetailsPageViewModel(OrderDetailsPageArguments arguments) : ObservableObject
    {
        public OrderDetailsPageArguments Arguments { get; } = arguments;

        [ObservableProperty] private Order order = null!;
        [ObservableProperty] private bool isEditing;
        [ObservableProperty] private bool hasChanges;
        [ObservableProperty] private string saveButtonText = "Okay";
        [ObservableProperty] private string cancelButtonText = "Back";
        [ObservableProperty] private bool isPrinting;
        [ObservableProperty] private string printButtonText = "Print";
        public OrderEditor OrderEditor { get; set; } = null!;
        public CheckBox EditCheckBox { get; set; } = null!;
        public Button PrintButton { get; set; } = null!;
        public Button SaveButton { get; set; } = null!;
        public Button CancelButton { get; set; } = null!;
    }
}
