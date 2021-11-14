using System.Web.Mvc;

namespace DiemDanhQR.Areas.GiangVien
{
    public class GiangVienAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GiangVien";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
             name: "GiangVien_lichGiangDayTheoTuan",
             url: "GiangVien/Lich-Giang-Day-Theo-Tuan",
             defaults: new { controller = "GiangVien", action = "LichGiangDayTheoTuan", AreaName = "GiangVien" }
            );
            context.MapRoute(
             name: "GiangVien_lichGiangDay",
             url: "GiangVien/Lich-Giang-Day",
             defaults: new { controller = "GiangVien", action = "LichGiangDay", AreaName = "GiangVien" }
            );
            context.MapRoute(
             name: "GiangVien_GenerateQR",
             url: "GiangVien/GiangVien/Generate",
             defaults: new { controller = "GiangVien", action = "Generate", AreaName = "GiangVien" }
            );
            context.MapRoute(
             name: "GiangVien_TaoMaQR",
             url: "GiangVien/GiangVien/TaoMaQR",
             defaults: new { controller = "GiangVien", action = "TaoMaQR", AreaName = "GiangVien" }
            );
            context.MapRoute(
             name: "GiangVien_index",
             url: "GiangVien/Trang-Chu",
             defaults: new { controller = "GiangVien", action = "Index_gv", AreaName = "GiangVien" }
            );
            context.MapRoute(
             name: "GiangVien_dangNhap",
             url: "GiangVien/Dang-Nhap",
             defaults: new { controller = "GiangVien", action = "DangNhap_gv", AreaName = "GiangVien" }
            );
            context.MapRoute(
                name:"GiangVien_default",
                url:"GiangVien/{controller}/{action}/{id}",
                defaults: new { controller="GiangVien",action = "Index_gv", AreaName="GiangVien", id = UrlParameter.Optional }
            );
        }
    }
}