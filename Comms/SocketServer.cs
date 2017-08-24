using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Comms
{
    /// <summary>
    /// Class abstracting a socket server
    /// </summary>
    public class SocketServer
    {
        /// <summary> TcpListener for accepting socket connections </summary>
        private TcpListener tcpListener { get; set; }
        /// <summary> Thread to process accepting socket connections </summary>
        private Thread acceptThread { get; set; }
        /// <summary> Array of current SocketClients </summary>
        private SocketClient[] socketClientList { get; set; }
        /// <summary> Maximum number of clients the server will take </summary>
        public int maxClientConnections { get; set; }
        /// <summary> Buffer size to be passed to new SocketClients upon instantiation </summary>
        private int bufferSize { get; set; }
        
        /// <summary> Function to be called when a socket message arrives </summary>
        private MESSAGE_HANDLER messageHandler { get; set; }
        /// <summary> Function to be called when a socket connection is closed </summary>
        private CLOSE_HANDLER closeHandler { get; set; }
        /// <summary> Function to be called when a socket error occurs </summary>
        private ERROR_HANDLER errorHandler { get; set; }
        /// <summary> Function to be called when a socket connection is accepted </summary>
        private ACCEPT_HANDLER acceptHandler { get; set; }

        /// <summary> A synchronization object to protect the class state </summary>
        private Object socketListSyncObj { get; set; }
        /// <summary> Flag to indicate if class is being disposed </summary>
        private bool disposedFlag { get; set; }

        /// <summary> The IP Address to connect to/listen on </summary>
        public string ipAddress { get; set; }
        /// <summary> The port to connect to/listen on </summary>
        public short port { get; set; }
        /// <summary> Object associated with socket connection to be passed through handler functions </summary>
        public Object userArg { get; set; }

        public SocketServer(){
            socketListSyncObj = new Object();

            disposedFlag = false;
        }

        ~SocketServer()
        {
            if (!disposedFlag)
                Stop();
        }

        public void Dispose()
        {
            try
            {
                disposedFlag = true;

                if (acceptThread != null)
                    Stop();
            }
            catch { }
        }

        /// <summary>
        /// Function to locate an empty slot in the socketClientList
        /// </summary>
        /// <returns>An open index or maxClientConnections if none free</returns>
        private int LocateSocketIndex()
        {
            int index = maxClientConnections;
            Monitor.Enter(socketClientList);

            try
            {
                for (index = 0; index < maxClientConnections; ++index)
                {
                    if (socketClientList[index] == null)
                        break;
                }
            }
            catch { }

            Monitor.Exit(socketClientList);
            return index;
        }

        /// <summary>
        /// Function to process and accept incoming connections
        /// </summary>
        private void AcceptThread()
        {
            Socket clientSocket = null;

            try
            {
                tcpListener = new TcpListener(Dns.GetHostEntry(ipAddress).AddressList[0], port);
                tcpListener.Start();

                for (; ; )
                {
                    clientSocket = tcpListener.AcceptSocket();
                    if (clientSocket.Connected)
                    {
                        int index = LocateSocketIndex();

                        if(index != maxClientConnections)
                        {
                            socketClientList[index] = new SocketClient(this,
                                clientSocket,
                                index,
                                clientSocket.RemoteEndPoint.ToString().Substring(0, 15),
                                port,
                                bufferSize,
                                userArg,
                                new SocketClient.MESSAGE_HANDLER(messageHandler),
                                new SocketClient.CLOSE_HANDLER(closeHandler),
                                new SocketClient.ERROR_HANDLER(errorHandler));

                            acceptHandler(socketClientList[index]);
                        }
                        else
                        {
                            errorHandler(null, new Exception("Unable to accept socket connection"));

                            clientSocket.Close();
                        }
                    }
                }
            }
            catch(SocketException e) {
                if(e.ErrorCode != 10004)
                {
                    errorHandler(null, e);

                    if (clientSocket != null)
                        if (clientSocket.Connected)
                            clientSocket.Close();
                }
            }
            catch(Exception e)
            {
                errorHandler(null, e);

                if (clientSocket != null)
                    if (clientSocket.Connected)
                        clientSocket.Close();
            }
        }

        public void RemoveSocket(SocketClient client)
        {
            Monitor.Enter(socketClientList);

            try
            {
                if (client == socketClientList[client.socketListIndex])
                    socketClientList[client.socketListIndex] = null;
            }
            catch { }

            Monitor.Exit(socketClientList);
        }

        /// <summary>
        /// Starts the SocketServer
        /// </summary>
        /// <param name="ipAddress">IP Address to listen on</param>
        /// <param name="port">Port to listen on</param>
        /// <param name="maxClientConnections">Maximum number of client connections</param>
        /// <param name="bufferSize">Size of incoming data buffer</param>
        /// <param name="userArg">(Optional) User supplied args. If not in use, pass in null</param>
        /// <param name="msgHandler">Pointer to custom MessageHandler function</param>
        /// <param name="acptHandler">Pointer to custom AcceptHandler function</param>
        /// <param name="clsHandler">Pointer to custom CloseHAndler function</param>
        /// <param name="errHandler">Pointer to custom ErrorHandler function</param>
        public void Start(string ipAddress, short port, int maxClientConnections, int bufferSize, Object userArg, 
            MESSAGE_HANDLER msgHandler, ACCEPT_HANDLER acptHandler, CLOSE_HANDLER clsHandler, ERROR_HANDLER errHandler)
        {
            if(acceptThread == null)
            {
                this.ipAddress = ipAddress;
                this.port = port;
                this.maxClientConnections = maxClientConnections;

                socketClientList = new SocketClient[maxClientConnections];

                this.bufferSize = bufferSize;
                this.userArg = userArg;
                messageHandler = msgHandler;
                acceptHandler = acptHandler;
                closeHandler = clsHandler;
                errorHandler = errHandler;

                ThreadStart ts = new ThreadStart(AcceptThread);
                acceptThread = new Thread(ts)
                {
                    Name = "Accept"
                };

                acceptThread.Start();
            }
        }

        /// <summary>
        /// Stops the SocketServer
        /// </summary>
        public void Stop()
        {
            if(acceptThread != null)
            {
                tcpListener.Stop();
                acceptThread.Join();
                acceptThread = null;
            }

            for(int i = 0; i < maxClientConnections; i++)
            {
                if(socketClientList[i] != null)
                {
                    socketClientList[i].Dispose();
                    socketClientList[i] = null;
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            messageHandler = null;
            acceptHandler = null;
            closeHandler = null;
            errorHandler = null;
            bufferSize = 0;
            userArg = null;
        }

        /// <summary>
        /// Called when a message is recieved
        /// </summary>
        /// <param name="client">The SocketClient instance</param>
        /// <param name="numBytes">Number of bytes to read from buffer</param>
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

        /// <summary>
        /// Called when a connection is accepted
        /// </summary>
        /// <param name="client">The SocketClient instance</param>
        public delegate void ACCEPT_HANDLER(SocketClient client);
    }
}
