using Repair.Frontend.Services;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderEditor : Border
{
    internal OrderEditorViewModel ViewModel => (OrderEditorViewModel) DataContext;

    private OrderEditorLogic Logic { get; }
    private OrderEditorUi Ui { get; }

    public OrderEditor(OrderEditorArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new OrderEditorViewModel(arguments);

        Logic = new OrderEditorLogic(ViewModel);
        Ui = new OrderEditorUi(Logic, ViewModel);

        Child = Ui.CreateContentGrid();
    }

    internal sealed record OrderEditorArguments(Order? Order = null);
}
