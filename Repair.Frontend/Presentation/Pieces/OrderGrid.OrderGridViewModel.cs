using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid
{
    internal sealed partial class OrderGridViewModel : ObservableObject
    {
        internal DataGrid DataGrid { get; set; } = null!;
        internal TextBox HandInWhatSearchBox { get; set; } = null!;
        internal TextBox RepairWhatSearchBox { get; set; } = null!;
        internal CheckBox FuzzySearchToggle { get; set; } = null!;

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

        public string SearchModeText =>
            UseFuzzySearch ? "Fuzzy search" : "Exact search";
    }
}
