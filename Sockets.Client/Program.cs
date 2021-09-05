using System;
using System.Net;
using System.Net.Sockets;

namespace Sockets.Client
{
    class Program
    {        
        static void Main(string[] args)
        {
            Client client = new Client();
            client.Run();
        }
    }
}
