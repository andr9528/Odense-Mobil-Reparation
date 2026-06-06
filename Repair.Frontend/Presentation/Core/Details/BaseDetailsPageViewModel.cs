namespace Repair.Frontend.Presentation.Core.Details;

internal abstract partial class BaseDetailsPageViewModel : ObservableObject
{
    [ObservableProperty] private bool isEditing;
    [ObservableProperty] private bool hasChanges;
    [ObservableProperty] private string saveButtonText = "Okay";
    [ObservableProperty] private string cancelButtonText = "Back";
    [ObservableProperty] private bool canDelete;

    public Button DeleteButton { get; set; } = null!;
    public CheckBox EditCheckBox { get; set; } = null!;
    public Button SaveButton { get; set; } = null!;
    public Button CancelButton { get; set; } = null!;
}
