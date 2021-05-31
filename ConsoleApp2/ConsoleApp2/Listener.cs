using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace ConsoleApp2
{
    class Listener
    {
        Socket _socket;
        public bool listening { get; set; }
        public int port { get; set; }

        public Listener(int port)
        {
            this.port = port;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            if (listening)
            {
                return;
            }
            _socket.Bind(new IPEndPoint(0, port));
            _socket.Listen(0);
            _socket.BeginAccept(callback, null);
            listening = true;
        }
        public void Stop()
        {
            if (!listening)
            {
                return;
            }
            _socket.Close();
            _socket.Dispose();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        void callback(IAsyncResult result)
        {
            try
            {
                Socket socket = _socket.EndAccept(result);

                if(SocketAccepted!=null)
                {
                    SocketAccepted(socket);
                }

                _socket.BeginAccept(callback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public delegate void SocketAcceptedHandler(Socket socket);
        public event SocketAcceptedHandler SocketAccepted;
    }
}
