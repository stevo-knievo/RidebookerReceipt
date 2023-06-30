namespace RidebookerReceiptCli;

public interface ITemplateProvider
{
    Task<string> GetContentAsync(string name);
    Task WriteContentAsync(string name, string content);
}