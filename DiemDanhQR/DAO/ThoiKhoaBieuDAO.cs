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
    }
}