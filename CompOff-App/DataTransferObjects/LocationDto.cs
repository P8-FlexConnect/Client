using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects;

public class LocationDto
{
    public string name { get; set; }
    public decimal latitude { get; set; }
    public decimal longitude { get; set; }
}
