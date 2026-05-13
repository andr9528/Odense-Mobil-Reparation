namespace Repair.Frontend.Presentation.Factory
{
    public static class BorderFactory
    {
        private static readonly Color SelectedMenuColor = Color.FromArgb(255, 19, 67, 105);
        private static readonly Color PartTopBorderColor = Color.FromArgb(255, 142, 154, 163);

        public static void ConfigureSectionBorder(this Border border)
        {
            border.BorderThickness = new Thickness(0, 3, 0, 0);
            border.BorderBrush = new SolidColorBrush(SelectedMenuColor);
            border.Padding = new Thickness(0, 5, 0, 0);
        }

        public static Border WrapWithTopBorder(this UIElement child)
        {
            return new Border
            {
                BorderThickness = new Thickness(0, 2, 0, 0),
                BorderBrush = new SolidColorBrush(PartTopBorderColor),

                Padding = new Thickness(0, 5, 0, 5),
                Margin = new Thickness(0, 5, 0, 5),

                Child = child,
            };
        }
    }
}
