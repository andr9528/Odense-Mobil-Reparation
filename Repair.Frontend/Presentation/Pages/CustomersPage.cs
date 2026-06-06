using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Pages;

/// <summary>
/// Shows all customers and allows narrowing the list through search.
/// </summary>
internal sealed partial class CustomersPage : Border, INavigationRefreshable
{
    private CustomersPageViewModel ViewModel => (CustomersPageViewModel) DataContext;

    public CustomersPage(CustomersPageArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        this.ConfigurePageBorder();

        DataContext = new CustomersPageViewModel(arguments);

        var logic = new CustomersPageLogic(ViewModel);
        var ui = new CustomersPageUi(logic, ViewModel);

        Child = ui.CreateContentGrid();
    }

    internal sealed record CustomersPageArguments(INavigationService NavigationService);

    /// <inheritdoc />
    public void RefreshAfterNavigation()
    {
        ViewModel.CustomersGrid.RefreshAfterNavigation();
    }
}
