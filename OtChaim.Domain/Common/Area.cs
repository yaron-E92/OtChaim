using System.Collections.Generic;

namespace OtChaim.Domain.Common;

public class Area : ValueObject
{
    public Location Center { get; }
    public double RadiusInMeters { get; }

    public Area(Location center, double radiusInMeters)
    {
        Center = center;
        RadiusInMeters = radiusInMeters;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Center;
        yield return RadiusInMeters;
    }
}