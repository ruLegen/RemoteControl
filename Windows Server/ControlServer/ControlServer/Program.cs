using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ControlServer
{
    class Server
    {
        TcpListener Listener;

public  Server(int port)
        {
            Listener = new TcpListener(IPAddress.Any,port);
            Listener.Start();
           
        }
        ~Server()
        {
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
