using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wrappers;

public interface INavigationWrapper
{
    Task RouteAndReplaceStackAsync(string route, bool isAnimated = true);

    Task RouteAndReplaceStackAsync(string route, Dictionary<string, object> queryDict, bool isAnimated = true);

    Task RouteAsync(string route, bool isAnimated = true);

    Task RouteAsync(string route, Dictionary<string, object> queryDict, bool isAnimated = true);

    string GetRootLevelPage();

    Task NavigateBackAsync(bool isAnimated);

    Task NavigateBackAsync(bool isAnimated, Dictionary<string, object> queryDict);

    string GetCurrent();
}
