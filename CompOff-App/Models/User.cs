using System;
using DataTransferObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Initials { get; set; }

    public User()
    { 
    }

    public User(string firstName, string lastName, string userName)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Initials = GetInitials();
    }

    public User(UserDto userDto)
    {
        FirstName = userDto.firstname;
        LastName = userDto.lastname;
        UserName = userDto.username;
        Initials = GetInitials();
    }

    private string GetInitials()
    {
        return string.Concat(FirstName.AsSpan(0,1), LastName.AsSpan(0,1)).ToUpper();
    }
}
