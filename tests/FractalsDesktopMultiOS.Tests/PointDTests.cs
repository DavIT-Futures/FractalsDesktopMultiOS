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

    [Fact]
    public void Midpoint_IsCommutativeAndIdempotent()
    {
        var first = new PointD(-1.25, 3.75);
        var second = new PointD(4.75, -0.25);

        PointD forward = first.Midpoint(second);
        PointD reverse = second.Midpoint(first);
        PointD self = first.Midpoint(first);

        Assert.Equal(forward.X, reverse.X, precision: 10);
        Assert.Equal(forward.Y, reverse.Y, precision: 10);
        Assert.Equal(first.X, self.X, precision: 10);
        Assert.Equal(first.Y, self.Y, precision: 10);
    }
}
