using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestLibrary
{
    [Serializable]
    public class Test:IEntity
    {
        public Test()
        {
            Groups = new List<Group>();
            Questions = new List<Question>();
        }

        [XmlIgnore]
        public int Id { get; set; }
      
        public string Title{ get; set; }
        [XmlIgnore]
        public TimeSpan Time { get; set; }
        public string Author { get; set; }
       //Groups to which this test was indended
        public virtual List<Group> Groups{ get; set; }
        public virtual List<Question> Questions{ get; set; }
    }
   }
