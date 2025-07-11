using OtChaim.Domain.EmergencyEvents;

namespace OtChaim.Domain.Common;

public class Area : ValueObject
{
    public Location Center { get; private set; }
    public double RadiusInMeters { get; private set; }

    private Area() { } // For EF Core

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

    public static Area FromLocation(Location location, double? radiusInMeters = null, EmergencyType? emergencyType = null)
    {
        double adjustedRadius = radiusInMeters ?? DetermineDefaultRadius(emergencyType);
        return new Area(location, adjustedRadius);
    }

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
