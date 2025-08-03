using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;
using OtChaim.Presentation.MAUI.Services;
using Location = OtChaim.Domain.Common.Location;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

/// <summary>
/// ViewModel for the emergency dashboard, managing emergencies and user interactions.
/// </summary>
public partial class EmergencyDashboardViewModel : ObservableObject
{
    /// <summary>
    /// The collection of active emergencies.
    /// </summary>
    public ObservableCollection<Emergency> Emergencies { get; }

    /// <summary>
    /// The collection of users.
    /// </summary>
    public ObservableCollection<User> Users { get; }

    /// <summary>
    /// The currently selected emergency.
    /// </summary>
    [ObservableProperty]
    private Emergency? _selectedEmergency;

    /// <summary>
    /// A value indicating whether data is loading.
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    private readonly ICommandHandler<StartEmergency> _startEmergencyHandler;
    private readonly ICommandHandler<MarkUserStatus> _markUserStatusHandler;
    private readonly ICommandHandler<EndEmergency> _endEmergencyHandler;
    private readonly EmergencyDataService _dataService;

    /// <summary>
    /// The available emergency types.
    /// </summary>
    public ObservableCollection<EmergencyType> EmergencyTypes { get; } = new(Enum.GetValues(typeof(EmergencyType)).Cast<EmergencyType>());

    /// <summary>
    /// The available severities.
    /// </summary>
    public ObservableCollection<Severity> Severities { get; } = new(Enum.GetValues(typeof(Severity)).Cast<Severity>());

    /// <summary>
    /// The selected emergency type.
    /// </summary>
    [ObservableProperty]
    private EmergencyType _selectedEmergencyType;

    /// <summary>
    /// The selected severity.
    /// </summary>
    [ObservableProperty]
    private Severity _selectedSeverity;

    /// <summary>
    /// The location description for a new emergency.
    /// </summary>
    [ObservableProperty]
    private string _locationDescription = string.Empty;

    /// <summary>
    /// The latitude for a new emergency.
    /// </summary>
    [ObservableProperty]
    private double _latitude;

    /// <summary>
    /// The longitude for a new emergency.
    /// </summary>
    [ObservableProperty]
    private double _longitude;

    /// <summary>
    /// A value indicating whether the create emergency popup is visible.
    /// </summary>
    [ObservableProperty]
    private bool _isCreatePopupVisible;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmergencyDashboardViewModel"/> class.
    /// </summary>
    /// <param name="startEmergencyHandler">Handler for starting emergencies.</param>
    /// <param name="markUserStatusHandler">Handler for marking user status.</param>
    /// <param name="endEmergencyHandler">Handler for ending emergencies.</param>
    /// <param name="dataService">Service for loading emergency and user data.</param>
    public EmergencyDashboardViewModel(
        ICommandHandler<StartEmergency> startEmergencyHandler,
        ICommandHandler<MarkUserStatus> markUserStatusHandler,
        ICommandHandler<EndEmergency> endEmergencyHandler,
        EmergencyDataService dataService)
    {
        _startEmergencyHandler = startEmergencyHandler;
        _markUserStatusHandler = markUserStatusHandler;
        _endEmergencyHandler = endEmergencyHandler;
        _dataService = dataService;

        Emergencies = new ObservableCollection<Emergency>();
        Users = new ObservableCollection<User>();

        SelectedEmergencyType = EmergencyTypes.FirstOrDefault();
        SelectedSeverity = Severities.FirstOrDefault();

        _ = LoadDataAsync();
    }

    private void ResetCreateEmergencyFields()
    {
        SelectedEmergencyType = EmergencyTypes.FirstOrDefault();
        SelectedSeverity = Severities.FirstOrDefault();
        LocationDescription = string.Empty;
        Latitude = 0;
        Longitude = 0;
    }

    [RelayCommand]
    private async Task StartEmergencyAsync()
    {
        try
        {
            IsLoading = true;

            var location = new Location(Latitude, Longitude, LocationDescription);
            var affectedAreas = new List<Area>
            {
                new Area(location, 5000) // TODO: allow user to specify radius/areas
            };

            var command = new StartEmergency(
                Guid.NewGuid(), // TODO: Replace with actual user ID from auth
                SelectedEmergencyType,
                location,
                affectedAreas,
                SelectedSeverity
            );

            await _startEmergencyHandler.Handle(command);
            IsCreatePopupVisible = false;
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            // In a real app, you'd want proper error handling and user notification
            System.Diagnostics.Debug.WriteLine($"Error starting emergency: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task MarkAsSafeAsync()
    {
        await MarkUserStatusAsync(true);
    }

    [RelayCommand]
    private async Task MarkAsNotSafeAsync()
    {
        await MarkUserStatusAsync(false);
    }

    private async Task MarkUserStatusAsync(bool isSafe)
    {
        if (SelectedEmergency is null) return;

        try
        {
            IsLoading = true;

            var userId = Guid.NewGuid(); // TODO: Replace with actual user ID from auth
            UserStatus status = isSafe ? UserStatus.Safe : UserStatus.HelpNeeded;
            string message = isSafe ? "I am safe" : "I need help";

            var command = new MarkUserStatus(
                userId,
                SelectedEmergency.Id,
                status,
                message
            );

            await _markUserStatusHandler.Handle(command);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error marking user status: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ResolveEmergencyAsync()
    {
        if (SelectedEmergency is null) return;

        try
        {
            IsLoading = true;

            var command = new EndEmergency(SelectedEmergency.Id);
            await _endEmergencyHandler.Handle(command);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error resolving emergency: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private void ShowCreatePopup()
    {
        ResetCreateEmergencyFields();
        IsCreatePopupVisible = true;
    }

    [RelayCommand]
    private void HideCreatePopup()
    {
        IsCreatePopupVisible = false;
    }

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            await _dataService.LoadActiveEmergenciesAsync(Emergencies);
            await _dataService.LoadUsersAsync(Users);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
