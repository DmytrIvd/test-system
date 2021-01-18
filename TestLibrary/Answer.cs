using System;

namespace TestLibrary
{
    [Serializable]
    public class Answer
    {
        public Question Question{ get; set; }
        public Variant UserAnswer{ get; set; }
    }
}
