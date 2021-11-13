using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiemDanhQR.Models;

namespace DiemDanhQR.DAO
{
    public class SinhVienDAO
    {
        private MyDBContext db;
        public SinhVienDAO()
        {
            db = new MyDBContext();
        }

        //public IEnumerable<SinhVien> LaySinhVienHocCungLopMon(int malopmon)
        //{
        //    IEnumerable<SinhVien> ds_SV = db.SinhViens.Where(m => m.ThoiKhoaBieu_DiemDanh. == malopmon);
        //    return ds_SV;
        //}
    }
}