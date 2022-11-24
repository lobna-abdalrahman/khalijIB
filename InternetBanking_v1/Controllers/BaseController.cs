using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InternetBanking_v1.Controllers
{
    public class BaseController : Controller
    {
        private string CurrentLanguageCode { get; set; }
 
        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.RouteData.Values["lang"] != null && requestContext.RouteData.Values["lang"] as string != "null")
            {
                CurrentLanguageCode = (string)requestContext.RouteData.Values["lang"];
                if (CurrentLanguageCode != null)
                {
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLanguageCode);
                    }
                    catch (Exception)
                    {
                        throw new NotSupportedException(
                            string.Format("Invalid language code '{0}'.", CurrentLanguageCode));
                    }
                }
            }
            base.Initialize(requestContext);
        }
	}
}