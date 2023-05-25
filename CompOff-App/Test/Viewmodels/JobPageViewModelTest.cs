using DataTransferObjects;
using Models;
using Moq;
using Services;
using Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers;
using Viewmodels;
using Viewmodels.Tabs;
using Wrappers;

namespace Tests.Viewmodels;

public class JobPageViewModelTest
{
    private readonly JobPageViewModel _sut;
    private readonly Mock<INavigationWrapper> _navigatorMock;
    private readonly Mock<IDataService> _dataServiceMock;
    private readonly Mock<IConnectionService> _connectionServiceMock;
    private readonly Mock<INetworkService> _networkServiceMock;
    private readonly Mock<IFileService> _fileServiceMock;

    public JobPageViewModelTest()
    {
        _navigatorMock = new Mock<INavigationWrapper>();
        _dataServiceMock = new Mock<IDataService>();
        _connectionServiceMock = new Mock<IConnectionService>();
        _networkServiceMock = new Mock<INetworkService>();
        _fileServiceMock = new Mock<IFileService>();

        _sut = new JobPageViewModel(_fileServiceMock.Object, _navigatorMock.Object, _dataServiceMock.Object, _networkServiceMock.Object, _connectionServiceMock.Object);

        _sut.ApplyQueryAttributes(new Dictionary<string, object>());
        _sut.ApplyQueryAttributes(new Dictionary<string, object>()
        {
            {"Job", DataHelper.GetJob(1) }
        });
    }

    [Fact]
    public void IsNotBusy_IsBusyTrue_ExpectIsNotBusyFalse()
    {
        _sut.IsBusy = true;

        Assert.False( _sut.IsNotBusy);
    }

    [Fact]
    public void IsNotBusy_IsBusyFalse_ExpectIsNotBusyTrue()
    {
        _sut.IsBusy = false;

        Assert.True(_sut.IsNotBusy);
    }

    [Fact]
    public void IsNotCollapsed_IsCollapsedTrue_ExpectIsNotCollapsedFalse()
    {
        _sut.Collapsed = true;

        Assert.False(_sut.IsNotCollapsed);
    }

    [Fact]
    public void IsNotCollapsed_IsCollapsedFalse_ExpectIsNotCollapsedTrue()
    {
        _sut.Collapsed = false;

        Assert.True(_sut.IsNotCollapsed);
    }

    [Fact]
    public void IsNotConnected_IsConnectedTrue_ExpectIsNotConnectedFalse()
    {
        _sut.Connected = true;

        Assert.False(_sut.IsNotConnected);
    }

    [Fact]
    public void IsNotConnected_IsConnectedFalse_ExpectIsNotConnectedTrue()
    {
        _sut.Connected = false;

        Assert.True(_sut.IsNotConnected);
    }

    [Fact]
    public void IsNotPreparing_IsPreparingTrue_ExpectIsNotPreparingFalse()
    {
        _sut.IsPreparing = true;

        Assert.False(_sut.IsNotPreparing);
    }

    [Fact]
    public void IsNotPreparing_IsPreparingFalse_ExpectIsNotPreparingTrue()
    {
        _sut.IsPreparing = false;

        Assert.True(_sut.IsNotPreparing);
    }

    [Fact]
    public async Task InitializeAsync_SetsCurrentJob_ExpectCurrentJobSet()
    {
        var expected = DataHelper.GetJob(1).JobID;

        _sut.CurrentJob = null;

        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));

        await _sut.InitializeAsync();

        var actual = _sut.CurrentJob.JobID;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InitializeAsync_HandleAlreadyBusy_ExpectNothingCalled()
    {

        _sut.IsBusy = true;

        await _sut.InitializeAsync();

        _dataServiceMock.Verify(mock => mock.GetJobByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task InitializeAsync_HandleAlreadyBusy_ExpectStatesNotSet()
    {
        _sut.IsWaiting = false;
        _sut.IsRunning = false;
        _sut.ShowResult = false;

        _sut.IsBusy = true;

        await _sut.InitializeAsync();

        Assert.False(_sut.IsWaiting);
        Assert.False(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task InitializeAsync_HandleNotBusy_ExpectGetJobByIdCalled()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));

        await _sut.InitializeAsync();

        _dataServiceMock.Verify(mock => mock.GetJobByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_CurrentJobStatusWaiting_ExpectIsWaitingTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Waiting});

        await _sut.InitializeAsync();

        Assert.True(_sut.IsWaiting);
        Assert.False(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task InitializeAsync_CurrentJobStatusRunning_ExpectIsRunningTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Running });

        await _sut.InitializeAsync();

        Assert.False(_sut.IsWaiting);
        Assert.True(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task InitializeAsync_CurrentJobStatusInQueue_ExpectIsRunningTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.InQueue});

        await _sut.InitializeAsync();

        Assert.False(_sut.IsWaiting);
        Assert.True(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task InitializeAsync_CurrentJobStatusDone_ExpectShowResultTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Done });

        await _sut.InitializeAsync();

        Assert.False(_sut.IsWaiting);
        Assert.False(_sut.IsRunning);
        Assert.True(_sut.ShowResult);
    }

    [Fact]
    public void Reload_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.ReloadCommand);
    }

    [Fact]
    public async Task Reload_SetsCurrentJob_ExpectCurrentJobSet()
    {
        var expected = DataHelper.GetJob(1).JobID;

        _sut.CurrentJob = null;

        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));

        await _sut.ReloadCommand.ExecuteAsync(null);

        var actual = _sut.CurrentJob.JobID;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Reload_CurrentJobStatusWaiting_ExpectIsWaitingTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Waiting });

        await _sut.ReloadCommand.ExecuteAsync(null);

        Assert.True(_sut.IsWaiting);
        Assert.False(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task Reload_CurrentJobStatusRunning_ExpectIsRunningTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Running });

        await _sut.ReloadCommand.ExecuteAsync(null);

        Assert.False(_sut.IsWaiting);
        Assert.True(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task Reload_CurrentJobStatusInQueue_ExpectIsRunningTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.InQueue });

        await _sut.ReloadCommand.ExecuteAsync(null);

        Assert.False(_sut.IsWaiting);
        Assert.True(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task Reload_CurrentJobStatusDone_ExpectShowResultTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Done });

        await _sut.ReloadCommand.ExecuteAsync(null);

        Assert.False(_sut.IsWaiting);
        Assert.False(_sut.IsRunning);
        Assert.True(_sut.ShowResult);
    }

    [Fact]
    public void Cancel_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.CancelCommand);
    }

    [Fact]
    public async void Cancel_CancelsJob_ExpectCancelJobCalledWithCurrentJob()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));

        await _sut.CancelCommand.ExecuteAsync(null);

        _dataServiceMock.Verify(mock => mock.CancelJobAsync(It.IsAny<Job>()), Times.Once);
    }

    [Fact]
    public async Task Cancel_SetsCurrentJob_ExpectCurrentJobSet()
    {
        var expected = DataHelper.GetJob(1).JobID;

        _sut.CurrentJob = DataHelper.GetJob(2);

        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));

        await _sut.CancelCommand.ExecuteAsync(null);

        var actual = _sut.CurrentJob.JobID;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Cancel_CurrentJobStatusWaiting_ExpectIsWaitingTrue()
    {
        _sut.CurrentJob = DataHelper.GetJob(2);

        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Waiting });

        await _sut.CancelCommand.ExecuteAsync(null);

        Assert.True(_sut.IsWaiting);
        Assert.False(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task Cancel_CurrentJobStatusRunning_ExpectIsRunningTrue()
    {
        _sut.CurrentJob = DataHelper.GetJob(2);

        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Running });

        await _sut.CancelCommand.ExecuteAsync(null);

        Assert.False(_sut.IsWaiting);
        Assert.True(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task Cancel_CurrentJobStatusInQueue_ExpectIsRunningTrue()
    {
        _sut.CurrentJob = DataHelper.GetJob(2);

        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.InQueue });

        await _sut.CancelCommand.ExecuteAsync(null);

        Assert.False(_sut.IsWaiting);
        Assert.True(_sut.IsRunning);
        Assert.False(_sut.ShowResult);
    }

    [Fact]
    public async Task Cancel_CurrentJobStatusDone_ExpectShowResultTrue()
    {
        _sut.CurrentJob = DataHelper.GetJob(2);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job { Status = JobStatus.Done });

        await _sut.CancelCommand.ExecuteAsync(null);

        Assert.False(_sut.IsWaiting);
        Assert.False(_sut.IsRunning);
        Assert.True(_sut.ShowResult);
    }

    [Fact]
    public void Start_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.StartCommand);
    }

    [Fact]
    public async Task Start_HandlePrepareDtoIsNull_ExpectShowWorkerErrorTrue()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);

        await _sut.StartCommand.ExecuteAsync(null);

        Assert.True(_sut.ShowWorkerError);
    }

    [Fact]
    public async Task Start_HandlePrepareDtoIsNull_ExpectMethodsNotCalled()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);

        await _sut.StartCommand.ExecuteAsync(null);

        Assert.True(_sut.ShowWorkerError);
    }

    [Fact]
    public async Task Start_StartJob_ExpectCurrentJobSet()
    {
        _connectionServiceMock.Setup(mock => mock.PrepareJob(It.IsAny<Job>())).ReturnsAsync(DataHelper.GetPrepareDto(1));
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job(DataHelper.GetPrepareDto(1).serviceTask));
        _fileServiceMock.Setup(mock => mock.UploadScript(It.IsAny<Job>())).Returns(true);
        var expected = DataHelper.GetPrepareDto(1).serviceTask.id;

        _sut.CurrentJob = DataHelper.GetJob(1);

        await _sut.StartCommand.ExecuteAsync(null);

        Assert.Equal(expected, _sut.CurrentJob.JobID);
    }

    [Fact]
    public async Task Start_StartJob_ExpectConnectToNetworkCalled()
    {
        var prepareDto = DataHelper.GetPrepareDto(1);
        _connectionServiceMock.Setup(mock => mock.PrepareJob(It.IsAny<Job>())).ReturnsAsync(prepareDto);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job(prepareDto.serviceTask));
        _fileServiceMock.Setup(mock => mock.UploadScript(It.IsAny<Job>())).Returns(true);

        _sut.CurrentJob = DataHelper.GetJob(1);

        await _sut.StartCommand.ExecuteAsync(null);

        _networkServiceMock.Verify(mock => mock.ConnectToNetwork(prepareDto.wifi.ssid, prepareDto.wifi.password),Times.Once);
    }

    [Fact]
    public async Task Start_JobPaused_ExpectUploadCheckpointCalled()
    {
        var prepareDto = DataHelper.GetPrepareDto(1);
        _connectionServiceMock.Setup(mock => mock.PrepareJob(It.IsAny<Job>())).ReturnsAsync(prepareDto);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job(prepareDto.serviceTask));
        _fileServiceMock.Setup(mock => mock.UploadCheckpointIsh(It.IsAny<Job>())).Returns(true);

        _sut.CurrentJob = DataHelper.GetJob(1);
        _sut.CurrentJob.Status = JobStatus.Paused;

        await _sut.StartCommand.ExecuteAsync(null);

        _fileServiceMock.Verify(mock => mock.UploadCheckpointIsh(It.IsAny<Job>()), Times.Once);
        _fileServiceMock.Verify(mock => mock.UploadScript(It.IsAny<Job>()), Times.Never);
    }

    [Fact]
    public async Task Start_JobPausedAndUploadSuccess_ExpectClearDataNotCalled()
    {
        var prepareDto = DataHelper.GetPrepareDto(1);
        _connectionServiceMock.Setup(mock => mock.PrepareJob(It.IsAny<Job>())).ReturnsAsync(prepareDto);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job(prepareDto.serviceTask));
        _fileServiceMock.Setup(mock => mock.UploadCheckpointIsh(It.IsAny<Job>())).Returns(true);

        _sut.CurrentJob = DataHelper.GetJob(1);
        _sut.CurrentJob.Status = JobStatus.Paused;

        await _sut.StartCommand.ExecuteAsync(null);

        _dataServiceMock.Verify(mock => mock.ClearDataAndLogout(), Times.Never);
    }

    [Fact]
    public async Task Start_JobPausedAndUploadSuccess_ExpectStartJobCalled()
    {
        var prepareDto = DataHelper.GetPrepareDto(1);
        _connectionServiceMock.Setup(mock => mock.PrepareJob(It.IsAny<Job>())).ReturnsAsync(prepareDto);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job(prepareDto.serviceTask));
        _fileServiceMock.Setup(mock => mock.UploadCheckpointIsh(It.IsAny<Job>())).Returns(true);

        _sut.CurrentJob = DataHelper.GetJob(1);
        _sut.CurrentJob.Status = JobStatus.Paused;

        await _sut.StartCommand.ExecuteAsync(null);

        _connectionServiceMock.Verify(mock => mock.StartJobAsync(It.IsAny<Job>()), Times.Once);
    }

    [Fact]
    public async Task Start_JobPausedAndUploadFaileds_ExpectClearDataCalled()
    {
        var prepareDto = DataHelper.GetPrepareDto(1);
        _connectionServiceMock.Setup(mock => mock.PrepareJob(It.IsAny<Job>())).ReturnsAsync(prepareDto);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job(prepareDto.serviceTask));
        _fileServiceMock.Setup(mock => mock.UploadCheckpointIsh(It.IsAny<Job>())).Returns(false);

        _sut.CurrentJob = DataHelper.GetJob(1);
        _sut.CurrentJob.Status = JobStatus.Paused;

        await _sut.StartCommand.ExecuteAsync(null);

        _dataServiceMock.Verify(mock => mock.ClearDataAndLogout(), Times.Once);
    }

    [Fact]
    public async Task Start_JobPausedAndUploadFailed_ExpectStartJobNotCalled()
    {
        var prepareDto = DataHelper.GetPrepareDto(1);
        _connectionServiceMock.Setup(mock => mock.PrepareJob(It.IsAny<Job>())).ReturnsAsync(prepareDto);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Job(prepareDto.serviceTask));
        _fileServiceMock.Setup(mock => mock.UploadCheckpointIsh(It.IsAny<Job>())).Returns(false);

        _sut.CurrentJob = DataHelper.GetJob(1);
        _sut.CurrentJob.Status = JobStatus.Paused;

        await _sut.StartCommand.ExecuteAsync(null);

        _connectionServiceMock.Verify(mock => mock.StartJobAsync(It.IsAny<Job>()), Times.Never);
    }

    [Fact]
    public void Back_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.BackCommand);
    }

    [Fact]
    public async void Back_NavigatesBack_ExpectNavigationCalled()
    {
        _navigatorMock.Setup(mock => mock.NavigateBackAsync(It.IsAny<bool>()));

        await _sut.BackCommand.ExecuteAsync(null);

        _navigatorMock.Verify(mock => mock.NavigateBackAsync(It.IsAny<bool>()),Times.Once);
    }


    [Fact]
    public void EditDescription_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.EditDescriptionCommand);
    }

    [Fact]
    public void EditDescription_EditDescription_ExpectNotEditingDescriptionFalse()
    {
        _sut.NotEditingDescription = true;

        _sut.EditDescriptionCommand.Execute(null);

        Assert.False(_sut.NotEditingDescription);
    }

    [Fact]
    public void SubmitDescriptionChange_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.SubmitDescriptionChangeCommand);
    }

    [Fact]
    public void SubmitDescriptionChange_DescriptionSubmitted_ExpectNotEditingDescription()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));

        _sut.SubmitDescriptionChangeCommand.Execute("Description");

        Assert.True(_sut.NotEditingDescription);
    }

    [Fact]
    public void SubmitDescriptionChange_DescriptionSubmitted_ExpectUpdateJobCalledWithDescription()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));

        _sut.SubmitDescriptionChangeCommand.Execute("Description");

        _dataServiceMock.Verify(mock => mock.UpdateJobAsync(It.Is<Job>(Job => Job.Description == "Description")));
    }

    [Fact]
    public void CancelDescriptionEdit_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.CancelDescriptionEditCommand);
    }

    [Fact]
    public void CancelDescriptionEdit_DescriptionEditCancelled_ExpectNotEditingDescription()
    {

        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));

        _sut.NotEditingDescription = false;

        _sut.CancelDescriptionEditCommand.Execute(null);

        Assert.True(_sut.NotEditingDescription);
    }

    [Fact]
    public void DownloadResult_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.DownloadResultCommand);
    }

    [Fact]
    public void DownloadResult_DownloadsResult_ExpectDownloadCalledWithCurrentJob()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);

        var expected = DataHelper.GetJob(1).JobID;

        _sut.DownloadResultCommand.ExecuteAsync(null);

        _fileServiceMock.Verify(mock => mock.DownloadScript(It.Is<Job>(job => job.JobID == expected)));
    }

    [Fact]
    public void DownloadResult_DownloadsResultIsSuccess_ExpectClearAndLogoutNotCalled()
    {
        _fileServiceMock.Setup(mock => mock.DownloadScript(It.IsAny<Job>())).Returns(true);
        _sut.CurrentJob = DataHelper.GetJob(1);

        var expected = DataHelper.GetJob(1).JobID;

        _sut.DownloadResultCommand.ExecuteAsync(null);

        _dataServiceMock.Verify(mock => mock.ClearDataAndLogout(), Times.Never);
    }

    [Fact]
    public void DownloadResult_DownloadsResultFailed_ExpectClearAndLogoutCalled()
    {
        _fileServiceMock.Setup(mock => mock.DownloadScript(It.IsAny<Job>())).Returns(false);
        _sut.CurrentJob = DataHelper.GetJob(1);

        var expected = DataHelper.GetJob(1).JobID;

        _sut.DownloadResultCommand.ExecuteAsync(null);

        _dataServiceMock.Verify(mock => mock.ClearDataAndLogout(), Times.Once);
    }

    [Fact]
    public void Stop_CheckCommandExistence_NotNull()
    {
        Assert.NotNull(_sut.StopCommand);
    }

    [Fact]
    public async Task Stop_DownloadsCheckpoint_ExpectDownloadCalledWithCurrentJob()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));
        _fileServiceMock.Setup(mock => mock.DownloadCheckpointIsh(It.IsAny<Job>())).Returns(true);
        var expected = DataHelper.GetJob(1).JobID;

        await _sut.StopCommand.ExecuteAsync(null);

        _fileServiceMock.Verify(mock => mock.DownloadCheckpointIsh(It.Is<Job>(job => job.JobID == expected)), Times.Once);
    }

    [Fact]
    public async Task Stop_DownloadCheckpointSuccess_ExpectClearAndLogoutNotCalled()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));
        _fileServiceMock.Setup(mock => mock.DownloadCheckpointIsh(It.IsAny<Job>())).Returns(true);

        await _sut.StopCommand.ExecuteAsync(null);

        _dataServiceMock.Verify(mock => mock.ClearDataAndLogout(), Times.Never);
    }

    [Fact]
    public async Task Stop_DownloadCheckpointSuccess_ExpectStopJobCalledWithCurrentJob()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));
        _fileServiceMock.Setup(mock => mock.DownloadCheckpointIsh(It.IsAny<Job>())).Returns(true);
        var expected = DataHelper.GetJob(1).JobID;

        await _sut.StopCommand.ExecuteAsync(null);

        _connectionServiceMock.Verify(mock => mock.StopJobAsync(It.Is<Job>(job => job.JobID == expected)), Times.Once);
    }

    [Fact]
    public async Task Stop_DownloadCheckpointFailed_ExpectClearAndLogoutCalled()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));
        _fileServiceMock.Setup(mock => mock.DownloadCheckpointIsh(It.IsAny<Job>())).Returns(false);

        await _sut.StopCommand.ExecuteAsync(null);

        _dataServiceMock.Verify(mock => mock.ClearDataAndLogout(), Times.Once);
    }

    [Fact]
    public async Task Stop_DownloadCheckpointSuccess_ExpectStopJobCalledWitCurrentJob()
    {
        _sut.CurrentJob = DataHelper.GetJob(1);
        _dataServiceMock.Setup(mock => mock.GetJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DataHelper.GetJob(1));
        _fileServiceMock.Setup(mock => mock.DownloadCheckpointIsh(It.IsAny<Job>())).Returns(false);
        var expected = DataHelper.GetJob(1).JobID;

        await _sut.StopCommand.ExecuteAsync(null);

        _connectionServiceMock.Verify(mock => mock.StopJobAsync(It.Is<Job>(job => job.JobID == expected)), Times.Once);
    }

}
