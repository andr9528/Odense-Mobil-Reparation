namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomerEditor : Border
{
    internal CustomerEditorViewModel ViewModel => (CustomerEditorViewModel) DataContext;

    private CustomerEditorLogic Logic { get; }
    private CustomerEditorUi Ui { get; }

    public CustomerEditor(CustomerEditorArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new CustomerEditorViewModel(arguments);

        Logic = new CustomerEditorLogic(ViewModel);
        Ui = new CustomerEditorUi(Logic, ViewModel);

        Child = Ui.CreateContentGrid();
    }

    internal sealed record CustomerEditorArguments(bool IsSearchMode = false);
}
