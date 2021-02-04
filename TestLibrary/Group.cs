using System.Collections.Generic;

namespace TestLibrary
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserGroup> GroupUsers { get; set; }
        //intended tests
        public List<TestGroup> TestGroups { get; set; }
    }
}
