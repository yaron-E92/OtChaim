using CommunityToolkit.Mvvm.ComponentModel;
using OtChaim.Domain.EmergencyEvents;

namespace OtChaim.Application.ViewModels;

public abstract partial class BaseEmergencyViewModel : ObservableObject
{
    [ObservableProperty]
    private EmergencyType _selectedEmergencyType = EmergencyType.BloodSugarLow;

    [ObservableProperty]
    private string _emergencyMessage = string.Empty;

    [ObservableProperty]
    private string _locationDescription = string.Empty;

    [ObservableProperty]
    private double _latitude;

    [ObservableProperty]
    private double _longitude;

    /// <summary>
    /// Gets or sets a value indicating whether the ViewModel is currently loading.
    /// </summary>
    /// <remarks>
    /// This property is used to show loading indicators in the UI during
    /// emergency creation operations.
    /// </remarks>
    [ObservableProperty]
    private bool _isLoading;

    private readonly Dictionary<EmergencyType, string> _emergencyTypeMessages = new()
    {
        { EmergencyType.BloodSugarLow, "My blood sugar is dangerously low. I need immediate assistance with glucose or medical help." },
        { EmergencyType.HeartAttack, "I'm experiencing chest pain and shortness of breath. Possible heart attack." },
        { EmergencyType.Stroke, "I'm showing stroke symptoms. Need immediate medical attention." },
        { EmergencyType.Fall, "I've fallen and cannot get up. Please send help." },
        { EmergencyType.MedicalEmergency, "I'm experiencing a medical emergency and need immediate assistance." },
        { EmergencyType.AllergicReaction, "I'm having a severe allergic reaction. Need emergency help." },
        { EmergencyType.Seizure, "I'm having a seizure. Please call emergency services." },
        { EmergencyType.BreathingDifficulty, "I'm having trouble breathing. Need immediate medical help." },
        { EmergencyType.PersonalEmergency, "I'm in a personal emergency situation. Please help." },
        { EmergencyType.Fire, "There's a fire in my home. Need immediate evacuation assistance." },
        { EmergencyType.BreakIn, "Someone has broken into my home. Need police assistance." },
        { EmergencyType.CarAccident, "I've been in a car accident. Need emergency services." },
        { EmergencyType.LostOrDisoriented, "I'm lost and disoriented. Need help finding my way." },
        { EmergencyType.NaturalDisaster, "Natural disaster occurring. Need emergency assistance." },
        { EmergencyType.WeatherAlert, "Severe weather conditions. Need shelter assistance." },
        { EmergencyType.InfrastructureFailure, "Infrastructure failure in my area. Need assistance." },
        { EmergencyType.UtilityOutage, "Utility outage affecting my home. Need emergency support." },
        { EmergencyType.TransportationDisruption, "Transportation disruption. Need alternative assistance." },
        { EmergencyType.SecurityThreat, "Security threat in my area. Need immediate assistance." },
        { EmergencyType.CivilUnrest, "Civil unrest in my area. Need evacuation assistance." },
        { EmergencyType.LocalIncident, "Local incident requiring emergency assistance." }
    };

    /// <summary>
    /// Gets the available emergency types that can be selected.
    /// </summary>
    /// <remarks>
    /// This array contains all values from the EmergencyType enum, providing users
    /// with a complete list of emergency categories to choose from.
    /// </remarks>
    public EmergencyType[] EmergencyTypes { get; } = [.. Enum.GetValues(typeof(EmergencyType)).Cast<EmergencyType>()];

    /// <summary>
    /// Gets a default message based on the selected emergency type.
    /// </summary>
    /// <param name="emergencyType">The type of emergency to get a default message for.</param>
    /// <returns>A default message string appropriate for the emergency type.</returns>
    /// <remarks>
    /// This method provides contextually appropriate default messages for different
    /// emergency types, ensuring that even if users don't provide a custom message,
    /// the emergency notification will still be meaningful and informative.
    /// </remarks>
    protected string GetDefaultMessage(EmergencyType emergencyType)
    {
        _emergencyTypeMessages.TryGetValue(emergencyType, out string? message);
        return message is not null ? message : "";
    }
}
