namespace Repair.Frontend.Presentation.Factory
{
    public static class GridFactory
    {
        private static readonly Random Random = new();

        public static Grid CreateDefaultGrid()
        {
            return new Grid
            {
                Margin = new Thickness(2),
                IsTabStop = false,
                Background = GetBackgroundBrush(true),
            };
        }

        private static Brush GetBackgroundBrush(bool useDebugBrush)
        {
            return !useDebugBrush ? new SolidColorBrush(Colors.Transparent) : GetRandomBrush();
        }

        private static Brush GetRandomBrush()
        {
            var r = (byte) Random.Next(256);
            var g = (byte) Random.Next(256);
            var b = (byte) Random.Next(256);

            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }
    }
}
