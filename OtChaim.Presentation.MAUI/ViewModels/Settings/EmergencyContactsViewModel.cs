using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OtChaim.Presentation.MAUI.ViewModels.Settings;

public class EmergencyContactsViewModel : INotifyPropertyChanged
{
    private string _selectedContact = "Grandma";
    private string _firstName = "Fiona";
    private string _lastName = "Sonne";
    private DateTime _birthday = new(1955, 2, 21);
    private string _selectedBloodType = "A";
    private string _address = "Mondstrasse 3, 71626 Bonn";
    private string _phone = "+71/182637263";
    private string _email = "Finn@moon.com";
    private int _currentContactIndex = 0;

    private readonly List<string> _contacts =
    [
        "Grandma",
        "Mom",
        "Dad",
        "Sister",
        "Brother",
        "Neighbor"
    ];

    public string SelectedContact
    {
        get => _selectedContact;
        set
        {
            _selectedContact = value;
            OnPropertyChanged();
        }
    }

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

    public ICommand PreviousContactCommand { get; }
    public ICommand NextContactCommand { get; }
    public ICommand AddContactCommand { get; }
    public ICommand RemoveContactCommand { get; }

    public EmergencyContactsViewModel()
    {
        PreviousContactCommand = new Command(PreviousContact);
        NextContactCommand = new Command(NextContact);
        AddContactCommand = new Command(AddContact);
        RemoveContactCommand = new Command(RemoveContact);
    }

    private void PreviousContact()
    {
        _currentContactIndex--;
        if (_currentContactIndex < 0)
            _currentContactIndex = _contacts.Count - 1;
        
        SelectedContact = _contacts[_currentContactIndex];
    }

    private void NextContact()
    {
        _currentContactIndex++;
        if (_currentContactIndex >= _contacts.Count)
            _currentContactIndex = 0;
        
        SelectedContact = _contacts[_currentContactIndex];
    }

    private void AddContact()
    {
        // TODO: Implement add contact functionality
        System.Diagnostics.Debug.WriteLine("Add Contact");
    }

    private void RemoveContact()
    {
        // TODO: Implement remove contact functionality
        System.Diagnostics.Debug.WriteLine("Remove Contact");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
