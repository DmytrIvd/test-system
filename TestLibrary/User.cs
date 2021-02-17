using System;
using System.Collections.Generic;

namespace TestLibrary
{
    [Serializable]
    public class User : IEntity
    {
        public User()
        {
            Groups = new List<Group>();
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}
