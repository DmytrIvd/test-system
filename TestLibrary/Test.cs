using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestLibrary
{
    [Serializable]
    public class Test
    {
        [XmlIgnore]
        public int Id { get; set; }
      
        public string Title{ get; set; }
        [XmlIgnore]
        public TimeSpan Time { get; set; }
        public string Author { get; set; }
       //Groups to which this test was indended
        public List<TestGroup> TestGroups{ get; set; }
    }
   }
