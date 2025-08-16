using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Application.ViewModels;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Presentation.MAUI.Pages.Tool;
using Location = OtChaim.Domain.Common.Location;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

public partial class EmergencyViewModel : BaseEmergencyViewModel
{
    private readonly ICommandHandler<StartEmergency> _startEmergencyHandler;

    [ObservableProperty]
    private EmergencyType _selectedEmergencyType = EmergencyType.BloodSugarLow;

    [ObservableProperty]
    private string _emergencyMessage = "I need immediate assistance. Please help.";

    [ObservableProperty]
    private bool _isEmergencyDialogVisible = false;

    [ObservableProperty]
    private string _confirmationMessage = "";

    [ObservableProperty]
    private bool _isCreatePopupVisible = false;

    [ObservableProperty]
    private ContentView? _emergencyCreationPopup;

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

    [ObservableProperty]
    private int _selectedMessageTemplateIndex = -1;

    public EmergencyViewModel(ICommandHandler<StartEmergency> startEmergencyHandler, EmergencyCreationPopup emergencyCreationPopup)
    {
        _startEmergencyHandler = startEmergencyHandler;
        EmergencyCreationPopup = emergencyCreationPopup;

        // Subscribe to popup events
        if (EmergencyCreationPopup?.BindingContext is EmergencyCreationViewModel creationViewModel)
        {
            creationViewModel.EmergencyCreated += OnEmergencyPopUpFinished;
            creationViewModel.Cancelled += OnEmergencyPopUpFinished;
        }
    }

    [RelayCommand]
    private void TriggerEmergency()
    {
        // Show the unified emergency creation popup
        IsCreatePopupVisible = true;
    }

    private void OnEmergencyPopUpFinished(object? sender, EventArgs e)
    {
        IsCreatePopupVisible = false;
    }

    [RelayCommand]
    private async Task ConfirmEmergency()
    {
        try
        {
            IsLoading = true;
            IsEmergencyDialogVisible = false;

            // Create location (in real implementation, get from GPS)
            var location = new Location(0, 0, "Current Location");
            var affectedAreas = new List<Area> { Area.FromLocation(location, emergencyType: SelectedEmergencyType) };

            // Create the emergency command
            var command = new StartEmergency(
                initiatorUserId: Guid.NewGuid(), // In real implementation, get current user ID
                type: SelectedEmergencyType,
                location: location,
                affectedAreas: affectedAreas,
                description: EmergencyMessage
            );

            // Execute the command
            await _startEmergencyHandler.Handle(command, CancellationToken.None);

            await Shell.Current.DisplayAlert("Emergency Triggered",
                $"Emergency of type {SelectedEmergencyType} has been triggered successfully. Help is on the way.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to trigger emergency: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void CancelEmergency()
    {
        IsEmergencyDialogVisible = false;
    }
}
