using System;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

namespace TestTCPClient
{

    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }


    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class TCPClientObject : TcpClient
    {
        private ConnectionInfo info = new ConnectionInfo();
        private Socket server = null;

        private ManualResetEvent synchThreads = null;

        public TCPClientObject(ManualResetEvent synchronizer)
        {
            //
            // TODO: Add constructor logic here
            //

            synchThreads = synchronizer;
        }

        

        public bool ConnectToServer()
        {
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

            int port = 11000;
            try
            {
                //IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);


                //this.Connect(IPAddress.Parse("127.0.0.1"), port);
                this.Connect(localEndPoint);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);

                return false;
            }

        }

        public void SendMessage(String messageString)
        {
            NetworkStream stream = this.GetStream();

            byte[] buffer = Encoding.ASCII.GetBytes(messageString);
            //stream.BeginRead(buffer, 0, 3, AckCallback, info);

            stream.Write(buffer, 0, messageString.Length);
        }

        public void AckCallback(object stater)
        {
            //ConnectionInfo info = stater as ConnectionInfo;

            //info.connectionSocket = server;

            Console.WriteLine("Reading ACK message");
        }

        public void StartReception()
        {
            Socket handler = this.Client;

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            synchThreads.Set();

            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);


            }
        }
    }
}

