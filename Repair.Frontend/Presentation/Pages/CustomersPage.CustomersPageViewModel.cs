using CommunityToolkit.WinUI.UI.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Repair.Models.Entity.Model;
using System.Collections.ObjectModel;
using Repair.Frontend.Presentation.Pieces;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    internal sealed partial class CustomersPageViewModel : ObservableObject
    {
        public CustomersPageViewModel()
        {
        }

        public event EventHandler? SearchChanged;

        public ObservableCollection<Customer> Customers { get; } = [];

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(SearchModeText))]
        private bool useFuzzySearch;

        public string SearchModeText => UseFuzzySearch ? "Fuzzy search" : "Exact search";

        public DataGrid DataGrid { get; set; } = null!;
        public CheckBox FuzzySearchToggle { get; set; } = null!;
        public CustomerEditor CustomerEditor { get; set; } = null!;

        public void ConnectCustomerEditor(CustomerEditor editor)
        {
            CustomerEditor = editor;

            CustomerEditor.ViewModel.NameChanged += CustomerEditorSearchChanged;
            CustomerEditor.ViewModel.PhoneChanged += CustomerEditorSearchChanged;
            CustomerEditor.ViewModel.EmailChanged += CustomerEditorSearchChanged;
        }

        partial void OnUseFuzzySearchChanged(bool value)
        {
            SearchChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CustomerEditorSearchChanged(object? sender, EventArgs e)
        {
            SearchChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
