using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiemDanhQR.Models;
using DiemDanhQR.DAO;
using System.Data.Entity;

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
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }

            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }
        //-------------Tu viet---------Start
        //Code lay session dang nhap -------
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
                    ViewBag.ThongBao = "Vui lòng điền tài khoản";
            }
            else
            {
                if (String.IsNullOrEmpty(matkhau))
                {
                    ViewBag.ThongBao = "Vui lòng điền mật khẩu";
                }
                else
                {
                    SinhVien sv = db.SinhViens.SingleOrDefault(n => n.MSSV == taikhoan && n.MatKhau == matkhau);
                    if (sv != null)
                    {
                        Session["taiKhoanSinhVien"] = sv;
                        return RedirectToAction("Index", "SinhVien");
                    }
                    else
                        ViewBag.ThongBao = "Thông tin đăng nhập không đúng";
                }
            }
            return View();
        }
        //Quet ma QR--------------------Start
        public ActionResult QuetMaQr()
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }
            return View();
        }
        [HttpPost]
        public ActionResult NhanMaQr(string MaQR)
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }
            List<QR> listQR = db.QRs.ToList();
            if (listQR.FirstOrDefault(m => m.MaQR == MaQR) == null)
            {
                ViewBag.thongtin = "Mã QR không tồn tại. Điểm danh thất bại!";
            }
            else
            {
                string[] thongTinMaQR = MaQR.Split('$');
                int maLopMon = Int32.Parse(thongTinMaQR[2]);
                string maGiangVien = thongTinMaQR[0];
                ThoiKhoaBieu_DiemDanh diemdanh = new ThoiKhoaBieu_DiemDanh();
                DateTime thoiGianXuLy = DateTime.Parse(thongTinMaQR[1]);
                int buoi = Int32.Parse(thongTinMaQR[3]);
                if (DateTime.Compare(DateTime.Now, thoiGianXuLy.AddMinutes(5)) <= 0)
                {
                    diemdanh = db.ThoiKhoaBieu_DiemDanh.First(a => a.MSSV == sv.MSSV && a.MaLopMon == maLopMon);
                    if (diemdanh.BuoiHoc != null)
                    {
                        diemdanh.BuoiHoc = diemdanh.BuoiHoc.ToString() + "," + buoi;
                    }
                    else
                    {
                        diemdanh.BuoiHoc =  buoi.ToString();

                    }
                    db.Entry(diemdanh).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.thongtin = "Sinh viên: " + sv.MSSV + " đã điểm danh thành công!";
                }
                else
                {
                    ViewBag.thongtin = "Mã QR hết hạn. Điểm danh thất bại!";
                }
            }
            return View();
        }
        //Quet ma QR--------------------End
        //-------------Tu viet---------End

        public ActionResult ThoiKhoaBieu()
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }

            SinhVien TaiKhoan = db.SinhViens.Where(m => m.MSSV == sv.MSSV).FirstOrDefault();
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayThoiKhoaBieu(sv.MSSV);
            return View(ds_TKB);
        }

        public ActionResult ThoiKhoaBieuTheoTuan(DateTime ?date)
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }

            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayThoiKhoaBieuTheoThuTrongTuan(sv.MSSV);
            ViewBag.date = date;
            return View(ds_TKB);
        }

        public ActionResult DanhSachLop()
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }

            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayThoiKhoaBieuBanCanSu(sv.MSSV);
            return View(ds_TKB);
        }

        public ActionResult DanhSachDiemDanh(int malopmon)
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }

            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayDsThoiKhoaBieuTheoMon(malopmon);
            ThoiKhoaBieu_DiemDanh TKB = thoikhoabieuDAO.LayThoiKhoaBieuTheoMon(malopmon);
            ViewBag.TKB = TKB;
            return View(ds_TKB);
        }


        [HttpPost]
        public ActionResult XacNhanDanhSachDiemDanh(FormCollection form)
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }

            string txtMaLopMon = form.Get("MaLopMon");
            string MoTa = form.Get("MoTa");
            int MaLopMon = int.Parse(txtMaLopMon) > 0 ? int.Parse(txtMaLopMon) : 0;
            thoikhoabieuDAO.CapNhatXacNhanDiemDanh(MaLopMon, MoTa, sv.MSSV, DateTime.Now);

            return RedirectToAction("XacNhanDiemDanhThanhCong");
        }

        public ActionResult XacNhanDiemDanhThanhCong()
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }

            return View();
        }

        public ActionResult HoSoCaNhan()
        {
            SinhVien sv = (SinhVien)Session["taiKhoanSinhVien"];
            if (sv == null)
            {
                return RedirectToAction("DangNhap", "SinhVien");
            }

            return View();
        }

        public ActionResult DangXuat()
        {
            Session["taiKhoanSinhVien"] = null;
            return RedirectToAction("DangNhap", "SinhVien");

        }

        public ActionResult DanhSachSinhVien(int malopmon)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayDsThoiKhoaBieuTheoMon(malopmon);
            ThoiKhoaBieu_DiemDanh TKB = thoikhoabieuDAO.LayThoiKhoaBieuTheoMon(malopmon);
            ViewBag.TKB = TKB;


            return View(ds_TKB);
        }
    }
}