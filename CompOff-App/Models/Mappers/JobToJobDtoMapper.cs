using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models.Mappers
{
    public static class JobToJobDtoMapper
    {
        public static JobDto Map(Job job)
        {
            return new JobDto() 
            {
                id = job.JobID,
                name = job.JobName,
                description = job.Description,
                status = (int)job.Status,
                sourcePath = job.SourcePath,
                backupPath = job.BackupPath,
                resultPath = job.ResultPath,
                dateAdded = job.DateAdded,
                lastActivity = job.LastActivity,
            };
        }
    }
}
