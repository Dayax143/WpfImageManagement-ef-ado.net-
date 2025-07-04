using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfEFProfile.ModelsWbh;

public partial class WbhContext : DbContext
{
    public WbhContext()
    {
    }

    public WbhContext(DbContextOptions<WbhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=rt\\rtser;Database=wbh_minisystem;User Id=sa;Password=sa@123;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tblUser__B9BE370F156E167F");

            entity.ToTable("tblUser");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PassWord)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("passWord");
            entity.Property(e => e.RecoveryAnswer)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("recoveryAnswer");
            entity.Property(e => e.RecoveryQuestion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("recoveryQuestion");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.UserStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("userStatus");
            entity.Property(e => e.Usertype)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("usertype");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
