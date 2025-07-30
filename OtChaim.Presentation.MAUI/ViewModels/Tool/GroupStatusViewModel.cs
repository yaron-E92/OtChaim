using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

public class GroupStatusViewModel : INotifyPropertyChanged
{
    private string _selectedGroup = "Family";
    private int _currentGroupIndex = 0;

    private readonly List<string> _groups =
    [
        "Family",
        "Friends",
        "Neighbors",
        "Emergency Contacts"
    ];

    public string SelectedGroup
    {
        get => _selectedGroup;
        set
        {
            _selectedGroup = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ContactStatus> Contacts { get; }

    public ICommand PreviousGroupCommand { get; }
    public ICommand NextGroupCommand { get; }
    public ICommand AddContactCommand { get; }
    public ICommand RemoveContactCommand { get; }
    public ICommand AreYouOkCommand { get; }

    public GroupStatusViewModel()
    {
        Contacts =
        [
            new ContactStatus { Name = "Grandma", StatusText = "ok", StatusColor = Color.FromArgb("#4CAF50") },
            new ContactStatus { Name = "Mamuschka", StatusText = "?", StatusColor = Color.FromArgb("#FFC107") },
            new ContactStatus { Name = "Pappa", StatusText = ":D", StatusColor = Color.FromArgb("#F44336") },
            new ContactStatus { Name = "Finn", StatusText = "!", StatusColor = Color.FromArgb("#F44336") }
        ];

        PreviousGroupCommand = new Command(PreviousGroup);
        NextGroupCommand = new Command(NextGroup);
        AddContactCommand = new Command(AddContact);
        RemoveContactCommand = new Command(RemoveContact);
        AreYouOkCommand = new Command(AreYouOk);
    }

    private void PreviousGroup()
    {
        _currentGroupIndex--;
        if (_currentGroupIndex < 0)
            _currentGroupIndex = _groups.Count - 1;
        
        SelectedGroup = _groups[_currentGroupIndex];
    }

    private void NextGroup()
    {
        _currentGroupIndex++;
        if (_currentGroupIndex >= _groups.Count)
            _currentGroupIndex = 0;
        
        SelectedGroup = _groups[_currentGroupIndex];
    }

    private void AddContact()
    {
        // TODO: Implement contact addition
        System.Diagnostics.Debug.WriteLine("Add Contact");
    }

    private void RemoveContact()
    {
        // TODO: Implement contact removal
        System.Diagnostics.Debug.WriteLine("Remove Contact");
    }

    private void AreYouOk()
    {
        // TODO: Implement "Are you ok?" functionality with priority overlay
        System.Diagnostics.Debug.WriteLine("Are you ok?");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
