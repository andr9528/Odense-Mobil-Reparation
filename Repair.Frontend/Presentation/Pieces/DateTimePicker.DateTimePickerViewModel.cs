namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class DateTimePicker
{
    internal sealed partial class DateTimePickerViewModel : ObservableObject
    {
        private bool shouldSkipNextDateChange = true;
        private bool shouldSkipNextTimeChange = true;

        public DateTimePickerArguments Arguments { get; }
        public event EventHandler? SelectedDateTimeChanged;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(SelectedDateText))]
        private DateTimeOffset selectedDate;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(SelectedTimeText))]
        private TimeSpan selectedTime;

        private readonly ILogger<DateTimePickerViewModel> logger;

        public string SelectedDateText => SelectedDate.ToString("dd.MM.yyyy");
        public string SelectedTimeText => SelectedTime.ToString(@"hh\:mm");

        /// <inheritdoc/>
        public DateTimePickerViewModel(DateTimePickerArguments arguments)
        {
            Arguments = arguments;
            selectedDate = arguments.InitialValue;
            selectedTime = arguments.InitialValue.TimeOfDay;

            logger = Arguments.LoggerFactory.CreateLogger<DateTimePickerViewModel>();
        }

        internal DateTime SelectedDateTime => SelectedDate.Date.Add(SelectedTime);
        internal DatePicker DatePicker { get; set; } = null!;
        internal TimePicker TimePicker { get; set; } = null!;

        partial void OnSelectedDateChanged(DateTimeOffset value)
        {
            if (shouldSkipNextDateChange)
            {
                shouldSkipNextDateChange = false;
                return;
            }

            logger.LogDebug("Date changed for '{ArgumentsHeader}'", Arguments.Header);
            SelectedDateTimeChanged?.Invoke(this, EventArgs.Empty);
        }

        partial void OnSelectedTimeChanged(TimeSpan value)
        {
            if (shouldSkipNextTimeChange)
            {
                shouldSkipNextTimeChange = false;
                return;
            }

            logger.LogDebug("Time changed for '{ArgumentsHeader}'", Arguments.Header);
            SelectedDateTimeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
