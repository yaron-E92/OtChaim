using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace OtChaim.Presentation.MAUI.ViewModels.Settings;

public partial class UserInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private string _firstName = "Finn";

    [ObservableProperty]
    private string _lastName = "Mond";

    [ObservableProperty]
    private DateTime _birthday = new(1955, 1, 21, 0, 0, 0, DateTimeKind.Utc);

    [ObservableProperty]
    private string _weight = "83 Kg";

    [ObservableProperty]
    private string _selectedBloodType = "A";

    [ObservableProperty]
    private string _address = "Mondstrasse 3, 71626 Bonn";

    [ObservableProperty]
    private string _currentLocation = "Get GPS coordinates";

    [ObservableProperty]
    private string _phone = "+71/182637263";

    [ObservableProperty]
    private string _email = "Finn@moon.com";

    public ObservableCollection<string> BloodTypes { get; } =
    [
        "A", "B", "AB", "O"
    ];

    public UserInfoViewModel()
    {
    }

    [RelayCommand]
    private void AddInfo()
    {
        // TODO: Implement add info functionality
        System.Diagnostics.Debug.WriteLine("Add Info");
    }

    [RelayCommand]
    private void RemoveInfo()
    {
        // TODO: Implement remove info functionality
        System.Diagnostics.Debug.WriteLine("Remove Info");
    }
}
