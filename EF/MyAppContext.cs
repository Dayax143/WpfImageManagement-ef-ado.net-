using Microsoft.EntityFrameworkCore;

namespace WpfEFProfile.EF
{
    public class MyAppContext : DbContext
    {
        string connectionString = Properties.Settings.Default.sqlConnection;
		//string connectionString = "Server=rt\\rtser; Database=testDB; user id=sa; password=sa@123; trustservercertificate=true";
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Test> test { get; set; }
        public DbSet<Sessions> sessions { get; set; }
        public DbSet<TblUser> TblUser { get; set; }    
        public DbSet<TblLora> TblLora { get; set; }    

        public DbSet<tblMedia> tblMedia { get; set; } 
    }
}
