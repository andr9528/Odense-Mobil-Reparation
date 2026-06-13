namespace Repair.Frontend.Presentation.Core.Details;

internal abstract partial class BaseDetailsPageViewModel : ObservableObject
{
    [ObservableProperty] private bool isEditing;
    [ObservableProperty] private bool hasChanges;
    [ObservableProperty] private string saveButtonText = "Okay";
    [ObservableProperty] private string cancelButtonText = "Back";
    [ObservableProperty] private bool canDelete;
    [ObservableProperty] private bool isPrinting;
    [ObservableProperty] private string printButtonText = "Print";

    public Button PrintButton { get; set; } = null!;
    public Button DeleteButton { get; set; } = null!;
    public CheckBox EditCheckBox { get; set; } = null!;
    public Button SaveButton { get; set; } = null!;
    public Button CancelButton { get; set; } = null!;
}
