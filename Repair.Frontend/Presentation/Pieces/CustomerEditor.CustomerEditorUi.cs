using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using static Repair.Frontend.Presentation.Pages.CustomersPage;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomerEditor
{
    internal sealed class CustomerEditorUi : BaseUi<CustomerEditorLogic, CustomerEditorViewModel>
    {
        private readonly bool isSearchMode;

        public CustomerEditorUi(
            CustomerEditorLogic logic, CustomerEditorViewModel viewModel, bool isSearchMode = false) : base(logic,
            viewModel)
        {
            this.isSearchMode = isSearchMode;
        }

        protected override void ConfigureGrid(Grid grid)
        {
            grid.RowSpacing = 8;
            grid.DefineRows(GridLength.Auto);
            grid.DefineColumns(GridUnitType.Star, [1, 1, 1,]);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateNameTextBox().SetRow(0).SetColumn(0));
            grid.Children.Add(CreatePhoneTextBox().SetRow(0).SetColumn(1));
            grid.Children.Add(CreateEmailTextBox().SetRow(0).SetColumn(2));
        }

        private TextBox CreateNameTextBox()
        {
            string placeholder = isSearchMode ? "Search name..." : "Customer name...";

            ViewModel.NameTextBox = TextBoxFactory.CreateSearchBox(
                "Name", placeholder, nameof(CustomerEditorViewModel.Name));
            ViewModel.NameTextBox.IsReadOnly = ViewModel.IsReadOnly;

            return ViewModel.NameTextBox;
        }

        private TextBox CreatePhoneTextBox()
        {
            string placeholder = isSearchMode ? "Search phone..." : "Customer phone...";

            ViewModel.PhoneTextBox = TextBoxFactory.CreateSearchBox(
                "Phone", placeholder, nameof(CustomerEditorViewModel.Phone));
            ViewModel.PhoneTextBox.IsReadOnly = ViewModel.IsReadOnly;

            ViewModel.PhoneTextBox.BeforeTextChanging += Logic.PhoneTextBoxBeforeTextChanging;

            return ViewModel.PhoneTextBox;
        }

        private TextBox CreateEmailTextBox()
        {
            string placeholder = isSearchMode ? "Search email..." : "Customer email...";

            ViewModel.EmailTextBox = TextBoxFactory.CreateSearchBox(
                "Email", placeholder, nameof(CustomerEditorViewModel.Email));
            ViewModel.EmailTextBox.IsReadOnly = ViewModel.IsReadOnly;

            return ViewModel.EmailTextBox;
        }
    }
}
