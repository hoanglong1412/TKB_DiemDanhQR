using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiemDanhQR.Models;
using DiemDanhQR.DAO;

namespace DiemDanhQR.Controllers
{
    public class SinhVienController : Controller
    {
        MyDBContext db = new MyDBContext();
        ThoiKhoaBieuDAO thoikhoabieuDAO = new ThoiKhoaBieuDAO();
        LopMonDAO lopmonDAO = new LopMonDAO();
        // GET: SinhVien
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        public ActionResult ThoiKhoaBieu()
        {
            SinhVien TaiKhoan = db.SinhViens.Where(m => m.MSSV == "1811063022").FirstOrDefault();
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayThoiKhoaBieu("1811063022");
            return View(ds_TKB);
        }

        public ActionResult ThoiKhoaBieuTheoTuan(DateTime ?date)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayThoiKhoaBieu("1811063022");
            ViewBag.date = date;
            return View(ds_TKB);
        }
    }
}