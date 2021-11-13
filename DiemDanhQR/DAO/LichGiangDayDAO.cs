using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiemDanhQR.Models;

namespace DiemDanhQR.DAO
{
    public class LichGiangDayDAO
    {
        private MyDBContext db;
        public LichGiangDayDAO()
        {
            db = new MyDBContext();
        }

        public IEnumerable<ThoiKhoaBieu_DiemDanh> LayLichGiangDay(int? maLopMon)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_LGD = db.ThoiKhoaBieu_DiemDanh.Where(m => m.MaLopMon == maLopMon);
            return ds_LGD;
        }
    }
}