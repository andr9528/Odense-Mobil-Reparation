using Microsoft.UI.Dispatching;
using Repair.Frontend.Abstraction;

namespace Repair.Frontend.Services
{
    public class NavigationService : INavigationService
    {
        private readonly DispatcherQueue dispatcherQueue;
        private Frame contentFrame;
        private Stack<UIElement> navigationStack;

        public NavigationService(DispatcherQueue dispatcherQueue)
        {
            this.dispatcherQueue = dispatcherQueue;
            navigationStack = new Stack<UIElement>();
        }

        /// <inheritdoc />
        public void RegisterContentFrame(Frame frame)
        {
            contentFrame = frame;
        }

        /// <inheritdoc />
        public void NavigateTo(UIElement element)
        {
            navigationStack.Push(element);
            UpdateFrameToTopElement();
        }

        /// <inheritdoc />
        public void NavigateBack()
        {
            if (navigationStack.Count <= 1)
                return;

            navigationStack.Pop();
            UpdateFrameToTopElement();
        }

        private void UpdateFrameToTopElement()
        {
            UIElement peekedElement = navigationStack.Peek();
            dispatcherQueue.TryEnqueue(() => { contentFrame.Content = peekedElement; });
        }
    }
}    
