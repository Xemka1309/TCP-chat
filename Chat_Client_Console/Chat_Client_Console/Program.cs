
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat_Client_Console
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8005; // порт сервера
        static String name="tesrt";
        static IPAddress ip = IPAddress.Parse("127.0.0.1");
        static IPEndPoint ipPoint;
        static Socket socket;
        static void Main(string[] args)
        {
            /*   Console.WriteLine("Enter your name in chat");
               name = Console.ReadLine();
               Boolean inputflag = true;
               while (inputflag)
               {
                   Console.WriteLine("Enter server IPAddress");
                   try
                   {
                       IPAddress ip_buff = IPAddress.Parse(Console.ReadLine());
                       ip = ip_buff;
                       inputflag = false;
                   }
                   catch
                   {
                       Console.WriteLine("Invalid ip, try one more time");
                   }
               }
               inputflag = true;
               while (inputflag)
               {
                   Console.WriteLine("Enter server port");
                   try
                   {
                       int port_buff = Convert.ToInt32(Console.ReadLine());
                       port = port_buff;
                       inputflag = false;

                   }

                   catch
                   {
                       Console.WriteLine("Invalid port, try one more time");
                   }
               }*/
            
            ipPoint = new IPEndPoint(ip, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                

                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                Boolean flag = true;
                Thread thread=new Thread(listener);
                thread.Start();
                while (flag)
                {
                    
                    Console.Write("Enter your message:");
                    string message = Console.ReadLine();
                    //string message = "first";
                    if (message == "//close")
                    {
                        flag = false;
                        break;
                    }
                        
                    byte[] data = Encoding.Unicode.GetBytes(name+":"+message);
                    socket.Send(data);

              
                }

                // закрываем сокет
                byte[] data_2 = Encoding.Unicode.GetBytes("close");
                socket.Send(data_2);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                thread.Abort();
            }
            
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        public static void listener()
        {
            //Socket listener = socket.Accept();
            // получаем ответ
            byte[] data_recieve = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            while (socket.Available > 0)
            {
                bytes = socket.Receive(data_recieve, data_recieve.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data_recieve, 0, bytes));
                Console.WriteLine(builder.ToString());
            }

            
        }
    }

    
}