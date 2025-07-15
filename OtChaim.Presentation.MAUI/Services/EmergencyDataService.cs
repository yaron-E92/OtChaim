using System.Collections.ObjectModel;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;

namespace OtChaim.Presentation.MAUI.Services;

public class EmergencyDataService
{
    private readonly IEmergencyRepository _emergencyRepository;
    private readonly IUserRepository _userRepository;

    public EmergencyDataService(IEmergencyRepository emergencyRepository, IUserRepository userRepository)
    {
        _emergencyRepository = emergencyRepository;
        _userRepository = userRepository;
    }

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
