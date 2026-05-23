using Repair.Frontend.Presentation.Converters;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class NullableBooleanOptionBar : Border
{
    internal NullableBooleanOptionBarViewModel ViewModel =>
        (NullableBooleanOptionBarViewModel) DataContext;

    private NullableBooleanOptionBarUi Ui { get; }
    private NullableBooleanOptionBarLogic Logic { get; }

    public NullableBooleanOptionBar(string header)
    {
        DataContext = new NullableBooleanOptionBarViewModel(header);

        Logic = new NullableBooleanOptionBarLogic(ViewModel);
        Ui = new NullableBooleanOptionBarUi(Logic, ViewModel);

        Child = Ui.CreateContentGrid();
    }
}
