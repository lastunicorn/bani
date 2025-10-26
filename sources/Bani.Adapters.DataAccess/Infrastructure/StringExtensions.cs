namespace DustInTheWind.Bani.Adapters.DataAccess.Infrastructure;

internal static class StringExtensions
{
    public static bool IsNullOrEmpty(this string value)
    {
        return value == null || value.Length == 0;
    }
}
