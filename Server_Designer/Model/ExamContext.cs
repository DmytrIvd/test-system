using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLibrary;

namespace Server_Designer.Model
{
    class ExamContext : DbContext
    {
        public ExamContext() : base("DBConnection")
        {

        }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Variant> Variants { get; set; }
       

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups{ get; set; }
       

    }
}
