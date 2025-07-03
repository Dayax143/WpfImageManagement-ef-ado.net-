using Microsoft.EntityFrameworkCore;

namespace WpfEFProfile.EF
{
    public class MyAppContext : DbContext
    {
        string connectionString = Properties.Settings.Default.sqlConnection;
        //string connectionString = "Server=IT-PC2; Database=wbh_minisystem; user id=sa; password=123; trustservercertificate=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Test> test { get; set; }
        public DbSet<Sessions> sessions { get; set; }
    }
}
