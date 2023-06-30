namespace RidebookerReceiptCli;

public class FileSystem : ITemplateProvider
{
    // Works with F5 execution
    private readonly string _basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../.."));

    public Task<string> GetContentAsync(string name)
    {
        var templatePath = $"{_basePath}/templates/{name}";
        return File.ReadAllTextAsync(templatePath);
    }

    public Task WriteContentAsync(string name, string content)
    {
        var receiptFilePath = $"{_basePath}/out/{name}";
        return File.WriteAllTextAsync(receiptFilePath, content);
    }
}