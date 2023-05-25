using Models;
using Moq;
using Services;
using Shared;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers;
using Viewmodels.Tabs;
using Wrappers;
using Location = Models.Location;

namespace Tests.Viewmodels.Tabs;

public class OverviewPageViewModelTest
{
    private readonly OverviewPageViewModel _sut;
    private readonly Mock<INavigationWrapper> _navigatorMock;
    private readonly Mock<IDataService> _dataServiceMock;
    private readonly Mock<INetworkService> _networkServiceMock;

    public OverviewPageViewModelTest()
    {
        _navigatorMock = new Mock<INavigationWrapper>();
        _dataServiceMock = new Mock<IDataService>();
        _networkServiceMock = new Mock<INetworkService>();

        _sut = new OverviewPageViewModel(_navigatorMock.Object, _dataServiceMock.Object, _networkServiceMock.Object);
        //_dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
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
    public void Locations_SetLocations_ExpectLocationsSet()
    {
        var expected = DataHelper.GetLocation(1).Name;

        _sut.Locations = new ObservableRangeCollection<Location>()
        {
            DataHelper.GetLocation(1)
        };

        Assert.Equal(expected, _sut.Locations[0].Name);
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
    public async Task InitializeAsync_HandleUserIsNull_ExpectClearDataAndLogoutCalled()
    {
        _sut.CurrentUser = null;

        await _sut.InitializeAsync();

        _dataServiceMock.Verify(mock => mock.ClearDataAndLogout(), Times.Once);

    }

    [Fact]
    public async Task InitializeAsync_HandleAlreadyBusy_ExpectNothingCalled()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
        _sut.IsBusy = true;

        await _sut.InitializeAsync();

        _dataServiceMock.Verify(mock => mock.GetCurrentUserAsync(), Times.Never);
        _dataServiceMock.Verify(mock => mock.GetJobsAsync(), Times.Never);
        _dataServiceMock.Verify(mock => mock.GetLocationsAsync(), Times.Never);
    }

    [Fact]
    public async Task InitializeAsync_HandleNotBusy_ExpectMethodsCalled()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
        await _sut.InitializeAsync();

        _dataServiceMock.Verify(mock => mock.GetCurrentUserAsync(), Times.Once);
        _dataServiceMock.Verify(mock => mock.GetJobsAsync(), Times.Once);
        _dataServiceMock.Verify(mock => mock.GetLocationsAsync(), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_AddJobs_ExpectAddedToJobs()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
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
    public async Task InitializeAsync_Add3Jobs_ExpectCountOf3()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
        var jobs = new List<Job>()
        {
            new Job() {JobID = DataHelper.DummyGuid(1)},
            new Job() {JobID = DataHelper.DummyGuid(2)},
            new Job() {JobID = DataHelper.DummyGuid(3)},
            new Job() {JobID = DataHelper.DummyGuid(4)}
        };

        _dataServiceMock.Setup(mock => mock.GetJobsAsync()).ReturnsAsync(jobs);

        await _sut.InitializeAsync();

        Assert.Equal(3, _sut.Jobs.Count);
    }

    [Fact]
    public async Task InitializeAsync_ResetsJobList_ExpectToHaveCountOf2()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
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
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
        _dataServiceMock.Setup(mock => mock.GetJobsAsync()).ReturnsAsync(new List<Job>());

        await _sut.InitializeAsync();

        Assert.True(_sut.ShowEmptyView);
    }

    [Fact]
    public async Task InitializeAsync_JobsNotEmpty_ExpectShowEmptyViewFalse()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
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
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));

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

    [Fact]
    public async Task InitializeAsync_AddLocations_ExpectAddedToLocations()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
        var locations = new List<Location>()
        {
            DataHelper.GetLocation(1),
            DataHelper.GetLocation(2),
            DataHelper.GetLocation(3)
        };

        _dataServiceMock.Setup(mock => mock.GetLocationsAsync()).ReturnsAsync(locations);

        await _sut.InitializeAsync();

        var expected = new List<string>()
        {
            DataHelper.GetLocation(1).Name,
            DataHelper.GetLocation(2).Name,
            DataHelper.GetLocation(3).Name
        };

        Assert.Equal(3, _sut.Locations.Count);
        Assert.Contains(_sut.Locations, l => l.Name == expected[0]);
        Assert.Contains(_sut.Locations, l => l.Name == expected[1]);
        Assert.Contains(_sut.Locations, l => l.Name == expected[2]);
    }

    [Fact]
    public async Task InitializeAsync_ResetsLocationList_ExpectToHaveCountOf2()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
        _sut.Locations.Add(DataHelper.GetLocation(3));
        _sut.Locations.Add(DataHelper.GetLocation(4));
        _sut.Locations.Add(DataHelper.GetLocation(5));

        var locations = new List<Location>()
        {
            DataHelper.GetLocation(1),
            DataHelper.GetLocation(2),
        };

        _dataServiceMock.Setup(mock => mock.GetLocationsAsync()).ReturnsAsync(locations);

        await _sut.InitializeAsync();

        Assert.Equal(2, _sut.Locations.Count);
    }

    [Fact]
    public async Task InitializeAsync_SortsLocations_ExpectLocationsSortedByDistance()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));

        var locations = new List<Location>()
        {
            DataHelper.GetLocation(3),
            DataHelper.GetLocation(2),
            DataHelper.GetLocation(1)
        };

        _dataServiceMock.Setup(mock => mock.GetLocationsAsync()).ReturnsAsync(locations);

        await _sut.InitializeAsync();

        var expected = new List<string>()
        {
            DataHelper.GetLocation(1).Name,
            DataHelper.GetLocation(2).Name,
            DataHelper.GetLocation(3).Name
        };

        Assert.Equal(3, _sut.Locations.Count);
        Assert.Equal(expected[0], _sut.Locations[0].Name);
        Assert.Equal(expected[1], _sut.Locations[1].Name);
        Assert.Equal(expected[2], _sut.Locations[2].Name);
    }


    [Fact]
    public void ShowAllJobs_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.ShowAllJobsCommand);
    }

    [Fact]
    public async void ShowAllJobs_NavigatesToJobListPage_ExpectNavigationCalled()
    {
        _navigatorMock.Setup(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, It.IsAny<bool>()));

        await _sut.ShowAllJobsCommand.ExecuteAsync(null);

        _navigatorMock.Verify(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public void AccordianCollapse_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.AccordionCollapseCommand);
    }

    [Fact]
    public void AccordianCollapse_CollapsedIsTrue_ExpectCollapsedFalse()
    {
        _sut.Collapsed = true;

        _sut.AccordionCollapseCommand.Execute(null);

        Assert.False(_sut.Collapsed);
    }

    [Fact]
    public void AccordianCollapse_CollapsedIsFalse_ExpectCollapsedTrue()
    {
        _sut.Collapsed = false;

        _sut.AccordionCollapseCommand.Execute(null);

        Assert.True(_sut.Collapsed);
    }


    [Fact]
    public void DisconnectFromNetowrk_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.DisconnectFromNetworkCommand);
    }

    [Fact]
    public void DisconnectFromNetowrk_Disconnects_ExpectDisconnectCalled()
    {
        _sut.DisconnectFromNetworkCommand.Execute(null);

        _networkServiceMock.Verify(mock => mock.DisconnectFromNetwork(),Times.Once);
    }

    [Fact]
    public void DisconnectFromNetowrk_ConnectedTrue_ExpectConnectedFalse()
    {
        _sut.Connected = true;

        _sut.DisconnectFromNetworkCommand.Execute(null);

        Assert.False(_sut.Connected);
    }

}
