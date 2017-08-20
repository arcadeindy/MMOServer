using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the test client.");
            Console.Title = "Test Client";
            while (true)
            {
                Console.WriteLine("Sending new coordinates to server:");
                Random r = new Random();
                float x = (float)r.NextDouble();
                float y = (float)r.NextDouble();
                float z = (float)r.NextDouble();
            }
        }
    }
}
