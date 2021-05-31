using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Client
    {
        public string ID { get; set; }
        public IPEndPoint IPEndPoint {get;set ;}

        Socket _socket;

        public Client(Socket socket)
        {
            _socket = socket;
            ID = Guid.NewGuid().ToString();
            IPEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
        }
        public void callback(IAsyncResult result)
        {
            try
            {
                _socket.EndReceive(result);
                byte[] buff = new byte[8192];

                int received = _socket.Receive(buff);

                if(received < buff.Length)
                {
                    Array.Resize<byte>(ref buff, received);
                }
                if(Received!=null)
                {
                    Received(this,buff);
                }
                _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                close();

                if(Disconnected!=null)
                {
                    Disconnected(this);
                }
            }
        }
        public void close()
        {
            _socket.Close();
            _socket.Dispose();
        }
        public delegate void ClientReceivedHandler(Client socket,byte[] arr);
        public delegate void ClientDisconnectedHandler(Client socket);

        public event ClientReceivedHandler Received;
        public event ClientDisconnectedHandler Disconnected;
    }
}
