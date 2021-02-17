using System;
using System.Net.Sockets;
using System.Threading;

namespace Networking
{

    /// <summary>
    /// Implements a simple TCP client which connects to a specified server and
    /// raises C# events when data is received from the server
    /// </summary>
    public class Client : IDisposable
    {
        public event ClientHandlePacketData OnDataReceived;

        private TcpClient tcpClient;
        private NetworkStream clientStream;
        private NetworkBuffer buffer;
        private int writeBufferSize = 1024;
        private int readBufferSize = 1024;
        private int port;
        private bool started = false;
        private bool disposedValue;

        /// <summary>
        /// Constructs a new client
        /// </summary>
        public Client()
        {
            buffer = new NetworkBuffer();
            buffer.WriteBuffer = new byte[writeBufferSize];
            buffer.ReadBuffer = new byte[readBufferSize];
            buffer.CurrentWriteByteCount = 0;
        }

        public void SendData(byte[] b, MessageType clientMessage)
        {

            SendImmediate(new TcpHeader { ChunkCount = b.Length, Type = clientMessage }.Serialize());
            foreach (var ch in b.DivideToChunks(1024))
            {
                AddToPacket(ch);

            }
            FlushData();
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Initiates a TCP connection to a TCP server with a given address and port
        /// </summary>
        /// <param name="ipAddress">The IP address (IPV4) of the server</param>
        /// <param name="port">The port the server is listening on</param>
        public void ConnectToServer(string ipAddress, int port)
        {
            this.port = port;

            tcpClient = new TcpClient(ipAddress, port);
            clientStream = tcpClient.GetStream();
            Console.WriteLine("Connected to server, listening for packets");

            Thread t = new Thread(new ThreadStart(ListenForPackets));
            started = true;
            t.Start();
        }

        /// <summary>
        /// This method runs on its own thread, and is responsible for
        /// receiving data from the server and raising an event when data
        /// is received
        /// </summary>
        private void ListenForPackets()
        {
            int bytesRead;

            while (started)
            {
                bytesRead = 0;
                Array.Clear(buffer.ReadBuffer, 0, buffer.ReadBuffer.Length);
                try
                {
                    //Blocks until a message is received from the server
                    bytesRead = clientStream.Read(buffer.ReadBuffer, 0, readBufferSize);
                }
                catch
                {
                    //A socket error has occurred
                    Console.WriteLine("A socket error has occurred with the client socket " + tcpClient.ToString());
                    break;
                }

                if (bytesRead == 0)
                {
                    //The server has disconnected
                    break;
                }

                if (OnDataReceived != null)
                {
                    //Send off the data for other classes to handle
                    OnDataReceived(buffer.ReadBuffer, bytesRead);
                }

                Thread.Sleep(15);
            }

            started = false;
            Disconnect();
        }

        /// <summary>
        /// Adds data to the packet to be sent out, but does not send it across the network
        /// </summary>
        /// <param name="data">The data to be sent</param>
        public void AddToPacket(byte[] data)
        {
            if (!IsConnected())
            {
                return;
            }

            if (buffer.CurrentWriteByteCount + data.Length > buffer.WriteBuffer.Length)
            {
                FlushData();
            }

            Array.ConstrainedCopy(data, 0, buffer.WriteBuffer, buffer.CurrentWriteByteCount, data.Length);
            buffer.CurrentWriteByteCount += data.Length;
        }

        /// <summary>
        /// Flushes all outgoing data to the server
        /// </summary>
        public void FlushData()
        {
            if (!IsConnected())
            {
                return;
            }

            clientStream.Write(buffer.WriteBuffer, 0, buffer.CurrentWriteByteCount);
            clientStream.Flush();
            buffer.CurrentWriteByteCount = 0;
        }

        /// <summary>
        /// Sends the byte array data immediately to the server
        /// </summary>
        /// <param name="data"></param>
        public void SendImmediate(byte[] data)
        {
            if (!IsConnected())
            {
                return;
            }

            AddToPacket(data);
            FlushData();
        }

        /// <summary>
        /// Tells whether we're connected to the server
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return started && tcpClient.Connected;
        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            if (tcpClient == null)
            {
                return;
            }

            Console.WriteLine("Disconnected from server");

            tcpClient.Close();

            started = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                if (tcpClient != null)
                    tcpClient.Client.Close();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Client()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
    //public class ServerClientWrapper{
    //public 
    //}
}
