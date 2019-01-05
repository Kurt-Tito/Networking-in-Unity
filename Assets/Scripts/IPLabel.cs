using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking; //for ipv4 address
using UnityEngine.UI;

public class IPLabel : MonoBehaviour
{

    public Text ipaddress;

    // Use this for initialization
    void Start ()
    {
        ipaddress.text = "Host Address: " +LocalIPAdress();
        //ipaddress.text = "Host Address: " +GetLocalIPv4(NetworkInterfaceType.Wireless80211);
    }

    /* Displays Local IP Adress */
    public string LocalIPAdress()
    {
        /*
        string hostName = System.Net.Dns.GetHostName();
        string localIP = System.Net.Dns.GetHostEntry(hostName).AddressList[0].ToString();
        return localIP;
        */

        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }

        return localIP;
    }

    /* Function that returns local ip (ipv4) address of machine connected via Ethernet or Wireless.
     * To call ipv4 of Ethernet, use GetLocalIPv4(NetworkInterfaceType.Ethernet);
     * To call ipv4 of Wireless, use GetLocalIPv4(NetworkInterfaceType.Wireless80211);
     */
    public string GetLocalIPv4(NetworkInterfaceType _type)
    {
        string output = "";
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        output = ip.Address.ToString();
                    }
                }
            }
        }
        return output;
    }

}
