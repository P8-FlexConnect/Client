using DataTransferObjects;
using Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Models.Mappers;

namespace Services.Impl;

public class ConnectionService : IConnectionService
{
    private readonly string baseUri = "http://192.168.1.5:5000/api/"; 
    private readonly HttpClient _httpClient = new();

    public ConnectionService()
    {
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task LoginAsync(string username, string password)
    {
        await GetToken(username, password);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync(StorageKeys.AuthTokenKey));
        await GetUser();
    }

    private async Task GetToken(string username, string password)
    {
        try
        {
            var logindto = new LoginInfoDto(username, password);
            var loginInfoJson = JsonSerializer.Serialize(logindto);
            var uri = baseUri + "auth/createtoken";
            StringContent content = new(loginInfoJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var tokenString = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenDto>(tokenString).token;
            await SecureStorage.SetAsync(StorageKeys.AuthTokenKey, token);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task GetUser()
    {
        try
        {
            var uri = baseUri + "Client/GetClient";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadAsStringAsync();
            await SecureStorage.SetAsync(StorageKeys.UserKey, user);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Job>> GetJobsAsync()
    {
        try
        {
            var uri = baseUri + "ServiceTask/Tasks";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var jobListString = await response.Content.ReadAsStringAsync();
            var jobs = JsonSerializer.Deserialize<List<JobDto>>(jobListString);
            var jobList = jobs.Select(j => new Job(j)).ToList();
            return jobList;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<Job> GetJobByIdAsync(Guid id)
    {
        try
        {
            var uri = baseUri + $"ServiceTask/Task?taskId={id}";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var jobString = await response.Content.ReadAsStringAsync();
            var job = JsonSerializer.Deserialize<JobDto>(jobString);
            return new Job(job);
        }
        catch (Exception e)
        {
            return new Job();
        }
    }

    public async Task<Job> CreateJobAsync(string name, string description)
    {
        try
        {
            var uri = baseUri + $"ServiceTask/Create?name={name}&description={description}'";
            HttpResponseMessage response = await _httpClient.PostAsync(uri, new StringContent(""));
            response.EnsureSuccessStatusCode();
            var jobString = await response.Content.ReadAsStringAsync();
            var job = JsonSerializer.Deserialize<JobDto>(jobString);
            return new Job(job);
        }
        catch (Exception e)
        {
            return new Job();
        }
    }
    
    public async Task UpdateJobAsync(Job job)
    {
        try
        {
            var jobDto = JobToJobDtoMapper.Map(job);
            var jobDtoString = JsonSerializer.Serialize(jobDto);
            var uri = baseUri + "ServiceTask/Update";
            StringContent content = new(jobDtoString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
        }
    }

    public async Task CancelJobAsync(Job job)
    {
        try
        {
            var uri = baseUri + $"ServiceTask/Cancel";
            var jobDto = JobToJobDtoMapper.Map(job);
            var jobDtoString = JsonSerializer.Serialize(jobDto);
            StringContent content = new(jobDtoString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
        }
    }

    public async Task StartJobAsync(Job job)
    {
        try
        {
            var uri = baseUri + $"ServiceTask/Start?taskId={job.JobID}";
            StringContent content = new("");
            HttpResponseMessage response = await _httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine();
        }
    }

    public async Task<PrepareDto> PrepareJob(Job job)
    {
        try
        {
            var uri = baseUri + $"ServiceTask/Prepare?taskId={job.JobID}";
            StringContent content = new("");
            HttpResponseMessage response = await _httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var prepareString = await response.Content.ReadAsStringAsync();
            var prepareDto = JsonSerializer.Deserialize<PrepareDto>(prepareString);
            return prepareDto;

        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task StopJobAsync(Job job)
    {
        try
        {
            var uri = baseUri + $"ServiceTask/Stop?taskId={job.JobID}";
            StringContent content = new("");
            HttpResponseMessage response = await _httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine();
        }
    }

    public async Task<List<LocationDto>> GetLocations()
    {
        try
        {
            var uri = baseUri + "Location/Locations";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var locations = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<LocationDto>>(locations);
            
        }
        catch (Exception e)
        {
            return new List<LocationDto>();
        }
    }
}
