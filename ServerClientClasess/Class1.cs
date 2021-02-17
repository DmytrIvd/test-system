using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLibrary;

namespace ServerClientClasess
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public List<Group> Groups { get; set; }
    }
    public class Group
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
    public class Tasks
    {
        public Test test { get; set; }
        public TimeSpan Time{ get; set; }
        public User Admin{ get; set; }
        public List<Group> Groups { get; set; }
    }
}
