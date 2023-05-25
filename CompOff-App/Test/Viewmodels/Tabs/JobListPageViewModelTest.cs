using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewmodels.Tabs;
using Wrappers;
using Tests.Helpers;
using Shared;
using Models;
using Shared.Common;

namespace Tests.Viewmodels.Tabs;

public class JobListPageViewModelTest
{
    private readonly JobListPageViewModel _sut;
    private readonly Mock<INavigationWrapper> _navigatorMock;
    private readonly Mock<IDataService> _dataServiceMock;

    public JobListPageViewModelTest()
    {
        _navigatorMock = new Mock<INavigationWrapper>();
        _dataServiceMock = new Mock<IDataService>();

        _sut = new JobListPageViewModel(_navigatorMock.Object, _dataServiceMock.Object);
    }

    [Fact]
    public void Jobs_SetJobs_ExpectJobsSet()
    {
        var expected = DataHelper.GetJob(1).JobID;

        _sut.Jobs = new ObservableRangeCollection<Job>()
        {
            DataHelper.GetJob(1)
        };

        Assert.Equal(expected, _sut.Jobs[0].JobID);
    }

    [Fact]
    public void JobClicked_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.JobClickedCommand);
    }

    [Fact]
    public async void JobClicked_NavigatesToJob_ExpectNavigationCalledWithJob()
    {
        _navigatorMock.Setup(mock => mock.RouteAsync(NavigationKeys.JobPage, It.IsAny<Dictionary<string, object>>(), It.IsAny<bool>()));

        await _sut.JobClickedCommand.ExecuteAsync(new Job());

        _navigatorMock.Verify(mock => mock.RouteAsync(NavigationKeys.JobPage, It.Is<Dictionary<string, object>>(dict => dict.ContainsKey("Job")), It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_SetsCurrentUser_ExpectCurrentUserSet()
    {
        var expected = DataHelper.GetUser(1).UserName;
        _sut.CurrentUser = null;

        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));

        await _sut.InitializeAsync();

        var actual = _sut.CurrentUser?.UserName;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InitializeAsync_HandleAlreadyBusy_ExpectNothingCalled()
    {
        _sut.IsBusy = true;

        await _sut.InitializeAsync();

        _dataServiceMock.Verify(mock => mock.GetCurrentUserAsync(), Times.Never);
        _dataServiceMock.Verify(mock => mock.GetJobsAsync(), Times.Never);
    }

    [Fact]
    public async Task InitializeAsync_HandleNotBusy_ExpectMethodsCalled()
    {
        await _sut.InitializeAsync();

        _dataServiceMock.Verify(mock => mock.GetCurrentUserAsync(), Times.Once);
        _dataServiceMock.Verify(mock => mock.GetJobsAsync(), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_AddJobs_ExpectAddedToJobs()
    {
        var jobs = new List<Job>()
        {
            new Job() {JobID = DataHelper.DummyGuid(1)},
            new Job() {JobID = DataHelper.DummyGuid(2)},
        };

        _dataServiceMock.Setup(mock => mock.GetJobsAsync()).ReturnsAsync(jobs);

        await _sut.InitializeAsync();

        var expected = new List<Guid>() { DataHelper.DummyGuid(1), DataHelper.DummyGuid(2) };
        Assert.Equal(2, _sut.Jobs.Count);
        Assert.Contains(_sut.Jobs, j => j.JobID == expected[0]);
        Assert.Contains(_sut.Jobs, j => j.JobID == expected[1]);
    }

    [Fact]
    public async Task InitializeAsync_ResetsJobList_ExpectToHaveCountOf2()
    {
        _sut.Jobs.Add(new Job() { JobID = DataHelper.DummyGuid(1) });
        _sut.Jobs.Add(new Job() { JobID = DataHelper.DummyGuid(2) });
        _sut.Jobs.Add(new Job() { JobID = DataHelper.DummyGuid(3) });

        var jobs = new List<Job>()
        {
            new Job() {JobID = DataHelper.DummyGuid(1)},
            new Job() {JobID = DataHelper.DummyGuid(2)},
        };

        _dataServiceMock.Setup(mock => mock.GetJobsAsync()).ReturnsAsync(jobs);

        await _sut.InitializeAsync();

        Assert.Equal(2, _sut.Jobs.Count);
    }

    [Fact]
    public async Task InitializeAsync_JobsEmpty_ExpectShowEmptyViewTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobsAsync()).ReturnsAsync(new List<Job>());

        await _sut.InitializeAsync();

        Assert.True(_sut.ShowEmptyView);
    }

    [Fact]
    public async Task InitializeAsync_JobsNotEmpty_ExpectShowEmptyViewFalse()
    {
        var jobs = new List<Job>()
        {
            new Job() {JobID = DataHelper.DummyGuid(1)},
            new Job() {JobID = DataHelper.DummyGuid(2)},
        };
        _dataServiceMock.Setup(mock => mock.GetJobsAsync()).ReturnsAsync(jobs);

        await _sut.InitializeAsync();

        Assert.False(_sut.ShowEmptyView);
    }

    [Fact]
    public async Task InitializeAsync_SortsJobs_ExpectJobsSortedByLastActivity()
    {
        var jobs = new List<Job>()
        {
            new Job() {JobID = DataHelper.DummyGuid(1), LastActivity = DataHelper.DummyDateTime(3)},
            new Job() {JobID = DataHelper.DummyGuid(2), LastActivity = DataHelper.DummyDateTime(2)},
            new Job() {JobID = DataHelper.DummyGuid(3), LastActivity = DataHelper.DummyDateTime(1)}
        };

        _dataServiceMock.Setup(mock => mock.GetJobsAsync()).ReturnsAsync(jobs);

        await _sut.InitializeAsync();

        var expected = new List<Guid>()
        {
            DataHelper.DummyGuid(3),
            DataHelper.DummyGuid(2),
            DataHelper.DummyGuid(1)
        };

        Assert.Equal(3, _sut.Jobs.Count);
        Assert.Equal(expected[0], _sut.Jobs[0].JobID);
        Assert.Equal(expected[1], _sut.Jobs[1].JobID);
        Assert.Equal(expected[2], _sut.Jobs[2].JobID);
    }
}
