using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

public class EmergencyViewModel : INotifyPropertyChanged
{
    private string _selectedEmergencyType = "Blood-Sugar Low";
    private string _emergencyMessage = "I fell again, bring the meds";
    private int _currentEmergencyTypeIndex = 0;

    private readonly List<string> _emergencyTypes =
    [
        "Blood-Sugar Low",
        "Heart Attack",
        "Stroke",
        "Fall",
        "Fire",
        "Medical Emergency"
    ];

    public string SelectedEmergencyType
    {
        get => _selectedEmergencyType;
        set
        {
            _selectedEmergencyType = value;
            OnPropertyChanged();
        }
    }

    public string EmergencyMessage
    {
        get => _emergencyMessage;
        set
        {
            _emergencyMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand PreviousEmergencyTypeCommand { get; }
    public ICommand NextEmergencyTypeCommand { get; }
    public ICommand AddPictureCommand { get; }
    public ICommand AddPersonalInfoCommand { get; }
    public ICommand AddMedicalInfoCommand { get; }
    public ICommand AddGpsCommand { get; }
    public ICommand AddDocumentCommand { get; }
    public ICommand AddMessageCommand { get; }
    public ICommand RemoveMessageCommand { get; }
    public ICommand TriggerEmergencyCommand { get; }

    public EmergencyViewModel()
    {
        PreviousEmergencyTypeCommand = new Command(PreviousEmergencyType);
        NextEmergencyTypeCommand = new Command(NextEmergencyType);
        AddPictureCommand = new Command(AddPicture);
        AddPersonalInfoCommand = new Command(AddPersonalInfo);
        AddMedicalInfoCommand = new Command(AddMedicalInfo);
        AddGpsCommand = new Command(AddGps);
        AddDocumentCommand = new Command(AddDocument);
        AddMessageCommand = new Command(AddMessage);
        RemoveMessageCommand = new Command(RemoveMessage);
        TriggerEmergencyCommand = new Command(TriggerEmergency);
    }

    private void PreviousEmergencyType()
    {
        _currentEmergencyTypeIndex--;
        if (_currentEmergencyTypeIndex < 0)
            _currentEmergencyTypeIndex = _emergencyTypes.Count - 1;
        
        SelectedEmergencyType = _emergencyTypes[_currentEmergencyTypeIndex];
    }

    private void NextEmergencyType()
    {
        _currentEmergencyTypeIndex++;
        if (_currentEmergencyTypeIndex >= _emergencyTypes.Count)
            _currentEmergencyTypeIndex = 0;
        
        SelectedEmergencyType = _emergencyTypes[_currentEmergencyTypeIndex];
    }

    private void AddPicture()
    {
        // TODO: Implement picture attachment
        System.Diagnostics.Debug.WriteLine("Add Picture");
    }

    private void AddPersonalInfo()
    {
        // TODO: Implement personal info attachment
        System.Diagnostics.Debug.WriteLine("Add Personal Info");
    }

    private void AddMedicalInfo()
    {
        // TODO: Implement medical info attachment
        System.Diagnostics.Debug.WriteLine("Add Medical Info");
    }

    private void AddGps()
    {
        // TODO: Implement GPS attachment
        System.Diagnostics.Debug.WriteLine("Add GPS");
    }

    private void AddDocument()
    {
        // TODO: Implement document attachment
        System.Diagnostics.Debug.WriteLine("Add Document");
    }

    private void AddMessage()
    {
        // TODO: Implement message addition
        System.Diagnostics.Debug.WriteLine("Add Message");
    }

    private void RemoveMessage()
    {
        // TODO: Implement message removal
        System.Diagnostics.Debug.WriteLine("Remove Message");
    }

    private void TriggerEmergency()
    {
        // TODO: Implement emergency trigger with confirmation
        System.Diagnostics.Debug.WriteLine("Trigger Emergency");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
