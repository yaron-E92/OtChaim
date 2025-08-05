using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OtChaim.Presentation.MAUI.ViewModels.Settings;

public class MedicalInfoViewModel : INotifyPropertyChanged
{
    private string _selectedCondition = "Diabetes Type B";
    private string _condition = "Diabetes Type A";
    private string _medication = "Insulin";
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

    public string SelectedCondition
    {
        get => _selectedCondition;
        set
        {
            _selectedCondition = value;
            OnPropertyChanged();
        }
    }

    public string Condition
    {
        get => _condition;
        set
        {
            _condition = value;
            OnPropertyChanged();
        }
    }

    public string Medication
    {
        get => _medication;
        set
        {
            _medication = value;
            OnPropertyChanged();
        }
    }

    public string Information
    {
        get => _information;
        set
        {
            _information = value;
            OnPropertyChanged();
        }
    }

    public ICommand PreviousConditionCommand { get; }
    public ICommand NextConditionCommand { get; }
    public ICommand AddMedicalInfoCommand { get; }
    public ICommand RemoveMedicalInfoCommand { get; }

    public MedicalInfoViewModel()
    {
        PreviousConditionCommand = new Command(PreviousCondition);
        NextConditionCommand = new Command(NextCondition);
        AddMedicalInfoCommand = new Command(AddMedicalInfo);
        RemoveMedicalInfoCommand = new Command(RemoveMedicalInfo);
    }

    private void PreviousCondition()
    {
        _currentConditionIndex--;
        if (_currentConditionIndex < 0)
            _currentConditionIndex = _conditions.Count - 1;
        
        SelectedCondition = _conditions[_currentConditionIndex];
    }

    private void NextCondition()
    {
        _currentConditionIndex++;
        if (_currentConditionIndex >= _conditions.Count)
            _currentConditionIndex = 0;
        
        SelectedCondition = _conditions[_currentConditionIndex];
    }

    private void AddMedicalInfo()
    {
        // TODO: Implement add medical info functionality
        System.Diagnostics.Debug.WriteLine("Add Medical Info");
    }

    private void RemoveMedicalInfo()
    {
        // TODO: Implement remove medical info functionality
        System.Diagnostics.Debug.WriteLine("Remove Medical Info");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
