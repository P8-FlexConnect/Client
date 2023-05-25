using Viewmodels;

namespace CompOff_App.Pages;

public partial class JobPage : ContentPage
{
    private readonly JobPageViewModel Vm;
	public JobPage(JobPageViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
        Vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Vm.InitializeAsync();
        Backdrop.Opacity = 0.6;
    }
}