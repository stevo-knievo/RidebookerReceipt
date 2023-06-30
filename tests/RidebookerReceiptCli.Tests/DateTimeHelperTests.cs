using FluentAssertions;

namespace RidebookerReceiptCli.Tests;

public class DateTimeHelperTests
{
    [Theory]
    [InlineData("2022-05-18 16:27:03", "18 May 2022, 4:27 PM (16:27) PDT")]
    [InlineData("2018-12-02 16:28:10", "02 Dec 2018, 4:28 PM (16:28) PST")]
    public void ToUiFormatTest(string dateTimeString, string expectedDateTimeFormattedString)
    {
        var dateTimeLocal = DateTimeHelper.ConvertToDateTimeOffset(dateTimeString);
        DateTimeHelper.ToUiFormat(dateTimeLocal).Should().Be(expectedDateTimeFormattedString);
    }
    
    [Theory]
    [InlineData("2022-05-18 16:27:03", "Wed, 18 May 2022 16:27:03 -0700 (PDT)")]
    [InlineData("2018-12-02 16:28:10", "Sun, 02 Dec 2018 16:28:10 -0800 (PST)")]
    public void ToEmailFormatLocalTest(string dateTimeString, string expectedDateTimeFormattedString)
    {
        var dateTimeLocal = DateTimeHelper.ConvertToDateTimeOffset(dateTimeString);
        DateTimeHelper.ToEmailFormatLocal(dateTimeLocal).Should().Be(expectedDateTimeFormattedString);
    }
    
    [Theory]
    [InlineData("2022-05-18 16:27:03", "Wed, 18 May 2022 23:27:03 +0000 (UTC)")]
    [InlineData("2018-12-02 16:28:10", "Mon, 03 Dec 2018 00:28:10 +0000 (UTC)")]
    public void ToEmailFormatUtcTest(string dateTimeString, string expectedDateTimeFormattedString)
    {
        var dateTimeLocal = DateTimeHelper.ConvertToDateTimeOffset(dateTimeString);
        DateTimeHelper.ToEmailFormatUtc(dateTimeLocal.ToUniversalTime()).Should().Be(expectedDateTimeFormattedString);
    }

    [Theory]
    [InlineData("2023-09-26", true)]
    [InlineData("2023-26-09", false)]
    [InlineData("2023-9-6", false)]
    public void IsValidDateFormatTest(string dateString, bool isValidDateFormat)
    {
        // Todo create PT TZ date -> NodeTime?
        DateTimeHelper.IsValidDateFormat(dateString).Should().Be(isValidDateFormat);
    }
}