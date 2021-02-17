using System.Data.Entity;
using TestLibrary;

namespace Server_Designer.Model
{
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<ExamContext>
    {
        protected override void Seed(ExamContext context)
        {
            context.Users.Add(new User { IsAdmin = true, Login = "1", Password = "1" });
            context.Users.Add(new User { IsAdmin = false, Login = "2", Password = "2" });
            base.Seed(context);
        }
    }
}
