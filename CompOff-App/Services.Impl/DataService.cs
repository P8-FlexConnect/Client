using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Services;
using Wrappers;
using Models;
using Shared;
using DataTransferObjects;

namespace Services.Impl;

public class DataService : IDataService
{
    private readonly INavigationWrapper _navigator;
    private readonly IConnectionService _connectionService;

    public DataService(INavigationWrapper navigator, IConnectionService connectionService)
    {
        _navigator = navigator;
        _connectionService = connectionService;
    }
    //Create a lot of mockup data to test these methods on

    public async Task<User> GetCurrentUserAsync()
    {
        var userDtoString = await SecureStorage.GetAsync(StorageKeys.UserKey);
        if (userDtoString == null)
        {
            return null;
        }
        var userDto = JsonSerializer.Deserialize<UserDto>(userDtoString);
        return new User(userDto);
    }

    public async Task<IEnumerable<Job>> GetJobsAsync()
    {
        return await _connectionService.GetJobsAsync();
    }

    public async Task<IEnumerable<Models.Location>> GetLocationsAsync()
    {
        var locationDtos = await _connectionService.GetLocations();
        var userLocation = await Geolocation.Default.GetLastKnownLocationAsync();
        List<Models.Location> locations = locationDtos.Select(l => new Models.Location(l, userLocation)).ToList();
        return locations;
    }

    public async Task<Job> GetJobByIdAsync(Guid id)
    {
        return await _connectionService.GetJobByIdAsync(id);
    }

    public async Task UpdateJobAsync(Job job)
    {
        await _connectionService.UpdateJobAsync(job);
    }

    public async Task CancelJobAsync(Job job)
    {
        await _connectionService.CancelJobAsync(job);
    }

    public async Task<Job> AddJobAsync(string name, string description)
    {
        return await _connectionService.CreateJobAsync(name, description);
    }

    public async Task ClearDataAndLogout()
    {
        SecureStorage.RemoveAll();
        await _navigator.RouteAndReplaceStackAsync(NavigationKeys.LandingPage);
    }

    public async Task<string> SecureStorageGetAsync(string key)
    {
        return await SecureStorage.GetAsync(key);
    }

    public async Task SecureStorageSetAsync(string key, string value)
    {
        await SecureStorage.SetAsync(key, value);
    }
}
