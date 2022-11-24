using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Attribute;

namespace InternetBanking_v1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LocalizationAttribute("en"), 0);
        }
    }
}
