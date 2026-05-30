using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomerGrid
{
    internal sealed partial class CustomerGridViewModel(CustomerGridArguments arguments) : ObservableObject
    {
        public CustomerGridArguments Arguments { get; } = arguments;

        public event EventHandler? SearchChanged;

        internal DataGrid DataGrid { get; set; } = null!;
        internal CustomerEditor CustomerEditor { get; set; } = null!;
        internal CheckBox FuzzySearchToggle { get; set; } = null!;

        public ObservableCollection<Customer> Customers { get; } = [];

        [ObservableProperty] private int selectedCustomerId = arguments.SelectedCustomerId;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(SearchModeText))]
        private bool useFuzzySearch;

        public void ConnectCustomerEditor(CustomerEditor editor)
        {
            CustomerEditor = editor;

            CustomerEditor.ViewModel.NameChanged += CustomerEditorSearchChanged;
            CustomerEditor.ViewModel.PhoneChanged += CustomerEditorSearchChanged;
            CustomerEditor.ViewModel.EmailChanged += CustomerEditorSearchChanged;
        }

        private void CustomerEditorSearchChanged(object? sender, EventArgs e)
        {
            SearchChanged?.Invoke(this, EventArgs.Empty);
        }

        public string SearchModeText =>
            UseFuzzySearch ? "Fuzzy search" : "Exact search";

        partial void OnUseFuzzySearchChanged(bool value) => FireSearchChanged();

        private void FireSearchChanged()
        {
            SearchChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
