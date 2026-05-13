using Repair.Frontend.Abstraction;

namespace Repair.Frontend.Services
{
    public class NavigationService : INavigationService
    {
        private Frame contentFrame;
        private Stack<UIElement> navigationStack;

        public NavigationService()
        {
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
            contentFrame.Content = element;
            navigationStack.Push(element);
        }

        /// <inheritdoc />
        public void NavigateBack()
        {
            if (navigationStack.Count <= 1)
                return;

            navigationStack.Pop();

            UIElement previousElement = navigationStack.Peek();

            contentFrame.Content = previousElement;
        }
    }
}    
