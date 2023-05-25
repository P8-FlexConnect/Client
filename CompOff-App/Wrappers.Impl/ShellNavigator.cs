using Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Wrappers.Impl;

public class ShellNavigator : INavigationWrapper
{
    /// <inheritdoc />
    public async Task RouteAndReplaceStackAsync(string route, bool isAnimated = false)
    {
        await Shell.Current.GoToAsync($"//{route}", animate: isAnimated);
    }

    /// <inheritdoc />
    public async Task RouteAndReplaceStackAsync(string route, Dictionary<string, object> queryDict, bool isAnimated = true)
    {
        await Shell.Current.GoToAsync($"//{route}", animate: isAnimated, queryDict);
    }

    /// <inheritdoc />
    public async Task RouteAsync(string route, bool isAnimated = true)
    {
        await Shell.Current.GoToAsync($"/{route}", animate: isAnimated);
    }

    /// <inheritdoc />
    public async Task RouteAsync(string route, Dictionary<string, object> queryDict, bool isAnimated = true)
    {
        await Shell.Current.GoToAsync($"/{route}", animate: isAnimated, queryDict);
    }

    /// <inheritdoc />
    public async Task NavigateBackAsync(bool isAnimated)
    {
        await Shell.Current.GoToAsync("..", isAnimated);
    }

    /// <inheritdoc />
    public async Task NavigateBackAsync(bool isAnimated, Dictionary<string, object> queryDict)
    {
        await Shell.Current.GoToAsync("..", isAnimated, queryDict);
    }

    /// <inheritdoc />
    public string GetRootLevelPage()
    {
        var loc = Shell.Current.CurrentState.Location;

        if (loc is null)
        {
            return NavigationKeys.OverviewPage;
        }

        return loc.ToString().Split("/", StringSplitOptions.RemoveEmptyEntries).First();
    }

    /// <inheritdoc />
    public string GetCurrent()
    {
        return Shell.Current.CurrentState.Location.ToString();
    }
}
