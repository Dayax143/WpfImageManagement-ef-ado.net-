using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WpfEFProfile.EF;

namespace WpfEFProfile.wbhLoraModels;

public partial class WbhLora1 : DbContext
{
    public WbhLora1()
    {
    }

    public WbhLora1(DbContextOptions<WbhLora1> options)
        : base(options)
    {
    }

    public virtual DbSet<TblLora> TblLoras { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=rt\\rtser;Database=wbh_minisystem;User Id=sa;Password=sa@123;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblLora>(entity =>
        {
            entity.HasKey(e => e.LoraId).HasName("PK__tblLora__7514413BF7B30D90");

            entity.ToTable("tblLora");

            entity.Property(e => e.LoraId).HasColumnName("lora_id");
            entity.Property(e => e.CorSupply)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("cor_supply");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.LoraSerial)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("lora_serial");
            entity.Property(e => e.ReceiptRv)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("receipt_rv");
            entity.Property(e => e.Refference)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("refference");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
