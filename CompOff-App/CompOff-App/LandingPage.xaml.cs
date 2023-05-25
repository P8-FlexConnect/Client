using Viewmodels;

namespace CompOff_App;

public partial class LandingPage : ContentPage
{
    private readonly LandingPageViewModel Vm;

	public LandingPage(LandingPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        Vm = vm;

        LoginButton.Command = new Command(async () =>
        {
            await Vm.Login(UsernameEntry.Text, PasswordEntry.Text);
        });
	}

    protected override async void OnAppearing()
    {

        base.OnAppearing();
        await Vm.InitializeAsync();
    }
}

