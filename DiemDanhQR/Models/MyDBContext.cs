using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DiemDanhQR.Models
{
    public partial class MyDBContext : DbContext
    {
        public MyDBContext()
            : base("name=MyDBContext")
        {
        }

        public virtual DbSet<BuoiHoc> BuoiHocs { get; set; }
        public virtual DbSet<GiaoVien> GiaoViens { get; set; }
        public virtual DbSet<Khoa> Khoas { get; set; }
        public virtual DbSet<Lop> Lops { get; set; }
        public virtual DbSet<LopMon> LopMons { get; set; }
        public virtual DbSet<MonHoc> MonHocs { get; set; }
        public virtual DbSet<Nhom_ToThucHanh> Nhom_ToThucHanh { get; set; }
        public virtual DbSet<Phong> Phongs { get; set; }
        public virtual DbSet<QR> QRs { get; set; }
        public virtual DbSet<SinhVien> SinhViens { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoanKhoa> TaiKhoanKhoas { get; set; }
        public virtual DbSet<ThoiKhoaBieu_DiemDanh> ThoiKhoaBieu_DiemDanh { get; set; }
        public virtual DbSet<Thu> Thus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.MaGiaoVien)
                .IsUnicode(false);

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.AnhDaiDien)
                .IsUnicode(false);

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<Lop>()
                .Property(e => e.MaLop)
                .IsUnicode(false);

            modelBuilder.Entity<LopMon>()
                .Property(e => e.TenLopMon)
                .IsUnicode(false);

            modelBuilder.Entity<LopMon>()
                .Property(e => e.MaMon)
                .IsUnicode(false);

            modelBuilder.Entity<LopMon>()
                .Property(e => e.MaGiaoVien)
                .IsUnicode(false);

            modelBuilder.Entity<MonHoc>()
                .Property(e => e.MaMon)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MSSV)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.AnhDaiDien)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MaLop)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoanKhoa>()
                .Property(e => e.MaTaiKhoan)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoanKhoa>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoanKhoa>()
                .Property(e => e.AnhDaiDien)
                .IsUnicode(false);

            modelBuilder.Entity<ThoiKhoaBieu_DiemDanh>()
                .Property(e => e.MSSV)
                .IsUnicode(false);
        }
    }
}
