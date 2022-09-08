namespace Utility.Extensions;

public static class DateTimeExtensions
{
    public static DateTime YearMonthIntToDate(int yearMonthTime)
    {
        var year = yearMonthTime / 100;
        var month = yearMonthTime - (year * 100);
        var yearMonthDate = new DateTime(year, month, 1);
        return yearMonthDate;
    }

    public static int YearMonthIntFromDate(DateTime targetDate)
    {
        return targetDate.Year * 100 + targetDate.Month;
    }

    public static DateTime TryConvertToDateTime(this string dateTime)
    {
        DateTime.TryParse(dateTime, out var result);
        return result;
    }

    public static DateTime FirstDayOfWeek(this DateTime dt)
    {
        try
        {
            var diff = (7 + (dt.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        catch (Exception)
        {
            return DateTime.MinValue;
        }
    }

    public static DateTime LastDayOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year,dt.Month,1).AddMonths(1).AddDays(-1);
    }

    public static DateTime FirstDayOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, 1);
    }

    public static DateTime LatestDate(this DateTime dt, DateTime datetime2)
    {
        return dt >= datetime2 ? dt : datetime2;
    }


    public static DateTime AddWeeks(this DateTime dt, int weeks)
    {
        return dt.AddDays(7 * weeks);
    }

    public static string GetDifferenceString(this DateTime dt, DateTime datetime2)
    {
        var diff = datetime2 - dt;
        if (diff.Days > 365)
        {
            return $"{(int) (diff.Days / 365.25)} años";
        }

        return diff.Days > 30 ? 
            $"{(int) (diff.Days / 30.4167)} meses" : 
            $"{diff.Days} días";
    }
        
    public static DateTime? GetDateOrNull(this List<DateTime?> dates, int index)
    {
        if (dates.Count <= index)
        {
            return null;
        }

        return dates[index]?.Date;
    }

    public static int GetDifference(this DateTime dt, DateTime datetime2)
    {
        var diff = datetime2 - dt;
        return diff.Days;
    }
}