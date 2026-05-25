namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Creates a new order. Editing is always enabled.
/// </summary>
internal sealed partial class OrderCreationPage : Border
{
    public OrderCreationPage(OrderCreationPageArguments arguments)
    {
        DataContext = new OrderCreationPageViewModel(arguments) {IsEditing = true,};
        Margin = new Thickness(0);

        var viewModel = (OrderCreationPageViewModel) DataContext;
        var logic = new OrderCreationPageLogic(viewModel);
        var ui = new OrderCreationPageUi(logic, viewModel);

        Child = ui.CreateContentGrid();
    }

    internal record OrderCreationPageArguments();
}
