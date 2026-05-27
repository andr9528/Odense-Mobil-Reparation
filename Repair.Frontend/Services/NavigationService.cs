using System.Diagnostics;
using Microsoft.UI.Dispatching;
using Repair.Frontend.Abstraction;

namespace Repair.Frontend.Services
{
    public class NavigationService(DispatcherQueue dispatcherQueue, ILogger<NavigationService> logger)
        : INavigationService
    {
        private readonly ILogger<NavigationService> logger = logger;
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

            TimeSpan elapsed = UpdateFrameToTopElement();
            string currentPageName = navigationStack.Peek().Name;

            logger.LogDebug("Navigated back from {PreviousPageName} to {CurrentPageName} in {ElapsedMilliseconds} ms",
                previousPageName, currentPageName, elapsed.TotalMilliseconds);
        }

        private TimeSpan UpdateFrameToTopElement()
        {
            var stopwatch = Stopwatch.StartNew();

            UIElement peekedElement = navigationStack.Peek().Element;

            dispatcherQueue.TryEnqueue(() => { contentFrame?.Content = peekedElement; });

            stopwatch.Stop();

            return stopwatch.Elapsed;
        }
    }
}    
