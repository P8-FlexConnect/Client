using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services;

public interface IDataService
{
    public Task<User> GetCurrentUserAsync();
    public Task UpdateJobAsync(Job job);
    public Task CancelJobAsync(Job job);
    public Task<IEnumerable<Job>> GetJobsAsync();
    public Task<IEnumerable<Models.Location>> GetLocationsAsync();
    public Task<Job> GetJobByIdAsync(Guid id);
    public Task<Job> AddJobAsync(string name, string description);
    public Task ClearDataAndLogout();
    public Task<string> SecureStorageGetAsync(string key);
    public Task SecureStorageSetAsync(string key, string value);
}
