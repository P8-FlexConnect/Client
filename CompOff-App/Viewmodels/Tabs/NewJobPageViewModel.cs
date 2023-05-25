using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;
using Shared;
using Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewmodels.Tabs;

public partial class NewJobPageViewModel : BaseViewModel
{
    private readonly INavigationWrapper _navigator; 
    private readonly IDataService _dataService;
    private readonly IFileService _fileService;

    public NewJobPageViewModel(INavigationWrapper navigator, IDataService dataService, IFileService fileService)
    {
        _navigator = navigator;
        _dataService = dataService;
        _fileService = fileService;
    }

    [ObservableProperty]
    public string fileName = "No file chosen...";

    [ObservableProperty]
    public bool showTitleError = false;

    [ObservableProperty]
    public bool showDescriptionError = false;

    [ObservableProperty]
    public bool showFileError = false;



    private string _filePath = string.Empty;

    public async Task InitializeAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;
        ResetErrors();
        await LoadCurrentUser();
        IsBusy = false;
    }

    private async Task LoadCurrentUser()
    {
        CurrentUser = await _dataService.GetCurrentUserAsync();
    }

    private void ResetErrors()
    {
        ShowTitleError = false;
        ShowDescriptionError = false;
        ShowFileError = false;
    }

    private void Clear()
    {
        FileName = "No file chosen...";
        _filePath = string.Empty;
        ResetErrors();
    }

    public async Task AddJob(string name, string description)
    {
        ShowTitleError = String.IsNullOrWhiteSpace(name);
        ShowDescriptionError = String.IsNullOrWhiteSpace(description);
        ShowFileError = String.IsNullOrWhiteSpace(_filePath);

        if (ShowTitleError || ShowDescriptionError || ShowFileError)
            return;
        var job = await _dataService.AddJobAsync(name, description);
        _fileService.AddScript(job.JobID, _filePath);
        Clear();
        await _navigator.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, false);
    }

    [RelayCommand]
    public async Task PickFile(object arg)
    {
        await PickAndShow();
    }

    private async Task PickAndShow()
    {
        try
        {
            var script = await _fileService.PickFile();
            if (script != null)
            {
                if (!script.FileName.EndsWith("py", StringComparison.OrdinalIgnoreCase))
                {
                    ShowFileError = true;
                    return;
                }
                    FileName = script.FileName;
                _filePath = script.FullPath;
            }

        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }
    }
}
