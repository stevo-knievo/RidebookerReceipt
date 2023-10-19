using System.ComponentModel;
using Spectre.Console.Cli;

namespace RidebookerReceiptCli;

public sealed class Settings : CommandSettings
{
    [CommandOption( "-d|--date")]
    [Description("The date from the transaction. Example: 2023-09-28")]
    public string? TransactionDate { get; init; }

    [CommandOption( "-h|--hour")]
    [Description("The transaction hour in 24H format from the transaction. Example: 15")]
    public int? TransactionHour { get; init; }

    [CommandOption("-t|--travel")]
    [Description("Direction")]
    public TravelDirection? TravelDirection { get; init; }

    [CommandOption("-r|--route")]
    [Description("Route")]
    public TravelRoutes? TravelRoute { get; init; }
}

