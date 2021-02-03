using System.Collections.Generic;

namespace TestLibrary
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public List<Test> Tasks { get; set; }
    }
}
