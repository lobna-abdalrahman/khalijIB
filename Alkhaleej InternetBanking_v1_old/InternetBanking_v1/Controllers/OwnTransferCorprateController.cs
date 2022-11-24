using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class OwnTransferCorprateController : Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /OwnTransferCorprate/
        public ActionResult OwnTransfer()
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

            List<OwnTransferViewModel> pcontent = new List<OwnTransferViewModel>();
            {
                pcontent = data.DropFromOwnTransferClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt32(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {

                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,//item.FromAccount,
                    Value = item.FromAccount.ToString()
                });
            }

            ViewBag.clientList = AccountList;

            //-----------------------------------------------
            //////////////
            //To Account
            List<OwnTransferViewModel> pcontent2 = new List<OwnTransferViewModel>();
            {
                pcontent2 = data.DropFromOwnTransferClient(user_id);
            };
            List<SelectListItem> AccountList2 = new List<SelectListItem>();
            foreach (var item in pcontent2)
            {

                var AccountNumber = Convert.ToInt32(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList2.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.ToAccount.ToString()
                });
            }

            ViewBag.clientList2 = AccountList2;


            return View();
        }


        [HttpPost]
        public ActionResult OwnTransfer(OwnTransferViewModel model)
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {

                string username = Session["username"].ToString();
                string tel = Session["UserTel"].ToString();
                string user_id = Session["UserID"].ToString();

                List<OwnTransferViewModel> pcontent = new List<OwnTransferViewModel>();
                {
                    pcontent = data.DropFromOwnTransferClient(user_id);
                };
                List<SelectListItem> AccountList = new List<SelectListItem>();
                foreach (var item in pcontent)
                {
                    var AccountNumber = Convert.ToInt32(item.FromAccount.Substring(13));
                    var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                    var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                    AccountList.Add(new SelectListItem
                    {
                        Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                        Value = item.FromAccount.ToString()
                    });
                }

                ViewBag.clientList = AccountList;

                //------Get Selected Account----------
                model.FromAccount = Request["FromAccount"];

                //////////////
                //To Account
                List<OwnTransferViewModel> pcontent2 = new List<OwnTransferViewModel>();
                {
                    pcontent2 = data.DropFromOwnTransferClient(user_id);
                };
                List<SelectListItem> AccountList2 = new List<SelectListItem>();
                foreach (var item in pcontent2)
                {
                    var AccountNumber = Convert.ToInt32(item.FromAccount.Substring(13));
                    var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                    var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                    AccountList2.Add(new SelectListItem
                    {
                        Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                        Value = item.ToAccount.ToString()
                    });
                }

                ViewBag.clientList2 = AccountList2;

                //Get Selected Account...
                model.ToAccount = Request["ToAccount"];


                int resp = -1;
                try
                {
                    resp = data.insertTransferTemp(user_id, model.FromAccount, model.ToAccount, model.Amount, username);
                    if (resp == 1)
                    {
                        Session["TranResponse"] = true;
                        Session["ResponseStat"] = "Successful";
                        Session["ResponseMSG"]  = "Transfer is Under Process..";

                        TempData["Success"] = true;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Successful";
                        ViewBag.ResponseMSG = "Transfer is Under Process..";

                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    ModelState.AddModelError("",e.Message);
                }
                
            }
            else
            {
                message = "All Fields are required ";
                ModelState.AddModelError("", "Something is missing" + message);
                return View();
            }

            return View();
        }
	}
}