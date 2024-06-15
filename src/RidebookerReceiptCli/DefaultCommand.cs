using Spectre.Console;
using Spectre.Console.Cli;

namespace RidebookerReceiptCli;

public sealed class DefaultCommand : AsyncCommand<Settings>
{
    private readonly IAnsiConsole _console;
    private readonly IReceiptBuilder _receiptBuilder;

    public DefaultCommand(IAnsiConsole console)
    {
        _console = console;
        _receiptBuilder = new ReceiptBuilder(new FileSystem());
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        _console.Write(new FigletText("Ridebooker Receipts CLI").Color(Color.Teal));

        // User input
        var transactionDate = AskDateIfMissing(settings.TransactionDate);
        var transactionHour = AskHourIfMissing(settings.TransactionHour);
        var travelDirection = AskTravelDirectionIfMissing(settings.TravelDirection);
        var travelRoute = AskTravelRouteIfMissing(settings.TravelRoute);

        // Prepare data
        var orderId = OrderIdHelper.GetId();
        var transactionId = TransactionIdHelper.GetId();
        var transactionDateTime = DateTimeHelper.CreateDateTime(transactionDate, transactionHour);
        var emailReceivedDateTime = DateTimeHelper.GetEmailReceivedDateTime(transactionDateTime);

        var options = new List<ReceiptCreateOption>();

        if (travelDirection == TravelDirection.RoundTrip)
        {
            // First transaction
            options.Add(new ReceiptCreateOption(transactionDateTime, emailReceivedDateTime, orderId, transactionId, travelRoute));

            // Second transaction
            var secondTransactionDateTime = DateTimeHelper.AddSecondTransactionRandomnessReceivedDateTime(transactionDateTime);
            var secondEmailReceivedDateTime = DateTimeHelper.AddSecondTransactionRandomnessReceivedDateTime(emailReceivedDateTime);
            var secondTransactionId = TransactionIdHelper.GetId();
            var secondOrderId = OrderIdHelper.GetNextId(orderId);
            options.Add(new ReceiptCreateOption(
                secondTransactionDateTime,
                secondEmailReceivedDateTime,
                secondOrderId,
                secondTransactionId,
                travelRoute.GetReturnRoute()));
        }
        else
        {
            options.Add(new ReceiptCreateOption(transactionDateTime, emailReceivedDateTime, orderId, transactionId, travelRoute));
        }

        WriteSummary(options);

        var proceedWithSettings = _console.Prompt(
            new SelectionPrompt<bool> {Converter = value => value ? "Yes" : "No"}
                .Title("Proceed with the above settings?")
                .AddChoices(true, false));

        if (!proceedWithSettings)
        {
            return 1;
        }

        foreach (var option in options)
        {
            const string templateName = "[ORDER_ID] Payment Receipt.eml";
            // const string templateName = "[ORDER_ID] Payment Receipt test.txt";
            await _receiptBuilder.CreateAsync(templateName, option);
        }

        return 0;
    }

    private void WriteSummary(List<ReceiptCreateOption> options)
    {
        foreach (var option in options)
        {
            _console.Write(new Text($"Trip: {option.TravelRoute.ToString()}\n"));
            _console.Write(
                new Table()
                    .AddColumn(new TableColumn("Settings"))
                    .AddColumn(new TableColumn("Value"))
                    .AddRow("Transaction datetime", option.TransactionDateTime.ToString("F"))
                    .AddRow("Email received datetime", option.EmailReceivedDateTime.ToString("F"))
                    .AddRow("Travel direction", option.TravelRoute.ToString())
                    .AddRow("OderId", option.OrderId.ToString())
                    .AddRow("TransactionId", option.TransactionId)
            );
        }
    }

    private TravelDirection AskTravelDirectionIfMissing(TravelDirection? current) =>
        current ?? _console.Prompt(
            new SelectionPrompt<TravelDirection>()
                .Title("For which direction should the receipt be?")
                .AddChoices(
                    TravelDirection.OneWayTrip,
                    TravelDirection.RoundTrip
                    ));

    private TravelRoutes AskTravelRouteIfMissing(TravelRoutes? current) =>
        current ?? _console.Prompt(
            new SelectionPrompt<TravelRoutes>()
                .Title("For which route should the receipt be?")
                .AddChoices(
                    TravelRoutes.Whistler_to_YVR,
                    TravelRoutes.YVR_to_Whistler,
                    TravelRoutes.Whistler_to_Vancouver,
                    TravelRoutes.Vancouver_to_Whistler
                ));

    private string AskDateIfMissing(string? current)
    {
        if (!string.IsNullOrWhiteSpace(current) && DateTimeHelper.IsValidDateFormat(current))
        {
            return current;
        }

        return _console.Prompt(
            new TextPrompt<string>("What is the transaction date (Format: YYYY-MM-dd)?")
                .PromptStyle("green")
                .Validate(date => DateTimeHelper.IsValidDateFormat(date)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Not a valid date (Format: YYYY-MM-dd).")));

    }

    private int AskHourIfMissing(int? current)
    {
        if (current.HasValue && IsClockHour(current.Value))
        {
            return current.Value;
        }

        return _console.Prompt(
            new TextPrompt<int>("What is the transaction hour (24h Format)?")
                .PromptStyle("green")
                .Validate(x => IsClockHour(x)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Not a valid hour (0-23).")));

    }

    private bool IsClockHour(int hour) => hour is >= 0 and <= 23;
}
