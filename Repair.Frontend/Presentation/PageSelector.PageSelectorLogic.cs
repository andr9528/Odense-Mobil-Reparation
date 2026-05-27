using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Core;
using SkiaSharp;

namespace Repair.Frontend.Presentation
{
    internal sealed partial class PageSelector
    {
        private sealed class PageSelectorLogic : BaseLogic<PageSelectorViewModel>
        {
            private readonly IServiceProvider serviceProvider;
            private readonly INavigationService navigationService;

            public PageSelectorLogic(
                PageSelectorViewModel viewModel, IServiceProvider serviceProvider,
                INavigationService navigationService) : base(viewModel)
            {
                this.serviceProvider = serviceProvider;
                this.navigationService = navigationService;
            }

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
        }
    }
}
