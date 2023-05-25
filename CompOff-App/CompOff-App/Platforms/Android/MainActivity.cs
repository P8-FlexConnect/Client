using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;


namespace CompOff_App;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        var color = Color.FromArgb("#345DA7");
        Window.SetNavigationBarColor(color.ToPlatform());

        base.OnCreate(savedInstanceState);
    }
}
