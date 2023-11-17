using NativeWifi;
using System;
using System.Text;
using System.Threading;
using WiFiAnalyzer;

class Program
{
    static void Main(string[] args)
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\WiFiAnalyzer";
        System.IO.Directory.CreateDirectory(folderPath);

        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
        string filePath = System.IO.Path.Combine(folderPath, fileName);


        WlanClient client = new WlanClient();

        Console.WriteLine("Do you want to see all SSIDs or only open networks? (Enter 'all' or 'open')" +
                          " OR do you want to connect to a selected network (only for open)? (Enter 'connect')");
        string userInput = Console.ReadLine();

        if (userInput == "connect")
        {
            Console.WriteLine("Enter the SSID of the network you want to connect to:");
            string targetSsid = Console.ReadLine();

            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                wlanIface.Scan();
                Thread.Sleep(1000);

                Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
                foreach (Wlan.WlanBssEntry network in wlanBssEntries)
                {
                    string securityType = GetData.GetSecurityType(wlanIface, network.dot11Ssid);

                    if (GetData.GetStringForSSID(network.dot11Ssid) == targetSsid && securityType == "IEEE80211_Open")
                    {
                        string profileName = GetData.GetStringForSSID(network.dot11Ssid);
                        Wlan.WlanConnectionMode connectionMode = Wlan.WlanConnectionMode.Profile;
                        Wlan.Dot11BssType bssType = Wlan.Dot11BssType.Infrastructure;
                        wlanIface.Connect(connectionMode, bssType, profileName);
                        Console.WriteLine("Connecting to network: " + targetSsid);

                        if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected &&
                            GetData.GetStringForSSID(wlanIface.CurrentConnection.wlanAssociationAttributes.dot11Ssid) ==
                            targetSsid)
                        {
                            Console.WriteLine("Successfully connected to network: " + targetSsid);
                        }
                        else
                        {
                            Console.WriteLine("Failed to connect to network: " + targetSsid);
                        }
                    }
                }
            }
        }
        else
        {
            while (true)
            {
                foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                {
                    wlanIface.Scan();

                    Thread.Sleep(1000);

                    Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
                    foreach (Wlan.WlanBssEntry network in wlanBssEntries)
                    {
                        string securityType = GetData.GetSecurityType(wlanIface, network.dot11Ssid);

                        if (userInput == "all" || (userInput == "open" && securityType == "IEEE80211_Open"))
                        {
                            string networkDetails = string.Format(
                                "Found network with SSID {0}, BSSID (MAC): {1}, Signal strength: {2} dBm, BSS Type: {3}, PHY Type: {4}, Channel: {5}, Security: {6}.",
                                GetData.GetStringForSSID(network.dot11Ssid),
                                GetData.GetMacAddress(network.dot11Bssid),
                                network.rssi,
                                network.dot11BssType,
                                network.dot11BssPhyType,
                                GetData.GetChannelFromFrequency(network.chCenterFrequency),
                                securityType
                            );

                            Console.WriteLine(networkDetails);

                            System.IO.File.AppendAllText(filePath, networkDetails + Environment.NewLine);
                        }
                    }
                }

                Thread.Sleep(5000);
            }
        }
    }
}