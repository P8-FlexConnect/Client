using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Shared;
using Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewmodels;

public partial class LandingPageViewModel : BaseViewModel
{
    private readonly INavigationWrapper _navigator;
    private readonly IDataService _dataService;
    private readonly IConnectionService _connectionService;

    [ObservableProperty]
    public bool showError = false;

    public LandingPageViewModel(INavigationWrapper navigator, IDataService dataService, IConnectionService connectionService)
    {
        _navigator = navigator;
        _dataService = dataService;
        _connectionService = connectionService;
    }

    public async Task InitializeAsync()
    {
        if (IsBusy)
            return;
    }

    public async Task Login(string username, string password)
    {
        await _dataService.ClearDataAndLogout();

        IsBusy = true;
        await _connectionService.LoginAsync(username, password);
        IsBusy = false;

        var token = await _dataService.SecureStorageGetAsync(StorageKeys.AuthTokenKey);
        var user = await _dataService.GetCurrentUserAsync();
        if (token == null || user == null)
        {
            ShowError = true;
            return;
        }

        await _navigator.RouteAndReplaceStackAsync(NavigationKeys.OverviewPage, false);
    }
}