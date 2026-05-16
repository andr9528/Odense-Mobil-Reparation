namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Creates a new order. Editing is always enabled.
/// </summary>
internal sealed partial class OrderCreationPage : Border
{
    public OrderCreationPage()
    {
        DataContext = new OrderCreationPageViewModel {IsEditing = true,};
        Margin = new Thickness(0);

        var viewModel = (OrderCreationPageViewModel) DataContext;
        var logic = new OrderCreationPageLogic(viewModel);
        var ui = new OrderCreationPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }
}
