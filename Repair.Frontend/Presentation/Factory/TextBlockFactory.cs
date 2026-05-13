namespace Repair.Frontend.Presentation.Factory
{
    public static class TextBlockFactory
    {
        public static TextBlock BuildSectionHeader(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 24,
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
        }
    }
}
