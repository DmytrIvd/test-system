using System;

namespace Networking
{
    [Serializable]
    public class Chunk{
        public int index;
        public byte[] data;
        public bool IsEnd;
    }
}
