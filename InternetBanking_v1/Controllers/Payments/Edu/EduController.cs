using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetBanking_v1.Controllers.Payments.Edu
{
    public class EduController : BaseController//Controller
    {
        //
        // GET: /Edu/
        public ActionResult Edu()
        {
            return View();
        }

        // GET: /Edu_Sudanses/
        public ActionResult EduSudanese()
        {
            return PartialView();
        }

        // GET: /Edu_Arabic/
        public ActionResult EduArabic()
        {
            return PartialView();
        }
	}
}