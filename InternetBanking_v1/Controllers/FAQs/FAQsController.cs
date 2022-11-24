using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetBanking_v1.Controllers.FAQs
{
    public class FAQsController : Controller
    {
        //
        // GET: /FAQs/
        public ActionResult Faqs()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
	}
}