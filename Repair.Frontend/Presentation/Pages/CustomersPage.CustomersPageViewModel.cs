using CommunityToolkit.WinUI.UI.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Repair.Models.Entity.Model;
using System.Collections.ObjectModel;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    internal sealed partial class CustomersPageViewModel : ObservableObject
    {
        public ObservableCollection<Customer> Customers { get; } = [];

        [ObservableProperty] private string nameSearchText = string.Empty;

        [ObservableProperty] private string phoneSearchText = string.Empty;

        [ObservableProperty] private string emailSearchText = string.Empty;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(SearchModeText))]
        private bool useFuzzySearch;

        public string SearchModeText => UseFuzzySearch ? "Fuzzy search" : "Exact search";

        public TextBox NameSearchBox { get; set; } = null!;
        public TextBox PhoneSearchBox { get; set; } = null!;
        public TextBox EmailSearchBox { get; set; } = null!;
        public DataGrid DataGrid { get; set; } = null!;
        public CheckBox FuzzySearchToggle { get; set; } = null!;
    }
}
