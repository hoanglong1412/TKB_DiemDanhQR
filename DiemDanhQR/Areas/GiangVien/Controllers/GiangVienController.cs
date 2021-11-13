using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiemDanhQR.Models;
using DiemDanhQR.DAO;
namespace DiemDanhQR.Areas.GiangVien.Controllers
{
    public class GiangVienController : Controller
    {
        MyDBContext db = new MyDBContext();
        LichGiangDayDAO lichgiangdayDAO = new LichGiangDayDAO();
        
        // GET: GiangVien/GiangVien
        public ActionResult Index_gv()
        {
            return View();
        }
        //---------- LOGIN ----------- Start
        public ActionResult DangNhap_gv()
        {
            return View();
        }
        //Neu dc lam them viewbag
        [HttpPost]
        public ActionResult DangNhap_gv(FormCollection collection)
        {

            var taikhoan = collection["txt__account"];
            var matkhau = collection["txt__password"];
            if (String.IsNullOrEmpty(taikhoan))
            {
                if (String.IsNullOrEmpty(matkhau))
                {
                    ViewBag.ThongBao = "Thông tin đăng nhập đang trống";
                }
                else
                    ViewBag.ThongBao = "Vui lòng điền Account";
            }
            else
            {
                if (String.IsNullOrEmpty(matkhau))
                {
                    ViewBag.ThongBao = "Vui lòng điền Password";
                }
                else
                {
                    GiaoVien gv = db.GiaoViens.SingleOrDefault(n => n.MaGiaoVien == taikhoan && n.MatKhau == matkhau);
                    if (gv != null)
                    {
                        Session["taiKhoanGiangVien"] = gv;
                        return RedirectToAction("Index_gv", "GiangVien");
                    }
                    else
                        ViewBag.ThongBao = "Thông tin đăng nhập không đúng";
                }
            }
            return View();
        }
        //---------- LOGIN ----------- End
        //---------- Hien Lich Giang Day --------- Start
        public List<LopMon> layDanhSachLopMon (string maGiaoVien)
        {
            List<LopMon> dsLopMon = db.LopMons.ToList();
            List<LopMon> dsLopMonGiangDay = new List<LopMon>();
            foreach (LopMon lopmon in dsLopMon)
            {
                if (maGiaoVien == lopmon.MaGiaoVien)
                {
                    dsLopMonGiangDay.Add(lopmon);
                }
            }
            return dsLopMonGiangDay;
        }
        public ActionResult LichGiangDay()
        {
            List<LopMon> dsLopMonGiangDay = new List<LopMon>();
            GiaoVien giangVien = (GiaoVien)Session["taiKhoanGiangVien"];
            if (giangVien == null)
            {
                return RedirectToAction("DangNhap_gv", "GiangVien");
            }
            dsLopMonGiangDay =layDanhSachLopMon(giangVien.MaGiaoVien);
            List<ThoiKhoaBieu_DiemDanh> dsTKB=new List<ThoiKhoaBieu_DiemDanh>();
            List<ThoiKhoaBieu_DiemDanh> dsLGD=new List<ThoiKhoaBieu_DiemDanh>();
            foreach (LopMon lop in dsLopMonGiangDay)
            {
                dsTKB = lichgiangdayDAO.LayLichGiangDay(lop.MaLopMon).ToList();
                dsLGD.AddRange(dsTKB);
            }
            return View(dsLGD);
        }
        //---------- Hien Lich Giang Day --------- End
        //---------- Hien Lich Giang Day Theo Tuan ---------- Start

        public ActionResult LichGiangDayTheoTuan(DateTime? date)
        {

            List<LopMon> dsLopMonGiangDay = new List<LopMon>();
            GiaoVien giangVien = (GiaoVien)Session["taiKhoanGiangVien"];
            if (giangVien == null)
            {
                return RedirectToAction("DangNhap_gv", "GiangVien");
            }
            dsLopMonGiangDay = layDanhSachLopMon(giangVien.MaGiaoVien);
            List<ThoiKhoaBieu_DiemDanh> dsTKB = new List<ThoiKhoaBieu_DiemDanh>();
            List<ThoiKhoaBieu_DiemDanh> dsLGD = new List<ThoiKhoaBieu_DiemDanh>();
            foreach (LopMon lop in dsLopMonGiangDay)
            {
                dsTKB = lichgiangdayDAO.LayLichGiangDay(lop.MaLopMon).ToList();
                dsLGD.AddRange(dsTKB);
            }
            ViewBag.date = date;
            return View(dsLGD);
        }
        //---------- Hien Lich Giang Day Theo Tuan ---------- End
        //---------- QR CODER ----------Start

        //---------- QR CODER ----------End
    }
}