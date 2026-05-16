namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows all customers and allows narrowing the list through search.
/// </summary>
internal sealed partial class CustomersPage : Border
{
    public CustomersPage()
    {
        DataContext = new CustomersPageViewModel();

        Margin = new Thickness(0);

        var viewModel = (CustomersPageViewModel) DataContext;
        var logic = new CustomersPageLogic(viewModel);
        var ui = new CustomersPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }
}
