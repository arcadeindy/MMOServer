using System;
using System.Net;
using Comms;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TestServer();
        }

        static void TestServer()
        {
            try
            {
                SocketServer server = new SocketServer();

                server.Start(Environment.MachineName, 9000, 1024, 10240, null,
                    new SocketServer.MESSAGE_HANDLER(MessageHandlerServer),
                    new SocketServer.ACCEPT_HANDLER(AcceptHandler),
                    new SocketServer.CLOSE_HANDLER(CloseHandler),
                    new SocketServer.ERROR_HANDLER(ErrorHandler));

                Console.WriteLine("Waiting for client connection on {0} port {1}", server.ipAddress, server.port);

                Console.ReadLine();
                server.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //All send and receive logic goes here
        static public void MessageHandlerServer(SocketClient client, int numBytes)
        {
            try
            {
                DoReceive(client, numBytes);
                DoSend(client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void DoSend(SocketClient client)
        {
            System.Threading.Thread.Sleep(5000);
            ushort id = 69;
            Data dat0 = new Data(Data.USHORT, Data.IT_PlayerID, id);
            Data dat1 = new Data(Data.STRING, Data.IT_TextMessage, "I'm the server. It's " + DateTime.Now.ToString());

            Message msg = new Message(new Data[] { dat0, dat1 });
            byte[] packetData = msg.GetSendableMessage();

            client.Send(packetData);
        }

        private static void DoReceive(SocketClient client, int numBytes)
        {
            Console.WriteLine("Parsing message contents...");
            Data[] dat = MessageParser.ParseMessage(client.rawBuffer);

            for (int i = 0; i < dat.Length; i++)
            {
                byte type = dat[i].type;
                ushort infoType = dat[i].infoType;

                switch (type)
                {
                    case Data.INT:
                        Console.WriteLine("Received int: " + dat[i].data_int + " of type " + infoType);
                        break;
                    case Data.UINT:
                        Console.WriteLine("Received uint: " + dat[i].data_uint + " of type " + infoType);
                        break;
                    case Data.SHORT:
                        Console.WriteLine("Received short: " + dat[i].data_short + " of type " + infoType);
                        break;
                    case Data.USHORT:
                        Console.WriteLine("Received ushort: " + dat[i].data_ushort + " of type " + infoType);
                        break;
                    case Data.FLOAT:
                        Console.WriteLine("Received float: " + dat[i].data_float + " of type " + infoType);
                        break;
                    case Data.DOUBLE:
                        Console.WriteLine("Received double: " + dat[i].data_double + " of type " + infoType);
                        break;
                    case Data.LONG:
                        Console.WriteLine("Received long: " + dat[i].data_long + " of type " + infoType);
                        break;
                    case Data.ULONG:
                        Console.WriteLine("Received ulong: " + dat[i].data_ulong + " of type " + infoType);
                        break;
                    case Data.BOOL:
                        Console.WriteLine("Received bool: " + dat[i].data_bool + " of type " + infoType);
                        break;
                    case Data.BYTE:
                        Console.WriteLine("Received byte: " + dat[i].data_byte + " of type " + infoType);
                        break;
                    case Data.SBYTE:
                        Console.WriteLine("Received sbyte: " + dat[i].data_sbyte + " of type " + infoType);
                        break;
                    case Data.CHAR:
                        Console.WriteLine("Received char: " + dat[i].data_char + " of type " + infoType);
                        break;
                    case Data.STRING:
                        Console.WriteLine("Received string: " + dat[i].data_string + " of type " + infoType);
                        break;
                    default:
                        //Throw unknown data exception
                        break;
                }
            }
        }

        //Called when socket is initially created.
        //INITIATE FIRST CONTACT HERE.
        static public void AcceptHandler(SocketClient client)
        {
            Console.WriteLine("Accepted new socket connection at IP: " + client.ipAddress);
            DoSend(client);
        }

        static public void CloseHandler(SocketClient client)
        {
            Console.WriteLine("Close Handler");
            Console.WriteLine("IP: " + client.ipAddress);
        }

        static public void ErrorHandler(SocketClient client, Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
