using Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TestLibrary;

namespace TestingClient
{
    class Program
    {
        public static Client client;
        public static bool loginIsVerifyed = false;
        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            using (client = new Client())
            {
                client.OnDataReceived += Client_OnDataReceived;
                client.ConnectToServer("192.168.0.103", 8888);
                IGotTheData += Program_IGotTheData;
                while (!loginIsVerifyed)
                {
                    Console.WriteLine("Enter str");
                    string str = Console.ReadLine();
                   var User = new User { IsAdmin = false, Login = str, Password = str };
                    Func<User, bool> func = new Func<User, bool>(x => x.IsAdmin == false && x.Login == str && x.Password == str);
                    var B = User.Serialize();

                    client.SendData(B, MessageType.ClientLoginTry);
                    //  client.AddToPacket(ASCIIEncoding.ASCII.GetBytes(str));
                    //client.FlushData();
                }
            }


            // client.Disconnect();
            Console.ReadKey();
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {

            System.Reflection.Assembly ayResult = null;
            string sShortAssemblyName = args.Name.Split(',')[0];
            System.Reflection.Assembly[] ayAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (System.Reflection.Assembly ayAssembly in ayAssemblies)
            {
                if (sShortAssemblyName == ayAssembly.FullName.Split(',')[0])
                {
                    ayResult = ayAssembly;
                    break;
                }
            }
            return ayResult;

        }

        private static void Program_IGotTheData(object sender, EventArgs e)
        {
            var ch = (List<Chunk>)sender;
            var chunks = ch.CombineChunks();
            if (type == MessageType.ServerLoginVerify)
            {
                var val = (bool)chunks.Deserialize();
                loginIsVerifyed = val;
            }

            ReceivedChunks.Clear();
        }
        static MessageType type;
        static List<Chunk> ReceivedChunks = new List<Chunk>();
        static int chunkCount = 0;
        public static bool GotHeader = false;

        public static event EventHandler IGotTheData;

        private static void Client_OnDataReceived(byte[] data, int bytesRead)
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
                    IGotTheData?.Invoke(ReceivedChunks, EventArgs.Empty);
                    GotHeader = false;
                }
            }



            Console.WriteLine(loginIsVerifyed);


        }
    }
}



