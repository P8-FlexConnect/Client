using Models;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public interface IConnectionService
{
    public Task LoginAsync(string username, string password);
    public Task<IEnumerable<Job>> GetJobsAsync();
    public Task<Job> GetJobByIdAsync(Guid id);
    public Task<Job> CreateJobAsync(string name, string description);
    public Task UpdateJobAsync(Job job);
    public Task CancelJobAsync(Job job);
    public Task<PrepareDto> PrepareJob(Job job);
    public Task StartJobAsync(Job job);
    public Task StopJobAsync(Job job);
    public Task<List<LocationDto>> GetLocations();
}
