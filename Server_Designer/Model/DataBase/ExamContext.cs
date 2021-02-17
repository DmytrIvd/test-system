using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLibrary;

namespace Server_Designer.Model
{
    public class ExamContext : DbContext
    {
        public ExamContext() : base("DBConnection")
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {





            modelBuilder.Entity<Group>().Property(g => g.Name).HasMaxLength(40).HasColumnType("nvarchar").IsRequired();

            modelBuilder.Entity<Test>().Property(t => t.Author).HasMaxLength(40).IsRequired();
            modelBuilder.Entity<Test>().Property(t => t.Title).HasMaxLength(40).HasColumnType("nvarchar").IsRequired();

            modelBuilder.Entity<User>().Property(u => u.Login).HasMaxLength(20).HasColumnType("nvarchar").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).HasMaxLength(20).HasColumnType("nvarchar").IsRequired();

            modelBuilder.Entity<Variant>().Property(v => v.Variant_str).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<Question>().Property(q => q.Question_str).HasMaxLength(50).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Variant> Variants { get; set; }

    }
}
