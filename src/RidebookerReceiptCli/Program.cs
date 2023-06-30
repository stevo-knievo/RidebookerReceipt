using Spectre.Console.Cli;

namespace RidebookerReceiptCli;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var app = new CommandApp<DefaultCommand>();
        app.Configure(config =>
        {
            config.SetApplicationName("Ridebooker Receipts CLI");
        });

        return await app.RunAsync(args).ConfigureAwait(false);
    }
}