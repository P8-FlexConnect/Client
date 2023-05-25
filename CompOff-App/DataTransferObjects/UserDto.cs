using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects;

public class UserDto
{
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string username { get; set; }

    public UserDto(string firstName, string lastName, string userName)
    {
        firstname = firstName;
        lastname = lastName;
        username = userName;
    }
}
