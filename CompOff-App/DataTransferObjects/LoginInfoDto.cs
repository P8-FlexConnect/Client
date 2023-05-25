using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects;

public class LoginInfoDto
{
    public string userName { get; set; }
    public string password { get; set; }

    public LoginInfoDto(string username, string password)
    {
        userName = username;
        this.password = password;
    }
}
