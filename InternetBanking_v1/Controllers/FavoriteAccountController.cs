using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class FavoriteAccountController : Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        LoginLogic obj = new LoginLogic();
        //
        // GET: /FavoriteAccount/
        public ActionResult AddAccount()
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();


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

          
            ViewBag.branchList = branchList;
            ViewBag.AccTypesList = AccTypeList;
            ViewBag.CurrencyList = CurrencyList;


            return View();
        }

        public ActionResult AddAccount(FavoriteAccountViewModel model)
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

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

           
            ViewBag.branchList = branchList;
            ViewBag.AccTypesList = AccTypeList;
            ViewBag.CurrencyList = CurrencyList;

            if (ModelState.IsValid)
            {
                string tel = Session["UserTel"].ToString();
                var user_ID = Session["UserID"].ToString();
                int userID = Convert.ToInt32(user_id);
                //----------------Doing the Service--------------------
                //Get Selected Branch, AccType & currency...
                model.BranchCode = Request["BranchList"];
                model.AccountTypeCode = Request["AccTypeList"];
                model.CurrencyCode = Request["CurrencyList"];
                model.FullAccountNumber = "99" + model.BranchCode + model.AccountTypeCode + model.CurrencyCode + model.ShortAccountNumber;

                try
                {
                    obj.AddAccount(model.AccountName, model.AccountDesc, model.FullAccountNumber, model.ShortAccountNumber, userID);
                    TempData["Success"] = true;

                    ViewBag.ResponseStat = GlobalRes.SuccesHeader;
                    ViewBag.ResponseMSG = GlobalRes.SuccessAccountMessage;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if (e.Message.Contains("ORA-00001: unique constraint"))
                    {
                        ModelState.AddModelError("", GlobalRes.AccountAlreadyExistsError);
                    }
                }
                

            }


            return View();
        }
	}
}