using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InternetBanking_v1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

       /*     routes.MapRoute(
                name: "Language",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"ar|en" }
            );*/
            //routes.MapRoute(
            //    name: "Language",
            //    url: "{controller}/{action}/{lang}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    constraints: new { lang = @"ar|en" }
            //);

            routes.MapRoute(
                name: "Language",
                url: "{controller}/{action}/{lang}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional },
                constraints: new { lang = @"ar|en" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional, lang = "en" }
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
