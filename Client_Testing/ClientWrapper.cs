using Networking;
using Server_Designer.ViewModel;
using System;
using System.Collections.Generic;
using TestLibrary;

namespace Client_Testing
{
    public delegate void GotEntity<T>(T[] arr);
    public class ClientWrapper : IDisposable
    {
        #region Events
        public event GotEntity<Group> GotGroups;
        public event GotEntity<Test> GotTests;
        public event LoginAnswer LoginAnswer;
        #endregion
        public ChunksReceiver ChunksReceiver;
        public Client client;

        public void Start(string ipAddress, int port)
        {
            ChunksReceiver = new ChunksReceiver();
            client = new Client();
            client.OnDataReceived += Client_OnDataReceived;
            client.ConnectToServer(ipAddress, port);
        }
        public void Close()
        {
            client.Disconnect();
        }
        #region Sender for types of data
        public void SendLoginTry(User e)
        {
            client.SendData(e.Serialize(), MessageType.ClientLoginTry);
        }
        public void SendGroupsRequest(User e)
        {
            client.SendData(e.Serialize(), MessageType.ClientRequestGroups);
        }
        public void SendTestsRequest(int groupId)
        {
            client.SendData(groupId.Serialize(), MessageType.ClientRequestTest);
        }
        public void SendTestResult(Result result)
        {
            client.SendData(result.Serialize(), MessageType.ClientSendResult);
        }
        #endregion
        #region Receive data stuff
        private void Client_OnDataReceived(byte[] data, int bytesRead)
        {
            byte[] arr = new byte[bytesRead];
            Array.ConstrainedCopy(data, 0, arr, 0, bytesRead);
            ChunksReceiver.ReceiveBytes(arr);
            if (ChunksReceiver.ReceiveAll)
            {

                GotFullChunks(ChunksReceiver.MessageType, ChunksReceiver.Chunks);
                ChunksReceiver.Clear();
            }
        }

        private void GotFullChunks(MessageType messageType, List<Chunk> receivedChunks)
        {
            //1. Server send Login Verify
            //2. Server send groups
            //3. Server send test for specified group
            switch (messageType)
            {
                case MessageType.ServerLoginVerify:
                    {
                        var val = CombineChunksInto<bool>(receivedChunks);
                        LoginAnswer?.Invoke(val);
                        break;
                    }
                case MessageType.ServerSendGroups:
                    {
                        var val = CombineChunksInto<Group[]>(receivedChunks);
                        GotGroups?.Invoke(val);
                        break;
                    }
                case MessageType.ServerSendTest:
                    {
                        var val = CombineChunksInto<Test[]>(receivedChunks);
                        GotTests?.Invoke(val);
                        break;
                    }
            }
            receivedChunks.Clear();
            // messageType = null;
        }

        private T CombineChunksInto<T>(List<Chunk> receivedChunks)
        {
            var data = receivedChunks.CombineChunks();
            var obj = data.Deserialize();
            if (obj is T)
            {
                return (T)obj;
            }
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
        #endregion
        #region Cleaners
        public void Dispose()
        {
            client.Dispose();
        }
        ~ClientWrapper()
        {
            client.Dispose();
        }
        #endregion
    }

}
