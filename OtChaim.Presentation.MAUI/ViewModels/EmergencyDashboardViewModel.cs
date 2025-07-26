using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;
using OtChaim.Presentation.MAUI.Services;
using Location = OtChaim.Domain.Common.Location;

namespace OtChaim.Presentation.MAUI.ViewModels;

/// <summary>
/// ViewModel for the emergency dashboard, managing emergencies and user interactions.
/// </summary>
public class EmergencyDashboardViewModel : INotifyPropertyChanged
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
    public Emergency? SelectedEmergency
    {
        get => _selectedEmergency;
        set
        {
            _selectedEmergency = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// A value indicating whether data is loading.
    /// </summary>
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Command to start a new emergency.
    /// </summary>
    public Command StartEmergencyCommand { get; }

    /// <summary>
    /// Command to mark a user as safe.
    /// </summary>
    public Command MarkAsSafeCommand { get; }

    /// <summary>
    /// Command to mark a user as not safe.
    /// </summary>
    public Command MarkAsNotSafeCommand { get; }

    /// <summary>
    /// Command to resolve an emergency.
    /// </summary>
    public Command ResolveEmergencyCommand { get; }

    /// <summary>
    /// Command to refresh emergency data.
    /// </summary>
    public Command RefreshCommand { get; }

    private readonly ICommandHandler<StartEmergency> _startEmergencyHandler;
    private readonly ICommandHandler<MarkUserStatus> _markUserStatusHandler;
    private readonly ICommandHandler<EndEmergency> _endEmergencyHandler;
    private readonly EmergencyDataService _dataService;

    private Emergency? _selectedEmergency;
    private bool _isLoading;

    /// <summary>
    /// The available emergency types.
    /// </summary>
    public ObservableCollection<EmergencyType> EmergencyTypes { get; } = new(Enum.GetValues(typeof(EmergencyType)).Cast<EmergencyType>());

    /// <summary>
    /// The available severities.
    /// </summary>
    public ObservableCollection<Severity> Severities { get; } = new(Enum.GetValues(typeof(Severity)).Cast<Severity>());

    private EmergencyType _selectedEmergencyType;
    /// <summary>
    /// The selected emergency type.
    /// </summary>
    public EmergencyType SelectedEmergencyType
    {
        get => _selectedEmergencyType;
        set { _selectedEmergencyType = value; OnPropertyChanged(); }
    }

    private Severity _selectedSeverity;
    /// <summary>
    /// The selected severity.
    /// </summary>
    public Severity SelectedSeverity
    {
        get => _selectedSeverity;
        set { _selectedSeverity = value; OnPropertyChanged(); }
    }

    private string _locationDescription = string.Empty;
    /// <summary>
    /// The location description for a new emergency.
    /// </summary>
    public string LocationDescription
    {
        get => _locationDescription;
        set { _locationDescription = value; OnPropertyChanged(); }
    }

    private double _latitude;
    /// <summary>
    /// The latitude for a new emergency.
    /// </summary>
    public double Latitude
    {
        get => _latitude;
        set { _latitude = value; OnPropertyChanged(); }
    }

    private double _longitude;
    /// <summary>
    /// The longitude for a new emergency.
    /// </summary>
    public double Longitude
    {
        get => _longitude;
        set { _longitude = value; OnPropertyChanged(); }
    }

    private bool _isCreatePopupVisible;
    /// <summary>
    /// A value indicating whether the create emergency popup is visible.
    /// </summary>
    public bool IsCreatePopupVisible
    {
        get => _isCreatePopupVisible;
        set { _isCreatePopupVisible = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Command to show the create emergency popup.
    /// </summary>
    public Command ShowCreatePopupCommand { get; }

    /// <summary>
    /// Command to hide the create emergency popup.
    /// </summary>
    public Command HideCreatePopupCommand { get; }

    /// <summary>
    /// Command triggered when the selected emergency changes.
    /// </summary>
    public Command<SelectionChangedEventArgs> SelectedEmergencyChanged { get; }

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

        StartEmergencyCommand = new Command(async () =>
        {
            Task startEmergency = StartEmergencyAsync();
            IsCreatePopupVisible = false;
            await startEmergency;
        });
        MarkAsSafeCommand = new Command(async () => await MarkUserStatusAsync(true));
        MarkAsNotSafeCommand = new Command(async () => await MarkUserStatusAsync(false));
        ResolveEmergencyCommand = new Command(async () => await ResolveEmergencyAsync());
        RefreshCommand = new Command(async () => await LoadDataAsync());
        ShowCreatePopupCommand = new Command(() =>
        {
            ResetCreateEmergencyFields();
            IsCreatePopupVisible = true;
        });
        HideCreatePopupCommand = new Command(() => IsCreatePopupVisible = false);
        SelectedEmergencyChanged = new Command<SelectionChangedEventArgs>((SelectionChangedEventArgs scea) =>
        {
            SelectedEmergency = scea?.CurrentSelection != null && scea.CurrentSelection.Count > 0
                                ? scea.CurrentSelection[0] as Emergency
                                : null;
        });

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
        }
        catch (Exception ex)
        {
            // In a real app, you'd want proper error handling and user notification
            System.Diagnostics.Debug.WriteLine($"Error starting emergency: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            RefreshCommand.Execute(null!);
        }
    }

    private async Task MarkUserStatusAsync(bool isSafe)
    {
        if (SelectedEmergency is null) return;

        try
        {
            IsLoading = true;

            var userId = Guid.NewGuid(); // TODO: Replace with actual user ID from auth
            var status = isSafe ? UserStatus.Safe : UserStatus.HelpNeeded;
            var message = isSafe ? "I am safe" : "I need help";

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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
