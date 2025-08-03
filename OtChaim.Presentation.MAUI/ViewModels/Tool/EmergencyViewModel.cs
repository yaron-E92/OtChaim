using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

public partial class EmergencyViewModel : ObservableObject
{
    [ObservableProperty]
    private string _selectedEmergencyType = "Blood-Sugar Low";

    [ObservableProperty]
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

    [RelayCommand]
    private void PreviousEmergencyType()
    {
        _currentEmergencyTypeIndex--;
        if (_currentEmergencyTypeIndex < 0)
            _currentEmergencyTypeIndex = _emergencyTypes.Count - 1;
        
        SelectedEmergencyType = _emergencyTypes[_currentEmergencyTypeIndex];
    }

    [RelayCommand]
    private void NextEmergencyType()
    {
        _currentEmergencyTypeIndex++;
        if (_currentEmergencyTypeIndex >= _emergencyTypes.Count)
            _currentEmergencyTypeIndex = 0;
        
        SelectedEmergencyType = _emergencyTypes[_currentEmergencyTypeIndex];
    }

    [RelayCommand]
    private void AddPicture()
    {
        // TODO: Implement picture attachment
        System.Diagnostics.Debug.WriteLine("Add Picture");
    }

    [RelayCommand]
    private void AddPersonalInfo()
    {
        // TODO: Implement personal info attachment
        System.Diagnostics.Debug.WriteLine("Add Personal Info");
    }

    [RelayCommand]
    private void AddMedicalInfo()
    {
        // TODO: Implement medical info attachment
        System.Diagnostics.Debug.WriteLine("Add Medical Info");
    }

    [RelayCommand]
    private void AddGps()
    {
        // TODO: Implement GPS attachment
        System.Diagnostics.Debug.WriteLine("Add GPS");
    }

    [RelayCommand]
    private void AddDocument()
    {
        // TODO: Implement document attachment
        System.Diagnostics.Debug.WriteLine("Add Document");
    }

    [RelayCommand]
    private void AddMessage()
    {
        // TODO: Implement message attachment
        System.Diagnostics.Debug.WriteLine("Add Message");
    }

    [RelayCommand]
    private void RemoveMessage()
    {
        // TODO: Implement message removal
        System.Diagnostics.Debug.WriteLine("Remove Message");
    }

    [RelayCommand]
    private void TriggerEmergency()
    {
        // TODO: Implement emergency trigger
        System.Diagnostics.Debug.WriteLine("Trigger Emergency");
    }
}
