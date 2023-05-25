using Models;
using Moq;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers;
using Viewmodels.Tabs;
using Wrappers;

namespace Tests.Viewmodels.Tabs;

public class NewJobPageViewModelTest
{
    private readonly NewJobPageViewModel _sut;
    private readonly Mock<INavigationWrapper> _navigatorMock;
    private readonly Mock<IDataService> _dataServiceMock;
    private readonly Mock<IFileService> _fileServiceMock;

    public NewJobPageViewModelTest()
    {
        _navigatorMock = new Mock<INavigationWrapper>();
        _dataServiceMock = new Mock<IDataService>();
        _fileServiceMock = new Mock<IFileService>();

        _sut = new NewJobPageViewModel(_navigatorMock.Object, _dataServiceMock.Object, _fileServiceMock.Object);
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
    }

    [Fact]
    public async Task InitializeAsync_HandleAlreadyBusy_ExpectNothingReset()
    {
        _sut.ShowDescriptionError = true;
        _sut.ShowTitleError = true;
        _sut.ShowFileError = true;

        _sut.IsBusy = true;

        await _sut.InitializeAsync();

        Assert.True(_sut.ShowDescriptionError);
        Assert.True(_sut.ShowTitleError);
        Assert.True(_sut.ShowFileError);
    }

    [Fact]
    public async Task InitializeAsync_HandleNotBusy_ExpectMethodsCalled()
    {
        await _sut.InitializeAsync();

        _dataServiceMock.Verify(mock => mock.GetCurrentUserAsync(), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_ResetsErrors_ExpectErrorsReset()
    {
        _sut.ShowDescriptionError = true;
        _sut.ShowTitleError = true;
        _sut.ShowFileError = true;

        await _sut.InitializeAsync();

        Assert.False(_sut.ShowDescriptionError);
        Assert.False(_sut.ShowTitleError);
        Assert.False(_sut.ShowFileError);
    }

    [Fact]
    public void PickFile_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.PickFileCommand);
    }

    [Fact]
    public void PickFile_FileIsNull_ExpectFileNameNotChanged()
    {
        var expected = DataHelper.GetScript(1).FileName;
        _sut.FileName = expected;

        _sut.PickFileCommand.ExecuteAsync(null);

        Assert.Equal(_sut.FileName, expected);
    }

    [Fact]
    public void PickFile_FileIsWrongFormat_ExpectFileNameNotChanged()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetIncorrectScript(1));
        var expected = DataHelper.GetScript(1).FileName;
        _sut.FileName = expected;

        _sut.PickFileCommand.ExecuteAsync(null);

        Assert.Equal(_sut.FileName, expected);
    }

    [Fact]
    public void PickFile_FileIsWrongFormat_ExpectShowFileError()
    {
        _sut.ShowFileError = false;

        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetIncorrectScript(1));

        _sut.PickFileCommand.ExecuteAsync(null);

        Assert.True(_sut.ShowFileError);
    }

    [Fact]
    public void PickFile_FileIsPicked_ExpectFileNameChanged()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));
        var expected = DataHelper.GetScript(1).FileName;
        _sut.FileName = DataHelper.GetScript(2).FileName;

        _sut.PickFileCommand.ExecuteAsync(null);

        Assert.Equal(_sut.FileName, expected);
    }

    [Fact]
    public async Task AddJob_NoName_ExpectShowTitleError()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));
        _sut.ShowDescriptionError = false;
        _sut.ShowTitleError = false;
        _sut.ShowFileError = false;

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("", "Description");

        Assert.True(_sut.ShowTitleError);
        Assert.False(_sut.ShowFileError);
        Assert.False(_sut.ShowDescriptionError);
    }

    [Fact]
    public async Task AddJob_NoName_ExpectMethodsNotCalled()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("", "Description");

        _dataServiceMock.Verify(mock => mock.AddJobAsync(It.IsAny<string>(), It.IsAny<string>()),Times.Never);
        _fileServiceMock.Verify(mock => mock.AddScript(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        _navigatorMock.Verify(mock => mock.RouteAndReplaceStackAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task AddJob_NoName_ExpectFileNameNotReset()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));

        _sut.PickFileCommand.Execute(null);

        var expected = DataHelper.GetScript(1).FileName;

        _sut.FileName = expected;

        await _sut.AddJob("", "Description");

        Assert.Equal(expected, _sut.FileName);
    }

    [Fact]
    public async Task AddJob_NoDescription_ExpectShowDescriptionError()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));
        _sut.ShowDescriptionError = false;
        _sut.ShowTitleError = false;
        _sut.ShowFileError = false;

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("Name", "");

        Assert.False(_sut.ShowTitleError);
        Assert.False(_sut.ShowFileError);
        Assert.True(_sut.ShowDescriptionError);
    }

    [Fact]
    public async Task AddJob_NoDescription_ExpectMethodsNotCalled()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("Name", "");

        _dataServiceMock.Verify(mock => mock.AddJobAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _fileServiceMock.Verify(mock => mock.AddScript(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        _navigatorMock.Verify(mock => mock.RouteAndReplaceStackAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task AddJob_NoDescription_ExpectFileNameNotReset()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));

        _sut.PickFileCommand.Execute(null);

        var expected = DataHelper.GetScript(1).FileName;

        _sut.FileName = expected;

        await _sut.AddJob("Name", "");

        Assert.Equal(expected, _sut.FileName);
    }

    [Fact]
    public async Task AddJob_NoFile_ExpectShowFileError()
    {
        _sut.ShowDescriptionError = false;
        _sut.ShowTitleError = false;
        _sut.ShowFileError = false;

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("Name", "Description");

        Assert.False(_sut.ShowTitleError);
        Assert.True(_sut.ShowFileError);
        Assert.False(_sut.ShowDescriptionError);
    }

    [Fact]
    public async Task AddJob_NoFile_ExpectMethodsNotCalled()
    {
        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("Name", "Description");

        _dataServiceMock.Verify(mock => mock.AddJobAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _fileServiceMock.Verify(mock => mock.AddScript(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        _navigatorMock.Verify(mock => mock.RouteAndReplaceStackAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task AddJob_NoFile_ExpectFileNameNotReset()
    {
        _sut.PickFileCommand.Execute(null);

        var expected = DataHelper.GetScript(1).FileName;

        _sut.FileName = expected;

        await _sut.AddJob("Name", "Description");

        Assert.Equal(expected, _sut.FileName);
    }

    [Fact]
    public async Task AddJob_AllFilled_ExpectNoErrors()
    {

        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));
        _dataServiceMock.Setup(mock => mock.AddJobAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Job() { JobID = DataHelper.DummyGuid(1) });
        _navigatorMock.Setup(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, It.IsAny<bool>()));

        _sut.ShowDescriptionError = true;
        _sut.ShowTitleError = true;
        _sut.ShowFileError = true;

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("Name", "Description");

        Assert.False(_sut.ShowTitleError);
        Assert.False(_sut.ShowFileError);
        Assert.False(_sut.ShowDescriptionError);
    }

    [Fact]
    public async Task AddJob_AllFilled_ExpectAddJobWithNameAndDescription()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));
        _dataServiceMock.Setup(mock => mock.AddJobAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Job() { JobID = DataHelper.DummyGuid(1) });
        _navigatorMock.Setup(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, It.IsAny<bool>()));

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("Name", "Description");

        _dataServiceMock.Verify(mock => mock.AddJobAsync("Name", "Description"), Times.Once);
    }

    [Fact]
    public async Task AddJob_AllFilled_ExpectAddScriptWithJobIdAndFilePath()
    {

        
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));
        _dataServiceMock.Setup(mock => mock.AddJobAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Job() { JobID = DataHelper.DummyGuid(1) });
        _navigatorMock.Setup(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, It.IsAny<bool>()));

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("Name", "Description");
        var expectedId = DataHelper.DummyGuid(1);
        var expectedPath = DataHelper.GetScript(1).FullPath;

        _fileServiceMock.Verify(mock => mock.AddScript(expectedId, expectedPath), Times.Once);
    }

    [Fact]
    public async Task AddJob_AllFilled_ExpectNavigationCalled()
    {


        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));
        _dataServiceMock.Setup(mock => mock.AddJobAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Job() { JobID = DataHelper.DummyGuid(1) });
        _navigatorMock.Setup(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, It.IsAny<bool>()));

        _sut.PickFileCommand.Execute(null);

        await _sut.AddJob("Name", "Description");

        _navigatorMock.Verify(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task AddJob_AllFilled_ExpectFileNameReset()
    {
        _fileServiceMock.Setup(mock => mock.PickFile()).ReturnsAsync(DataHelper.GetScript(1));
        _dataServiceMock.Setup(mock => mock.AddJobAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Job() { JobID = DataHelper.DummyGuid(1) });
        _navigatorMock.Setup(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.JobListPage, It.IsAny<bool>()));

        _sut.PickFileCommand.Execute(null);

        var expected = "No file chosen...";

        _sut.FileName = DataHelper.GetScript(1).FileName;

        await _sut.AddJob("Name", "Description");

        Assert.Equal(expected, _sut.FileName);
    }
}
