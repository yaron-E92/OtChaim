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

    [ObservableProperty]
    private bool _isGroupSelected = true;

    [ObservableProperty]
    private bool _isSingleSelected = false;

    [ObservableProperty]
    private bool _isEmailSelected = true;

    [ObservableProperty]
    private bool _isSmsSelected = true;

    [ObservableProperty]
    private bool _isMessengerSelected = false;

    [ObservableProperty]
    private bool _attachPersonalInfo = true;

    [ObservableProperty]
    private bool _attachMedicalInfo = true;

    [ObservableProperty]
    private bool _attachGps = true;

    [ObservableProperty]
    private string _attachedPicturePath = string.Empty;

    [ObservableProperty]
    private string _attachedDocumentPath = string.Empty;

    /// <summary>
    /// Gets the available emergency types that can be selected.
    /// </summary>
    /// <remarks>
    /// This array contains all values from the EmergencyType enum, providing users
    /// with a complete list of emergency categories to choose from.
    /// </remarks>
    public EmergencyType[] EmergencyTypes { get; } = [.. Enum.GetValues(typeof(EmergencyType)).Cast<EmergencyType>()];

    public bool IsPictureAttached => !string.IsNullOrEmpty(AttachedPicturePath);
    public bool IsPersonalInfoAttached => AttachPersonalInfo;
    public bool IsMedicalInfoAttached => AttachMedicalInfo;
    public bool IsGpsAttached => AttachGps;
    public bool IsDocumentAttached => !string.IsNullOrEmpty(AttachedDocumentPath);

    protected static string GetDefaultMessage(EmergencyType emergencyType)
    {
        return emergencyType switch
        {
            EmergencyType.BloodSugarLow => "My blood sugar is dangerously low. I need immediate assistance with glucose or medical help.",
            EmergencyType.HeartAttack => "I'm experiencing chest pain and shortness of breath. Possible heart attack.",
            EmergencyType.Stroke => "I'm showing stroke symptoms. Need immediate medical attention.",
            EmergencyType.Fall => "I've fallen and cannot get up. Please send help.",
            EmergencyType.MedicalEmergency => "I'm experiencing a medical emergency and need immediate assistance.",
            EmergencyType.AllergicReaction => "I'm having a severe allergic reaction. Need emergency help.",
            EmergencyType.Seizure => "I'm having a seizure. Please call emergency services.",
            EmergencyType.BreathingDifficulty => "I'm having trouble breathing. Need immediate medical help.",
            EmergencyType.PersonalEmergency => "I'm in a personal emergency situation. Please help.",
            EmergencyType.Fire => "There's a fire in my home. Need immediate evacuation assistance.",
            EmergencyType.BreakIn => "Someone has broken into my home. Need police assistance.",
            EmergencyType.CarAccident => "I've been in a car accident. Need emergency services.",
            EmergencyType.LostOrDisoriented => "I'm lost and disoriented. Need help finding my way.",
            EmergencyType.NaturalDisaster => "Natural disaster occurring. Need emergency assistance.",
            EmergencyType.WeatherAlert => "Severe weather conditions. Need shelter assistance.",
            EmergencyType.InfrastructureFailure => "Infrastructure failure in my area. Need assistance.",
            EmergencyType.UtilityOutage => "Utility outage affecting my home. Need emergency support.",
            EmergencyType.TransportationDisruption => "Transportation disruption. Need alternative assistance.",
            EmergencyType.SecurityThreat => "Security threat in my area. Need immediate assistance.",
            EmergencyType.CivilUnrest => "Civil unrest in my area. Need evacuation assistance.",
            EmergencyType.LocalIncident => "Local incident requiring emergency assistance.",
            _ => "Emergency situation requiring immediate attention"
        };
    }

    protected void ResetCreateEmergencyFields()
    {
        SelectedEmergencyType = EmergencyTypes.FirstOrDefault();
        EmergencyMessage = GetDefaultMessage(SelectedEmergencyType);
        LocationDescription = string.Empty;
        Latitude = 0;
        Longitude = 0;
        AttachPersonalInfo = true;
        AttachMedicalInfo = true;
        AttachGps = true;
        AttachedPicturePath = string.Empty;
        AttachedDocumentPath = string.Empty;
        IsGroupSelected = true;
        IsSingleSelected = false;
        IsEmailSelected = true;
        IsSmsSelected = true;
        IsMessengerSelected = false;
    }
}
