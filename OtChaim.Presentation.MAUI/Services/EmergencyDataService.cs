using System.Collections.ObjectModel;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;

namespace OtChaim.Presentation.MAUI.Services;

/// <summary>
/// Provides data operations for emergencies and users in the OtChaim MAUI application.
/// </summary>
public class EmergencyDataService
{
    private readonly IEmergencyRepository _emergencyRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmergencyDataService"/> class.
    /// </summary>
    /// <param name="emergencyRepository">The emergency repository.</param>
    /// <param name="userRepository">The user repository.</param>
    public EmergencyDataService(IEmergencyRepository emergencyRepository, IUserRepository userRepository)
    {
        _emergencyRepository = emergencyRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Loads active emergencies into the provided collection.
    /// </summary>
    /// <param name="emergencies">The collection to populate with active emergencies.</param>
    public async Task LoadActiveEmergenciesAsync(ObservableCollection<Emergency> emergencies)
    {
        try
        {
            IReadOnlyList<Emergency> allEmergencies = await _emergencyRepository.GetActiveAsync();
            emergencies.Clear();
            foreach (Emergency emergency in allEmergencies)
            {
                emergencies.Add(emergency);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading emergencies: {ex.Message}");
        }
    }

    /// <summary>
    /// Loads all users into the provided collection.
    /// </summary>
    /// <param name="users">The collection to populate with users.</param>
    public async Task LoadUsersAsync(ObservableCollection<User> users)
    {
        try
        {
            IReadOnlyList<User> allUsers = await _userRepository.GetAllAsync();
            users.Clear();
            foreach (User user in allUsers)
            {
                users.Add(user);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading users: {ex.Message}");
        }
    }
}
