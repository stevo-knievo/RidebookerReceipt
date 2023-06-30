namespace RidebookerReceiptCli;

public record ReceiptCreateOption(
    DateTimeOffset TransactionDateTime,
    DateTimeOffset EmailReceivedDateTime,
    int OrderId,
    string TransactionId,
    TravelDirection TravelDirection);