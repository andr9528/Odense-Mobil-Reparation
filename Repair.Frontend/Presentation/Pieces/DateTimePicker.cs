namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class DateTimePicker : Border
{
    internal DateTimePickerViewModel ViewModel => (DateTimePickerViewModel) DataContext;

    public DateTimePicker(DateTimePickerArguments arguments)
    {
        DataContext = new DateTimePickerViewModel(arguments);

        var logic = new DateTimePickerLogic(ViewModel);
        var ui = new DateTimePickerUi(logic, ViewModel);

        Child = ui.CreateContentGrid();
    }

    internal record DateTimePickerArguments(
        string Header,
        ILoggerFactory LoggerFactory,
        DateTime? InitialValue = null,
        int MinuteIncrement = 5);
}
