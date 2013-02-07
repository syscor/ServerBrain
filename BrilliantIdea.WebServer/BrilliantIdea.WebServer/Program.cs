using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace BrilliantIdea.WebServer
{
    public class Program
    {
        public static void Main()
        {
            //set up the LED and turn it off by default
            var led = new OutputPort(Pins.ONBOARD_LED, false);

            //configure the port # (the standar web server port is 80)
            const int port = 80;

            //wait a few seconds for the Netduino Plus to get a network address
            Thread.Sleep(5000);

            //display the IP address
            var networkInterface = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0];
            
            Debug.Print("mi dirección ip: " + networkInterface.IPAddress);

            using (var server = new WebServer(led, port))
            {
                while (true)
                {
                    
                }
            }
        }

    }
}
