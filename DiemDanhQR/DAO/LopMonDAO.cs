using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiemDanhQR.Models;

namespace DiemDanhQR.DAO
{
    public class LopMonDAO
    {
        private MyDBContext db;
        public LopMonDAO()
        {
            db = new MyDBContext();
        }

        public LopMon LayLopMon(int malopmon)
        {
            LopMon lopmon = db.LopMons.Where(m => m.MaLopMon == malopmon).FirstOrDefault();
            return lopmon;
        }
 
    }
}