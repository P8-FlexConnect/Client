using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects;

public class PrepareDto
{
    public JobDto serviceTask { get; set; }
    public WifiDto wifi { get; set; }
}
