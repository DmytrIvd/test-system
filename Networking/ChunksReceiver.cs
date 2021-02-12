using System.Collections.Generic;

namespace Networking
{
    public class ChunksReceiver
    {
        public event GotAllChunks GotAll;
        public List<Chunk> Chunks = new List<Chunk>();
        private bool GotHeader;
        public MessageType MessageType;
        private int chunkCount;
        public bool ReceiveAll;
        public void ReceiveBytes(byte[] arr)
        {
            var obj = arr.Deserialize();
            //var obj = data.Deserialize();
            if (!GotHeader)
            {
                var h = obj as TcpHeader;
                chunkCount = h.ChunkCount;
                GotHeader = true;
                MessageType = h.Type;
                ReceiveAll = false;
                return;
            }
            if (chunkCount != 0)
            {
                var c = obj as Chunk;
                Chunks.Add(c);
                chunkCount--;
                if (c.IsEnd)
                {
                    ReceiveAll = true;
                    //GotFullChunks(MessageType, ReceivedChunks);
                    GotHeader = false;
                }
            }
        }
        public void Clear()
        {
            Chunks.Clear();

        }
    }
    public delegate void GotAllChunks(MessageType messageType, List<Chunk> receivedChunks);
    //public class ServerDataCollector
    //{
    //    public bool GotHeader;
    //    private int chunkCount = 0;
    //    private MessageType type;
    //    private List<Chunk> ReceivedChunks = new List<Chunk>();
    //    public IG
    //    public void Client_OnDataReceived(byte[] data, int bytesRead)
    //    {
    //        byte[] arr = new byte[bytesRead];
    //        Array.ConstrainedCopy(data, 0, arr, 0, bytesRead);
    //        var obj = arr.Deserialize();
    //        //var obj = data.Deserialize();
    //        if (!GotHeader)
    //        {
    //            var h = obj as TcpHeader;
    //            chunkCount = h.ChunkCount;
    //            GotHeader = true;
    //            type = h.Type;
    //            return;
    //        }
    //        if (chunkCount != 0)
    //        {
    //            var c = obj as Chunk;
    //            ReceivedChunks.Add(c);
    //            chunkCount--;
    //            if (c.IsEnd)
    //            {
    //                IGotTheData?.Invoke(ReceivedChunks, EventArgs.Empty);
    //                GotHeader = false;
    //            }
    //        }
    //    }
    //}
}
