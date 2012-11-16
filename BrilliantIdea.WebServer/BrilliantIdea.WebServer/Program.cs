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

            //create a socket to listen for incoming connections 
            var listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var listenerEndPoint = new IPEndPoint(IPAddress.Any, port);

            //bind to the listening socket
            listenerSocket.Bind(listenerEndPoint);
            //and start listening for incoming connections
            listenerSocket.Listen(1);

            //listen for and process incoming request
            while (true)
            {
                //wait for a client to connect
                Socket clientSocket = listenerSocket.Accept();
                //wait for data to arrive
                bool dataReady = clientSocket.Poll(5000000, SelectMode.SelectRead);
                //if dataReady is true and there are bytes available to read,
                //then you have a good connection
                if (dataReady && clientSocket.Available>0)
                {
                    byte[] buffer = new byte[clientSocket.Available];
                    int bytesRead = clientSocket.Receive(buffer);
                    string request = new string(System.Text.Encoding.UTF8.GetChars(buffer));
                    if (request.IndexOf("ON")>=0)
                    {
                        led.Write(true);
                    }
                    else if (request.IndexOf("OFF")>=0)
                    {
                        led.Write(false);
                    }

                    string statusText = "LED is " + (led.Read() ? "ON" : "OFF") + ".";
                    //return a message to the client letting it know if the LED is now on or off
                    string response = "HTTP/1.1 200 OK\r\n" + "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                                      "<html><head><title>Netduino Plus LED Sample</title></head>" +
                                      "<body>" + statusText + "</body></html>";
                    clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(response));
                }

                //important: close the client socket
                clientSocket.Close();
                
            }
        }

    }
}
