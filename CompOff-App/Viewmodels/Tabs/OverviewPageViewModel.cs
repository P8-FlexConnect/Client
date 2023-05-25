using Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Shared;
using Services;
using Shared.Common;
using Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Services.Impl;

namespace Viewmodels.Tabs;

public partial class OverviewPageViewModel : BaseViewModel
{
    private readonly INavigationWrapper _navigator;
    private readonly IDataService _dataService;
    private readonly INetworkService _networkService;


    private const int NUMBER_OF_JOBS_SHOWN = 3;

    public ObservableRangeCollection<Job> Jobs { get; set; } = new();

    public ObservableRangeCollection<Models.Location> Locations { get; set; } = new();

    [ObservableProperty]
    public bool showEmptyView = false;

    public OverviewPageViewModel(INavigationWrapper navigator, IDataService dataService, INetworkService networkService)
    {
        _navigator = navigator;
        _dataService = dataService;
        _networkService = networkService;
    }

    public async Task InitializeAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;
        Jobs.Clear();
        Locations.Clear();
        await LoadCurrentUser();

        if(CurrentUser == null)
        {
            await _dataService.ClearDataAndLogout();
            return;
        }
        List<Task> tasks = new()
        {
            LoadLatestJobs(),
            LoadLocations()
        };

        await Task.WhenAll(tasks);
        IsBusy = false;
    }

    private async Task LoadLatestJobs()
    {
        var jobList = await _dataService.GetJobsAsync();
        var orderedList = jobList.OrderByDescending(x => x.LastActivity).ToList().Take(NUMBER_OF_JOBS_SHOWN);

        Jobs.AddRange(orderedList);
        if (Jobs.Any())
            ShowEmptyView = false;
        else 
            ShowEmptyView = true;
    }

    private async Task LoadLocations()
    {
        var locationList = await _dataService.GetLocationsAsync();
        locationList = locationList.OrderBy(x => x.Distance);
        Locations.AddRange(locationList);
    }

    private async Task LoadCurrentUser()
    {
        CurrentUser = await _dataService.GetCurrentUserAsync();
    }

    [RelayCommand]
    public async Task JobClicked(Job job)
    {
        await _navigator.RouteAsync(NavigationKeys.JobPage, new Dictionary<string, object>
        {
            {"Job", job }
        });
    }

    [RelayCommand]
    public async Task ShowAllJobs(object arg)
    {
        await _navigator.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, false);
    }

    [RelayCommand]
    public void AccordionCollapse(object arg)
    {
        Collapsed = !Collapsed;
        OnPropertyChanged(nameof(Collapsed));
    }

    [RelayCommand]
    public void DisconnectFromNetwork(object arg)
    {
        _networkService.DisconnectFromNetwork();

        Connected = !Connected;
        OnPropertyChanged(nameof(Connected));
    }
}
