using System.Diagnostics;
using Microsoft.UI.Dispatching;
using Repair.Frontend.Abstraction;
using Repair.Frontend.Presentation.Pages;

namespace Repair.Frontend.Services
{
    public class NavigationService(
        IUiDispatcher uiDispatcher,
        ILogger<NavigationService> logger,
        TrialService trialService) : INavigationService
    {
        private Frame? contentFrame;
        private readonly Stack<(UIElement Element, string Name)> navigationStack = new();

        /// <inheritdoc />
        public void RegisterContentFrame(Frame frame)
        {
            contentFrame = frame;
        }

        /// <inheritdoc />
        public void NavigateTo(UIElement element, string name)
        {
            navigationStack.Push((element, name));

            TimeSpan elapsed = UpdateFrameToTopElement();

            logger.LogDebug("Navigated to {PageName} in {ElapsedMilliseconds} ms", name, elapsed.TotalMilliseconds);
        }

        /// <inheritdoc />
        public void NavigateBack()
        {
            if (navigationStack.Count <= 1)
            {
                return;
            }

            string previousPageName = navigationStack.Peek().Name;

            navigationStack.Pop();
            PopCreatePageIfNeeded();

            TimeSpan elapsed = UpdateFrameToTopElement();
            string currentPageName = navigationStack.Peek().Name;

            logger.LogDebug("Navigated back from {PreviousPageName} to {CurrentPageName} in {ElapsedMilliseconds} ms",
                previousPageName, currentPageName, elapsed.TotalMilliseconds);
        }

        private void PopCreatePageIfNeeded()
        {
            if (navigationStack.Count <= 1)
                return;

            UIElement element = navigationStack.Peek().Element;

            if (element is not CustomerCreationPage && element is not OrderCreationPage)
                return;

            navigationStack.Pop();
        }

        private TimeSpan UpdateFrameToTopElement()
        {
            var stopwatch = Stopwatch.StartNew();

            (UIElement peekedElement, string requestedPageName) = navigationStack.Peek();

            TrialService.NavigationResult navigationResult =
                trialService.GetNavigationElementOrDefault(peekedElement, requestedPageName);

            uiDispatcher.TryEnqueue(() =>
            {
                contentFrame?.Content = navigationResult.Element;

                if (navigationResult.Element == peekedElement && peekedElement is INavigationRefreshable refreshable)
                {
                    refreshable.RefreshAfterNavigation();
                }
            });

            stopwatch.Stop();

            logger.LogInformation(
                "Navigated to {DisplayedPageName} (requested: {RequestedPageName}) in {ElapsedMilliseconds} ms",
                navigationResult.DisplayName, requestedPageName, stopwatch.Elapsed.TotalMilliseconds);

            return stopwatch.Elapsed;
        }
    }
}    
