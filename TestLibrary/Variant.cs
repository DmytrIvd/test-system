using System;
using System.Xml.Serialization;

namespace TestLibrary
{
    [Serializable]
    public class Variant
    {
        public Variant()
        {
        }

        [XmlElement("Text")]
        public string Variant_str { get; set; }
        public bool IsRight { get; set; }
    }
    //[Serializable]
    //public class Answer{
    //public Questui
    //}
}
