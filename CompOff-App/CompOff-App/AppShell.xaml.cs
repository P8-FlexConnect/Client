using CompOff_App.Pages;
using Shared;

namespace CompOff_App;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(NavigationKeys.JobPage, typeof(JobPage));
	}
}
