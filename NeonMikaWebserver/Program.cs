using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Collections;
using NeonMika.Webserver;
using NeonMika.Webserver.EventArgs;
using NeonMika.Webserver.Responses;

namespace NeonMikaWebserverExecuteable
{
    public class Program
    {
        public static void Main()
        {
            Server WebServer = new Server(PinManagement.OnboardLED);
            WebServer.AddResponse(new XMLResponse("wave", new XMLResponseMethod(WebserverXMLMethods.Wave)));

            while (true)
            {
                PinManagement.OnboardLED.Write(true);
                Thread.Sleep(100);
                PinManagement.OnboardLED.Write(false);
                Thread.Sleep(2400);                
            }
        }
    }
}
