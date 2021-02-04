using System.Collections.Generic;

namespace TestLibrary
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public IList<UserGroup> UserGroups { get; set; }
    }
}
