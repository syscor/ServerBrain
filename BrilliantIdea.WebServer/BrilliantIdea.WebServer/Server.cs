using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Net.NetworkInformation;

namespace BrilliantIdea.WebServer
{
    public class Server
    {
        public int PortNumber { get; private set; }
        private readonly Socket _listeningSocket;
        private Hashtable _responses = new Hashtable();
        private OutputPort _onboardLed;

        /// <summary>
        /// Create an instance runing in a separate thread
        /// </summary>
        /// <param name="ledPort"></param>
        /// <param name="portNumber">The port to listen</param>
        /// <param name="dhcpEnable"></param>
        public Server(OutputPort ledPort, int portNumber = 80, bool dhcpEnable = false)
        {
            //wait a few seconds for the Netduino Plus to get a network address
            Thread.Sleep(5000);

            var networkInterface = NetworkInterface.GetAllNetworkInterfaces()[0];
            if (dhcpEnable)
            {
                networkInterface.EnableDhcp();
                networkInterface.RenewDhcpLease();
            }
            Debug.Print("SysCor Webserver is runing on " + networkInterface.IPAddress + " /// DHCP: " + networkInterface.IsDhcpEnabled);

            PortNumber = portNumber;
            _onboardLed = ledPort;

            //create a socket to listen for incoming connections
            _listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var listenerEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            //bind to the listening socket
            _listeningSocket.Bind(listenerEndPoint);
            //star listening for incoming connections
            _listeningSocket.Listen(1);

            //listen for and process incoming request
            var webserverThread = new Thread(WaitingForRequest);
            webserverThread.Start();

        }

        private void WaitingForRequest()
        {
            while (true)
            {
                try
                {
                   //wait for a client to connect
                    Socket clientSocket = _listeningSocket.Accept();
                    //wait for data to arrive
                    bool dataReady = clientSocket.Poll(5000000, SelectMode.SelectRead);
                    //if dataReady is true and there are bytes available to read,
                    //then you have a good connection
                    if (dataReady && clientSocket.Available>0 && clientSocket.Available<Settings.MaxRequestsize)
                    {
                        var buffer = new byte[clientSocket.Available];
                        var bytesRead = clientSocket.Receive(buffer);

                        //request created, checking the reponse possibilities
                        /////////////////////////////////////////////////Line 52 Program
                        ///////////////////////Line 109 Server (NeonMika)
                    }

                    clientSocket.Close();
                }
                catch (Exception exception)
                {
                   Debug.Print(exception.Message);
                }
            }
// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns
    }
}
