using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomersGrid
{
    internal sealed partial class CustomersGridViewModel(CustomersGridArguments arguments) : ObservableObject
    {
        public CustomersGridArguments Arguments { get; } = arguments;

        public event EventHandler? SearchChanged;
        public event EventHandler? CustomerSelectionChanged;

        internal DataGrid DataGrid { get; set; } = null!;
        internal CustomerEditor CustomerEditor { get; set; } = null!;
        internal CheckBox FuzzySearchToggle { get; set; } = null!;

        public ObservableCollection<Customer> Customers { get; } = [];

        [ObservableProperty] private int selectedCustomerId = arguments.SelectedCustomerId;
        [ObservableProperty] private Customer? selectedCustomer;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(SearchModeText))]
        private bool useFuzzySearch = true;

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

        partial void OnSelectedCustomerIdChanged(int value)
        {
            Customer? customer = Customers.FirstOrDefault(x => x.Id == value);

            if (SelectedCustomer?.Id != customer?.Id)
            {
                SelectedCustomer = customer;
            }

            CustomerSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        partial void OnSelectedCustomerChanged(Customer? value)
        {
            int customerId = value?.Id ?? 0;

            if (SelectedCustomerId == customerId)
            {
                return;
            }

            SelectedCustomerId = customerId;
        }
    }
}
