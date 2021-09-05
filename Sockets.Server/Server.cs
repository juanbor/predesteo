using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sockets.Server
{
    public class Server
    {
        private readonly int port = 9000;
        private readonly int backlog = 1;

        public Server()
        {
        }

        public void Run()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, port);

            Socket listener = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(endpoint);

            listener.Listen(backlog);

            Console.WriteLine("Start listening for clients...");

            while (true)
            {
                Socket client = listener.Accept();
                Thread th = new Thread(() => HandleIncomingClients(client));
                th.Start();
            }
        }

        public void ReceiveMessages(Socket handler)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                string data = null;

                while (true)
                {
                    int byteRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, byteRec);

                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }

                Console.WriteLine($"Client: {data}");
            }
        }

        public void HandleIncomingClients(Socket handler)
        {
            // Creo un thread destinado a recibir mensajes
            Thread th = new Thread(() => ReceiveMessages(handler));
            th.Start();

            while (true)
            {
                String message = Console.ReadLine();

                byte[] messageBytes = Encoding.ASCII.GetBytes(message + " <EOF>");

                int byteSent = handler.Send(messageBytes);
            }

            //handler.Shutdown(SocketShutdown.Both);

            //handler.Close();
        }
    }
}
