using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public struct Address
{
    public string Street;
    public string Number;
    public string City;
    public string ZipCode;
    public string Country;

    public Address(string street, string number, string city, string zipCode, string country)
    {
        Street = street;
        Number = number;
        City = city;
        ZipCode = zipCode;
        Country = country;
    }
}
