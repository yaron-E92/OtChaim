using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OtChaim.Presentation.MAUI.ViewModels.Settings;

public partial class MedicalInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private string _selectedCondition = "Diabetes Type B";

    [ObservableProperty]
    private string _condition = "Diabetes Type A";

    [ObservableProperty]
    private string _medication = "Insulin";

    [ObservableProperty]
    private string _information = "";

    private int _currentConditionIndex = 0;

    private readonly List<string> _conditions =
    [
        "Diabetes Type B",
        "Heart Disease",
        "Hypertension",
        "Asthma",
        "Allergies",
        "None"
    ];

    public MedicalInfoViewModel()
    {
    }

    [RelayCommand]
    private void PreviousCondition()
    {
        _currentConditionIndex--;
        if (_currentConditionIndex < 0)
            _currentConditionIndex = _conditions.Count - 1;

        SelectedCondition = _conditions[_currentConditionIndex];
    }

    [RelayCommand]
    private void NextCondition()
    {
        _currentConditionIndex++;
        if (_currentConditionIndex >= _conditions.Count)
            _currentConditionIndex = 0;

        SelectedCondition = _conditions[_currentConditionIndex];
    }

    [RelayCommand]
    private void AddMedicalInfo()
    {
        // TODO: Implement add medical info functionality
        System.Diagnostics.Debug.WriteLine("Add Medical Info");
    }

    [RelayCommand]
    private void RemoveMedicalInfo()
    {
        // TODO: Implement remove medical info functionality
        System.Diagnostics.Debug.WriteLine("Remove Medical Info");
    }
}
