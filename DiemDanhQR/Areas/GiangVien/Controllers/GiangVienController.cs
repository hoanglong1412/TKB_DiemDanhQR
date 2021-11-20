using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiemDanhQR.Models;
using DiemDanhQR.DAO;
using System.IO;
using ZXing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.Entity;

namespace DiemDanhQR.Areas.GiangVien.Controllers
{
    public class GiangVienController : Controller
    {
        MyDBContext db = new MyDBContext();
        LichGiangDayDAO lichgiangdayDAO = new LichGiangDayDAO();
        ThoiKhoaBieuDAO thoikhoabieuDAO = new ThoiKhoaBieuDAO();

        // GET: GiangVien/GiangVien
        public ActionResult Index_gv()
        {
            GiaoVien giangVien = (GiaoVien)Session["taiKhoanGiangVien"];
            if (giangVien == null)
            {
                return RedirectToAction("DangNhap_gv", "GiangVien");
            }
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
            dsLopMonGiangDay = layDanhSachLopMon(giangVien.MaGiaoVien);
            ThoiKhoaBieu_DiemDanh dsTKB=new ThoiKhoaBieu_DiemDanh();
            List<ThoiKhoaBieu_DiemDanh> dsLGD=new List<ThoiKhoaBieu_DiemDanh>();
            foreach (LopMon lop in dsLopMonGiangDay)
            {
                dsTKB = lichgiangdayDAO.LayLichGiangDay(lop.MaLopMon);
                if (dsTKB != null)
                {
                    dsLGD.Add(dsTKB);
                }
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
            ThoiKhoaBieu_DiemDanh dsTKB = new ThoiKhoaBieu_DiemDanh();
            List<ThoiKhoaBieu_DiemDanh> dsLGD = new List<ThoiKhoaBieu_DiemDanh>();
            foreach (LopMon lop in dsLopMonGiangDay)
            {
                dsTKB = lichgiangdayDAO.LayLichGiangDay(lop.MaLopMon);
                if (dsTKB != null)
                {
                    dsLGD.Add(dsTKB);
                }
            }
            ViewBag.date = date;

            return View(dsLGD.OrderBy(m => m.LopMon.BuoiHoc.MaThu).ThenBy(m => m.LopMon.BuoiHoc.TietBatDau));

        }
        public ActionResult DanhSachDiemDanh_gv(int malopmon)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayDsThoiKhoaBieuTheoMon(malopmon);
            ThoiKhoaBieu_DiemDanh TKB = thoikhoabieuDAO.LayThoiKhoaBieuTheoMon(malopmon);
            ViewBag.TKB = TKB;

            return View(ds_TKB);
        }
        //---------- Hien Lich Giang Day Theo Tuan ---------- End
        //---------- QR CODER ----------Start
        public ActionResult TaoMaQR(int? id)
        {
            GiaoVien giangVien = (GiaoVien)Session["taiKhoanGiangVien"];
            if (giangVien == null)
            {
                return RedirectToAction("DangNhap_gv", "GiangVien");
            }
            LopMon lopmon = db.LopMons.Find(id);
            BuoiHoc buoihoc = db.BuoiHocs.Find(lopmon.MaBuoi);
            DateTime ngayKT = lopmon.NgayKetThuc.Value.AddDays(7);
            DateTime ngayBD = lopmon.NgayBatDau.Value;
            DateTime ngayDauTuan = ngayBD;
            DateTime NgayHienTai = DateTime.Now;
            int buoi = -1;

            //Neu thoi gian hien tai con trong thoi gian hoc
            if (DateTime.Now.Date <= ngayKT && DateTime.Now.Date >= ngayBD)
            {
                buoi = -2;
                //tinh theo tuan
                for(int i = 0; i <= (int)(ngayKT.Subtract(ngayBD).TotalDays / 7); i++)
                {
                    if (DateTime.Compare(DateTime.Now.Date, ngayDauTuan.AddDays(7 * i)) >= 0 &&
                        DateTime.Compare(DateTime.Now.Date, ngayDauTuan.AddDays(7 * i + 6)) < 0)
                    {
                        buoi = i+1;
                    }
                }
            }
            QR qrcode = new QR();
            qrcode.MaQR = giangVien.MaGiaoVien + "$" + NgayHienTai + "$" + lopmon.MaLopMon + "$" + buoi;
            db.QRs.Add(qrcode);
            db.SaveChanges();
            ViewBag.LopMon = lopmon;
            return View(qrcode);
        }
        [HttpPost]
        public ActionResult Generate(QR qrcode)
        {
            try
            {
                qrcode.DuongDanQR = GenerateQRCode(qrcode.MaQR);
                ViewBag.Message = "Tạo mã thành công";
            }
            catch (Exception ex)
            {
                //catch exception if there is any
            }
            
            return View("TaoMaQR", qrcode);
        }

        private string GenerateQRCode(string qrcodeText)
        {
            string folderPath = "~/Areas/GiangVien/Data/img/";
            string imagePath = "~/Areas/GiangVien/Data/img/QrCode.jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
        }
        //---------- QR CODER ----------End
        //---------- Danh Sách Sinh Viên ----- Start
        public ActionResult DanhSachSinhVien_gv(int malopmon)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayDsThoiKhoaBieuTheoMon(malopmon);
            ThoiKhoaBieu_DiemDanh TKB = thoikhoabieuDAO.LayThoiKhoaBieuTheoMon(malopmon);
            ViewBag.TKB = TKB;
          

            return View(ds_TKB);
        }
        [HttpPost]
        public ActionResult ChonBanCanSu(FormCollection form)
        {
            ThoiKhoaBieu_DiemDanh tkb = new ThoiKhoaBieu_DiemDanh();
            string malopmon = form["malopmon"];
            string dsnguoiduocchon = form["nguoiduocchon"];
            string[] nguoiduocchon = dsnguoiduocchon.Split('&');
            foreach (var item in nguoiduocchon)
            {
                if(item.Contains("_") == true)
                {
                    string[] chuoi = item.Split('_');
                    string mssv = chuoi[0];
                    string trangthai = chuoi[1];
                    if (trangthai == "1")
                    {
                        tkb = db.ThoiKhoaBieu_DiemDanh.FirstOrDefault(m => m.MSSV == mssv && m.MaLopMon.ToString().Equals(malopmon));
                        tkb.LaBanCanSu = true;
                        db.Entry(tkb).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        tkb = db.ThoiKhoaBieu_DiemDanh.FirstOrDefault(m => m.MSSV == mssv && m.MaLopMon.ToString().Equals(malopmon));
                        tkb.LaBanCanSu = false;
                        db.Entry(tkb).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("DanhSachSinhVien_gv", "GiangVien",new { malopmon=malopmon});
        }
        //---------- Danh Sách Sinh Viên ----- End

        public ActionResult DangXuat_gv()
        {
            Session["taiKhoanGiangVien"] = null;
            return RedirectToAction("DangNhap_gv", "GiangVien");
        }
    }
}