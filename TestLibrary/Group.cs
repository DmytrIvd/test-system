using System.Collections.Generic;

namespace TestLibrary
{
    public class Group:IEntity
    {
        public Group()
        {
            Users = new List<User>();
            Tests = new List<Test>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }
        //intended tests
        public virtual List<Test> Tests { get; set; }
    }
   
}
