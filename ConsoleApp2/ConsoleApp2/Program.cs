using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static Listener _listener;
        static List<Socket> sockets = new List<Socket>();
        static void Main(string[] args)
        {
            _listener = new Listener(8080);
            _listener.SocketAccepted += new Listener.SocketAcceptedHandler(listenerSocketAccepted);
            _listener.Start();
            Console.Read();
        }
        static void listenerSocketAccepted(Socket socket)
        {
            Console.WriteLine("New Connection {0}\n{1}\n================", socket.RemoteEndPoint, DateTime.Now);
            Client client = new Client(socket);
            client.Received += new Client.ClientReceivedHandler(clientReceived);
            client.Disconnected += new Client.ClientDisconnectedHandler(clientDisconnected);
        }
        static void clientReceived(Client socket,byte[] data)
        {
            Console.WriteLine(Convert.ToBase64String(data));
        }
        static void clientDisconnected(Client socket)
        {

        }
    }
}
