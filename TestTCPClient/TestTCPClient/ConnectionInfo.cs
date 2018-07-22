using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Sockets;
using System.Net;

namespace TestTCPClient
{
    class ConnectionInfo
    {
        public Socket connectionSocket { get; set; }
        private byte[] buffer { get; set; }
    }
}
