using CommunityToolkit.WinUI.UI.Controls;
using Repair.Models.Extensions;

namespace Repair.Frontend.Presentation.Factory;

internal static class DataGridFactory
{
    public static DataGrid Create<TColumn>(IEnumerable<object> itemsSource, Func<TColumn, string> getBindingPath)
        where TColumn : struct, Enum
    {
        var dataGrid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            ItemsSource = itemsSource,
            Foreground = new SolidColorBrush(Colors.Black),
        };

        foreach (TColumn column in Enum.GetValues<TColumn>())
        {
            dataGrid.Columns.Add(CreateTextColumn(column.ToColumnHeader(), getBindingPath(column)));
        }

        return dataGrid;
    }

    private static DataGridTextColumn CreateTextColumn(string header, string bindingPath)
    {
        return new DataGridTextColumn
        {
            Header = header,
            Binding = new Binding
            {
                Path = new PropertyPath(bindingPath),
            },
        };
    }
}
