using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;
public class Location
{
    public string Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public double Distance { get; set; }

    public Location(string name, decimal latitude, decimal longitude, double distance)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Distance = distance;
    }

    public Location(LocationDto dto, Microsoft.Maui.Devices.Sensors.Location userLocation)
    {
        Name = dto.name;
        Latitude = dto.latitude;
        Longitude = dto.longitude;
        Distance = GetDistance(userLocation.Latitude, userLocation.Longitude, Decimal.ToDouble(Latitude), Decimal.ToDouble(Longitude));
    }

    //from https://www.geodatasource.com/developers/c-sharp
    private double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        if ((lat1 == lat2) && (lon1 == lon2))
        {
            return 0;
        }
        else
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) + Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2deg(dist);
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;
            return (dist);
        }
    }

    //From https://www.geodatasource.com/developers/c-sharp
    private double Deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    //From https://www.geodatasource.com/developers/c-sharp
    private double Rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }
}
