using System.Collections.Generic;
using OtChaim.Domain.EmergencyEvents;

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

    public static Area FromLocation(Location location, double? radiusInMeters = null, EmergencyType? emergencyType = null)
    {
        double adjustedRadius = radiusInMeters ?? DetermineDefaultRadius(emergencyType);
        return new Area(location, adjustedRadius);
    }

    private static double DetermineDefaultRadius(EmergencyType? emergencyType)
        {
            if (emergencyType != null)
            {
                switch (emergencyType)
                {
                    case EmergencyType.NaturalDisaster:
                    case EmergencyType.CivilUnrest:
                        return 1000;
                    case EmergencyType.WeatherAlert:
                    case EmergencyType.SecurityThreat:
                        return 500;
                    case EmergencyType.InfrastructureFailure:
                    case EmergencyType.TransportationDisruption:
                        return 300;
                    case EmergencyType.UtilityOutage:
                        return 200;
                    case EmergencyType.LocalIncident:
                        return 100;
                    case EmergencyType.PersonalEmergency:
                    case EmergencyType.MedicalEmergency:
                        return 10;
                    default:
                        return 100;
                }
            }
            return 100;
        }
}