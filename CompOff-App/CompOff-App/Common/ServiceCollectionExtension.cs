using CompOff_App.Pages;
using CompOff_App.Pages.Tabs;
using Viewmodels;
using Services;
using Services.Impl;
using Wrappers;
using Wrappers.Impl;
using Viewmodels.Tabs;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace CompOff_App.Common
{
    [ExcludeFromCodeCoverage]
    internal static class ServiceCollectionExtension
    {
        public static MauiAppBuilder AddPages(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LandingPage>();
            builder.Services.AddTransient<JobPage>();

            //Tabs
            builder.Services.AddTransient<OverviewPage>();
            builder.Services.AddSingleton<JobListPage>();
            builder.Services.AddTransient<NewJobPage>();
            return builder;
        }
        public static MauiAppBuilder AddWrappers(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<INavigationWrapper, ShellNavigator>();
            return builder;
        }
        public static MauiAppBuilder AddServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDataService, DataService>();
            builder.Services.AddSingleton<IConnectionService, ConnectionService>();
            builder.Services.AddSingleton<IFileService, FileService>();
            builder.Services.AddSingleton<INetworkService, NetworkService>();
            builder.Services.AddSingleton<IPlatformPathService, AndroidPathService>();
            return builder;
        }

        public static MauiAppBuilder AddViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LandingPageViewModel>();
            builder.Services.AddTransient<JobPageViewModel>();

            //Tabs
            builder.Services.AddSingleton<OverviewPageViewModel>();
            builder.Services.AddSingleton<JobListPageViewModel>();
            builder.Services.AddTransient<NewJobPageViewModel>();
            return builder;
        }
    }
}
