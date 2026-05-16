namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows order details and information about the related customer.
/// </summary>
internal sealed partial class OrderDetailsPage : Border
{
    public OrderDetailsPage()
    {
        DataContext = new OrderDetailsPageViewModel();
        Margin = new Thickness(0);

        var viewModel = (OrderDetailsPageViewModel) DataContext;
        var logic = new OrderDetailsPageLogic(viewModel);
        var ui = new OrderDetailsPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }
}