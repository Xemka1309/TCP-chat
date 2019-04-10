
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
 
namespace Chat_Client_Console
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8005; // порт сервера
        static String name="tesrt";
        static IPAddress ip = IPAddress.Parse("127.0.0.1");
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
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(ip, port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                Boolean flag = true;
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

                    // получаем ответ
                    //data = new byte[256]; // буфер для ответа
                    //StringBuilder builder = new StringBuilder();
                   // int bytes = 0; // количество полученных байт

                   // while (socket.Available > 0) 
                   // {
                   //     bytes = socket.Receive(data, data.Length, 0);
                   //     builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                   // }
                    
                   // Console.WriteLine("Server: " + builder.ToString());
                    

                }

                // закрываем сокет
                byte[] data_2 = Encoding.Unicode.GetBytes("close");
                socket.Send(data_2);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}