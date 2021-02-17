using Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestLibrary;

namespace TestingTheServer
{
    class Program
    {
        public static bool loginIsReceived = false;
        public static bool HeaderSended = false;
        public static Server server;
        static void Main(string[] args)
        {
            using (server = new Server(8888))
            {
                
                server.OnDataReceived += Server_OnDataReceived;
                IGotTheData += Program_IGotTheData1; ;
                server.Start();
                
                while (!loginIsReceived)
                {
                    if (server.NumClients != 0 && !HeaderSended)
                    {
                        // server.SendImmediateToAll(new TcpHeader { Type = MessageType.ServerLoginRequest, MessageLenght = 0 }.Serialize());
                        HeaderSended = true;
                    }
                }

            }
            Console.ReadKey();
        }

        private static void Program_IGotTheData1(object sender, TcpClient e)
        {
            var ch = (List<Chunk>)sender;
            var chunks = ch.CombineChunks();
            var obj = chunks.Deserialize();
            if (type == MessageType.ClientLoginTry)
            {
                var method = ((User)obj);
                // var f = method as Func<User, bool>;
                // var val = f.Invoke(User);
                Func<User, bool> f = new Func<User, bool>(u => u.Login == User.Login && u.Password == User.Password && u.IsAdmin == false); ;
                var val = f.Invoke(method);
                server.SendData(val.Serialize(), MessageType.ServerLoginVerify, e);
               // var val = chunks.Deserialize();
               // loginIsVerifyed = val;
            }

            ReceivedChunks.Clear();
        }

        private static int needToGet = 0;
       // pri  vate static byte
        public static bool HeaderReceived = false;
        private static int chunkCount;
        private static bool GotHeader;
        private static List<Chunk> ReceivedChunks=new List<Chunk>();
        private static User User = new User { IsAdmin = false, Login = "123", Password = "123" };
        private static void Program_IGotTheData(object sender, EventArgs e)
        {
          
        }
        public static event EventHandler<TcpClient> IGotTheData;
       static MessageType type;
        private static void Server_OnDataReceived(byte[] data, int bytesRead, TcpClient client)
        {
            byte[] arr = new byte[bytesRead];
            Array.ConstrainedCopy(data, 0, arr, 0, bytesRead);
            var obj = arr.Deserialize();
            //var obj = data.Deserialize();
            if (!GotHeader)
            {
                var h = obj as TcpHeader;
                chunkCount = h.ChunkCount;
                GotHeader = true;
                type = h.Type;
                return;
            }
            if (chunkCount != 0)
            {
                var c = obj as Chunk;
                ReceivedChunks.Add(c);
                chunkCount--;
                if (c.IsEnd)
                {
                    IGotTheData?.Invoke(ReceivedChunks, client);
                    GotHeader = false;
                }
            }


        }
    }
}
