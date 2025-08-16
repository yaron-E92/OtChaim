using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;
using System.Collections.ObjectModel;

namespace OtChaim.Application.Services;

/// <summary>
/// Provides data operations for emergencies and users in the OtChaim application.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EmergencyDataService"/> class.
/// </remarks>
/// <param name="emergencyRepository">The emergency repository.</param>
/// <param name="userRepository">The user repository.</param>
public class EmergencyDataService(IEmergencyRepository emergencyRepository, IUserRepository userRepository)
{
    private readonly IEmergencyRepository _emergencyRepository = emergencyRepository;
    private readonly IUserRepository _userRepository = userRepository;

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
