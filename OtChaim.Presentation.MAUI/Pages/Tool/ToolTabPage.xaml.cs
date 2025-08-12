using OtChaim.Presentation.MAUI.ViewModels.Tool;
using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace OtChaim.Presentation.MAUI.Pages.Tool;

public partial class ToolTabPage : ContentPage
{
    private ToolTabViewModel _viewModel;

    public ToolTabPage(ToolTabViewModel viewModel)
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
        if (e.PropertyName == nameof(ToolTabViewModel.CurrentPage))
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
