using Microsoft.UI.Dispatching;
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
            private readonly DispatcherQueue dispatcherQueue;

            public PageSelectorLogic(
                PageSelectorViewModel viewModel, IServiceProvider serviceProvider, INavigationService navigationService,
                DispatcherQueue dispatcherQueue) : base(viewModel)
            {
                this.serviceProvider = serviceProvider;
                this.navigationService = navigationService;
                this.dispatcherQueue = dispatcherQueue;
            }

            internal void MenuListSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (sender is not ListView {SelectedItem: IPageRegion region,})
                    return;

                dispatcherQueue.TryEnqueue(() =>
                {
                    UIElement control = region.CreateControl(serviceProvider);
                    navigationService.NavigateTo(control);
                });
            }

            internal void BackButtonClicked(object sender, RoutedEventArgs e)
            {
                dispatcherQueue.TryEnqueue(() => { navigationService.NavigateBack(); });
            }
        }
    }
}
