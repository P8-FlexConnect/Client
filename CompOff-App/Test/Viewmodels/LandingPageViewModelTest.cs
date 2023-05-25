using Moq;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers;
using Viewmodels;
using Wrappers;

namespace Tests.Viewmodels;

public class LandingPageViewModelTest
{
    private readonly LandingPageViewModel _sut;
    private readonly Mock<INavigationWrapper> _navigatorMock;
    private readonly Mock<IDataService> _dataServiceMock;
    private readonly Mock<IConnectionService> _connectionServiceMock;

    public LandingPageViewModelTest()
    {
        _navigatorMock = new Mock<INavigationWrapper>();
        _dataServiceMock = new Mock<IDataService>();
        _connectionServiceMock = new Mock<IConnectionService>();

        _sut = new LandingPageViewModel(_navigatorMock.Object, _dataServiceMock.Object, _connectionServiceMock.Object);
    }

    [Fact]
    public async Task Login_ClearsDataFirst_ExpectClearDataCalledAsync()
    {
        await _sut.Login("Username", "Password");

        _dataServiceMock.Verify(mock => mock.ClearDataAndLogout(), Times.Once);
    }

    [Fact]
    public async Task Login_LogsIn_ExpectLoginCalldWithUsernameAndPassword()
    {
        await _sut.Login("Username", "Password");

        _connectionServiceMock.Verify(mock => mock.LoginAsync("Username", "Password"), Times.Once);
    }

    [Fact]
    public async Task Login_GetsTokenFromStorage_ExpectSecureStorageGetTokenCalled()
    {
        await _sut.Login("Username", "Password");

        _dataServiceMock.Verify(mock => mock.SecureStorageGetAsync(StorageKeys.AuthTokenKey), Times.Once);
    }

    [Fact]
    public async Task Login_GetsCurrentUser_ExpectGetUserCalled()
    {
        await _sut.Login("Username", "Password");

        _dataServiceMock.Verify(mock => mock.GetCurrentUserAsync(), Times.Once);
    }

    [Fact]
    public async Task Login_TokenIsNull_ExpectShowErrorTrue()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));

        await _sut.Login("Username", "Password");

        Assert.True(_sut.ShowError);
    }


    [Fact]
    public async Task Login_TokenIsNull_ExpectNavigationNotCalled()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));

        await _sut.Login("Username", "Password");

        _navigatorMock.Verify(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.OverviewPage, It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task Login_UserIsNull_ExpectShowErrorTrue()
    {
        _dataServiceMock.Setup(mock => mock.SecureStorageGetAsync(StorageKeys.AuthTokenKey)).ReturnsAsync("1234");

        await _sut.Login("Username", "Password");

        Assert.True(_sut.ShowError);
    }


    [Fact]
    public async Task Login_UserIsNull_ExpectNavigationNotCalled()
    {
        _dataServiceMock.Setup(mock => mock.SecureStorageGetAsync(StorageKeys.AuthTokenKey)).ReturnsAsync("1234");

        await _sut.Login("Username", "Password");

        _navigatorMock.Verify(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.OverviewPage, It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task Login_LoginSuccess_ExpectNavigationCalled()
    {
        _dataServiceMock.Setup(mock => mock.GetCurrentUserAsync()).ReturnsAsync(DataHelper.GetUser(1));
        _dataServiceMock.Setup(mock => mock.SecureStorageGetAsync(StorageKeys.AuthTokenKey)).ReturnsAsync("1234");

        await _sut.Login("Username", "Password");

        _navigatorMock.Verify(mock => mock.RouteAndReplaceStackAsync(NavigationKeys.OverviewPage, It.IsAny<bool>()), Times.Once);
    }

}
