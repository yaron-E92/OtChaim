using OtChaim.Presentation.MAUI.ViewModels.Settings;
using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace OtChaim.Presentation.MAUI.Pages.Settings;

public partial class SettingsTabPage : ContentPage
{
    private SettingsTabViewModel _viewModel;

    public SettingsTabPage(SettingsTabViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        
        // Subscribe to property changes
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        
        // Set initial page
        UpdateCurrentPage();
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SettingsTabViewModel.CurrentPage))
        {
            UpdateCurrentPage();
        }
    }

    private void UpdateCurrentPage()
    {
        if (_viewModel.CurrentPage != null)
        {
            // Clear existing content
            ContentGrid.Children.Clear();
            
            // Add the current page (full width, no overlay)
            ContentGrid.Children.Add(_viewModel.CurrentPage);
        }
    }
}
