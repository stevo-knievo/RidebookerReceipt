using FluentAssertions;

namespace RidebookerReceiptCli.Tests;

public class ReceiptBuilderTests
{
    [Fact]
    public async Task CreationTest()
    {
        // Arrange
        const int orderId = 123456;
        const string transactionId = "123456789ABCD";
        var transactionDateTime = DateTimeHelper.ConvertToDateTimeOffset("2023-01-15 13:13:10");
        var emailReceivedDateTime = DateTimeHelper.ConvertToDateTimeOffset("2023-01-15 13:23:10");
        var option = new ReceiptCreateOption(transactionDateTime, emailReceivedDateTime, orderId, transactionId, TravelRoutes.Whistler_to_YVR);
        var receiptBuilder = new ReceiptBuilder(new InMemoryTemplateProvider(s => AssertOnContentWritten(s, transactionDateTime, emailReceivedDateTime)));

        // Act
        await receiptBuilder.CreateAsync("dummy", option);
        return;

        // Assert
        void AssertOnContentWritten(string content, DateTimeOffset theTransactionDateTime, DateTimeOffset theEmailReceivedDateTime)
        {
            // Test if place holder replaced with values
            content.Should().NotContain(ReplaceConstance.OrderId);
            content.Should().NotContain(ReplaceConstance.TransactionId);
            content.Should().NotContain(ReplaceConstance.TransactionDateTimeLocal);
            content.Should().NotContain(ReplaceConstance.EmailMetaDataDateTimeLocal);
            content.Should().NotContain(ReplaceConstance.EmailMetaDataDateTimeUtc);
            content.Should().NotContain(ReplaceConstance.TravelDirection);
            content.Should().NotContain(ReplaceConstance.OtherLocations);

            // Test if proper replaced
            content.Should().Contain(orderId.ToString());
            content.Should().Contain(transactionId);
            content.Should().Contain(DateTimeHelper.ToUiFormat(theTransactionDateTime));
            content.Should().Contain(DateTimeHelper.ToEmailFormatLocal(theEmailReceivedDateTime));
            content.Should().Contain(DateTimeHelper.ToEmailFormatUtc(theEmailReceivedDateTime.ToUniversalTime()));
            content.Should().Contain("Whistler");
            content.Should().Contain("YVR");
        }
    }
}
