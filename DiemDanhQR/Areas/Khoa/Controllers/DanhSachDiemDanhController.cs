using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiemDanhQR.Models;
using DiemDanhQR.DAO;
using OfficeOpenXml;
using System.Data;
using OfficeOpenXml.Table;
using System.IO;

namespace DiemDanhQR.Areas.Khoa.Controllers
{
    public class DanhSachDiemDanhController : Controller
    {

        MyDBContext data = new MyDBContext();
        ThoiKhoaBieuDAO thoikhoabieuDAO = new ThoiKhoaBieuDAO();
        // GET: Khoa/DanhSachDiemDanh
        public ActionResult Index()
        {
            TaiKhoanKhoa khoa = (TaiKhoanKhoa)Session["taiKhoanKhoa"];
            if (khoa == null)
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }
            var list = data.LopMons.ToList();
            var tkb = data.ThoiKhoaBieu_DiemDanh.ToList();
            List<int> listMaLopMon = new List<int>();
            List<LopMon> listLopMon = new List<LopMon>();
            foreach (var item in tkb)
            {
                bool ktr = false;
                foreach (var i in listMaLopMon)
                {
                    if (i == item.MaLopMon)
                    {
                        ktr = true;
                        break;
                    }
                }
                if (!ktr)
                {
                    listMaLopMon.Add((int)item.MaLopMon);
                }
            }
            foreach (var lopMon in list)
            {
                bool ktr = false;
                foreach (var i in listMaLopMon)
                {
                    if (i == lopMon.MaLopMon)
                    {
                        ktr = true;
                        break;
                    }
                }
                if (ktr)
                {
                    listLopMon.Add(lopMon);
                }
            }
            return View(listLopMon);
            //IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayThoiKhoaBieuBanCanSu("1811063022");
            //return View(ds_TKB);
        }

        public ActionResult DanhSach(int malopmon)
        {
            IEnumerable<ThoiKhoaBieu_DiemDanh> ds_TKB = thoikhoabieuDAO.LayDsThoiKhoaBieuTheoMon(malopmon);
            ThoiKhoaBieu_DiemDanh TKB = thoikhoabieuDAO.LayThoiKhoaBieuTheoMon(malopmon);
            ViewBag.TKB = TKB;
            ViewBag.maLopMon = malopmon;
            return View(ds_TKB);
        }

        public ActionResult XuatExcelDanhSachDiemDanh(int maLopMon)
        {
            List<ThoiKhoaBieu_DiemDanh> listTKB_DD = data.ThoiKhoaBieu_DiemDanh.ToList();
            LopMon lopMon = data.LopMons.Where(m => m.MaLopMon == maLopMon).FirstOrDefault();
            ThoiKhoaBieu_DiemDanh tkb = data.ThoiKhoaBieu_DiemDanh.Where(m => m.MaLopMon == maLopMon).FirstOrDefault();
                DateTime date1 = (DateTime)lopMon.NgayBatDau;
                DateTime date2 = (DateTime)lopMon.NgayKetThuc;
                TimeSpan d3 = date2 - date1;
                int KhoangThoiGian = d3.Days;
                int SoBuoiHoc = KhoangThoiGian / 7 + 1;
                int ThuCuaNgayDau = 0;
                DateTime NgayDauHoc = new DateTime();
                int Thu = (int)lopMon.BuoiHoc.MaThu;
                switch (date1.DayOfWeek.ToString())
                {
                    case "Monday":
                        ThuCuaNgayDau = 2;
                        break;

                    case "Tuesday":
                        ThuCuaNgayDau = 3;
                        break;

                    case "Wednesday":
                        ThuCuaNgayDau = 4;
                        break;

                    case "Thursday":
                        ThuCuaNgayDau = 5;
                        break;

                    case "Friday":
                        ThuCuaNgayDau = 6;
                        break;

                    case "Saturday":
                        ThuCuaNgayDau = 7;
                        break;

                    case "Sunday":
                        ThuCuaNgayDau = 8;
                        break;
                }

                if (ThuCuaNgayDau - Thu == 0)
                {
                    NgayDauHoc = date1;
                }
                else if (ThuCuaNgayDau - Thu == 1 || ThuCuaNgayDau - Thu == -6)
                {
                    NgayDauHoc = date1.AddDays(6);
                }
                else if (ThuCuaNgayDau - Thu == 2 || ThuCuaNgayDau - Thu == -5)
                {
                    NgayDauHoc = date1.AddDays(5);
                }
                else if (ThuCuaNgayDau - Thu == 3 || ThuCuaNgayDau - Thu == -4)
                {
                    NgayDauHoc = date1.AddDays(4);
                }
                else if (ThuCuaNgayDau - Thu == 4 || ThuCuaNgayDau - Thu == -3)
                {
                    NgayDauHoc = date1.AddDays(3);
                }
                else if (ThuCuaNgayDau - Thu == 5 || ThuCuaNgayDau - Thu == -2)
                {
                    NgayDauHoc = date1.AddDays(2);
                }
                else if (ThuCuaNgayDau - Thu == 6 || ThuCuaNgayDau - Thu == -1)
                {
                    NgayDauHoc = date1.AddDays(1);
                }
            try
            {

                DataTable Dt = new DataTable();
                Dt.Columns.Add("MSSV", typeof(string));
                Dt.Columns.Add("Họ tên", typeof(string));
                for(int i = 1; i <= SoBuoiHoc; i++)
                {
                    Dt.Columns.Add(NgayDauHoc.AddDays(7 * i - 7).ToString("dd/MM"), typeof(string));
                }    

                foreach (var item in listTKB_DD)
                {
                    if (item.MaLopMon == maLopMon)
                    {
                        DataRow row = Dt.NewRow();
                        row[0] = item.MSSV;
                        row[1] = item.SinhVien.HoTen;
                        for (var i = 2; i <= SoBuoiHoc+1; i++)
                        {

                            if (item.BuoiHoc != null)
                            {
                                string[] str = item.BuoiHoc.Split(',');
                                foreach (var s in str)
                                {
                                    if (s.Equals((i - 1).ToString()))
                                    {
                                        row[i] = "✔";
                                        break;
                                    }

                                }

                            }
                        }
                        Dt.Rows.Add(row);
                    }
                }
                string fName;
                if (tkb.NgayDuyet != null)
                {
                    fName = lopMon.MaMon + "_" + lopMon.Nhom_ToThucHanh.Nhom + "_" + lopMon.Nhom_ToThucHanh.ToThucHanh.ToString() + "_" + "DaDuyet.xlsx";
                }
                else
                {
                    fName = lopMon.MaMon + "_" + lopMon.Nhom_ToThucHanh.Nhom + "_" + lopMon.Nhom_ToThucHanh.ToThucHanh.ToString() + "_" + "ChuaDuyet.xlsx";
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    //worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    //worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //worksheet.DefaultColWidth = 20;

                    worksheet.Column(1).AutoFit();
                    worksheet.Column(2).AutoFit();

                    //Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    byte[] data = excelPackage.GetAsByteArray() as byte[];
                    return File(data, "application/octet-stream", fName);
                    //return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        //public ActionResult Download()
        //{

        //    if (Session["DownloadExcel_FileManager"] != null)
        //    {
        //        byte[] data = Session["DownloadExcel_FileManager"] as byte[];
        //        return File(data, "application/octet-stream", "FileManager.xlsx");
        //    }
        //    else
        //    {
        //        return new EmptyResult();
        //    }
        //}
    }
}
