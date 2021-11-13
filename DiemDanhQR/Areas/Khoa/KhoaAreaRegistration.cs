using System.Web.Mvc;

namespace DiemDanhQR.Areas.Khoa
{
    public class KhoaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Khoa";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                   "Khoa_default",
                   "Khoa/{controller}/{action}/{id}",
                   new { action = "Index", id = UrlParameter.Optional }
               );

            context.MapRoute(
                name: "Khoa_TrangChu",
                url: "Khoa/TrangChu/{id}",
                defaults: new { controller = "TrangChu", action = "Index", AreaName = "Khoa", id = UrlParameter.Optional }
            );
        }
    }
}