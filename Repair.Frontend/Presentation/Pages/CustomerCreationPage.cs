namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Creates a new customer. Editing is always enabled.
/// </summary>
internal sealed partial class CustomerCreationPage : Border
{
    public CustomerCreationPage()
    {
        DataContext = new CustomerCreationPageViewModel {IsEditing = true,};
        Margin = new Thickness(0);

        var viewModel = (CustomerCreationPageViewModel) DataContext;
        var logic = new CustomerCreationPageLogic(viewModel);
        var ui = new CustomerCreationPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }
}
