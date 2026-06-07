using System.Collections;
using System.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Converters;
using Repair.Models.Extensions;
using BooleanConverter = Repair.Frontend.Presentation.Converters.BooleanConverter;

namespace Repair.Frontend.Presentation.Factory;

internal static class DataGridFactory
{
    public static DataGrid Create<TColumn>(
        IEnumerable<object> itemsSource, Func<TColumn, string> getBindingPath, Func<TColumn, Type> getColumnType)
        where TColumn : struct, Enum
    {
        var dataGrid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            ItemsSource = itemsSource,
            CanUserSortColumns = true,
            Foreground = new SolidColorBrush(Colors.Black),
        };

        dataGrid.Sorting += SortColumnClicked;

        foreach (TColumn column in Enum.GetValues<TColumn>())
        {
            string header = column.ToColumnHeader();
            string bindingPath = getBindingPath(column);
            Type columnType = getColumnType(column);

            dataGrid.Columns.Add(CreateColumn(header, bindingPath, columnType));
        }

        return dataGrid;
    }

    private static void SortColumnClicked(object? sender, DataGridColumnEventArgs e)
    {
        if (sender is not DataGrid dataGrid)
        {
            return;
        }

        e.Column.SortDirection = e.Column.SortDirection switch
        {
            null => DataGridSortDirection.Ascending,
            DataGridSortDirection.Ascending => DataGridSortDirection.Descending,
            var _ => null,
        };

        foreach (DataGridColumn column in dataGrid.Columns)
        {
            if (column != e.Column)
            {
                column.SortDirection = null;
            }
        }

        dataGrid.ApplyCurrentSort();
    }

    private static void ApplyCurrentSort(this DataGrid dataGrid)
    {
        if (dataGrid.ItemsSource is not IEnumerable<object> items || dataGrid.ItemsSource is not IList list)
        {
            return;
        }

        List<object> sortedItems = dataGrid.ApplyCurrentSort(items).ToList();

        list.ReplaceItems(sortedItems);
    }

    public static IEnumerable<T> ApplyCurrentSort<T>(this DataGrid dataGrid, IEnumerable<T> items)
    {
        DataGridColumn? sortedColumn = dataGrid.Columns.FirstOrDefault(x => x.SortDirection is not null);

        if (sortedColumn?.Tag is not string bindingPath || sortedColumn.SortDirection is null)
        {
            return items;
        }

        return sortedColumn.SortDirection switch
        {
            DataGridSortDirection.Ascending => items.OrderBy(x => GetPropertyValue(x!, bindingPath)),

            DataGridSortDirection.Descending => items.OrderByDescending(x => GetPropertyValue(x!, bindingPath)),

            var _ => items,
        };
    }

    private static object? GetPropertyValue(object item, string propertyPath)
    {
        object? currentValue = item;

        foreach (string propertyName in propertyPath.Split('.'))
        {
            if (currentValue is null)
            {
                return null;
            }

            currentValue = currentValue.GetType().GetProperty(propertyName)?.GetValue(currentValue);
        }

        return currentValue;
    }

    private static DataGridColumn CreateColumn(string header, string bindingPath, Type columnType)
    {
        if (columnType == typeof(DateTime?))
        {
            return CreateNullableDateTimeColumn(header, bindingPath);
        }

        if (columnType == typeof(bool))
        {
            return CreateBooleanColumn(header, bindingPath);
        }

        return CreateTextColumn(header, bindingPath);
    }

    private static DataGridTextColumn CreateTextColumn(
        string header, string bindingPath, IValueConverter? converter = null)
    {
        return new DataGridTextColumn
        {
            Header = header,
            MaxWidth = 500,
            CanUserSort = true,
            Tag = bindingPath,
            Binding = new Binding
            {
                Path = new PropertyPath(bindingPath),
                Converter = converter,
            },
        };
    }

    private static DataGridTextColumn CreateNullableDateTimeColumn(string header, string bindingPath)
    {
        return CreateTextColumn(header, bindingPath, new NullableDateTimeConverter());
    }

    private static DataGridTextColumn CreateBooleanColumn(string header, string bindingPath)
    {
        return CreateTextColumn(header, bindingPath, new BooleanConverter());
    }

    public static void Refresh(this DataGrid dataGrid)
    {
        IEnumerable? source = dataGrid.ItemsSource;

        dataGrid.ItemsSource = null;
        dataGrid.ItemsSource = source;
    }
}
