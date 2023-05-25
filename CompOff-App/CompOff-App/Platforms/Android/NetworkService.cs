using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Provider;
using Java.Util;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using Services;

namespace CompOff_App;

public class NetworkService : INetworkService
{
    WifiManager _wifiManager;

    public NetworkService()
    {
        Init();
    }

    void Init()
    {
        _wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
    }

    public void ConnectToNetwork(string networkSsid, string networkPassword)
    {
        // Create list of WiFi networks to add to the users saved networks.
        WifiNetworkSuggestion networkSuggestions = new WifiNetworkSuggestion.Builder()
            .SetSsid(ssid: networkSsid)
            .SetWpa2Passphrase(passphrase: networkPassword)
            .SetIsAppInteractionRequired(true)
            .Build();

        ArrayList wifiNetworkSuggestions = new();
        wifiNetworkSuggestions.Add(networkSuggestions);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Prompt the user to add the networks.
            Intent intent = new(action: Settings.ActionWifiAddNetworks);
        intent.PutExtra(name: Settings.ExtraWifiNetworkList, value: wifiNetworkSuggestions);
        
            Platform.CurrentActivity.StartActivityForResult(intent: intent, requestCode: 1);
        });
    }

    public void DisconnectFromNetwork()
    {
        if (!_wifiManager.IsWifiEnabled)
        {
            return;
        }

        // Passing an empty suggestionlist removes all suggestions made by the app and immediately disconnect from either of them.
        List<WifiNetworkSuggestion> wifiNetworkSuggestions = new();

        _wifiManager.RemoveNetworkSuggestions(wifiNetworkSuggestions);
}

    public string GetNetworkSsid()
    {
        throw new NotImplementedException();
    }

    public void FindNearbyNetworks()
    {
        if (!_wifiManager.IsWifiEnabled)
        {
            Platform.CurrentActivity.StartActivity(intent: new Intent(action: Settings.Panel.ActionWifi));
        }

        Intent intent = new(action: Settings.Panel.ActionInternetConnectivity);

        Platform.CurrentActivity.StartActivity(intent: intent);

    }
}
