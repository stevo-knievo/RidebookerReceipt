using System.Security.Cryptography;

namespace RidebookerReceiptCli;

public static class DateTimeHelper
{
    public const string DefaultTimeZoneId = "America/Vancouver";
    
    public static string ToUiFormat(DateTimeOffset dateTime)
    {
        var dateTimeString = dateTime.ToString("dd MMM yyyy, h:mm tt (HH:mm)");
        var tzAbbreviation = GetTzAbbreviation(dateTime.Offset);
        return $"{dateTimeString} {tzAbbreviation}";
    }

    // Wed, 18 May 2022 23:27:54 +0000 (UTC)
    public static string ToEmailFormatUtc(DateTimeOffset localDateTime)
    {
        var dateTimeUtc = localDateTime.ToUniversalTime();
        var formattedDateTime =  dateTimeUtc.ToString("ddd, dd MMM yyyy HH:mm:ss");

        return $"{formattedDateTime} +0000 (UTC)";
    }
    
    // Wed, 18 May 2022 16:27:54 -0700 (PDT)
    // Sun, 02 Dec 2018 16:28:00 -0800 (PST)
    public static string ToEmailFormatLocal(DateTimeOffset localDateTime)
    {
        var offset = localDateTime.Offset;
        var formattedDateTime =  localDateTime.ToString("ddd, dd MMM yyyy HH:mm:ss");
        // TODO: How do I get PDT and PST??
        var tzAbbreviation = GetTzAbbreviation(offset);
        
        // TODO: How do I get -0700 and/or +0400
        return $"{formattedDateTime} {offset.ToString()[..3]}00 ({tzAbbreviation})";
    }

    public static DateTimeOffset GetEmailReceivedDateTime(DateTimeOffset dateTime)
    {
        var randomAdditionalMinutes = Random.Shared.Next(6, 30);
        var randomAdditionalSeconds = Random.Shared.Next(5, 59);
        return dateTime.AddMinutes(randomAdditionalMinutes).AddSeconds(randomAdditionalSeconds);
    }
    
    public static DateTimeOffset AddSecondTransactionRandomnessReceivedDateTime(DateTimeOffset dateTime)
    {
        var randomAdditionalMinutes = Random.Shared.Next(5, 10);
        var randomAdditionalSeconds = Random.Shared.Next(5, 59);
        return dateTime.AddMinutes(randomAdditionalMinutes).AddSeconds(randomAdditionalSeconds);
    }

    public static bool IsValidDateFormat(string dateString)
    {
        const string format = "yyyy-MM-dd";
        return DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out _);
    }

    public static DateTimeOffset CreateDateTime(string transactionDate, int transactionHour)
    {
        var timeString = CreateTimeString(transactionHour);
        var dateTimeString = $"{transactionDate} {timeString}";
        return ConvertToDateTimeOffset(dateTimeString);
    }
    
    public static DateTimeOffset ConvertToDateTimeOffset(string dateString, string timeZoneId = DefaultTimeZoneId)
    {
        var dateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", null);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var offset = timeZone.GetUtcOffset(dateTime);
        var dateTimeOffset = new DateTimeOffset(dateTime, offset);
        return dateTimeOffset;
    }

    private static string CreateTimeString(int transactionHour)
    {
        var minute = Random.Shared.Next(0, 60);
        var second = Random.Shared.Next(0, 60);
        return $"{transactionHour:D2}:{minute:D2}:{second:D2}";
    }
    
    private static string GetTzAbbreviation(TimeSpan offset) => offset.Hours == -7 ? "PDT" : "PST";
}