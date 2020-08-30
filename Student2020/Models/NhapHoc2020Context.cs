using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Student2020.Models
{
    public partial class NhapHoc2020Context : DbContext
    {
        public NhapHoc2020Context()
        {
        }

        public NhapHoc2020Context(DbContextOptions<NhapHoc2020Context> options)
            : base(options)
        {
        }

        public virtual DbSet<ThiSinh> ThiSinh { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=WIN-CE230LNQVTR\\SQLEXPRESS;Database=NhapHoc2020;Trusted_Connection=True;user id=sms;password=StYNuRyvPK99G6D6;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThiSinh>(entity =>
            {
                entity.HasKey(e => e.Cmnd);

                entity.Property(e => e.Cmnd)
                    .HasColumnName("CMND")
                    .HasMaxLength(30);

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.DienThoai).HasMaxLength(25);

                entity.Property(e => e.FileGcn)
                    .HasColumnName("FileGCN")
                    .HasMaxLength(100);

                entity.Property(e => e.HotenTs)
                    .HasColumnName("HotenTS")
                    .HasMaxLength(150);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MaNganh).HasMaxLength(15);

                entity.Property(e => e.NgayNopGcn)
                    .HasColumnName("NgayNopGCN")
                    .HasColumnType("datetime");

                entity.Property(e => e.Note1).HasMaxLength(250);

                entity.Property(e => e.PathImage)
                    .HasColumnName("path_image")
                    .HasMaxLength(50);

                entity.Property(e => e.TenNganh).HasMaxLength(250);

                entity.Property(e => e.UsXacNhanBoHs)
                    .HasColumnName("Us_XacNhanBoHS")
                    .HasMaxLength(5);

                entity.Property(e => e.UsXacNhanGcn)
                    .HasColumnName("Us_XacNhanGCN")
                    .HasMaxLength(5);

                entity.Property(e => e.UsXacNhanTien)
                    .HasColumnName("Us_XacNhanTien")
                    .HasMaxLength(5);

                entity.Property(e => e.XacNhanBoHs)
                    .HasColumnName("XacNhanBoHS")
                    .HasColumnType("datetime");

                entity.Property(e => e.XacNhanGcn)
                    .HasColumnName("XacNhanGCN")
                    .HasColumnType("datetime");

                entity.Property(e => e.XacNhanTien)
                    .HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
