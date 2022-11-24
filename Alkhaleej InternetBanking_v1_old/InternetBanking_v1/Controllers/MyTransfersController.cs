using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class MyTransfersController : Controller
    {
        DataSource ds = new DataSource();
        //
        // GET: /MyTransfers/
        public ActionResult AllTransfers()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

            List<AllTransfersViewModel> trans = new List<AllTransfersViewModel>();

            trans = ds.getMyTransfers(user_id);


            return View(trans);
        }


        //[HttpPost]
        //public ActionResult AllTransfers(AllTransfersViewModel model)
        //{

        //    string user_id = Session["UserID"].ToString();

        //    List<AllTransfersViewModel> trans = new List<AllTransfersViewModel>();

        //    trans = ds.getMyTransfers(user_id);

        //    return View(trans);
        //}
	}
}