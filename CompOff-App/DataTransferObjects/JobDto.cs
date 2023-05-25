using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects;

public class JobDto
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int status { get; set; }
    public string sourcePath { get; set; }
    public string backupPath { get; set; }
    public string resultPath { get; set; }
    public DateTime dateAdded { get; set; }
    public DateTime lastActivity { get; set; }
}
