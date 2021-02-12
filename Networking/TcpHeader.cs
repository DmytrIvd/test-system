using System;

namespace Networking
{

    [Serializable]
    public class TcpHeader{
        public MessageType Type;
        public int ChunkCount;
    }
}
