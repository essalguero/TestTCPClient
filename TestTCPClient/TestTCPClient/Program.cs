using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;


using System.Threading;

namespace TestTCPClient
{
    class Program
    {
        // Thread signal.  
        public static ManualResetEvent synchronizer = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            TCPClientObject client = new TCPClientObject(synchronizer);

            if (client.ConnectToServer())
            {

                for (int i = 100; i < 110; ++i)
                {
                    client.SendMessage(i.ToString());
                }

                client.SendMessage("<EOF>");
            }

            

            while (true)
            {
                synchronizer.Reset();

                client.StartReception();

                // Wait until a connection is made before continuing.  
                synchronizer.WaitOne();
            }
        }
    }
}
