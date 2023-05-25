using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace CompOff_App;

class AndroidPathService : IPlatformPathService
{
    public string GetDownloadDirectoryPath()
    {
        return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path;
    }
}
