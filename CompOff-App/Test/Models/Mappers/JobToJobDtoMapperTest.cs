using DataTransferObjects;
using Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers;

namespace Tests.Models.Mappers
{
    public class JobToJobDtoMapperTest
    {
        [Fact]
        public void Map_MapsJobToJobDto_ExpectResultToBeJobDto()
        {
            var job = DataHelper.GetJob(1);

            var actual = JobToJobDtoMapper.Map(job);

            Assert.IsType<JobDto>(actual);
        }

        [Fact]
        public void Map_MapsJobToJobDto_ExpectFieldsMapped()
        {
            var job = DataHelper.GetJob(1);

            var actual = JobToJobDtoMapper.Map(job);

            Assert.Equal(job.JobID, actual.id);
            Assert.Equal(job.JobName, actual.name);
            Assert.Equal(job.Description, actual.description);
            Assert.Equal((int)job.Status, actual.status);
            Assert.Equal(job.SourcePath, actual.sourcePath);
            Assert.Equal(job.BackupPath, actual.backupPath);
            Assert.Equal(job.ResultPath, actual.resultPath);
            Assert.Equal(job.DateAdded, actual.dateAdded);
            Assert.Equal(job.LastActivity, actual.lastActivity);
        }
    }
}
