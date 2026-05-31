namespace Repair.Frontend.Presentation.Factory;

internal sealed class BooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value switch
        {
            bool boolean => boolean ? "Yes" : "No",
            var _ => value,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
