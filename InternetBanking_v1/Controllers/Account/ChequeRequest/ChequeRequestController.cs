using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using InternetBanking_v1.Models.ViewModels;

namespace InternetBanking_v1.Controllers.Account.ChequeRequest
{
    public class ChequeRequestController : BaseController//Controller
    {

        DataSource data = new DataSource();

        //
        // GET: /ChequeRequest/
        public ActionResult ChequeRequest()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();
            //model.AccountNo = dataObj.populateAccounts(user_id);
            List<AccountStatementViewModel> pcontent = new List<AccountStatementViewModel>();
            {
                pcontent = data.getCurrentAccounts(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.AccountNo.Substring(13));
                var AccountType = data.getaccounttype(item.AccountNo.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.AccountNo.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.AccountNo.ToString()
                });
            }

            ViewBag.clientList = AccountList;

            return View();
        }


        [HttpPost]
        public ActionResult ChequeRequest(ChequeRequestViewModel model)
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            List<AccountStatementViewModel> pcontent = new List<AccountStatementViewModel>();
            {
                pcontent = data.getCurrentAccounts(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.AccountNo.Substring(13));
                var AccountType = data.getaccounttype(item.AccountNo.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.AccountNo.ToString().Substring(2, 3));
                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.AccountNo.ToString()
                });
            }

            ViewBag.clientList = AccountList;

            if (ModelState.IsValid)
            {

                //Get Selected Account...
                model.AccountNumber = Request["Clients"];

                try
                {
                    int res = data.InsertChequeReq(user_id, model.AccountNumber, model.ChequeBookSize);

                    if (res == 1)
                    {
                        TempData["Success"] = true;
                        ViewBag.ResponseMSG = "Successfully Completed";
                        ViewBag.ResponseStat = "Success";

                    }
                    else
                    {
                        TempData["Success"] = true;
                        ViewBag.ResponseMSG = "Not Completed";
                        ViewBag.ResponseStat = "Failed";
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    ModelState.AddModelError("",e.Message);
                }

                
            }


            return View();
        }
	}
}