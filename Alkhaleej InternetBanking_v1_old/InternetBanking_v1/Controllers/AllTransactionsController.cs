using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class AllTransactionsController : Controller
    {
        DataSource ds = new DataSource();
        //
        // GET: /AllTransactions/
        public ActionResult MyTransactions()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

            List<AllTransfersViewModel> trans = new List<AllTransfersViewModel>();

            trans = ds.getAllTransactions(user_id);

            return View(trans);
        }
	}
}