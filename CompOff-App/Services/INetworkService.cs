using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services;

public interface INetworkService
{
    void ConnectToNetwork(string networkSsid, string networkPassword);
    void DisconnectFromNetwork();
    string GetNetworkSsid();
    void FindNearbyNetworks();
}
