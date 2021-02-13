using Networking;
using Server_Designer.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using TestLibrary;

namespace Server_Designer
{

    public delegate void Get(object entity, TcpClient tcpClient);
    public delegate void SendResult(Result result, TcpClient tcpClient);
    public class ServerWrapper
    {
        public Dictionary<TcpClient, ChunksReceiver> ClientsData;
        public Server server;

        internal void Start(int port)
        {
            ClientsData = new Dictionary<TcpClient, ChunksReceiver>();
            server = new Server(port);
            server.OnDataReceived += Server_OnDataReceived;
            //server.OnDataReceived += Client_OnDataReceived;
            server.Start();
        }
        public void Stop()
        {
            server.Stop();
        }

        #region Events
        public event Get GetGroups;
        public event Get GetTests;
        public event LoginTryForClient LogTry;
        public event SendResult ResultSend;
        #endregion
        #region Senders
        public void SendLoginAnswer(bool answer, TcpClient Towhere)
        {
            server.SendData(answer.Serialize(), MessageType.ServerLoginVerify, Towhere);
        }
        public void SendGroup(Group[] groups, TcpClient Towhere)
        {
            server.SendData(groups.Serialize(), MessageType.ServerSendGroups, Towhere);
        }
        public void SendTests(Test[] tests, TcpClient Towhere)
        {
            server.SendData(tests.Serialize(), MessageType.ServerSendTest, Towhere);
        }
        #endregion
        #region Chunks stuff
        private void Server_OnDataReceived(byte[] data, int bytesRead, System.Net.Sockets.TcpClient client)
        {
            byte[] arr = new byte[bytesRead];
            Array.ConstrainedCopy(data, 0, arr, 0, bytesRead);
            ChunksReceiver chunksReceiver;
            if (!ClientsData.ContainsKey(client))
            {
                ClientsData.Add(client, new ChunksReceiver());


            }
            chunksReceiver = ClientsData[client];
            chunksReceiver.ReceiveBytes(arr);
            if (chunksReceiver.ReceiveAll)
            {
                GotFullChunks(chunksReceiver.MessageType, client, chunksReceiver.Chunks);
                chunksReceiver.Clear();
            }
        }

        private void GotFullChunks(MessageType messageType, TcpClient client, List<byte[]> receivedChunks)
        {
            //1. Client send Login Try
            //2. Client asks for a groups
            //3. Client asks for a tests
            //4. Client send result
            switch (messageType)
            {
                case MessageType.ClientLoginTry:
                    {
                        var val = receivedChunks.CombineChunksInto<User>();
                        LogTry?.Invoke(val, client);
                        break;
                    }
                case MessageType.ClientRequestGroups:
                    {
                        var val = receivedChunks.CombineChunksInto<User>();
                        GetGroups?.Invoke(val, client);
                        break;
                    }
                case MessageType.ClientRequestTests:
                    {

                        //Group id
                        var val = receivedChunks.CombineChunksInto<int>();
                        GetTests?.Invoke(val, client);
                        break;
                    }
                case MessageType.ClientSendResult:
                    {
                        var val = receivedChunks.CombineChunksInto<Result>();
                        ResultSend?.Invoke(val, client);
                        break;

                    }
            }
        }


        #endregion
        #region Cleaners
        public void Dispose()
        {
            server.Dispose();
        }
        ~ServerWrapper()
        {
            server.Dispose();
        }
        #endregion
    }

}

