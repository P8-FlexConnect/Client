using CommunityToolkit.Maui;
using CompOff_App.Common;
using Xamarin.CommunityToolkit.Ports.Effects;
using Xamarin.CommunityToolkit.Ports.Android.Effects;

namespace CompOff_App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

        builder
            .AddWrappers()
            .AddViewModels()
            .AddPages()
            .AddServices()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("GillSans-Regular.ttf", "GillSansRegular");
                fonts.AddFont("GillSans-Bold.ttf", "GillSansBold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
                fonts.AddFont("MaterialIconsOutlined-Regular.otf", "MaterialIconsOutlined");
            }).ConfigureEffects(effects =>
            {
#if ANDROID || IOS
                effects.Add(typeof(TouchEffect), typeof(PlatformTouchEffect));
#endif
            });

        return builder.Build();
	}
}
