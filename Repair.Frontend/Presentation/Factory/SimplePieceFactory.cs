using Repair.Frontend.Extensions;

namespace Repair.Frontend.Presentation.Factory;

internal static class SimplePieceFactory
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

        fuzzySearchCheckBox = CheckBoxFactory.CreateLightCheckBox(useFuzzySearchPath);

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

    public static Grid CreateSaveCancelButtonGrid(
        Func<object, RoutedEventArgs, Task> saveClicked, RoutedEventHandler cancelClicked)
    {
        Grid grid = GridFactory.CreateDefaultGrid();

        grid.HorizontalAlignment = HorizontalAlignment.Right;
        grid.VerticalAlignment = VerticalAlignment.Center;
        grid.ColumnSpacing = 8;
        grid.DefineColumns(new GridLength(1, GridUnitType.Star), GridLength.Auto, GridLength.Auto);

        Button saveButton = CreateSaveButton(saveClicked);
        Button cancelButton = CreateCancelButton(cancelClicked);

        grid.Children.Add(saveButton.SetColumn(1));
        grid.Children.Add(cancelButton.SetColumn(2));

        return grid;
    }

    public static Button CreateSaveButton(Func<object, RoutedEventArgs, Task> saveClicked)
    {
        Button saveButton = new()
        {
            Content = "Save",
            Padding = new Thickness(20, 8, 20, 8),
        };

        saveButton.Click += async (sender, args) => await saveClicked(sender, args);

        return saveButton;
    }

    public static Button CreateCancelButton(RoutedEventHandler cancelClicked)
    {
        Button cancelButton = new()
        {
            Content = "Cancel",
            Padding = new Thickness(20, 8, 20, 8),
        };

        cancelButton.Click += cancelClicked;

        return cancelButton;
    }

    public static Grid CreateLeftButtonGrid(string buttonText, RoutedEventHandler clicked)
    {
        Grid grid = GridFactory.CreateDefaultGrid()
            .DefineColumns(GridLength.Auto, new GridLength(1, GridUnitType.Star));

        Button button = new()
        {
            Content = buttonText,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        button.Click += clicked;

        grid.Children.Add(button.SetColumn(0));

        return grid;
    }
}
