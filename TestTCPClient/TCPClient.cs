using System;

using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TestTCPClient
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class TCPClientObject : TcpClient
    {
        public TCPClientObject()
        {
            //
            // TODO: Add constructor logic here
            //


        }

        private ConnectionInfo info = new ConnectionInfo();
        private Socket server = null;

        public void ConnectToServer()
        {
            Connect();
            /*Console.WriteLine("Hello World!");

            IPEndPoint clonedIPEndPoint = (IPEndPoint)endpoint.Create(socketAddress);

            Socket serverSocket = new Socket();


            try
            {
                // Create a socket object to establish a connection with the server.
                socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the cloned end point.
                socket.Connect(clonedIPEndPoint);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }*/

            int port = 125;
            try
            {
                //IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

                client.Connect(IPAddress.Parse("127.0.0.1"), port);

                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[4];

                stream.BeginRead(buffer, 0, 4, AckCallback, info);
            }
            catch (Exception e)
            {
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

        }


        public void AckCallback(object stater)
        {
            ConnectionInfo info = stater as ConnectionInfo;

            info.connectionSocket = server;
        }

    }
}

