using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the test server.");
            Console.Title = "Test Server";

            try
            {
                IPAddress address = IPAddress.Parse("96.32.44.229");
                TcpListener listener = new TcpListener(address, 8001);
                listener.Start();
                Console.WriteLine("Listener running on port 8001");

                Socket s = listener.AcceptSocket();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            while (true)
            {
                Console.Write("Continue running server? (y/n): ");
                string input = Console.ReadLine();
                if (input.ToLower().Equals("n"))
                {
                    return;
                }
                else
                {

                }
            }
        }
    }
}
