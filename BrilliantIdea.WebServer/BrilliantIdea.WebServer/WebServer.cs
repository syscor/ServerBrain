using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace BrilliantIdea.WebServer
{
    public class WebServer : IDisposable
    {
        private const int Backlog = 1;
        private readonly Socket _socket;
        private readonly Thread _thread;
        private readonly OutputPort _led;

        public WebServer(OutputPort output, int port = 80 )
        {
            _led = output;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(Backlog);

            _thread = new Thread(new ThreadStart(ListenForClients));
            _thread.Start();
        }

        private void ListenForClients()
        {
            while (true)
            {
                using (Socket clientSocket = _socket.Accept())
                {
                    bool dataReady = clientSocket.Poll(5000000, SelectMode.SelectRead);
                    if ( dataReady && clientSocket.Available>0)
                    {
                        byte[] buffer = new byte[clientSocket.Available];
                        int bytesRead = clientSocket.Receive(buffer);
                        string request = new string(System.Text.Encoding.UTF8.GetChars(buffer));
                        if (request.IndexOf("ON") >= 0)
                        {
                            _led.Write(true);
                        }
                        else if (request.IndexOf("OFF") >= 0)
                        {
                            _led.Write(false);
                        }

                        string statusText = "LED is " + (_led.Read() ? "ON" : "OFF") + ".";
                        //return a message to the client letting it know if the LED is now on or off
                        string response = "HTTP/1.1 200 OK\r\n" + "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                                          "<html><head><title>Netduino Plus LED Sample</title></head>" +
                                          "<body>" + statusText + "</body></html>";
                        clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(response));
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _dispose = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_dispose)
            {
                _socket.Close();
                _thread.Abort();
            }

            _dispose = true;
        }
        
        ~WebServer()
        {
            Dispose(false);
        }
    }
}
