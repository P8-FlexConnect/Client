using System.Data;
using System.Windows.Input;

namespace CompOff_App.Templates;

public partial class JobCardTemplate : ContentView
{

    /// <summary>
    /// Backing BindableProperty for the <see cref="Item"/> property.
    /// </summary>
    public static readonly BindableProperty ItemProperty = BindableProperty.Create(nameof(Item), typeof(Models.Job), typeof(JobCardTemplate), new Models.Job());

    /// <summary>
    /// Backing BindableProperty for the <see cref="Command"/> property.
    /// </summary>
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(JobCardTemplate));


    /// <summary>
    /// The chat list item to be displayed
    /// </summary>
    public Models.Job Item
    {
        get => (Models.Job)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    /// <summary>
    /// The command which is executed when the avatar is tapped. This can be used to open a settings-modal or similarly.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public JobCardTemplate()
	{
		InitializeComponent();
	}
}