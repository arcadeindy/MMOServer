using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Comms
{
    /// <summary>
    /// Class abstracting a socket client
    /// </summary>
    public class SocketClient
    {
        /// <summary> I/O stream through which all data flows </summary>
        private NetworkStream netStream { get; set; }
        /// <summary> TcpListener for initializing socket connections </summary>
        private TcpClient tcpClient { get; set; }
        private AsyncCallback callbackReadMethod { get; set; }
        private AsyncCallback callbackWriteMethod { get; set; }
        //Async functions
        private MESSAGE_HANDLER messageHandler { get; set; }
        private CLOSE_HANDLER closeHandler { get; set; }
        private ERROR_HANDLER errorHandler { get; set; }
        //Flag to indicate if class has been disposed of
        private bool disposedFlag { get; set; }
        /// <summary> SocketServer for this object </summary>
        private SocketServer socketServer { get; set; }

        //Socket info
        public string ipAddress { get; set; }
        public short port { get; set; }
        //Optional state associated with socket connection
        public Object userArg { get; set; }
        //NetworkStream settings
        public byte[] rawBuffer { get; set; }
        public int bufferSize { get; set; }
        public int socketListIndex { get; set; }
        public Socket clientSocket { get; set; }

        public SocketClient(int bufferSize, Object userArg, MESSAGE_HANDLER msgHandler, CLOSE_HANDLER clsHandler, ERROR_HANDLER errHandler)
        {
            this.bufferSize = bufferSize;
            rawBuffer = new byte[bufferSize];
            this.userArg = userArg;
            messageHandler = msgHandler;
            closeHandler = clsHandler;
            errorHandler = errHandler;
            callbackReadMethod = new AsyncCallback(ReceiveComplete);
            callbackWriteMethod = new AsyncCallback(SendComplete);
            disposedFlag = false;
        }

        public SocketClient(SocketServer socketServer, Socket clientSocket, int socketListIndex, string ipAddress, short port, 
            int bufferSize, Object userArg, MESSAGE_HANDLER msgHandler, CLOSE_HANDLER clsHandler, ERROR_HANDLER errHandler)
        {
            this.bufferSize = bufferSize;
            rawBuffer = new byte[bufferSize];
            this.userArg = userArg;
            messageHandler = msgHandler;
            closeHandler = clsHandler;
            errorHandler = errHandler;

            callbackReadMethod = new AsyncCallback(ReceiveComplete);
            callbackWriteMethod = new AsyncCallback(SendComplete);

            disposedFlag = false;
            this.socketServer = socketServer;
            this.clientSocket = clientSocket;
            this.socketListIndex = socketListIndex;
            this.ipAddress = ipAddress;
            this.port = port;

            netStream = new NetworkStream(this.clientSocket);
            this.clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1048576);
            this.clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 1048576);
            this.clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            this.clientSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

            Receive();
        }

        ~SocketClient()
        {
            if (!disposedFlag)
                Dispose();
        }

        private void ReceiveComplete(IAsyncResult ar)
        {
            Console.WriteLine("ReceiveComplete");
            try
            {
                if (netStream.CanRead)
                {
                    int numBytesReceived = netStream.EndRead(ar);

                    if(numBytesReceived > 0) {
                        try
                        {
                            byte zero = 0;
                            messageHandler(this, Array.FindLastIndex(rawBuffer, b => b != zero) - 1);
                        }
                        catch { }

                        Receive();
                    }
                    else throw new Exception("Shut Down");
                }
            }
            catch(Exception) {
                try
                {
                    closeHandler(this);
                }
                catch { }

                Dispose();
            }
        }

        private void SendComplete(IAsyncResult ar)
        {
            try
            {
                if (netStream.CanWrite)
                {
                    netStream.EndWrite(ar);
                }
            }
            catch(Exception) { }
        }

        /// <summary>
        /// Connect the client to a socket
        /// </summary>
        /// <param name="ipAddress">IP Address to connect to (example: "127.0.0.0")</param>
        /// <param name="port">Port to run socket through</param>
        public void Connect(string ipAddress, short port)
        {
            try
            {
                if (netStream == null)
                {
                    this.ipAddress = ipAddress;
                    this.port = port;
                    //Establish connection
                    tcpClient = new TcpClient(ipAddress, port);
                    netStream = tcpClient.GetStream();
                    //Set socket options
                    tcpClient.ReceiveBufferSize = 1048576;
                    tcpClient.SendBufferSize = 1048576;
                    tcpClient.NoDelay = true;
                    tcpClient.LingerState = new LingerOption(false, 0);

                    Receive();
                }
            }
            catch(SocketException e) {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// Disconnect the client from the socket
        /// </summary>
        public void Disconnect()
        {
            if (netStream != null)
                netStream.Close();
            if (tcpClient != null)
                tcpClient.Close();
            if (clientSocket != null)
                clientSocket.Close();

            netStream = null;
            tcpClient = null;
            clientSocket = null;
        }

        /// <summary>
        /// Send data on the socket
        /// </summary>
        /// <param name="buffer">Data to be sent</param>
        public void Send(byte[] buffer)
        {
            Console.WriteLine("Sending message");
            if((netStream != null) && netStream.CanWrite)
            {
                netStream.BeginWrite(buffer, 0, buffer.Length, callbackWriteMethod, null);
            }
            else
            {
                throw new Exception("Socket Closed");
            }
            Console.WriteLine("Message sent");
        }

        /// <summary>
        /// Receive incoming data on the socket
        /// </summary>
        public void Receive()
        {
            Console.WriteLine("Receiving message");
            if ((netStream != null) && netStream.CanRead)
            {
                netStream.BeginRead(rawBuffer, 0, bufferSize, callbackReadMethod, null);
            }
            else
            {
                throw new Exception("Socket Closed");
            }
            Console.WriteLine("Message received");
        }

        /// <summary>
        /// Dispose of this socket
        /// </summary>
        public void Dispose()
        {
            try
            {
                disposedFlag = true;
                Disconnect();
            }
            catch { }

            if (socketServer != null)
                socketServer.RemoveSocket(this);
        }

        //Delegate functions

        /// <summary>
        /// Called when a message is recieved
        /// </summary>
        /// <param name="client">The SocketClient instance</param>
        /// <param name="numBytes">The number of bytes to read from buffer</param>
        public delegate void MESSAGE_HANDLER(SocketClient client, int numBytes);

        /// <summary>
        /// Called when a connection is closed
        /// </summary>
        /// <param name="client">The SocketClient instance</param>
        public delegate void CLOSE_HANDLER(SocketClient client);

        /// <summary>
        /// Called when a socket error occurs
        /// </summary>
        /// <param name="client">The SocketClient instance</param>
        /// <param name="ex">The error that occurred</param>
        public delegate void ERROR_HANDLER(SocketClient client, Exception ex);


    }
}
