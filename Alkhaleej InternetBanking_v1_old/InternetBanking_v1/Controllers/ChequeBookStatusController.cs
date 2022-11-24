using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class ChequeBookStatusController : Controller
    {
        DataSource data = new DataSource();
        //
        // GET: /ChequeBookStatus/
        public ActionResult ChequeBookStatus()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();


            return View();
        }



        [HttpPost]
        public ActionResult ChequeBookStatus(ChequeBookStatusViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            if (ModelState.IsValid)
            {

                List<ChequeBookStatusViewModel> accas = new List<ChequeBookStatusViewModel>();
                accas = data.getChequeBookRequestsStatus(user_id, model.FromDate, model.ToDate);

                Session["ChequeBookStatus"] = accas;

                if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
                {
                    return RedirectToAction("ViewChequeBookStatus",new { lang = "ar" });
                }
                else
                {
                    return RedirectToAction("ViewChequeBookStatus", new { lang = "en" });
                }
            }
            else
            {
                ModelState.AddModelError("","Fill the Data Please");
            }


            return View();
        }


        public ActionResult ViewChequeBookStatus()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            ChequeBookStatusViewModel model = new ChequeBookStatusViewModel();

            List<ChequeBookStatusViewModel> accas = new List<ChequeBookStatusViewModel>();
            accas = (List<ChequeBookStatusViewModel>) Session["ChequeBookStatus"];

            foreach (var item in accas)
            {
                /*var AccountNumber = Convert.ToInt32(item.AccountNumber.Substring(13));
                var AccountType = data.getaccounttype(item.AccountNumber.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.AccountNumber.ToString().Substring(2, 3));*/

               /* model.AccountNumber = item.AccountNumber;//BranchName + " - " + AccountType + " - " + AccountNumber;
                model.RequestedSize = item.RequestedSize;
                model.Date = item.Date;
                model.RequestStatus = item.RequestStatus;*/


            }

            return View(accas);
        }



	}
}