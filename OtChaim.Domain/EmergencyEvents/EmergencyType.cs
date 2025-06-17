namespace OtChaim.Domain.EmergencyEvents;

public enum EmergencyType
{
    // Natural Events
    NaturalDisaster,
    WeatherAlert,

    // Infrastructure & Utilities
    InfrastructureFailure,
    UtilityOutage,
    TransportationDisruption,

    // Social & Security
    SecurityThreat,
    CivilUnrest,

    // Personal & Health
    PersonalEmergency,
    MedicalEmergency,

    // General
    LocalIncident
}