using CommunityToolkit.Maui.Behaviors;
using System.Windows.Input;

namespace CompOff_App.Components;

public partial class NavigationBarSecondary : ContentView
{
    public event EventHandler? Clicked;

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(NavigationBarSecondary));

    public static readonly BindableProperty UpdateCommandProperty = BindableProperty.Create(nameof(UpdateCommand), typeof(ICommand), typeof(NavigationBarSecondary));

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NavigationBarSecondary), string.Empty);


    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public ICommand UpdateCommand
    {
        get => (ICommand)GetValue(UpdateCommandProperty);
        set => SetValue(UpdateCommandProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
    }

    public NavigationBarSecondary()
    {
        InitializeComponent();
    }
}