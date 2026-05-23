using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid
{
    internal sealed partial class OrderGridViewModel : ObservableObject
    {
        public event EventHandler? SearchChanged;

        internal DataGrid DataGrid { get; set; } = null!;
        internal TextBox HandInWhatSearchBox { get; set; } = null!;
        internal TextBox RepairWhatSearchBox { get; set; } = null!;
        internal CheckBox FuzzySearchToggle { get; set; } = null!;
        internal TextBox CustomerNameSearchBox { get; set; } = null!;
        internal NullableBooleanOptionBar IsOrderCompleteOptionBar { get; set; } = null!;
        internal NullableBooleanOptionBar HasBorrowedPhoneOptionBar { get; set; } = null!;


        public OrderGridViewModel(int customerId = 0)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get; }

        public ObservableCollection<Order> Orders { get; } = [];

        [ObservableProperty] private string handInWhatSearchText = string.Empty;

        [ObservableProperty] private string repairWhatSearchText = string.Empty;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(SearchModeText))]
        private bool useFuzzySearch;

        [ObservableProperty] private string customerNameSearchText = string.Empty;

        [ObservableProperty] private bool? isOrderComplete = null;

        [ObservableProperty] private bool? hasBorrowedPhone = null;

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

        private void FireSearchChanged()
        {
            SearchChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
