using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiemDanhQR.Models;
using DiemDanhQR.DAO;

namespace DiemDanhQR.Areas.Khoa.Controllers
{
    public class TrangChuController : Controller
    {
        MyDBContext data = new MyDBContext();
        // GET: Khoa/TrangChu
        public ActionResult Index()
        {
            TaiKhoanKhoa khoa = (TaiKhoanKhoa)Session["taiKhoanKhoa"];
            if (khoa == null)
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }
            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
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
                    TaiKhoanKhoa khoa = data.TaiKhoanKhoas.SingleOrDefault(n => n.MaTaiKhoan == taikhoan && n.MatKhau == matkhau);
                    if (khoa != null)
                    {
                        Session["taiKhoanKhoa"] = khoa;
                        return RedirectToAction("Index", "TrangChu");
                    }
                    else
                        ViewBag.ThongBao = "Thông tin đăng nhập không đúng";
                }
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["taiKhoanKhoa"] = null;
            return RedirectToAction("DangNhap", "TrangChu");
        }
    }
}