using Repair.Frontend.Abstraction;

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

        DataContext = new CustomersPageViewModel(arguments);
        Margin = new Thickness(0);

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
