using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiemDanhQR.Models;
using ExcelDataReader;
using System.Data;
using System.Globalization;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace DiemDanhQR.Areas.Khoa.Controllers
{
    public class ThemDanhSachSinhVienController : Controller
    {
        MyDBContext data = new MyDBContext();
        DataSet dataSet;
        string chuaCoTKB = "";
        private string fileName;
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            TaiKhoanKhoa khoa = (TaiKhoanKhoa)Session["taiKhoanKhoa"];
            if (khoa == null)
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public System.Web.Mvc.ActionResult Index(HttpPostedFileBase fileupload)
        {
            //var fileName = Path.GetFileName(fileupload.FileName);
            //var fileName = "test.xlsx";
            if (fileupload != null && fileupload.ContentLength > 0)
            {
                bool ktrFail = false;
                var time = DateTime.Now.ToString("dd_MM_yyyy");
                string ext = Path.GetExtension(fileupload.FileName);
                fileName = time + "_" + fileupload.FileName;
                if (ext.Equals(".xlsx"))
                {
                    var path = Path.Combine(Server.MapPath(@"~/Assets/excel"), fileName);
                    if (System.IO.File.Exists(path))
                        //ViewBag.Thongbao = "Đã tồn tại";
                        fileupload.SaveAs(path);
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                }
                else
                {
                    ktrFail = true;
                    ViewBag.Thongbao = "Không phải file excel";
                }
                if (!ktrFail)
                {
                    if (KiemTraCauTruc_DSSV(fileName))
                    {
                        string strUrl = "~/Assets/excel/" + fileName;
                        var fileNameInProject = Server.MapPath(strUrl);
                        using (var stream = System.IO.File.Open(fileNameInProject, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {

                                dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                                {
                                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true,
                                        ReadHeaderRow = rowReader =>
                                        {
                                            for (var i = 1; i < rowReader.RowCount; i++)
                                            {
                                                if (!rowReader.IsDBNull(0))
                                                {
                                                    string str = rowReader.GetValue(0).ToString();
                                                    if (str.Equals("STT"))
                                                        break;
                                                }
                                                rowReader.Read();
                                            }
                                        },
                                    }
                                });
                                reader.Close();
                            }
                        }
                        var dataTable = dataSet.Tables[0];
                        for (var i = 0; i < dataTable.Rows.Count; i++)
                        {
                            if (dataTable.Rows[i][0].ToString().Length == 0)
                                dataTable.Rows[i].Delete();
                        }
                        dataTable.AcceptChanges();
                        ViewData.Model = dataTable;
                        ViewBag.FileName = fileName;
                        System.IO.File.Delete(fileName);
                    }
                    else
                    {
                        ViewBag.Thongbao = "Không đúng cấu trúc file excel thêm danh sách sinh viên";
                    }

                }
            }
            return View();
        }

        public System.Web.Mvc.ActionResult ThemDS(string name)
        {
            int ktr = 0;
            int importSuccess = 0;
            int importFail = 0;
            //string importFailIndex = "";
            List<int> indexError = new List<int>();
            DataTable table = new DataTable();
            fileName = name;
            if(fileName != null)
            {
                if (KiemTraCauTruc_DSSV(fileName))
                {
                    string strUrl = "~/Assets/excel/" + fileName;
                    var fileNameInProject = Server.MapPath(strUrl);
                    using (var stream = System.IO.File.Open(fileNameInProject, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {

                            dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                            {
                                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration
                                {
                                    UseHeaderRow = true,
                                    ReadHeaderRow = rowReader =>
                                    {
                                        for (var i = 1; i < rowReader.RowCount; i++)
                                        {
                                            if (!rowReader.IsDBNull(0))
                                            {
                                                string str = rowReader.GetValue(0).ToString();
                                                if (str.Equals("STT"))
                                                    break;
                                            }
                                            rowReader.Read();
                                        }
                                    },
                                }
                            });
                            var dataTable = dataSet.Tables[0];
                            for (var i = 0; i < dataTable.Rows.Count; i++)
                            {
                                if (dataTable.Rows[i][0].ToString().Length == 0)
                                    dataTable.Rows[i].Delete();
                            }
                            dataTable.AcceptChanges();
                            table = dataTable;
                            for (var i = 0; i < dataTable.Rows.Count; i++)
                            {
                                if (KiemtraColumEmpty_DSSV(i))
                                {
                                    if (ThemDuLieuVaoSQL_DSSV(i))
                                    {
                                        importSuccess++;
                                    }
                                    else
                                    {
                                        importFail++;
                                        indexError.Add(i + 1);
                                        //importFailIndex = importFailIndex + (i + 1).ToString() + " ";
                                    }
                                }
                                else
                                {
                                    importFail++;
                                    indexError.Add(i + 1);
                                    //importFailIndex = importFailIndex + (i + 1).ToString() + " ";
                                }
                            }
                            ktr = 1;
                            reader.Close();
                        }
                    }
                    System.IO.File.Delete(fileName);
                }
            }
            else
            {
                ViewBag.Thongbao = "Chưa chọn và đọc file excel";
                return View("Index");
            }
            
            ViewBag.ImportSuccess = importSuccess;
            ViewBag.ImportFail = importFail;
            ViewBag.ImportFailIndex = indexError;
            ViewData.Model = table;
            if (ktr == 1)
                return View("Success");
            else
            {
                return View("Success");
            }    
                
        }

        public bool KiemTraCauTruc_DSSV(string name)
        {
            bool a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17;
            a1 = a2 = a3 = a4 = a5 = a6 = a7 = a8 = a9 = a10 = a11 = a12 = a13 = a14 = a15 = a16 = a17 = false;
            fileName = name;
            string strUrl = "~/Assets/excel/" + fileName;
            var fileNameInProject = Server.MapPath(strUrl);
            var path = Path.Combine(Server.MapPath(@"~/Assets/excel"), fileName);
            if (System.IO.File.Exists(path))
                using (var stream = System.IO.File.Open(fileNameInProject, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read();
                        for (var i = 1; i < reader.RowCount; i++)
                        {
                            if (!reader.IsDBNull(0))
                            {
                                string str = reader.GetValue(0).ToString();
                                if (str.Equals("STT"))
                                    break;
                            }
                            reader.Read();
                        }
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            if (!reader.IsDBNull(i))
                            {
                                string str = reader.GetValue(i).ToString();
                                if (i == 0 && str.Equals("STT"))
                                    a1 = true;
                                if (i == 1 && str.Equals("Mã MH"))
                                    a2 = true;
                                if (i == 2 && str.Equals("Nhóm"))
                                    a3 = true;
                                if (i == 3 && str.Equals("Tên môn học"))
                                    a4 = true;
                                if (i == 4 && str.Equals("Số TC"))
                                    a5 = true;
                                if (i == 5 && str.Equals("Tổ"))
                                    a6 = true;
                                if (i == 6 && str.Equals("Lớp MH"))
                                    a7 = true;
                                if (i == 7 && str.Equals("Giảng viên"))
                                    a8 = true;
                                if (i == 8 && str.Equals("Mã SV"))
                                    a9 = true;
                                if (i == 9 && str.Equals("Họ lót"))
                                    a10 = true;
                                if (i == 10 && str.Equals("Tên"))
                                    a11 = true;
                                if (i == 11 && str.Equals("Ngày sinh"))
                                    a12 = true;
                                if (i == 12 && str.Equals("Mã lớp"))
                                    a13 = true;
                                if (i == 13 && str.Equals("Email SV"))
                                    a14 = true;
                                if (i == 14 && str.Equals("SDT SV"))
                                    a15 = true;
                                if (i == 15 && str.Equals("Đợt"))
                                    a16 = true;
                                if (i == 16 && str.Equals("BM"))
                                    a17 = true;
                            }
                        }
                        reader.Close();
                    }
                }
            if (a1 == true && a2 == true && a3 == true && a4 == true && a5 == true && a6 == true && a7 == true && a8 == true && a9 == true && a10 == true && a11 == true && a12 == true && a13 == true && a14 == true && a15 == true && a16 == true && a17 == true)
                return true;
            else
                return false;
        }

        public bool KiemtraColumEmpty_DSSV(int index)
        {
            var dataTable = dataSet.Tables[0];
            for (var i = 0; i < 17; i++)
            {
                if (i != 5)
                    if (dataTable.Rows[index][i].ToString().Length == 0)
                        return false;
            }
            return true;
        }

        public bool ThemDuLieuVaoSQL_DSSV(int index)
        {
            //Kiem tra trung va them du lieu bang Lop
            var lop = Them_Lop(index);
            var sinhVien = Them_SinhVien(index);
            var thoiKhoaBieu_DiemDanh = Them_ThoiKhoaBieu_DiemDanh(index);
            if (lop != null && sinhVien != null && thoiKhoaBieu_DiemDanh != null)
            {
                if (thoiKhoaBieu_DiemDanh.MaLopMon != -1)
                {
                    var lops = data.Lops.ToList();
                    bool ktr1 = false;
                    foreach (var item in lops)
                    {
                        if (item.MaLop.Equals(lop.MaLop))
                        {
                            ktr1 = true;
                            break;
                        }
                    }
                    if (!ktr1)
                    {
                        data.Lops.Add(lop);
                        data.SaveChanges();
                    }

                    //Kiem tra trung va them du lieu bang SinhVien
                    var sinhViens = data.SinhViens.ToList();
                    bool ktr2 = false;
                    foreach (var item in sinhViens)
                    {
                        if (item.MSSV.Equals(sinhVien.MSSV))
                        {
                            ktr2 = true;
                            break;
                        }
                    }
                    if (!ktr2)
                    {
                        data.SinhViens.Add(sinhVien);
                        data.SaveChanges();
                    }
                    else
                    {
                        //data.SinhViens.Where(x => x.MSSV.Equals(dataSinhVien.MSSV)).FirstOrDefault();
                        //data.Entry(dataSinhVien).State = System.Data.Entity.EntityState.Modified;
                        var dataSV = data.SinhViens.Find(sinhVien.MSSV);
                        dataSV.HoTen = sinhVien.HoTen;
                        dataSV.NgaySinh = sinhVien.NgaySinh;
                        dataSV.Email = sinhVien.Email;
                        dataSV.SDT = sinhVien.SDT;
                        dataSV.MaLop = sinhVien.MaLop;
                        //dataSV.MatKhau = sinhVien.MatKhau;
                        //dataSV.LaBanCanSu = sinhVien.LaBanCanSu;
                        data.SaveChanges();
                    }

                    //Kiem tra trung va them du lieu bang ThoiKhoaBieu_DiemDanh
                    bool ktr3 = false;
                    var tkb_dd = data.ThoiKhoaBieu_DiemDanh.ToList();
                    foreach (ThoiKhoaBieu_DiemDanh item in tkb_dd)
                    {
                        if (item.MSSV.Equals(thoiKhoaBieu_DiemDanh.MSSV))
                            if (item.MaLopMon == thoiKhoaBieu_DiemDanh.MaLopMon)
                            {
                                ktr3 = true;
                            }
                    }
                    if (!ktr3)
                    {
                        data.ThoiKhoaBieu_DiemDanh.Add(thoiKhoaBieu_DiemDanh);
                        data.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    chuaCoTKB = index.ToString() + " ";
                    return false;
                }
            }
            return false;
        }

        public Lop Them_Lop(int index)
        {
            var dataTable = dataSet.Tables[0];
            Lop dataLop = new Lop();
            try
            {
                dataLop.MaLop = dataTable.Rows[index][12].ToString();
                //dataLop.MaKhoa = 1;
            }
            catch { dataLop = null; }

            return dataLop;
        }

        public SinhVien Them_SinhVien(int index)
        {
            var dataTable = dataSet.Tables[0];
            SinhVien dataSinhVien = new SinhVien();
            try
            {
                dataSinhVien.MSSV = dataTable.Rows[index][8].ToString();
                dataSinhVien.HoTen = dataTable.Rows[index][9].ToString() + " " + dataTable.Rows[index][10].ToString();
                dataSinhVien.NgaySinh = DateTime.ParseExact(dataTable.Rows[index][11].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dataSinhVien.Email = dataTable.Rows[index][13].ToString();
                dataSinhVien.SDT = dataTable.Rows[index][14].ToString();
                dataSinhVien.MaLop = dataTable.Rows[index][12].ToString();
                dataSinhVien.MatKhau = dataTable.Rows[index][8].ToString();
                dataSinhVien.LaBanCanSu = false;
            }
            catch { dataSinhVien = null; }

            return dataSinhVien;
        }

        public ThoiKhoaBieu_DiemDanh Them_ThoiKhoaBieu_DiemDanh(int index)
        {
            var dataTable = dataSet.Tables[0];
            ThoiKhoaBieu_DiemDanh dataTKB = new ThoiKhoaBieu_DiemDanh();
            try
            {
                string eMaMon = dataTable.Rows[index][1].ToString();
                string eLopMon = dataTable.Rows[index][6].ToString();
                string eNhom = dataTable.Rows[index][2].ToString();
                int eTo = -1;
                if (dataTable.Rows[index][5].ToString().Length != 0)
                    eTo = int.Parse(dataTable.Rows[index][5].ToString());
                int maLopMon = -1;
                dataTKB.MSSV = dataTable.Rows[index][8].ToString();
                var tkbDiemDanhs = data.LopMons.ToList();

                bool br = false;
                foreach (var item in tkbDiemDanhs)
                {

                    if (item.TenLopMon.Equals(eLopMon))
                        if (item.MaMon.Equals(eMaMon))
                        {
                            var nhom_ToTH = data.Nhom_ToThucHanh.ToList();
                            foreach (var nhom in nhom_ToTH)
                            {
                                if (nhom.MaNhomToThucHanh == item.MaNhomToThucHanh)
                                {
                                    if (nhom.Nhom.Equals(eNhom))
                                    {
                                        if (nhom.ToThucHanh.ToString().Length == 0)
                                        {
                                            if (eTo == -1)
                                            {
                                                maLopMon = item.MaLopMon;
                                                br = true;
                                                break;
                                            }
                                        }
                                        else if (nhom.ToThucHanh == eTo)
                                        {
                                            maLopMon = item.MaLopMon;
                                            br = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (br)
                            {
                                break;
                            }
                        }
                }
                dataTKB.MaLopMon = maLopMon;
            }
            catch { dataTKB = null; }

            return dataTKB;
        }


        public ActionResult XuatExcelMauThemDSSV()
        {
            try
            {

                DataTable Dt = new DataTable();
                Dt.Columns.Add("STT", typeof(string));
                Dt.Columns.Add("Mã MH", typeof(string));
                Dt.Columns.Add("Nhóm", typeof(string));
                Dt.Columns.Add("Tên môn học", typeof(string));
                Dt.Columns.Add("Số TC", typeof(string));
                Dt.Columns.Add("Tổ", typeof(string));
                Dt.Columns.Add("Lớp MH", typeof(string));
                Dt.Columns.Add("Giảng viên", typeof(string));
                Dt.Columns.Add("Mã SV", typeof(string));
                Dt.Columns.Add("Họ lót", typeof(string));
                Dt.Columns.Add("Tên", typeof(string));
                Dt.Columns.Add("Ngày sinh", typeof(string));
                Dt.Columns.Add("Mã lớp", typeof(string));
                Dt.Columns.Add("Email SV", typeof(string));
                Dt.Columns.Add("SDT SV", typeof(string));
                Dt.Columns.Add("Đợt", typeof(string));
                Dt.Columns.Add("BM", typeof(string));

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:Q1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    //worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    //worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 10;

                    //worksheet.Column(1).AutoFit();
                    //worksheet.Column(2).AutoFit();

                    //Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    byte[] data = excelPackage.GetAsByteArray() as byte[];
                    return File(data, "application/octet-stream", "Mau_ThemDSSV.xlsx");
                    //return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}