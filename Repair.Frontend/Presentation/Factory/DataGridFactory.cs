using System.Collections;
using System.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
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
            Foreground = new SolidColorBrush(Colors.Black),
        };

        foreach (TColumn column in Enum.GetValues<TColumn>())
        {
            string header = column.ToColumnHeader();
            string bindingPath = getBindingPath(column);
            Type columnType = getColumnType(column);

            dataGrid.Columns.Add(CreateColumn(header, bindingPath, columnType));
        }

        return dataGrid;
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
