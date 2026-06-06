using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerCreationPage
{
    private sealed class CustomerCreationPageUi(
        CustomerCreationPageLogic logic,
        CustomerCreationPageViewModel viewModel)
        : BaseUi<CustomerCreationPageLogic, CustomerCreationPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            ConfigureDefaultPageGrid(grid);

            grid.DefineRows(GridLength.Auto, new GridLength(1, GridUnitType.Star));
            grid.DefineColumns(new GridLength(1, GridUnitType.Star));
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateButtonsGrid().SetRow(0));
            grid.Children.Add(CreateCustomerEditor().SetRow(1));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Create customer");
        }

        private CustomerEditor CreateCustomerEditor()
        {
            CustomerEditor.CustomerEditorArguments arguments =
                Logic.GetArgumentsFactory().CreateCustomerEditorArguments();

            ViewModel.CustomerEditor = new CustomerEditor(arguments)
            {
                ViewModel =
                {
                    IsReadOnly = false,
                },
            };

            return ViewModel.CustomerEditor;
        }

        private Grid CreateButtonsGrid()
        {
            return SimplePieceFactory.CreateSaveCancelButtonGrid(Logic.SaveClicked, Logic.CancelClicked);
        }
    }
}
