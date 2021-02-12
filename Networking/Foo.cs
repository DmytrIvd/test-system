using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(data, 0, data.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
        public static List<Chunk> DivideByChunks(this byte[] data, int chunkSize)
        {
            //Store to int and bool values
         
            List<Chunk> Chunks = new List<Chunk>();
            int countOfArray = data.Length / chunkSize;
            if (data.Length % chunkSize > 0)
                countOfArray++;
            for (int i = 0; i < countOfArray; i++)
            {
                Chunks.Add(new Chunk { data = data.Skip(i * chunkSize).Take(chunkSize).ToArray(), index = i, IsEnd = (i == countOfArray - 1) });

            }
            return Chunks;

        }
        public static byte[] CombineChunks(this IEnumerable<Chunk> chunks)
        {

            var ordered = chunks.OrderBy(c => c.index);
            var lenght = ordered.Sum(x => x.data.Length);
            var data = new byte[lenght];
            int offset = 0;
            foreach (var o in ordered.Select(c=>c.data))
            {
               // Buffer.BlockCopy(o, 0, data, offset, o.Length);
               Buffer.BlockCopy(o, 0, data, offset, o.Length);
                offset += o.Length;
            }
            return data;
        }
    }
    [Serializable]
    public class SerializeDelegate : ISerializable
    {
        public SerializeDelegate(Delegate delegate_)
        {
            this.delegate_ = delegate_;
        }

        public SerializeDelegate(SerializationInfo info, StreamingContext context)
        {
            Type delType = (Type)info.GetValue("delegateType", typeof(Type));

            //If it's a "simple" delegate we just read it straight off
            if (info.GetBoolean("isSerializable"))
                this.delegate_ = (Delegate)info.GetValue("delegate", delType);

            //otherwise, we need to read its anonymous class
            else
            {
                MethodInfo method = (MethodInfo)info.GetValue("method", typeof(MethodInfo));

                AnonymousClassWrapper w =
                    (AnonymousClassWrapper)info.GetValue
                ("class", typeof(AnonymousClassWrapper));

                delegate_ = Delegate.CreateDelegate(delType, w.obj, method);
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("delegateType", delegate_.GetType());

            //If it's an "simple" delegate we can serialize it directly
            if ((delegate_.Target == null ||
                delegate_.Method.DeclaringType
                    .GetCustomAttributes(typeof(SerializableAttribute), false).Length > 0) &&
                delegate_ != null)
            {
                info.AddValue("isSerializable", true);
                info.AddValue("delegate", delegate_);
            }

            //otherwise, serialize anonymous class
            else
            {
                info.AddValue("isSerializable", false);
                info.AddValue("method", delegate_.Method);
                info.AddValue("class",
                    new AnonymousClassWrapper
            (delegate_.Method.DeclaringType, delegate_.Target));
            }
        }

        public Delegate Delegate { get { return delegate_; } }

        Delegate delegate_;

        [Serializable]
        class AnonymousClassWrapper : ISerializable
        {
            internal AnonymousClassWrapper(Type bclass, object bobject)
            {
                this.type = bclass;
                this.obj = bobject;
            }

            internal AnonymousClassWrapper(SerializationInfo info, StreamingContext context)
            {
                Type classType = (Type)info.GetValue("classType", typeof(Type));
                obj = Activator.CreateInstance(classType);

                foreach (FieldInfo field in classType.GetFields())
                {
                    //If the field is a delegate
                    if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                        field.SetValue(obj,
                            ((SerializeDelegate)info.GetValue
                    (field.Name, typeof(SerializeDelegate)))
                                .Delegate);
                    //If the field is an anonymous class
                    else if (!field.FieldType.IsSerializable)
                        field.SetValue(obj,
                            ((AnonymousClassWrapper)info.GetValue
                    (field.Name, typeof(AnonymousClassWrapper)))
                                .obj);
                    //otherwise
                    else
                        field.SetValue(obj, info.GetValue(field.Name, field.FieldType));
                }
            }

            void ISerializable.GetObjectData
            (SerializationInfo info, StreamingContext context)
            {
                info.AddValue("classType", type);

                foreach (FieldInfo field in type.GetFields())
                {
                    //See corresponding comments above
                    if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                        info.AddValue(field.Name, new SerializeDelegate
                        ((Delegate)field.GetValue(obj)));
                    else if (!field.FieldType.IsSerializable)
                        info.AddValue(field.Name, new AnonymousClassWrapper
                    (field.FieldType, field.GetValue(obj)));
                    else
                        info.AddValue(field.Name, field.GetValue(obj));
                }
            }

            public Type type;
            public object obj;
        }
    }
}
