using Repair.Frontend.Presentation.Converters;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class NullableBooleanOptionBar : Border
{
    internal NullableBooleanOptionBarViewModel ViewModel =>
        (NullableBooleanOptionBarViewModel) DataContext;

    private NullableBooleanOptionBarUi Ui { get; }
    private NullableBooleanOptionBarLogic Logic { get; }

    public NullableBooleanOptionBar(NullableBooleanOptionBarArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        DataContext = new NullableBooleanOptionBarViewModel(arguments);

        Logic = new NullableBooleanOptionBarLogic(ViewModel);
        Ui = new NullableBooleanOptionBarUi(Logic, ViewModel);

        Child = Ui.CreateContentGrid();
    }

    internal sealed record NullableBooleanOptionBarArguments(string Header);
}
