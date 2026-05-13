namespace Repair.Frontend.Presentation.Factory
{
    public static class ListViewFactory
    {
        public static ListView? BuildLinksListView(IList<string>? links)
        {
            if (links == null || !links.Any())
            {
                return null;
            }

            var listView = new ListView
            {
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            var items = links.Select(link => new ListViewItem
            {
                Content = ButtonFactory.BuildHyperlinkButton(link),
            });

            listView.Items.AddRange(items);

            return listView;
        }
    }
}
