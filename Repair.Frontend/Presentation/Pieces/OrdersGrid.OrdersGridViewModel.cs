using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrdersGrid
{
    internal sealed partial class OrdersGridViewModel(OrdersGridArguments arguments) : ObservableObject
    {
        public OrdersGridArguments Arguments { get; } = arguments;

        public event EventHandler? SearchChanged;

        internal DataGrid DataGrid { get; set; } = null!;
        internal TextBox HandInWhatSearchBox { get; set; } = null!;
        internal TextBox RepairWhatSearchBox { get; set; } = null!;
        internal CheckBox FuzzySearchToggle { get; set; } = null!;
        internal TextBox CustomerNameSearchBox { get; set; } = null!;
        internal NullableBooleanOptionBar IsOrderCompleteOptionBar { get; set; } = null!;
        internal NullableBooleanOptionBar HasBorrowedPhoneOptionBar { get; set; } = null!;
        internal CheckBox UseHandInFromFilterCheckBox { get; set; } = null!;
        internal CheckBox UseHandInToFilterCheckBox { get; set; } = null!;
        internal CheckBox UseReturnedFromFilterCheckBox { get; set; } = null!;
        internal CheckBox UseReturnedToFilterCheckBox { get; set; } = null!;
        internal DateTimePicker HandInFromDateTimePicker { get; set; } = null!;
        internal DateTimePicker HandInToDateTimePicker { get; set; } = null!;
        internal DateTimePicker ReturnedFromDateTimePicker { get; set; } = null!;
        internal DateTimePicker ReturnedToDateTimePicker { get; set; } = null!;

        public int CustomerId
        {
            get => Arguments.CustomerId;
        }

        public ObservableCollection<Order> Orders { get; } = [];

        [ObservableProperty] private string handInWhatSearchText = string.Empty;

        [ObservableProperty] private string repairWhatSearchText = string.Empty;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(SearchModeText))]
        private bool useFuzzySearch;

        [ObservableProperty] private string customerNameSearchText = string.Empty;

        [ObservableProperty] private bool? isOrderComplete = null;

        [ObservableProperty] private bool? hasBorrowedPhone = null;

        [ObservableProperty] private bool useHandInFromFilter;
        [ObservableProperty] private bool useHandInToFilter;
        [ObservableProperty] private bool useReturnedFromFilter;
        [ObservableProperty] private bool useReturnedToFilter;

        [ObservableProperty] private DateTimeOffset handInFromDate = DateTimeOffset.Now;
        [ObservableProperty] private TimeSpan handInFromTime = DateTimeOffset.Now.TimeOfDay;

        [ObservableProperty] private DateTimeOffset handInToDate = DateTimeOffset.Now;
        [ObservableProperty] private TimeSpan handInToTime = DateTimeOffset.Now.TimeOfDay;

        [ObservableProperty] private DateTimeOffset returnedFromDate = DateTimeOffset.Now;
        [ObservableProperty] private TimeSpan returnedFromTime = DateTimeOffset.Now.TimeOfDay;

        [ObservableProperty] private DateTimeOffset returnedToDate = DateTimeOffset.Now;
        [ObservableProperty] private TimeSpan returnedToTime = DateTimeOffset.Now.TimeOfDay;

        public string SearchModeText =>
            UseFuzzySearch ? "Fuzzy search" : "Exact search";

        partial void OnHandInWhatSearchTextChanged(string value)
        {
            FireSearchChanged();
        }

        partial void OnRepairWhatSearchTextChanged(string value)
        {
            FireSearchChanged();
        }

        partial void OnCustomerNameSearchTextChanged(string value)
        {
            FireSearchChanged();
        }

        partial void OnIsOrderCompleteChanged(bool? value)
        {
            FireSearchChanged();
        }

        partial void OnHasBorrowedPhoneChanged(bool? value)
        {
            FireSearchChanged();
        }

        partial void OnUseFuzzySearchChanged(bool value)
        {
            FireSearchChanged();
        }

        partial void OnUseHandInFromFilterChanged(bool value) => FireSearchChanged();
        partial void OnUseHandInToFilterChanged(bool value) => FireSearchChanged();
        partial void OnUseReturnedFromFilterChanged(bool value) => FireSearchChanged();
        partial void OnUseReturnedToFilterChanged(bool value) => FireSearchChanged();

        private void FireSearchChanged()
        {
            SearchChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
