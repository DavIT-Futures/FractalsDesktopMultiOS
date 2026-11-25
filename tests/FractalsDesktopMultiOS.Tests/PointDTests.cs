using Xunit;

namespace FractalsDesktopMultiOS.Tests;

public class PointDTests
{
    [Fact]
    public void Midpoint_MixedSignCoordinates_ReturnsExactCenterPoint()
    {
        var start = new PointD(-4.5, 10.0);
        var end = new PointD(2.5, -2.0);

        PointD midpoint = start.Midpoint(end);

        Assert.Equal(-1.0, midpoint.X, precision: 10);
        Assert.Equal(4.0, midpoint.Y, precision: 10);
    }
}
