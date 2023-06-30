namespace RidebookerReceiptCli;

public interface IReceiptBuilder
{
    Task CreateAsync(string name, ReceiptCreateOption option);
}