using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.EmergencyEvents.Events;
using OtChaim.Domain.Users;
using OtChaim.Presentation.MAUI.Pages.Tool;
using OtChaim.Presentation.MAUI.Services;
using Yaref92.Events.Abstractions;
using Location = OtChaim.Domain.Common.Location;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

/// <summary>
/// ViewModel for managing the emergency dashboard, which displays active emergencies and user status.
/// This ViewModel handles the display and management of emergency information, user lists, and
/// provides functionality to create new emergencies through a unified popup interface.
/// </summary>
/// <remarks>
/// The EmergencyDashboardViewModel subscribes to domain events to stay synchronized with
/// emergency state changes. It manages both the emergency list display and the emergency
/// creation popup functionality.
/// </remarks>
    public partial class EmergencyDashboardViewModel : ObservableObject, IDisposable, IAsyncEventSubscriber<EmergencyAlterationPersisted>
{
    /// <summary>
    /// The currently selected emergency.
    /// </summary>
    [ObservableProperty]
    private Emergency? _selectedEmergency;

    private readonly EmergencyDataService _dataService;
    private readonly ICommandHandler<StartEmergency> _startEmergencyHandler;
    private readonly ICommandHandler<MarkUserStatus> _markUserStatusHandler;
    private readonly ICommandHandler<EndEmergency> _endEmergencyHandler;
    private readonly IEventAggregator _eventAggregator;
    private readonly EmergencyCreationPopup _emergencyCreationPopup;

    /// <summary>
    /// Gets or sets the collection of active emergencies displayed in the dashboard.
    /// </summary>
    /// <remarks>
    /// This collection is automatically updated when new emergencies are created or
    /// when existing emergencies are modified through domain events.
    /// </remarks>
    [ObservableProperty]
    private ObservableCollection<Emergency> _emergencies = [];

    /// <summary>
    /// Gets or sets the collection of users displayed in the dashboard.
    /// </summary>
    /// <remarks>
    /// This collection shows all users that can be involved in emergency situations
    /// and their current status information.
    /// </remarks>
    [ObservableProperty]
    private ObservableCollection<User> _users = [];

    /// <summary>
    /// Gets or sets a value indicating whether the dashboard is currently loading data.
    /// </summary>
    /// <remarks>
    /// This property is used to show loading indicators in the UI during data operations.
    /// </remarks>
    [ObservableProperty]
    private bool _isLoading;

    /// <summary>
    /// Gets or sets a value indicating whether the emergency creation popup is visible.
    /// </summary>
    /// <remarks>
    /// When true, the emergency creation popup is displayed as an overlay on the dashboard.
    /// </remarks>
    [ObservableProperty]
    private bool _isCreatePopupVisible;

    /// <summary>
    /// Gets the emergency creation popup content view.
    /// </summary>
    /// <remarks>
    /// This popup provides a unified interface for creating new emergencies with all
    /// available options including attachments, location, and contact preferences.
    /// </remarks>
    public ContentView? EmergencyCreationPopup => _emergencyCreationPopup;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmergencyDashboardViewModel"/> class.
    /// </summary>
    /// <param name="startEmergencyHandler">Handler for starting emergencies.</param>
    /// <param name="markUserStatusHandler">Handler for marking user status.</param>
    /// <param name="endEmergencyHandler">Handler for ending emergencies.</param>
    /// <param name="dataService">The service for loading emergency and user data.</param>
    /// <param name="eventAggregator">The event aggregator for subscribing to domain events.</param>
    /// <param name="emergencyCreationPopup">The popup for creating new emergencies.</param>
    /// <remarks>
    /// The constructor sets up event subscriptions and initializes the data loading process.
    /// It subscribes to EmergencyPersisted events to keep the dashboard synchronized.
    /// </remarks>
    public EmergencyDashboardViewModel(
        ICommandHandler<StartEmergency> startEmergencyHandler,
        ICommandHandler<MarkUserStatus> markUserStatusHandler,
        ICommandHandler<EndEmergency> endEmergencyHandler,
        EmergencyDataService dataService,
        IEventAggregator eventAggregator,
        EmergencyCreationPopup emergencyCreationPopup)
    {
        _startEmergencyHandler = startEmergencyHandler;
        _markUserStatusHandler = markUserStatusHandler;
        _endEmergencyHandler = endEmergencyHandler;
        _dataService = dataService;
        _eventAggregator = eventAggregator;
        _emergencyCreationPopup = emergencyCreationPopup;

        // Subscribe to EmergencyStarted events
        _eventAggregator.SubscribeToEventType(this);

        // Subscribe to popup events
        if (EmergencyCreationPopup?.BindingContext is EmergencyCreationViewModel creationViewModel)
        {
            creationViewModel.EmergencyCreated += OnEmergencyPopUpFinished;
            creationViewModel.Cancelled += OnEmergencyPopUpFinished;
        }

        // Initialize data loading with proper error handling
        _ = Task.Run(async () =>
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during initial data load: {ex.Message}");
            }
        });
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


    /// <summary>
    /// Command to show the emergency creation popup.
    /// </summary>
    /// <remarks>
    /// This command displays the unified emergency creation interface that allows users
    /// to configure all emergency parameters including type, location, attachments, and contacts.
    /// </remarks>
    [RelayCommand]
    private void ShowCreatePopup()
    {
        IsCreatePopupVisible = true;
    }

    /// <summary>
    /// Handles the completion of emergency creation popup operations (create or cancel).
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An EventArgs that contains no event data.</param>
    /// <remarks>
    /// This method is called when either an emergency is created or the popup is cancelled.
    /// It hides the popup and refreshes the emergency list if an emergency was created.
    /// </remarks>
    private void OnEmergencyPopUpFinished(object? sender, EventArgs e)
    {
        IsCreatePopupVisible = false;
    }

    /// <summary>
    /// Loads emergency and user data asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous data loading operation.</returns>
    /// <remarks>
    /// This method loads both active emergencies and user information from the data service.
    /// It includes error handling to prevent application crashes during data loading failures.
    /// </remarks>
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
            // Ensure UI is not stuck in loading state
            IsLoading = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Handles the EmergencyAlterationPersisted domain event to refresh the dashboard data.
    /// </summary>
    /// <param name="domainEvent">The EmergencyAlterationPersisted event containing the emergency ID and alteration type.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous event handling operation.</returns>
    /// <remarks>
    /// This method is called when an emergency alteration has been successfully persisted to the database.
    /// It refreshes the emergency list to reflect the current state of all emergencies.
    /// </remarks>
    public async Task OnNextAsync(EmergencyAlterationPersisted domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            // Refresh the emergency list when an emergency alteration is persisted
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error handling EmergencyAlterationPersisted event: {ex.Message}");
        }
    }

    /// <summary>
    /// Disposes of the ViewModel and unsubscribes from events.
    /// </summary>
    /// <remarks>
    /// This method ensures proper cleanup of event subscriptions to prevent memory leaks
    /// and unexpected behavior when the ViewModel is no longer needed.
    /// </remarks>
    public void Dispose()
    {
        _eventAggregator.UnsubscribeFromEventType(this);
        
        if (EmergencyCreationPopup?.BindingContext is EmergencyCreationViewModel creationViewModel)
        {
            creationViewModel.EmergencyCreated -= OnEmergencyPopUpFinished;
            creationViewModel.Cancelled -= OnEmergencyPopUpFinished;
        }
    }
}
