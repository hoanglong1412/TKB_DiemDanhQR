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
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayThoiKhoaBieuTheoThuTrongTuan("1811063022");
            ViewBag.date = date;
            return View(ds_TKB);
        }

        public ActionResult DanhSachLop()
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayThoiKhoaBieuBanCanSu("1811063022");
            return View(ds_TKB);
        }

        public ActionResult DanhSachDiemDanh(int malopmon)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayDsThoiKhoaBieuTheoMon(malopmon);
            ThoiKhoaBieu_DiemDanh TKB = thoikhoabieuDAO.LayThoiKhoaBieuTheoMon(malopmon);
            ViewBag.TKB = TKB;
            return View(ds_TKB);
        }


        [HttpPost]
        public ActionResult XacNhanDanhSachDiemDanh(FormCollection form)
        {
            string txtMaLopMon = form.Get("MaLopMon");
            int MaLopMon = int.Parse(txtMaLopMon) > 0 ? int.Parse(txtMaLopMon) : 0;
            thoikhoabieuDAO.CapNhatXacNhanDiemDanh(MaLopMon, "1811063022", DateTime.Now);

            return RedirectToAction("Index");
        }
    }
}