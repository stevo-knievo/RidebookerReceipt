namespace RidebookerReceiptCli;

public static class TransactionIdHelper
{
    public static string GetId()
    {
        return $"pl_{Guid.NewGuid().ToString().Replace("-", "")[..24]}";
    }
}