using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat_Server_Console
{
    class Program
    {
        static int i;
        static Socket listenSocket;
        static int port = 8005; // порт для приема входящих запросов
        static IPAddress ip = IPAddress.Parse("127.0.0.1");
        static Socket[] clients = new Socket[10];
        static int cind = 0;
        static void Main(string[] args)
        {
            /*Boolean inputflag = true;
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

                    IPAddress ipAddress = ip;
                    TcpListener tcpListener = new TcpListener(ipAddress, port_buff);
                    tcpListener.Start();


                    port = port_buff;
                    inputflag = false;
                    tcpListener.Stop();
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("This port in not avaible, try again");
                }
                catch
                {
                    Console.WriteLine("Invalid port, try one more time");
                }
            }
            */

            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(ip, port);

            // создаем сокет
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Server is up, waiting for connections...");
                
                Boolean listening = true;
                i = 0;
                while (i < 10)
                {
                    Thread thread = new Thread(new ParameterizedThreadStart(thread_proc));
                    thread.Start(i);
                    i++;
                }   
                        //string message = "Your message is recived";
                        //data = Encoding.Unicode.GetBytes(message);
                        //handler.Send(data);
                        // закрываем сокет
                       
                    

                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void thread_proc(object param)
        {
            Socket handler = listenSocket.Accept();
            clients[cind] = handler;
            cind++;
            Boolean flag = true;
            while (flag)
            {
                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[256]; // буфер для получаемых данных
                while (handler.Available > 0) 
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                }

                

                 //отправляем ответ всем остальным
                for (int i = 0; i < 10; i++)
                {
                    if (i!=cind && clients[i]!=null)
                    {
                        if (clients[i].Connected && data[0]!=0)
                          handler.Send(data);
                    }
                }
                //string message = "Your message is recived";
                //data = Encoding.Unicode.GetBytes(message);
                //handler.Send(data);
                // закрываем сокет
                if (builder.ToString() == "close")
                {
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    i--;
                    clients[cind - 1] = null;
                    cind--;
                    flag = false;

                }
            }
            
        }
    }
}