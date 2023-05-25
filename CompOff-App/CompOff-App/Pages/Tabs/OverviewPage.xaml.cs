using Viewmodels.Tabs;

namespace CompOff_App.Pages.Tabs;

public partial class OverviewPage : ContentPage
{
    private readonly OverviewPageViewModel Vm;

    public OverviewPage(OverviewPageViewModel vm)
    {
	    InitializeComponent();
        BindingContext = vm;
        Vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Vm.InitializeAsync();

    }
}