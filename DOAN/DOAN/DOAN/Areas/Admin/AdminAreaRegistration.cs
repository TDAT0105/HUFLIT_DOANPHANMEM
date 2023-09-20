using System.Web.Mvc;

namespace DOAN.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "AdminLogin",
               "Admin/login",
               new { action = "Login", controller = "Auth", id = UrlParameter.Optional }
            );

            /*context.MapRoute(
               "AdminLogin",
               "Admin/login",
               new { action = "Logout", controller = "Auth", id = UrlParameter.Optional }
            );*/

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "index", controller = "HomeAdmin", id = UrlParameter.Optional }
            );

            

        }




    }
}