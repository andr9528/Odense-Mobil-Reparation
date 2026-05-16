namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows customer details and the customer's orders.
/// </summary>
internal sealed partial class CustomerDetailsPage : Border
{
    public CustomerDetailsPage()
    {
        DataContext = new CustomerDetailsPageViewModel();
        Margin = new Thickness(0);

        var viewModel = (CustomerDetailsPageViewModel) DataContext;
        var logic = new CustomerDetailsPageLogic(viewModel);
        var ui = new CustomerDetailsPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }
}
