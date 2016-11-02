using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace ControlServer
{
   public class Launcher
    {
        String arguments;
        ProcessStartInfo startInfo;
        Process program;
        public Launcher(string _name, string _argumnents)
        {
            this.arguments = _argumnents;
            this.startInfo = new ProcessStartInfo();
            this.startInfo.FileName = _name;
           // this.startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            this.startInfo.CreateNoWindow = true;
            this.startInfo.RedirectStandardInput = true;
            this.startInfo.RedirectStandardOutput = true;
            this.startInfo.UseShellExecute = false;
            this.program = new Process();
            this.program.StartInfo = startInfo;
        }
        public String Start()
        {
            string response;

            this.program.Start();
            this.program.StandardInput.Flush();
            this.program.StandardInput.WriteLine(this.arguments);
            this.program.StandardInput.Flush();
            this.program.StandardInput.Close();
            response = this.program.StandardOutput.ReadToEnd();
            return response;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
start:
            int port = 9000;
            string cmd = "";
            String[] splitedResponse;
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, port);
            Console.WriteLine(IPAddress.Any.ToString()); 
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
                    Launcher program;

                    String   response,
                             programName,
                             arguments;

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

                  
                    splitedResponse = builder.ToString().Split('%');
                    programName = splitedResponse[0];
                    arguments = splitedResponse[1];

                    program = new Launcher(programName,arguments);
                    response = program.Start();
                    // отправляем ответ
                    // string message = "ваше сообщение доставлено";
                    data = Encoding.ASCII.GetBytes(response);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }
            }
            catch (Exception ex)
            {
                goto start; 
            }
        }
    }
}
