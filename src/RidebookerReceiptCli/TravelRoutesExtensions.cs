namespace RidebookerReceiptCli;

public static class TravelRoutesExtensions
{
    public static TravelRoutes GetReturnRoute(this TravelRoutes travelRoutes)
    {
        var theRoute = string.Join('_', travelRoutes.ToString().Split('_').Reverse());

        if (!Enum.TryParse(theRoute, out TravelRoutes returnRoute))
        {
            throw new ArgumentException("Could not find the return route.");
        }

        return returnRoute;
    }
}
