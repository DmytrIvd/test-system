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

        [XmlElement("Text")]
        public string Question_str { get; set; }
        public List<Variant> Variants { get; set; }
    }
}
