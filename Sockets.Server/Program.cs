using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Sockets.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
        }
    }
}
