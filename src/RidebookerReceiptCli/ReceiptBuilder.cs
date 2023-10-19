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
            .Replace(ReplaceConstance.TravelDirection, DirectionUiStringBuilder(option.TravelRoute))
            .Replace(ReplaceConstance.OtherLocations, OtherLocationsUiStringBuilder(option.TravelRoute));

        var newName = name.Replace(ReplaceConstance.OrderId, orderId);
        await _templateProvider.WriteContentAsync(newName, newContent);
    }

    private static string DirectionUiStringBuilder(TravelRoutes travelRoute)
    {
        return travelRoute switch
        {
            TravelRoutes.Whistler_to_YVR => "Whistler to Vancouver Airport (YVR)",
            TravelRoutes.YVR_to_Whistler => "Vancouver Airport (YVR) to Whistler",
            TravelRoutes.Whistler_to_Vancouver => "Whistler to Vancouver Downtown",
            TravelRoutes.Vancouver_to_Whistler => "Vancouver Downtown to Whistler",
            _ => "+++++++++++ ERROR: NO DIRECTION FOUND +++++++++++"
        };
    }

    private static string OtherLocationsUiStringBuilder(TravelRoutes travelRoute)
    {
        return travelRoute switch
        {
            TravelRoutes.Whistler_to_YVR => "&quot;Other Locations&quot; Pickup Service Per Trip",
            TravelRoutes.YVR_to_Whistler => "&quot;Other Locations&quot; Dropoff Service Per Trip",
            TravelRoutes.Whistler_to_Vancouver => "&quot;Other Locations&quot; Pickup Service Per Trip",
            TravelRoutes.Vancouver_to_Whistler => "&quot;Other Locations&quot; Dropoff Service Per Trip",
            _ => "+++++++++++ ERROR: NO OTHER LOCATIONS FOUND +++++++++++"
        };
    }
}
