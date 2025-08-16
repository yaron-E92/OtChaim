using OtChaim.Domain.EmergencyEvents;

namespace OtChaim.Domain.Common;

/// <summary>
/// Represents a geographical area with a center location and radius.
/// </summary>
public class Area : ValueObject
{
    /// <summary>
    /// Gets the center location of the area.
    /// </summary>
    public Location Center { get; private set; } = Location.Empty;
    /// <summary>
    /// Gets the radius of the area in meters.
    /// </summary>
    public double RadiusInMeters { get; private set; }

    private Area() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="Area"/> class.
    /// </summary>
    /// <param name="center">The center location of the area.</param>
    /// <param name="radiusInMeters">The radius of the area in meters.</param>
    public Area(Location center, double radiusInMeters)
    {
        Center = center;
        RadiusInMeters = radiusInMeters;
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Center;
        yield return RadiusInMeters;
    }

    /// <summary>
    /// Creates an area from a location and optional radius and emergency type.
    /// </summary>
    public static Area FromLocation(Location location, double? radiusInMeters = null, EmergencyType? emergencyType = null)
    {
        double adjustedRadius = radiusInMeters ?? DetermineDefaultRadius(emergencyType);
        return new Area(location, adjustedRadius);
    }

    /// <summary>
    /// Determines the default radius for a given emergency type.
    /// </summary>
    private static double DetermineDefaultRadius(EmergencyType? emergencyType)
    {
        return emergencyType != null
            ? emergencyType switch
            {
                EmergencyType.NaturalDisaster or EmergencyType.CivilUnrest => 1000.0,
                EmergencyType.WeatherAlert or EmergencyType.SecurityThreat => 500.0,
                EmergencyType.InfrastructureFailure or EmergencyType.TransportationDisruption => 300.0,
                EmergencyType.UtilityOutage => 200.0,
                EmergencyType.LocalIncident => 100.0,
                EmergencyType.PersonalEmergency or EmergencyType.MedicalEmergency => 10.0,
                _ => 100.0,
            }
            : 100.0;
    }
}
