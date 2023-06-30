namespace RidebookerReceiptCli;

public static class OrderIdHelper
{
    public static int GetId()
    {
        return Random.Shared.Next(376541, 999999);
    }
    
    public static int GetNextId(int currentId)
    {
        return currentId + Random.Shared.Next(9, 30);
    }
}