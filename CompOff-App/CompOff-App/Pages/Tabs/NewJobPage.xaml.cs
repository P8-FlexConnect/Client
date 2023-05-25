using CommunityToolkit.Mvvm.Input;
using Viewmodels.Tabs;
using Microsoft.Maui;

namespace CompOff_App.Pages.Tabs;

public partial class NewJobPage : ContentPage
{
    public event EventHandler? Clicked;
    private readonly NewJobPageViewModel Vm;

    public NewJobPage(NewJobPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Vm = vm;


        AddButton.Command = new Command(async () =>
        {
            await Vm.AddJob(TitleEditor.Text, DescriptionEditor.Text);
            TitleEditor.Text = string.Empty;
            DescriptionEditor.Text = string.Empty;
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Vm.InitializeAsync();
    }
}