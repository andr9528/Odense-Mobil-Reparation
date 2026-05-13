namespace Repair.Frontend.Presentation.Factory
{
    public static class ButtonFactory
    {
        public static HyperlinkButton BuildHyperlinkButton(string url)
        {
            Uri.TryCreate(url, UriKind.Absolute, out Uri? uri);

            return new HyperlinkButton
            {
                NavigateUri = uri,
                IsEnabled = uri is not null,
                VerticalAlignment = VerticalAlignment.Center,
                Content = new TextBlock
                {
                    Text = url,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                },
            };
        }
    }
}
