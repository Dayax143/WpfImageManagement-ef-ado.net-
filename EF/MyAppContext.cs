using Microsoft.EntityFrameworkCore;

namespace WpfEFProfile.EF
{
    public class MyAppContext : DbContext
    {
        //string connectionString = Properties.Settings.Default.sqlConnection;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=rt\\rtser; Database=testDB; user id=sa; password=sa@123; trustservercertificate=true");
        }

        public DbSet<Test> test { get; set; }
        public DbSet<Sessions> sessions { get; set; }
    }
}
