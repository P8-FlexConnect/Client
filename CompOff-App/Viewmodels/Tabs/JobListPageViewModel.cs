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
using System.Windows.Input;

namespace Viewmodels.Tabs;

public partial class JobListPageViewModel : BaseViewModel
{
    private readonly INavigationWrapper _navigator;
    private readonly IDataService _dataService;

    public ObservableRangeCollection<Job> Jobs { get; set; } = new();

    [ObservableProperty]
    public bool showEmptyView = false;

    public JobListPageViewModel(INavigationWrapper navigator, IDataService dataService)
    {
        _navigator = navigator;
        _dataService = dataService;
    }

    public async Task InitializeAsync()
    {
        if (IsBusy)
            return;
        IsBusy = true;
        Jobs.Clear();
        await LoadCurrentUser();
        List<Task> tasks = new() { LoadLatestJobs() };
        await Task.WhenAll(tasks);
        IsBusy = false;
    }

    private async Task LoadCurrentUser()
    {
        CurrentUser = await _dataService.GetCurrentUserAsync();
    }

    private async Task LoadLatestJobs()
    {
        var jobList = await _dataService.GetJobsAsync();
        var orderedList = jobList.OrderByDescending(x => x.LastActivity).ToList();
        Jobs.AddRange(orderedList);

        if (Jobs.Any())
            ShowEmptyView = false;
        else
            ShowEmptyView = true;
    }

    [RelayCommand]
    public async Task JobClicked(Job job)
    {
        await _navigator.RouteAsync(NavigationKeys.JobPage, new Dictionary<string, object>
        {
            {"Job", job }
        });
    }


}
