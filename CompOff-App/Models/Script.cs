using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Script
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }

        public Script(string filename, string fullPath) 
        {
            FileName = filename;
            FullPath = fullPath;
        }
    }
}
