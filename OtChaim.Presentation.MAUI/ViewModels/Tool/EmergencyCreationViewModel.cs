using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Application.Common;
using System.Collections.ObjectModel;
using Location = OtChaim.Domain.Common.Location;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

public partial class EmergencyCreationViewModel : ObservableObject
{
    private readonly ICommandHandler<StartEmergency> _startEmergencyHandler;

    [ObservableProperty]
    private EmergencyType _selectedEmergencyType = EmergencyType.BloodSugarLow;

    [ObservableProperty]
    private string _emergencyMessage = "I need immediate assistance. Please help.";

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _locationDescription = "Current Location";

    [ObservableProperty]
    private double _latitude = 0;

    [ObservableProperty]
    private double _longitude = 0;

    // Contact selection properties
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

    // Attachment properties
    [ObservableProperty]
    private bool _isPictureAttached = false;

    [ObservableProperty]
    private bool _isPersonalInfoAttached = false;

    [ObservableProperty]
    private bool _isMedicalInfoAttached = false;

    [ObservableProperty]
    private bool _isGpsAttached = true;

    [ObservableProperty]
    private bool _isDocumentAttached = false;

    // Message templates
    [ObservableProperty]
    private ObservableCollection<string> _messageTemplates;

    [ObservableProperty]
    private int _selectedMessageTemplateIndex = -1;

    // Events for popup management
    public event EventHandler? EmergencyCreated;
    public event EventHandler? Cancelled;

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

    public EmergencyCreationViewModel(ICommandHandler<StartEmergency> startEmergencyHandler)
    {
        _startEmergencyHandler = startEmergencyHandler;
        
        // Initialize message templates
        _messageTemplates = new ObservableCollection<string>
        {
            "I need immediate assistance. Please help.",
            "Medical emergency - please call 911.",
            "I've fallen and cannot get up.",
            "I'm having trouble breathing.",
            "I'm lost and need help finding my way.",
            "There's a fire in my home.",
            "Someone has broken into my home.",
            "I've been in an accident.",
            "I'm experiencing severe pain.",
            "I need emergency medical attention."
        };
    }

    [RelayCommand]
    private void PreviousEmergencyType()
    {
        var values = Enum.GetValues<EmergencyType>();
        var currentIndex = Array.IndexOf(values, SelectedEmergencyType);
        var previousIndex = currentIndex - 1;
        
        if (previousIndex < 0)
            previousIndex = values.Length - 1;
        
        SelectedEmergencyType = values[previousIndex];
        UpdateEmergencyMessage();
    }

    [RelayCommand]
    private void NextEmergencyType()
    {
        var values = Enum.GetValues<EmergencyType>();
        var currentIndex = Array.IndexOf(values, SelectedEmergencyType);
        var nextIndex = currentIndex + 1;
        
        if (nextIndex >= values.Length)
            nextIndex = 0;
        
        SelectedEmergencyType = values[nextIndex];
        UpdateEmergencyMessage();
    }

    partial void OnSelectedEmergencyTypeChanged(EmergencyType value)
    {
        UpdateEmergencyMessage();
    }

    private void UpdateEmergencyMessage()
    {
        if (_emergencyTypeMessages.TryGetValue(SelectedEmergencyType, out var message))
        {
            EmergencyMessage = message;
        }
    }

    [RelayCommand]
    private async Task AddPicture()
    {
        try
        {
            if (IsPictureAttached)
            {
                IsPictureAttached = false;
                await Shell.Current.DisplayAlert("Removed", "Picture removed", "OK");
            }
            else
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    IsPictureAttached = true;
                    await Shell.Current.DisplayAlert("Success", "Picture attached successfully", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to attach picture: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task AddPersonalInfo()
    {
        try
        {
            if (IsPersonalInfoAttached)
            {
                IsPersonalInfoAttached = false;
                await Shell.Current.DisplayAlert("Removed", "Personal information removed", "OK");
            }
            else
            {
                IsPersonalInfoAttached = true;
                await Shell.Current.DisplayAlert("Success", "Personal information attached", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to attach personal info: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task AddMedicalInfo()
    {
        try
        {
            if (IsMedicalInfoAttached)
            {
                IsMedicalInfoAttached = false;
                await Shell.Current.DisplayAlert("Removed", "Medical information removed", "OK");
            }
            else
            {
                IsMedicalInfoAttached = true;
                await Shell.Current.DisplayAlert("Success", "Medical information attached", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to attach medical info: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task AddGps()
    {
        try
        {
            if (IsGpsAttached)
            {
                IsGpsAttached = false;
                await Shell.Current.DisplayAlert("Removed", "GPS location removed", "OK");
            }
            else
            {
                // In a real implementation, this would get current GPS location
                IsGpsAttached = true;
                await Shell.Current.DisplayAlert("Success", "GPS location attached", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to attach GPS: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task AddDocument()
    {
        try
        {
            if (IsDocumentAttached)
            {
                IsDocumentAttached = false;
                await Shell.Current.DisplayAlert("Removed", "Document removed", "OK");
            }
            else
            {
                var document = await FilePicker.PickAsync();
                if (document != null)
                {
                    IsDocumentAttached = true;
                    await Shell.Current.DisplayAlert("Success", $"Document '{document.FileName}' attached successfully", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to attach document: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private void AddMessage()
    {
        if (SelectedMessageTemplateIndex >= 0 && SelectedMessageTemplateIndex < MessageTemplates.Count)
        {
            EmergencyMessage = MessageTemplates[SelectedMessageTemplateIndex];
        }
    }

    [RelayCommand]
    private void RemoveMessage()
    {
        EmergencyMessage = "";
    }

    [RelayCommand]
    private async Task CreateEmergency()
    {
        try
        {
            IsLoading = true;

            // Create location
            var location = new Location(Latitude, Longitude, LocationDescription);
            var affectedAreas = new List<Area> { Area.FromLocation(location, emergencyType: SelectedEmergencyType) };

            // Create attachments
            var attachments = new EmergencyAttachments(
                hasPicture: IsPictureAttached,
                hasPersonalInfo: IsPersonalInfoAttached,
                hasMedicalInfo: IsMedicalInfoAttached,
                hasGpsLocation: IsGpsAttached,
                hasDocument: IsDocumentAttached
            );

            // Create the emergency command
            var command = new StartEmergency(
                initiatorUserId: Guid.NewGuid(), // In real implementation, get current user ID
                type: SelectedEmergencyType,
                location: location,
                affectedAreas: affectedAreas,
                severity: Severity.High,
                description: EmergencyMessage,
                attachments: attachments
            );

            // Execute the command
            await _startEmergencyHandler.Handle(command, CancellationToken.None);

            await Shell.Current.DisplayAlert("Emergency Created", 
                $"Emergency of type {SelectedEmergencyType} has been created successfully. Help is on the way.", "OK");

            // Notify that emergency was created
            EmergencyCreated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to create emergency: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        Cancelled?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void ToggleGroup()
    {
        IsGroupSelected = !IsGroupSelected;
        if (IsGroupSelected)
            IsSingleSelected = false;
    }

    [RelayCommand]
    private void ToggleSingle()
    {
        IsSingleSelected = !IsSingleSelected;
        if (IsSingleSelected)
            IsGroupSelected = false;
    }

    [RelayCommand]
    private void ToggleEmail()
    {
        IsEmailSelected = !IsEmailSelected;
    }

    [RelayCommand]
    private void ToggleSms()
    {
        IsSmsSelected = !IsSmsSelected;
    }

    [RelayCommand]
    private void ToggleMessenger()
    {
        IsMessengerSelected = !IsMessengerSelected;
    }

    partial void OnSelectedMessageTemplateIndexChanged(int value)
    {
        if (value >= 0 && value < MessageTemplates.Count)
        {
            EmergencyMessage = MessageTemplates[value];
        }
    }
} 
