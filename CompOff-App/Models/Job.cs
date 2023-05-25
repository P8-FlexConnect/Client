using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;


public enum JobStatus
{
    Waiting,
    InQueue,
    Running,
    Cancelled,
    Done,
    Paused
}

public class Job
{
    public Guid JobID { get; set; }
    public string JobName { get; set; }
    public string Description { get; set; }
    public JobStatus Status { get; set; }
    public string SourcePath { get; set; }
    public string BackupPath { get; set; }
    public string ResultPath { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime LastActivity { get; set; }

    public Job(string jobName, string description, string sourcePath, string backupPath, string resultPath)
    {
        JobID = Guid.NewGuid();
        JobName = jobName;
        Description = description;
        Status = JobStatus.Waiting;
        SourcePath = sourcePath;
        BackupPath = backupPath;
        ResultPath = resultPath;
        DateAdded = DateTime.Now;
        LastActivity = DateTime.Now;
    }

    public Job(JobDto job)
    {
        JobID = job.id;
        JobName = job.name;
        Description = job.description;
        Status = EnumConverter(job.status);
        SourcePath = job.sourcePath;
        BackupPath = job.backupPath;
        ResultPath = job.resultPath;
        DateAdded = job.dateAdded;
        LastActivity = job.lastActivity;

    }

    public Job()
    {
    }

    private static JobStatus EnumConverter(int status)
    {
        switch (status)
        {
            case 0:
                return JobStatus.Waiting;
            case 1:
                return JobStatus.InQueue;
            case 2:
                return JobStatus.Running;
            case 3:
            case 4:
                return JobStatus.Cancelled;
            case 5:
            case 6:
                return JobStatus.Done;
            case 7:
            case 8:
                return JobStatus.Paused;
            case 9:
                return JobStatus.InQueue;
            default:
                return JobStatus.Waiting;
        }
    }
}
