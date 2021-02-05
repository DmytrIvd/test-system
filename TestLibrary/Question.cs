using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestLibrary
{
    [Serializable]
    public class Question
    {
        public Question()
        {
        }
        [XmlIgnore]
        public int Id{ get; set; }
        [XmlElement("Text")]
        public string Question_str { get; set; }
        public List<Variant> Variants { get; set; }
        public int Dificulty { get; set; }
        [XmlIgnore]
        public Test Test { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Question question &&
                   Question_str == question.Question_str &&
                   EqualityComparer<List<Variant>>.Default.Equals(Variants, question.Variants) &&
                   Dificulty == question.Dificulty;
        }

        public override int GetHashCode()
        {
            int hashCode = 1103400834;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Question_str);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Variant>>.Default.GetHashCode(Variants);
            hashCode = hashCode * -1521134295 + Dificulty.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return "Name:" + Question_str + "|Dificulty:" + Dificulty + "|Count of variants:" + Variants.Count;
        }
    }
}
