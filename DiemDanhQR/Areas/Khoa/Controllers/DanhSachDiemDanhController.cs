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
        private int maLM;
        // GET: Khoa/DanhSachDiemDanh
        public ActionResult Index()
        {
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

            try
            {

                DataTable Dt = new DataTable();
                Dt.Columns.Add("MSSV", typeof(string));
                Dt.Columns.Add("Họ tên", typeof(string));
                Dt.Columns.Add("Buổi 1", typeof(string));
                Dt.Columns.Add("Buổi 2", typeof(string));
                Dt.Columns.Add("Buổi 3", typeof(string));
                Dt.Columns.Add("Buổi 4", typeof(string));
                Dt.Columns.Add("Buổi 5", typeof(string));
                Dt.Columns.Add("Buổi 6", typeof(string));
                Dt.Columns.Add("Buổi 7", typeof(string));
                Dt.Columns.Add("Buổi 8", typeof(string));
                Dt.Columns.Add("Buổi 9", typeof(string));
                Dt.Columns.Add("Buổi 10", typeof(string));
                Dt.Columns.Add("Buổi 11", typeof(string));
                Dt.Columns.Add("Buổi 12", typeof(string));

                foreach (var item in listTKB_DD)
                {
                    if (item.MaLopMon ==maLopMon)
                    {
                        DataRow row = Dt.NewRow();
                        row[0] = item.MSSV;
                        row[1] = item.SinhVien.HoTen;
                        for (var i = 2; i < 8; i++)
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

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                    return File(data, "application/octet-stream", "FileManager.xlsx");
                    //return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                throw;
            }


        }

        public ActionResult Download()
        {

            if (Session["DownloadExcel_FileManager"] != null)
            {
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                return File(data, "application/octet-stream", "FileManager.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}
