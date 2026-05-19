namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomerEditor : Border
{
    internal CustomerEditorViewModel ViewModel => (CustomerEditorViewModel) DataContext;

    private CustomerEditorLogic Logic { get; }
    private CustomerEditorUi Ui { get; }

    public CustomerEditor(bool isSearchMode = false)
    {
        DataContext = new CustomerEditorViewModel();

        Logic = new CustomerEditorLogic(ViewModel);
        Ui = new CustomerEditorUi(Logic, ViewModel, isSearchMode);

        Child = Ui.CreateContentGrid();
    }
}
