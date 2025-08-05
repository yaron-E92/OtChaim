namespace OtChaim.Domain.EmergencyEvents;

/// <summary>
/// Represents the type of an emergency event.
/// </summary>
public enum EmergencyType
{
    // Medical Emergencies
    BloodSugarLow,
    HeartAttack,
    Stroke,
    Fall,
    MedicalEmergency,
    AllergicReaction,
    Seizure,
    BreathingDifficulty,
    
    // Personal Emergencies
    PersonalEmergency,
    Fire,
    BreakIn,
    CarAccident,
    LostOrDisoriented,
    
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

    // General
    LocalIncident
}
