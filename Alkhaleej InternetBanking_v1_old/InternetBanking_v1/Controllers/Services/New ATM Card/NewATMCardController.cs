using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetBanking_v1.Controllers.Services.New_ATM_Card
{
    public class NewATMCardController : Controller
    {
        //
        // GET: /NewATMCard/
        public ActionResult NewAtmCard()
        {
            return View();
        }

        // GET: /Debit Card/
        public ActionResult DebitCardRequest()
        {
            return PartialView();
        }

        // GET: /E-purse Card/
        public ActionResult EpurseCardRequest()
        {
            return PartialView();
        }
	}
}