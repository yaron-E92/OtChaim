using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OtChaim.Presentation.MAUI.ViewModels.Settings;

public class UserInfoViewModel : INotifyPropertyChanged
{
    private string _firstName = "Finn";
    private string _lastName = "Mond";
    private DateTime _birthday = new(1955, 1, 21, 0, 0, 0, DateTimeKind.Utc);
    private string _weight = "83 Kg";
    private string _selectedBloodType = "A";
    private string _address = "Mondstrasse 3, 71626 Bonn";
    private string _currentLocation = "Get GPS coordinates";
    private string _phone = "+71/182637263";
    private string _email = "Finn@moon.com";

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged();
        }
    }

    public DateTime Birthday
    {
        get => _birthday;
        set
        {
            _birthday = value;
            OnPropertyChanged();
        }
    }

    public string Weight
    {
        get => _weight;
        set
        {
            _weight = value;
            OnPropertyChanged();
        }
    }

    public string SelectedBloodType
    {
        get => _selectedBloodType;
        set
        {
            _selectedBloodType = value;
            OnPropertyChanged();
        }
    }

    public string Address
    {
        get => _address;
        set
        {
            _address = value;
            OnPropertyChanged();
        }
    }

    public string CurrentLocation
    {
        get => _currentLocation;
        set
        {
            _currentLocation = value;
            OnPropertyChanged();
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            _phone = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> BloodTypes { get; } =
    [
        "A", "B", "AB", "O"
    ];

    public ICommand AddInfoCommand { get; }
    public ICommand RemoveInfoCommand { get; }

    public UserInfoViewModel()
    {
        AddInfoCommand = new Command(AddInfo);
        RemoveInfoCommand = new Command(RemoveInfo);
    }

    private void AddInfo()
    {
        // TODO: Implement add info functionality
        System.Diagnostics.Debug.WriteLine("Add Info");
    }

    private void RemoveInfo()
    {
        // TODO: Implement remove info functionality
        System.Diagnostics.Debug.WriteLine("Remove Info");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
