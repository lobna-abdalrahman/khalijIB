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
    public class ToBankCustomerTransferCorprateController : Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        ToBanCustomerViewModel model = new ToBanCustomerViewModel();
        //
        // GET: /ToBankCustomerTransferCorprate/
        public ActionResult ToBankCustomerTransfer()
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            
            string user_id = Session["UserID"].ToString();

            //------------------Get Accounts----------------
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

            //--------get branchList----------
            List<ToBanCustomerViewModel> branchs = new List<ToBanCustomerViewModel>();
            {
                branchs = data.GetBranchs();
            };

            List<SelectListItem> branchList = new List<SelectListItem>();
            foreach (var item in branchs)
            {
                branchList.Add(new SelectListItem
                {
                    Text = item.BranchName,
                    Value = item.BranchCode.ToString()
                });
            }

            //-------------------Get Account Types-------------
            List<ToBanCustomerViewModel> accTypes = new List<ToBanCustomerViewModel>();
            {
                accTypes = data.GetAccountType();
            };

            List<SelectListItem> AccTypeList = new List<SelectListItem>();
            foreach (var item in accTypes)
            {
                AccTypeList.Add(new SelectListItem
                {
                    Text = item.AccountTypeName,
                    Value = item.AccountTypeCode.ToString()
                });
            }

            //----------------------Get Currency----------------
            List<ToBanCustomerViewModel> currency = new List<ToBanCustomerViewModel>();
            {
                currency = data.GetSDGCurrency();
            };

            List<SelectListItem> CurrencyList = new List<SelectListItem>();
            foreach (var item in currency)
            {
                CurrencyList.Add(new SelectListItem
                {
                    Text = item.CurrencyName,
                    Value = item.CurrencyCode.ToString()
                });
            }


            ViewBag.clientList = AccountList;
            ViewBag.branchList = branchList;
            ViewBag.AccTypesList = AccTypeList;
            ViewBag.CurrencyList = CurrencyList;

            return View();
        }


        [HttpPost]
        public ActionResult ToBankCustomerTransfer(ToBanCustomerViewModel model)
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string username = Session["username"].ToString();
            string user_id = Session["UserID"].ToString();

            //------------------Get Accounts----------------
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

            //--------get branchList----------
            List<ToBanCustomerViewModel> branchs = new List<ToBanCustomerViewModel>();
            {
                branchs = data.GetBranchs();
            };

            List<SelectListItem> branchList = new List<SelectListItem>();
            foreach (var item in branchs)
            {
                branchList.Add(new SelectListItem
                {
                    Text = item.BranchName,
                    Value = item.BranchCode.ToString()
                });
            }

            //-------------------Get Account Types-------------
            List<ToBanCustomerViewModel> accTypes = new List<ToBanCustomerViewModel>();
            {
                accTypes = data.GetAccountType();
            };

            List<SelectListItem> AccTypeList = new List<SelectListItem>();
            foreach (var item in accTypes)
            {
                AccTypeList.Add(new SelectListItem
                {
                    Text = item.AccountTypeName,
                    Value = item.AccountTypeCode.ToString()
                });
            }

            //----------------------Get Currency----------------
            List<ToBanCustomerViewModel> currency = new List<ToBanCustomerViewModel>();
            {
                currency = data.GetSDGCurrency();
            };

            List<SelectListItem> CurrencyList = new List<SelectListItem>();
            foreach (var item in currency)
            {
                CurrencyList.Add(new SelectListItem
                {
                    Text = item.CurrencyName,
                    Value = item.CurrencyCode.ToString()
                });
            }

            ViewBag.clientList = AccountList;
            ViewBag.branchList = branchList;
            ViewBag.AccTypesList = AccTypeList;
            ViewBag.CurrencyList = CurrencyList;

            if (ModelState.IsValid)
            {
                string tel = Session["UserTel"].ToString();
                //----------------Doing the Service--------------------
                //Get Selected Account...
                model.FromAccount = Request["FromAccount"];
                model.BranchCode = Request["BranchList"];
                model.AccountTypeCode = Request["AccTypeList"];
                model.CurrencyCode = Request["CurrencyList"];
                string toAccount = "";

                toAccount = "99" + model.BranchCode + model.AccountTypeCode + model.CurrencyCode + model.ToAccount;

                int resp = -1;
                try
                {
                    resp = data.insertTransferTemp(user_id, model.FromAccount, toAccount, model.Amount, username);
                    if (resp == 1)
                    {
                        Session["TranResponse"] = true;
                        Session["ResponseStat"] = "Successful";
                        Session["ResponseMSG"] = "Transfer is Under Process..";

                        TempData["Success"] = true;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Successful";
                        ViewBag.ResponseMSG = "Transfer is Under Process..";

                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    ModelState.AddModelError("", e.Message);
                }
            }
            else
            {
                message = "All Fields are required ";
                ModelState.AddModelError("", "Something is missing" + message);
                return View();
            }

            ModelState.Clear();

            return View();

        }




	}
}