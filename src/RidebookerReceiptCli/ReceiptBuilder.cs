namespace RidebookerReceiptCli;

public class ReceiptBuilder : IReceiptBuilder
{
    private readonly ITemplateProvider _templateProvider;

    public ReceiptBuilder(ITemplateProvider templateProvider)
    {
        _templateProvider = templateProvider;
    }

    public async Task CreateAsync(string name, ReceiptCreateOption option)
    {
        var content = await _templateProvider.GetContentAsync(name);
        var emailMetaDateTime = option.EmailReceivedDateTime;
        var orderId = option.OrderId.ToString();

        var newContent = content
            .Replace(ReplaceConstance.EmailMetaDataDateTimeLocal, DateTimeHelper.ToEmailFormatLocal(emailMetaDateTime))
            .Replace(ReplaceConstance.EmailMetaDataDateTimeUtc, DateTimeHelper.ToEmailFormatUtc(emailMetaDateTime))
            .Replace(ReplaceConstance.OrderId, orderId)
            .Replace(ReplaceConstance.TransactionId, option.TransactionId)
            .Replace(ReplaceConstance.TransactionDateTimeLocal, DateTimeHelper.ToUiFormat(option.TransactionDateTime))
            .Replace(ReplaceConstance.TravelDirection, DirectionUiStringBuilder(option.TravelDirection))
            .Replace(ReplaceConstance.OtherLocations, OtherLocationsUiStringBuilder(option.TravelDirection));

        var newName = name.Replace(ReplaceConstance.OrderId, orderId);
        await _templateProvider.WriteContentAsync(newName, newContent);
    }

    private static string DirectionUiStringBuilder(TravelDirection travelDirection)
    {
        return travelDirection switch
        {
            TravelDirection.Whistler_to_YVR => "Whistler to Vancouver Airport (YVR)",
            TravelDirection.YVR_to_Whistler => "Vancouver Airport (YVR) to Whistler",
            _ => "+++++++++++ ERROR: NO DIRECTION FOUND +++++++++++"
        };
    }

    private static string OtherLocationsUiStringBuilder(TravelDirection travelDirection)
    {
        return travelDirection switch
        {
            TravelDirection.Whistler_to_YVR => "&quot;Other Locations&quot; Pickup Service Per Trip",
            TravelDirection.YVR_to_Whistler => "&quot;Other Locations&quot; Dropoff Service Per Trip",
            _ => "+++++++++++ ERROR: NO OTHER LOCATIONS FOUND +++++++++++"
        };
    }
}
