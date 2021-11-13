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
             url: "gv/Lich-Giang-Day-Theo-Tuan",
             defaults: new { controller = "GiangVien", action = "LichGiangDayTheoTuan", AreaName = "GiangVien" }
            );
            context.MapRoute(
             name: "GiangVien_lichGiangDay",
             url: "gv/Lich-Giang-Day",
             defaults: new { controller = "GiangVien", action = "LichGiangDay", AreaName = "GiangVien" }
            );
            context.MapRoute(
             name: "GiangVien_index",
             url: "gv/Trang-Chu",
             defaults: new { controller = "GiangVien", action = "Index_gv", AreaName = "GiangVien" }
            );
            context.MapRoute(
             name: "GiangVien_dangNhap",
             url: "gv/Dang-Nhap",
             defaults: new { controller = "GiangVien", action = "DangNhap_gv", AreaName = "GiangVien" }
            );
            context.MapRoute(
                name:"GiangVien_default",
                url:"gv/{controller}/{action}/{id}",
                defaults: new { controller="GiangVien",action = "Index_gv", AreaName="GiangVien", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "GiangVien_default2",
                url: "GiangVien/{controller}/{action}/{id}",
                defaults: new { controller = "GiangVien", action = "Index_gv", AreaName = "GiangVien", id = UrlParameter.Optional }
            );
        }
    }
}