using Repair.Frontend.Presentation.Core;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class CustomerEditor
{
    internal sealed class CustomerEditorLogic : BaseLogic<CustomerEditorViewModel>
    {
        public CustomerEditorLogic(CustomerEditorViewModel viewModel) : base(viewModel)
        {
            ViewModel.IsReadOnlyChanged += ToggleTextBoxReadOnlyState;
        }

        private void ToggleTextBoxReadOnlyState(object? sender, EventArgs e)
        {
            ViewModel.NameTextBox.IsReadOnly = ViewModel.IsReadOnly;
            ViewModel.PhoneTextBox.IsReadOnly = ViewModel.IsReadOnly;
            ViewModel.EmailTextBox.IsReadOnly = ViewModel.IsReadOnly;
        }

        public void PhoneTextBoxBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            string text = args.NewText;

            bool isValid = text.Select((character, index) => new {character, index,})
                .All(x => char.IsDigit(x.character) || (x.character == '+' && x.index == 0));


            args.Cancel = !isValid;
        }
    }
}
