using Microsoft.EntityFrameworkCore;

namespace WpfEFProfile.EF
{
    public class MyAppContext : DbContext
    {
        string connectionString = Properties.Settings.Default.sqlConnection;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Test> test { get; set; }
    }
}
