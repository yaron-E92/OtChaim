using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

public partial class GroupStatusViewModel : ObservableObject
{
    [ObservableProperty]
    private string _selectedGroup = "Family";

    private int _currentGroupIndex = 0;

    private readonly List<string> _groups =
    [
        "Family",
        "Friends",
        "Neighbors",
        "Emergency Contacts"
    ];

    public ObservableCollection<ContactStatus> Contacts { get; }

    public GroupStatusViewModel()
    {
        Contacts =
        [
            new ContactStatus { Name = "Grandma", StatusText = "ok", StatusColor = Color.FromArgb("#4CAF50") },
            new ContactStatus { Name = "Mamuschka", StatusText = "?", StatusColor = Color.FromArgb("#FFC107") },
            new ContactStatus { Name = "Pappa", StatusText = ":D", StatusColor = Color.FromArgb("#F44336") },
            new ContactStatus { Name = "Finn", StatusText = "!", StatusColor = Color.FromArgb("#F44336") }
        ];
    }

    [RelayCommand]
    private void PreviousGroup()
    {
        _currentGroupIndex--;
        if (_currentGroupIndex < 0)
            _currentGroupIndex = _groups.Count - 1;
        
        SelectedGroup = _groups[_currentGroupIndex];
    }

    [RelayCommand]
    private void NextGroup()
    {
        _currentGroupIndex++;
        if (_currentGroupIndex >= _groups.Count)
            _currentGroupIndex = 0;
        
        SelectedGroup = _groups[_currentGroupIndex];
    }

    [RelayCommand]
    private void AddContact()
    {
        // TODO: Implement contact addition
        System.Diagnostics.Debug.WriteLine("Add Contact");
    }

    [RelayCommand]
    private void RemoveContact()
    {
        // TODO: Implement contact removal
        System.Diagnostics.Debug.WriteLine("Remove Contact");
    }

    [RelayCommand]
    private void AreYouOk()
    {
        // TODO: Implement "Are you ok?" functionality with priority overlay
        System.Diagnostics.Debug.WriteLine("Are you ok?");
    }
}
