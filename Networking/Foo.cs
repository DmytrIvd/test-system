using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
namespace Networking
{
    public static class Foo
    {
        public static byte[] Serialize(this object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }
        public static object Deserialize(this byte[] data)
        {
            MemoryStream memStream = new MemoryStream(data);
            BinaryFormatter binForm = new BinaryFormatter();
            // memStream.Write(data, 0, data.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
        public static List<byte[]> DivideToChunks(this byte[] data, int chunkSize)
        {
            //Store to int and bool values
            List<byte[]> bytes = new List<byte[]>();

            int countOfArray = data.Length / chunkSize;
            if (data.Length % chunkSize > 0)
                countOfArray++;
            for (int i = 0; i < countOfArray; i++)
            {
                bytes.Add(data.Skip(i * chunkSize).Take(chunkSize).ToArray());

            }
            return bytes;

        }

        public static byte[] CombineChunks(this IEnumerable<byte[]> chunks)
        {

            var totalLenght = chunks.Sum(a => a.Length);
            var data = new byte[totalLenght];
            int offset = 0;
            foreach (var o in chunks)
            {
                // Buffer.BlockCopy(o, 0, data, offset, o.Length);
                Buffer.BlockCopy(o, 0, data, offset, o.Length);
                offset += o.Length;
            }
            return data;
        }

        public static T CombineChunksInto<T>(this IEnumerable<byte[]> receivedChunks)
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
    }
}


