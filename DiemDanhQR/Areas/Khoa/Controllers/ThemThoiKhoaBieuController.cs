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


namespace DiemDanhQR.Areas.Khoa.Controllers
{
    public class ThemThoiKhoaBieuController : Controller
    {
        MyDBContext data = new MyDBContext();
        DataSet dataSet;
        [System.Web.Mvc.HttpGet]
        public System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public System.Web.Mvc.ActionResult Index(HttpPostedFileBase fileupload)
        {
            //var fileName = Path.GetFileName(fileupload.FileName);
            var fileName = "test.xlsx";
            if (fileupload != null && fileupload.ContentLength > 0)
            {
                string ext = Path.GetExtension(fileupload.FileName);
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
                    ViewBag.Thongbao = "Không phải file excel";
                }
                var fileName2 = Server.MapPath(@"~/Assets/excel/test.xlsx");
                using (var stream = System.IO.File.Open(fileName2, FileMode.Open, FileAccess.Read))
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
                System.IO.File.Delete(fileName);
            }

            return View();
        }

        public System.Web.Mvc.ActionResult ThemTKB()
        {
            int ktr = 0;
            int importSuccess = 0;
            int importFail = 0;
            string importFailIndex = "";
            if (KiemTraCauTruc_TKB())
            {
                var fileName = Server.MapPath(@"~/Assets/excel/test.xlsx");
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
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
                        for (var i = 0; i < dataTable.Rows.Count; i++)
                        {
                            if (KiemtraColumEmpty_TKB(i))
                            {
                                if (ThemDuLieuVaoSQL_TKB(i))
                                {
                                    importSuccess++;
                                }
                                else
                                {
                                    importFail++;
                                    importFailIndex = importFailIndex + (i + 1).ToString() + " ";
                                }
                            }
                            else
                            {
                                importFail++;
                                importFailIndex = importFailIndex + (i + 1).ToString() + " ";
                            }
                        }
                        ktr = 1;
                        reader.Close();
                    }
                }
                System.IO.File.Delete(fileName);
            }
            ViewBag.ImportSuccess = importSuccess;
            ViewBag.ImportFail = importFail;
            ViewBag.ImportFailIndex = importFailIndex;
            if (ktr == 1)
                return View("Success");
            else
                return View("Success");
        }

        public bool KiemTraCauTruc_TKB()
        {
            bool a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15;
            a1 = a2 = a3 = a4 = a5 = a6 = a7 = a8 = a9 = a10 = a11 = a12 = a13 = a14 = a15 = false;
            var fileName = Server.MapPath(@"~/Assets/excel/test.xlsx");
            var path = Path.Combine(Server.MapPath(@"~/Assets/excel"), fileName);
            if (System.IO.File.Exists(path))
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
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
                                if (i == 1 && str.Equals("Nhóm"))
                                    a2 = true;
                                if (i == 2 && str.Equals("Tổ TH"))
                                    a3 = true;
                                if (i == 3 && str.Equals("Mã MH"))
                                    a4 = true;
                                if (i == 4 && str.Equals("Tên môn học"))
                                    a5 = true;
                                if (i == 5 && str.Equals("TC"))
                                    a6 = true;
                                if (i == 6 && str.Equals("Sĩ số"))
                                    a7 = true;
                                if (i == 7 && str.Equals("Mã GV"))
                                    a8 = true;
                                if (i == 8 && str.Equals("GV giảng dạy"))
                                    a9 = true;
                                if (i == 9 && str.Equals("Thứ"))
                                    a10 = true;
                                if (i == 10 && str.Equals("Tiết bắt đầu"))
                                    a11 = true;
                                if (i == 11 && str.Equals("Số tiết"))
                                    a12 = true;
                                if (i == 12 && str.Equals("Phòng"))
                                    a13 = true;
                                if (i == 13 && str.Equals("Tên Lớp"))
                                    a14 = true;
                                if (i == 14 && str.Equals("Ngày học"))
                                    a15 = true;
                            }
                        }
                    }
                }
            if (a1 == true && a2 == true && a3 == true && a4 == true && a5 == true && a6 == true && a7 == true && a8 == true && a9 == true && a10 == true && a11 == true && a12 == true && a13 == true && a14 == true && a15 == true)
                return true;
            else
                return false;
        }

        public Boolean KiemtraColumEmpty_TKB(int index)
        {
            var dataTable = dataSet.Tables[0];
            for (var i = 0; i < 15; i++)
            {
                //nếu cột nào có thể trống thì thêm ở đây //nhớ thêm điều kiện khi add vào sql
                if (i != 2)
                {
                    if (dataTable.Rows[index][i].ToString().Length == 0)
                        return false;
                }
            }
            return true;
        }

        public bool ThemDuLieuVaoSQL_TKB(int index)
        {
            var nhom_ToTH = Them_Nhom_ToThucHanh(index);
            var monHoc = Them_MonHoc(index);
            var giangVien = Them_GiangVien(index);
            var buoiHoc = Them_BuoiHoc(index);
            var phong = Them_Phong(index);
            var lopMon = Them_LopMon(index);
            if (nhom_ToTH != null && monHoc != null && giangVien != null && buoiHoc != null && phong != null && lopMon != null)
            {
                //Kiem tra trung va them du lieu bang Nhom_ToThucHanh
                var nhom_ToThucHanhs = data.Nhom_ToThucHanh.ToList();
                bool ktr1 = false;
                foreach (var item in nhom_ToThucHanhs)
                {
                    if (item.Nhom == nhom_ToTH.Nhom)
                        if (item.ToThucHanh == nhom_ToTH.ToThucHanh)
                        {
                            ktr1 = true;
                            break;
                        }
                }
                if (!ktr1)
                {
                    data.Nhom_ToThucHanh.Add(nhom_ToTH);
                    data.SaveChanges();
                }

                //Kiem tra trung va them du lieu bang MonHoC
                var monHocs = data.MonHocs.ToList();
                bool ktr2 = false;
                foreach (var item in monHocs)
                {
                    if (item.MaMon.Equals(monHoc.MaMon))
                        if (item.TenMon.Equals(monHoc.TenMon))
                        {
                            ktr2 = true;
                            break;
                        }
                }
                if (!ktr2)
                {
                    data.MonHocs.Add(monHoc);
                    data.SaveChanges();
                }

                //Kiem tra trung va them du lieu bang GiaoVien
                var giaoViens = data.GiaoViens.ToList();
                bool ktr3 = false;
                foreach (var item in giaoViens)
                {
                    //if (item.MaGiaoVien.Equals(dataGiaoVien.MaGiaoVien))
                    if (String.Compare(item.MaGiaoVien, giangVien.MaGiaoVien, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0)
                    {
                        ktr3 = true;
                        break;
                    }
                }
                if (!ktr3)
                {
                    data.GiaoViens.Add(giangVien);
                    data.SaveChanges();
                }

                //Kiem tra trung va them du lieu bang BuoiHoc
                var buoiHocs = data.BuoiHocs.ToList();
                bool ktr4 = false;
                foreach (var item in buoiHocs)
                {
                    if (item.MaThu == buoiHoc.MaThu)
                        if (item.TietBatDau == buoiHoc.TietBatDau)
                            if (item.SoTiet == buoiHoc.SoTiet)
                            {
                                ktr4 = true;
                                break;
                            }
                }
                if (!ktr4)
                {
                    data.BuoiHocs.Add(buoiHoc);
                    data.SaveChanges();
                }

                //Kiem tra trung va them du lieu bang Phong
                var phongs = data.Phongs.ToList();
                bool ktr5 = false;
                foreach (var item in phongs)
                {
                    if (String.Compare(item.MaPhong, phong.MaPhong, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0)
                    {
                        ktr5 = true;
                        break;
                    }
                }
                if (!ktr5)
                {
                    data.Phongs.Add(phong);
                    data.SaveChanges();
                }

                //Kiem tra trung va them du lieu bang LopMon
                var lopMonHocs = data.LopMons.ToList();
                bool ktr6 = false;
                foreach (var item in lopMonHocs)
                {
                    if (item.TenLopMon.Equals(lopMon.TenLopMon))
                        if (String.Compare(item.MaPhong, lopMon.MaPhong, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0)
                            if (item.MaBuoi == lopMon.MaBuoi)
                                if (item.MaMon.Equals(lopMon.MaMon))
                                    if (String.Compare(item.MaGiaoVien, lopMon.MaGiaoVien, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0)
                                        if (item.MaNhomToThucHanh == lopMon.MaNhomToThucHanh)
                                        {
                                            ktr6 = true;
                                            break;
                                        }
                }
                if (!ktr6)
                {
                    data.LopMons.Add(lopMon);
                    data.SaveChanges();
                }
                return true;
            }
            return false;
        }

        public Nhom_ToThucHanh Them_Nhom_ToThucHanh(int index)
        {
            var dataTable = dataSet.Tables[0];
            Nhom_ToThucHanh dataNhom_To = new Nhom_ToThucHanh();
            try
            {
                dataNhom_To.Nhom = dataTable.Rows[index][1].ToString();
                if (dataTable.Rows[index][2].ToString().Length != 0)
                    dataNhom_To.ToThucHanh = int.Parse(dataTable.Rows[index][2].ToString());
            }
            catch { dataNhom_To = null; }

            return dataNhom_To;
        }

        public MonHoc Them_MonHoc(int index)
        {
            var dataTable = dataSet.Tables[0];
            MonHoc dataMonHoc = new MonHoc();
            try
            {
                dataMonHoc.MaMon = dataTable.Rows[index][3].ToString();
                dataMonHoc.TenMon = dataTable.Rows[index][4].ToString();
                dataMonHoc.SoTinhChi = int.Parse(dataTable.Rows[index][5].ToString());
            }
            catch
            {
                dataMonHoc = null;
            }

            return dataMonHoc;
        }

        public GiaoVien Them_GiangVien(int index)
        {
            var dataTable = dataSet.Tables[0];
            GiaoVien dataGiaoVien = new GiaoVien();
            try
            {
                dataGiaoVien.MaGiaoVien = dataTable.Rows[index][7].ToString();
                dataGiaoVien.HoTen = dataTable.Rows[index][8].ToString();
                dataGiaoVien.MatKhau = "123456";
                //dataGiaoVien.MaKhoa = 1;
            }
            catch { dataGiaoVien = null; }

            return dataGiaoVien;
        }

        public BuoiHoc Them_BuoiHoc(int index)
        {
            var dataTable = dataSet.Tables[0];
            BuoiHoc dataBuoiHoc = new BuoiHoc();
            try
            {
                dataBuoiHoc.MaThu = int.Parse(dataTable.Rows[index][9].ToString());
                dataBuoiHoc.TietBatDau = int.Parse(dataTable.Rows[index][10].ToString());
                dataBuoiHoc.SoTiet = int.Parse(dataTable.Rows[index][11].ToString());
            }
            catch { dataBuoiHoc = null; }

            return dataBuoiHoc;
        }

        public Phong Them_Phong(int index)
        {
            var dataTable = dataSet.Tables[0];
            Phong dataPhong = new Phong();
            try
            {
                dataPhong.MaPhong = dataTable.Rows[index][12].ToString();
            }
            catch { dataPhong = null; }

            return dataPhong;
        }

        public LopMon Them_LopMon(int index)
        {
            var dataTable = dataSet.Tables[0];
            LopMon dataLopMon = new LopMon();
            try
            {
                dataLopMon.MaPhong = dataTable.Rows[index][12].ToString();
                var buoiHocs = data.BuoiHocs.ToList();
                foreach (var item in buoiHocs)
                {
                    if (item.MaThu == int.Parse(dataTable.Rows[index][9].ToString()))
                        if (item.TietBatDau == int.Parse(dataTable.Rows[index][10].ToString()))
                            if (item.SoTiet == int.Parse(dataTable.Rows[index][11].ToString()))
                            {
                                dataLopMon.MaBuoi = item.MaBuoi;
                                break;
                            }
                }
                dataLopMon.MaMon = dataTable.Rows[index][3].ToString();
                dataLopMon.MaGiaoVien = dataTable.Rows[index][7].ToString();
                var nhom_ToThucHanhs = data.Nhom_ToThucHanh.ToList();
                foreach (var item in nhom_ToThucHanhs)
                {
                    if (dataTable.Rows[index][2].ToString().Length == 0)
                    {
                        if (item.Nhom.Equals(dataTable.Rows[index][1].ToString()))
                        {
                            dataLopMon.MaNhomToThucHanh = item.MaNhomToThucHanh;
                            break;
                        }
                    }
                    else
                    {
                        if (item.Nhom.Equals(dataTable.Rows[index][1].ToString()))
                            if (item.ToThucHanh == int.Parse(dataTable.Rows[index][2].ToString()))
                            {
                                dataLopMon.MaNhomToThucHanh = item.MaNhomToThucHanh;
                                break;
                            }
                    }
                }
                string str = dataTable.Rows[index][14].ToString();
                string newstr = str.Replace(" - ", "-");
                string[] arrayStr = newstr.Split('-');
                dataLopMon.NgayBatDau = DateTime.ParseExact(arrayStr[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dataLopMon.NgayKetThuc = DateTime.ParseExact(arrayStr[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dataLopMon.TenLopMon = dataTable.Rows[index][13].ToString();
            }
            catch { dataLopMon = null; }

            return dataLopMon;
        }
    }
}