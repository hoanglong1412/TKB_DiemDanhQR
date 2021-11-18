using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiemDanhQR.Models;

namespace DiemDanhQR.DAO
{
    public class ThoiKhoaBieuDAO
    {
        private MyDBContext db;
        public ThoiKhoaBieuDAO()
        {
            db = new MyDBContext();
        }

        public IEnumerable<ThoiKhoaBieu_DiemDanh> LayThoiKhoaBieu(string mssv)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = db.ThoiKhoaBieu_DiemDanh.Where(m => m.MSSV == mssv);
            return ds_TKB;
        }

        public IEnumerable<ThoiKhoaBieu_DiemDanh> LayThoiKhoaBieuTheoThuTrongTuan(string mssv)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = db.ThoiKhoaBieu_DiemDanh.Where(m => m.MSSV == mssv).OrderBy(m => m.LopMon.BuoiHoc.MaThu)
                   .ThenBy(m => m.LopMon.BuoiHoc.TietBatDau);
            return ds_TKB;
        }

        public IEnumerable<ThoiKhoaBieu_DiemDanh> LayThoiKhoaBieuBanCanSu(string mssv)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = db.ThoiKhoaBieu_DiemDanh.Where(m => m.MSSV == mssv && m.LaBanCanSu == true);
            return ds_TKB;
        }

        public IEnumerable<ThoiKhoaBieu_DiemDanh> LayDsThoiKhoaBieuTheoMon(int malopmon)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = db.ThoiKhoaBieu_DiemDanh.Where(m => m.MaLopMon == malopmon);
            return ds_TKB;
        }

        public ThoiKhoaBieu_DiemDanh LayThoiKhoaBieuTheoMon(int malopmon)
        {
            ThoiKhoaBieu_DiemDanh TKB = db.ThoiKhoaBieu_DiemDanh.Where(m => m.MaLopMon == malopmon).FirstOrDefault();
            return TKB;
        }

        public ThoiKhoaBieu_DiemDanh LayThoiKhoaBieuTheoMonVaSinhVien(int malopmon, string mssv)
        {
            ThoiKhoaBieu_DiemDanh TKB = db.ThoiKhoaBieu_DiemDanh.Where(m => m.MaLopMon == malopmon && m.MSSV == mssv).FirstOrDefault();
            return TKB;
        }

        public ThoiKhoaBieu_DiemDanh CapNhatXacNhanDiemDanh(int malopmon, string mssv, DateTime NgayCapNhat)
        {
            ThoiKhoaBieu_DiemDanh row = LayThoiKhoaBieuTheoMonVaSinhVien(malopmon, mssv);
            row.NgayDuyet = NgayCapNhat;
            db.SaveChanges();
            return row;
        }

        public ThoiKhoaBieu_DiemDanh LayThoiKhoaBieuMaTKB(int matkb)
        {
            ThoiKhoaBieu_DiemDanh TKB = db.ThoiKhoaBieu_DiemDanh.Where(m => m.MaThoiKhoaBieu == matkb).FirstOrDefault();
            return TKB;
        }
    }
}