using TruweatherMobile.ViewModels;

namespace TruweatherMobile.Pages;

public partial class LocationsPage : ContentPage
{
    private readonly LocationsViewModel _viewModel;

    public LocationsPage(LocationsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}
