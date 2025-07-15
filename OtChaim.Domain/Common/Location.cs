using System.Text;

namespace OtChaim.Domain.Common;

/// <summary>
/// Represents a geographical location with latitude and longitude.
/// </summary>
public class Location : ValueObject
{
    /// <summary>
    /// Gets the latitude of the location.
    /// </summary>
    public double Latitude { get; private set; }
    /// <summary>
    /// Gets the longitude of the location.
    /// </summary>
    public double Longitude { get; private set; }
    /// <summary>
    /// Gets the description of the location.
    /// </summary>
    public string Description { get; private set; }

    private Location() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> class.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    /// <param name="description">The description of the location.</param>
    public Location(double latitude, double longitude, string description = "")
    {
        Latitude = latitude;
        Longitude = longitude;
        Description = description;
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }

    /// <summary>
    /// Returns a copy of this location.
    /// </summary>
    public Location Clone() => new(Latitude, Longitude, Description);

    public override string? ToString()
    {
        StringBuilder sb = new StringBuilder($"[{Latitude},{Longitude}]");
        if ( !string.IsNullOrWhiteSpace(Description) )
        {
            sb.Append($": {Description}");
        }
        return sb.ToString();
    }
}
