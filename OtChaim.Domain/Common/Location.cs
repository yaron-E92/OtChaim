namespace OtChaim.Domain.Common;

public class Location : ValueObject
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string Description { get; private set; }

    private Location() { } // For EF Core

    public Location(double latitude, double longitude, string description = "")
    {
        Latitude = latitude;
        Longitude = longitude;
        Description = description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }

    public Location Clone() => new(Latitude, Longitude, Description);
}
