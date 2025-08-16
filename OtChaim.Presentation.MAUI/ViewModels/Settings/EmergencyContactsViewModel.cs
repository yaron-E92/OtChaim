using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace OtChaim.Presentation.MAUI.ViewModels.Settings;

public partial class EmergencyContactsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _selectedContact = "Grandma";
    [ObservableProperty]
    private string _firstName = "Fiona";
    [ObservableProperty]
    private string _lastName = "Sonne";
    [ObservableProperty]
    private DateTime _birthday = new(1955, 2, 21, 0, 0, 0, DateTimeKind.Utc);
    [ObservableProperty]
    private string _selectedBloodType = "A";
    [ObservableProperty]
    private string _address = "Mondstrasse 3, 71626 Bonn";
    [ObservableProperty]
    private string _phone = "+71/182637263";
    [ObservableProperty]
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

    public ObservableCollection<string> BloodTypes { get; } =
    [
        "A", "B", "AB", "O"
    ];

    public EmergencyContactsViewModel()
    {}

    [RelayCommand]
    private void PreviousContact()
    {
        _currentContactIndex--;
        if (_currentContactIndex < 0)
            _currentContactIndex = _contacts.Count - 1;

        SelectedContact = _contacts[_currentContactIndex];
    }

    [RelayCommand]
    private void NextContact()
    {
        _currentContactIndex++;
        if (_currentContactIndex >= _contacts.Count)
            _currentContactIndex = 0;

        SelectedContact = _contacts[_currentContactIndex];
    }

    [RelayCommand]
    private void AddContact()
    {
        // TODO: Implement add contact functionality
        System.Diagnostics.Debug.WriteLine("Add Contact");
    }

    [RelayCommand]
    private void RemoveContact()
    {
        // TODO: Implement remove contact functionality
        System.Diagnostics.Debug.WriteLine("Remove Contact");
    }
}
