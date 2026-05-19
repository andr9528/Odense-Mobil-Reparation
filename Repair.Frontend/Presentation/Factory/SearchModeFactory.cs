using Repair.Frontend.Extensions;

namespace Repair.Frontend.Presentation.Factory;

internal static class SearchModeFactory
{
    public static Grid CreateFuzzySearchGrid(
        string useFuzzySearchPath, string searchModeTextPath, out CheckBox fuzzySearchCheckBox)
    {
        Grid grid = GridFactory.CreateDefaultGrid();

        grid.RowSpacing = 4;
        grid.ColumnSpacing = 8;
        grid.Margin = new Thickness(4);

        grid.DefineRows(GridLength.Auto, GridLength.Auto);
        grid.DefineColumns(GridLength.Auto, GridLength.Auto);

        fuzzySearchCheckBox = CreateFuzzyToggle(useFuzzySearchPath);

        grid.Children.Add(CreateHeader().SetRow(0).SetColumn(0, 2));
        grid.Children.Add(fuzzySearchCheckBox.SetRow(1).SetColumn(0));
        grid.Children.Add(CreateSearchModeTextBlock(searchModeTextPath).SetRow(1).SetColumn(1));

        return grid;
    }

    private static TextBlock CreateHeader()
    {
        TextBlock header = TextBlockFactory.CreateBlackText("Use fuzzy search");
        header.Margin = new Thickness(4);

        return header;
    }

    private static CheckBox CreateFuzzyToggle(string bindingPath)
    {
        var checkBox = new CheckBox
        {
            Foreground = new SolidColorBrush(Colors.Black),
            Margin = new Thickness(4),
            BorderBrush = new SolidColorBrush(Colors.Black),
        };

        checkBox.SetBinding(ToggleButton.IsCheckedProperty, new Binding
        {
            Path = new PropertyPath(bindingPath),
            Mode = BindingMode.TwoWay,
        });

        return checkBox;
    }

    private static TextBlock CreateSearchModeTextBlock(string bindingPath)
    {
        TextBlock textBlock = TextBlockFactory.CreateBlackText();
        textBlock.Margin = new Thickness(4);

        textBlock.SetBinding(TextBlock.TextProperty, new Binding
        {
            Path = new PropertyPath(bindingPath),
            Mode = BindingMode.OneWay,
        });

        return textBlock;
    }
}
