using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Core;
using SkiaSharp;

namespace Repair.Frontend.Presentation
{
    internal sealed partial class PageSelector
    {
        private sealed class PageSelectorLogic(
            PageSelectorViewModel viewModel,
            IServiceProvider serviceProvider,
            INavigationService navigationService) : BaseLogic<PageSelectorViewModel>(viewModel)
        {
            internal void BackButtonClicked(object sender, RoutedEventArgs e)
            {
                navigationService.NavigateBack();
            }

            public void MenuListItemClicked(object sender, ItemClickEventArgs e)
            {
                if (e.ClickedItem is not IPageRegion region)
                {
                    return;
                }

                UIElement control = region.CreateControl(serviceProvider);
                navigationService.NavigateTo(control, region.DisplayName);
            }

            internal void NavigateToFirstRegion()
            {
                if (!ViewModel.Regions.Any())
                {
                    return;
                }

                IPageRegion region = ViewModel.Regions[0];

                UIElement control = region.CreateControl(serviceProvider);
                navigationService.NavigateTo(control, region.DisplayName);
            }
        }
    }
}
