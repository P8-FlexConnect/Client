using DataTransferObjects;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location = Models.Location;

namespace Tests.Helpers;

internal static class DataHelper
{
    public static Guid DummyGuid(int n)
    {
        return n switch
        {
            1 => Guid.Parse("4e096c77-0774-4b80-8d0c-b86a17f3c512"),
            2 => Guid.Parse("3f5e96e8-d180-4e6b-b5d6-02a732d7374e"),
            3 => Guid.Parse("45650fbf-bba3-4016-a801-5cc90f09ec86"),
            4 => Guid.Parse("24172d0a-d5ef-41df-a837-3d03264116a0"),
            _ => Guid.Parse("b9f2ab8f-d3c1-49ef-a39a-c0b7c4bf38ad")
        };
    }

    public static User GetUser(int n)
    {
        return n switch
        {
            1 => new User("Jeff", "Winger", "Tango"),
            2 => new User("Abed", "Nadir", "Inspector"),
            3 => new User("Troy", "Barnes", "T-Bone"),
            4 => new User("Britta", "Perry", "The_Worst"),
            5 => new User("Annie", "Eddison", "Annie_Adderal"),
            6 => new User("Pierce", "Hawthorne", "Anastasia"),
            7 => new User("Shirley", "Bennet", "Big_Cheddar"),
            _ => new User("Ben", "Chang", "El_Tigre_Chino"),
        };
    }

    public static DateTime DummyDateTime(int n)
    {
        return n switch
        {
            1 => new DateTime(2023, 1, 5),
            2 => new DateTime(2023, 1, 4),
            3 => new DateTime(2023, 1, 3),
            4 => new DateTime(2023, 1, 2),
            _ => new DateTime(2023, 1, 1)
        };
    }

    public static Location GetLocation(int n)
    {
        return n switch
        {
            1 => new Location("Location 1", 50, 50, 10),
            2 => new Location("Location 2", 50, 50, 20),
            3 => new Location("Location 3", 50, 50, 30),
            4 => new Location("Location 4", 50, 50, 40),
            _ => new Location("Location 5", 50, 50, 50),
        };
    }

    public static Script GetScript(int n)
    {
        return n switch
        {
            1 => new Script("Script 1.py", "path"),
            _ => new Script("Script 2.py", "path"),
        };
    }

    public static Script GetIncorrectScript(int n)
    {
        return n switch
        {
            1 => new Script("Script 1.txt", "path"),
            _ => new Script("Script 2.txt", "path"),
        };
    }
    
    public static Job GetJob(int n)
    {
        return n switch
        {
            1 => new Job()
            {
                JobID = DummyGuid(1),
                JobName = "Job1",
                Description = "Description1",
                Status = JobStatus.Waiting,
                SourcePath = "SourcePath1",
                BackupPath = "BackupPath1",
                ResultPath = "ResultPath1",
                DateAdded = DummyDateTime(1),
                LastActivity = DummyDateTime(1),
            },
            _ => new Job()
            {
                JobID = DummyGuid(2),
                JobName = "Job2",
                Description = "Description2",
                Status = JobStatus.Waiting,
                SourcePath = "SourcePath2",
                BackupPath = "BackupPath2",
                ResultPath = "ResultPath2",
                DateAdded = DummyDateTime(2),
                LastActivity = DummyDateTime(2),
            },
        };
    }

    public static JobDto GetJobDto(int n)
    {
        return n switch
        {
            1 => new JobDto()
            {
                id = DummyGuid(1),
                name = "Job1",
                description = "Description1",
                status = 0,
                sourcePath = "SourcePath1",
                backupPath = "BackupPath1",
                resultPath = "ResultPath1",
                dateAdded = DummyDateTime(1),
                lastActivity = DummyDateTime(1),
            },
            _ => new JobDto()
            {
                id = DummyGuid(2),
                name = "Job2",
                description = "Description2",
                status = 5,
                sourcePath = "SourcePath2",
                backupPath = "BackupPath2",
                resultPath = "ResultPath2",
                dateAdded = DummyDateTime(2),
                lastActivity = DummyDateTime(2),
            },
        };
    }

    public static WifiDto GetWifiDto(int n)
    {
        return n switch
        {
            1 => new WifiDto()
            {
                ssid = "ssid1",
                password = "password1",
            },
            _ => new WifiDto()
            {
                ssid = "ssid2",
                password = "password2",
            }
        };
    }

    public static PrepareDto GetPrepareDto(int n)
    {
        return n switch
        {
            1 => new PrepareDto()
            {
                serviceTask = GetJobDto(1),
                wifi = GetWifiDto(1)
            },
            _ => new PrepareDto()
            {
                serviceTask = GetJobDto(2),
                wifi = GetWifiDto(2)
            }
        };
    }
}
