using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sockets.Client
{
    public class Client
    {
        private string ServerIPAddress = "127.0.0.1";
        private int ServerPort = 9000;

        //private string ClientIPAddress = "127.0.0.1";
        //private int ClientPort = 0; // va a agarrar un puerto disponible, es para generar multiples clientes

        public Client()
        {
        }

        public void ConnectTo()
        {
            Console.WriteLine("Hi! I need the IP and Port, please write the IP bellow:");

            String ip = Console.ReadLine();

            int portInt = 0;

            Console.WriteLine("Great!, now please write the Port bellow:");

            while (portInt == 0)
            {
                String portStr = Console.ReadLine();

                Int32.TryParse(portStr, out portInt);

                if (portInt == 0)
                {
                    Console.WriteLine("Port its not a number...");
                    Console.WriteLine("Try again...");
                }
            }

            Console.WriteLine(ip + ":" + portInt);

            this.ServerIPAddress = ip;
            this.ServerPort = portInt;
        }

        public void Run()
        {
            //IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Parse(ClientIPAddress), ClientPort);
            //Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //clientSocket.Bind(clientEndPoint);

            ConnectTo();

            Console.WriteLine("Trying to connect to server...");

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ServerIPAddress), ServerPort);
            Socket clientSocket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverEndPoint);

            Console.WriteLine("Connected to server... :)");

            Console.WriteLine("Write some message and press ENTER!");

            Thread th = new Thread(() => ReceiveMessages(clientSocket));
            th.Start();

            bool getOut = false;
            while (!getOut)
            {
                String message = Console.ReadLine();

                byte[] messageBytes = Encoding.ASCII.GetBytes(message + " <EOF>");

                try
                {
                    int byteSent = clientSocket.Send(messageBytes);
                }
                catch(ObjectDisposedException e)
                {
                    Console.WriteLine("Oh... apparently the server is offline...");
                    clientSocket.Close();
                    getOut = true;
                }
                
            }

            //clientSocket.Shutdown(SocketShutdown.Both);

            //clientSocket.Close();

            //Console.ReadLine();
        }

        public void ReceiveMessages(Socket handler)
        {
            try
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

                    Console.WriteLine($"Server: {data}");
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("Server is offline...");
                handler.Close();
            }
        }

    }
}
