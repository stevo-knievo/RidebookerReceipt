namespace RidebookerReceiptCli.Tests;

public class InMemoryTemplateProvider : ITemplateProvider
{
    private readonly Action<string> _onContentWritten;

    private const string ContentTemplate = """
                                           OrderId: ORDER_ID
                                           TransactionId: TRANSACTION_ID
                                           TransactionDateTime: TRANSACTION_DATETIME_LOCAL
                                           Direction: TRAVEL_DIRECTION
                                           EmailMetaDataDateTimeLocal: EMAIL_META_DATA_DATETIME_LOCAL
                                           EmailMetaDataDateTimeUtc: EMAIL_META_DATA_DATETIME_UTC
                                           Other Locations: OTHER_LOCATIONS
                                           """;

    public InMemoryTemplateProvider(Action<string> onContentWritten)
    {
        _onContentWritten = onContentWritten;
    }

    public Task<string> GetContentAsync(string name) => Task.FromResult(ContentTemplate);

    public Task WriteContentAsync(string name, string content)
    {
        _onContentWritten(content);
        return Task.CompletedTask;
    }
}
