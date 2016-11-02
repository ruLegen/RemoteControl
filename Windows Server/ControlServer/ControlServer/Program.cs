using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ControlServer
{
    class Programm
    {
        string name = "";
        string arguments = "";
        string path = "";
    }
    class ProgrammLauncher
    {
        string name = "";
        string arguments = "";
        string path = "";
        ProgrammLauncher()
        {

        }
        
    };

    class Program
    {
        static void Main(string[] args)
        {
            int port = 8005;
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);
                // начинаем прослушивание
                listenSocket.Listen(10);

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();

                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[1024]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(builder.ToString());

                    // отправляем ответ
                   // string message = "ваше сообщение доставлено";
                    //data = Encoding.Unicode.GetBytes(message);
                    //handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
