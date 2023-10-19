using FluentAssertions;

namespace RidebookerReceiptCli.Tests;

public class TravelRoutesExtensionsTests
{
    [Fact]
    public void ShouldGetReturnRoute()
    {
        // Arrange
        const TravelRoutes route = TravelRoutes.Whistler_to_YVR;

        // Act
        var returnRoute = route.GetReturnRoute();

        // Assert
        returnRoute.Should().Be(TravelRoutes.YVR_to_Whistler);
    }
}
