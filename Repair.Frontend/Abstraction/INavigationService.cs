namespace Repair.Frontend.Abstraction
{
    internal interface INavigationService
    {
        void RegisterContentFrame(Frame frame);
        void NavigateTo(UIElement element, string name);
        void NavigateBack();
    }
}
