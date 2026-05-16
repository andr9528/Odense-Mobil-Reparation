namespace Repair.Models.Extensions;

public static class StringExtensions
{
    public static string ScreamingSnakeCaseToTitleCase(this string input)
    {
        return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(string.Join(' ', input.Split('_')).ToLower());
    }

    public static string ToColumnHeader<TColumn>(this TColumn column) where TColumn : Enum
    {
        return column.ToString().ScreamingSnakeCaseToTitleCase();
    }
}
