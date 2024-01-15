using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuanLySinhVien.Models.Entity
{
    public partial class QuanLySinhVienContext : DbContext
    {
        public QuanLySinhVienContext()
        {
        }

        public QuanLySinhVienContext(DbContextOptions<QuanLySinhVienContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Diem> Diems { get; set; } = null!;
        public virtual DbSet<Khoa> Khoas { get; set; } = null!;
        public virtual DbSet<Lop> Lops { get; set; } = null!;
        public virtual DbSet<MonHoc> MonHocs { get; set; } = null!;
        public virtual DbSet<Nghanh> Nghanhs { get; set; } = null!;
        public virtual DbSet<SinhVien> SinhViens { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-HKTU0QF\\SQLEXPRESS;Initial Catalog=QuanLySinhVien;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diem>(entity =>
            {
                entity.HasKey(e => new { e.MaDiem, e.MaMonHoc, e.MaSv })
                    .HasName("PK__Diem__AC07321A5B81A0B2");

                entity.ToTable("Diem");

                entity.Property(e => e.MaDiem)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaMonHoc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaSv)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.MaMonHocNavigation)
                    .WithMany(p => p.Diems)
                    .HasForeignKey(d => d.MaMonHoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Diem_MonHoc");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.Diems)
                    .HasForeignKey(d => d.MaSv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Diem_SinhVien");
            });

            modelBuilder.Entity<Khoa>(entity =>
            {
                entity.HasKey(e => e.MaKhoa);

                entity.ToTable("Khoa");

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NamThanhLap).HasMaxLength(100);

                entity.Property(e => e.TenKhoa).HasMaxLength(100);
            });

            modelBuilder.Entity<Lop>(entity =>
            {
                entity.HasKey(e => e.MaLop)
                    .HasName("PK__Lop__3B98D27357D8F454");

                entity.ToTable("Lop");

                entity.Property(e => e.MaLop)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaNghanh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenLop).HasMaxLength(50);

                entity.HasOne(d => d.MaNghanhNavigation)
                    .WithMany(p => p.Lops)
                    .HasForeignKey(d => d.MaNghanh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lop_Nghanh");
            });

            modelBuilder.Entity<MonHoc>(entity =>
            {
                entity.HasKey(e => e.MaMh)
                    .HasName("PK__MonHoc__2725DF3979C94319");

                entity.ToTable("MonHoc");

                entity.Property(e => e.MaMh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ki).HasMaxLength(20);

                entity.Property(e => e.TenMh).HasMaxLength(50);
            });

            modelBuilder.Entity<Nghanh>(entity =>
            {
                entity.HasKey(e => e.MaNghanh)
                    .HasName("PK__Nghanh__292470ADA4C6BA00");

                entity.ToTable("Nghanh");

                entity.Property(e => e.MaNghanh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenNghanh).HasMaxLength(50);

                entity.HasOne(d => d.MaKhoaNavigation)
                    .WithMany(p => p.Nghanhs)
                    .HasForeignKey(d => d.MaKhoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nghanh_Khoa");
            });

            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.HasKey(e => e.MaSv)
                    .HasName("PK__SinhVien__2725087A7245122E");

                entity.ToTable("SinhVien");

                entity.Property(e => e.MaSv)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AnhSv).HasMaxLength(255);

                entity.Property(e => e.Cccd)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("CCCD");

                entity.Property(e => e.DiaChi).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.MaLop)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(10)
                    .HasColumnName("SDT");

                entity.Property(e => e.TenSv).HasMaxLength(20);

                entity.HasOne(d => d.MaLopNavigation)
                    .WithMany(p => p.SinhViens)
                    .HasForeignKey(d => d.MaLop)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SinhVien_Lop");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
