using System;
using System.Collections.Generic;
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

        public override bool Equals(object obj)
        {
            return obj is Variant variant &&
                   Variant_str == variant.Variant_str &&
                   IsRight == variant.IsRight;
        }

        public override int GetHashCode()
        {
            int hashCode = 46551335;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Variant_str);
            hashCode = hashCode * -1521134295 + IsRight.GetHashCode();
            return hashCode;
        }
    }
    //[Serializable]
    //public class Answer{
    //public Questui
    //}
}
