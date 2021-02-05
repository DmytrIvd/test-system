using System.Collections.Generic;

namespace TestLibrary
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        //intended tests
        public List<Test> Tests { get; set; }
    }
}
