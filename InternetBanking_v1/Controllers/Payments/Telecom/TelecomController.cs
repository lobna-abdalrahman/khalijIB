using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using LinqToDB.Common;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers.Payments.Telecom
{
    public class TelecomController : BaseController//Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /Telecom/
        public ActionResult Telecom()
        {
            TelecomPostPaidViewModel modell = new TelecomPostPaidViewModel();

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            List<CardTransferViewModel> pcontent = new List<CardTransferViewModel>();
            {
                pcontent = data.DropCardClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                AccountList.Add(new SelectListItem
                {
                    Text = item.CardName,
                    Value = item.CardNumber.ToString()
                });
            }

            ViewBag.clientList = AccountList;

            if (Session["FavPhone"] != null)
            {
                string id = Session["FavPhoneID"].ToString();
                List<TelecomPostPaidViewModel> accas = new List<TelecomPostPaidViewModel>();
                accas = (List<TelecomPostPaidViewModel>)Session["phonesList"];

                foreach (var item in accas)
                {
                    if (item.FavoritePhoneID.Equals(id.ToString()))
                        modell.ToPhoneNo = item.ToPhoneNo;
                }

                Session["FavPhone"] = null;
                Session["metersList"] = "";
            }
            return View();
        }


        [HttpPost]
        public ActionResult Telecom(TelecomPrePaidViewModel model)
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            List<CardTransferViewModel> pcontent = new List<CardTransferViewModel>();
            {
                pcontent = data.DropCardClient(user_id);
            }
            ;
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                AccountList.Add(new SelectListItem
                {
                    Text = item.CardName,
                    Value = item.CardNumber.ToString()
                });
            }

            ViewBag.clientList = AccountList;
            //Get Selected Account...
            model.PAN = Request["Clients"];
            foreach (var item in pcontent)
            {
                if (item.CardNumber.Equals(model.PAN.ToString()))
                {
                    
                    model.CardExp = item.CardExp;
                    model.Month = model.CardExp.Substring(2, 2);
                    model.Year = model.CardExp.Substring(1, 2);
                  
                    break;
                }
                else
                {
                    model.CardExp = model.Year + model.Month;
                }
            }
            //model.CardExp = model.Year + model.Month;
            if (model.CardExp == null)
            {
                model.CardExp = model.Year + model.Month;
            }

            if (ModelState.IsValid)
            {


                string tel = Session["UserTel"].ToString();
                string desc = "top up";

                ViewBag.flag = 0;
                try
                {

                    string res = myCheque.EBSDoBill(model.PAN, model.CardExp, model.Amount, tel,
                        model.IPIN, model.ToPhoneNo, model.BillerName, user_id, desc);

                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    string responseStatus = result.responseStatus;
                    string responseMessage = result.responseMessage;

                    ViewBag.flag = 1;

                    if (responseStatus != null && !responseStatus.ToString().Equals("Failed"))
                    {
                        TempData["Success"] = true;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Secussfully";
                        ViewBag.ResponseMSG = responseMessage;

                    }
                    else
                    {
                        TempData["Success"] = true;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Not Successful";
                        ViewBag.ResponseMSG = responseMessage;

                    }


                    //insert into TranLog
                    string req = model.PAN + "," + model.CardExp + "," + model.Amount + "," + tel + "," + model.IPIN + "," + model.ToPhoneNo + "," + model.BillerName + "," + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Top Up", req, res, responseMessage.ToString(), responseStatus.ToString(), model.Amount,"N/A");
                    ModelState.Clear();
                    if (!responseMessage.IsNullOrEmpty())
                    {
                        TempData["Success"] = true;
                    }

                }
                catch (Exception e)
                {
                    //insert into TranLog
                    string req = model.PAN + "," + model.CardExp + "," + model.Amount + "," + tel + "," + model.IPIN + "," + model.ToPhoneNo + "," + model.BillerName + "," + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Top Up", req, "", "Connectivity Error", "Timed out","N/A","N/A");

                    ModelState.Clear();
                    ModelState.AddModelError("", GlobalRes.ConnectivityErrorMessage);

                    return View();
                }


            }

            //ModelState.Clear();
            return View();
        }

/*        // GET: /prePaid Telecom/
        public ActionResult PrePaid()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            List<CardTransferViewModel> pcontent = new List<CardTransferViewModel>();
            {
                pcontent = data.DropCardClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                AccountList.Add(new SelectListItem
                {
                    Text = item.CardName,
                    Value = item.CardNumber.ToString()
                });
            }

            ViewBag.clientList = AccountList;

            return PartialView();
        }


        [HttpPost]
        public ActionResult PrePaid(TelecomPrePaidViewModel model)
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            List<CardTransferViewModel> pcontent = new List<CardTransferViewModel>();
            {
                pcontent = data.DropCardClient(user_id);
            }
            ;
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                AccountList.Add(new SelectListItem
                {
                    Text = item.CardName,
                    Value = item.CardNumber.ToString()
                });
            }

            ViewBag.clientList = AccountList;
            //Get Selected Account...
            model.PAN = Request["Clients"]; 
            model.CardExp = model.Year + model.Month;

            if (ModelState.IsValid)
            {
               
                string tel = Session["UserTel"].ToString();
                string desc = "top up";

                ViewBag.flag = 0;
                try
                {

                    string res = myCheque.EBSDoBill(model.PAN, model.CardExp, model.Amount, tel,
                        model.IPIN, model.ToPhoneNo,model.BillerName,user_id,desc);

                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    string responseStatus = result.responseStatus;
                    string responseMessage = result.responseMessage;
                  
                    ViewBag.flag = 1;

                    if (responseStatus != null && !responseStatus.ToString().Equals("Failed"))
                    {
                        TempData["Success"] = true;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Secussfully";
                        ViewBag.ResponseMSG = responseMessage;

                    }
                    else
                    {
                        TempData["Success"] = true;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Not Successful";
                        ViewBag.ResponseMSG = responseMessage;

                    }


                    //insert into TranLog
                    string req = model.PAN + "," + model.CardExp + "," + model.Amount + "," + tel + "," + model.IPIN + "," + model.ToPhoneNo + "," + model.BillerName + "," + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Top Up", req, res, responseMessage.ToString(), responseStatus.ToString(), model.Amount,"N/A");

                    if (!responseMessage.IsNullOrEmpty())
                    {
                        TempData["Success"] = true;
                    }

                }
                catch (Exception e)
                {
                    //insert into TranLog
                    string req = model.PAN + "," + model.CardExp + "," + model.Amount + "," + tel + "," + model.IPIN + "," + model.ToPhoneNo + "," + model.BillerName + "," + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Top Up", req, "", "failed", "", model.Amount, "");

                    //Console.WriteLine(e);
                    ModelState.AddModelError("",e.Message);
                }


            }
            
            return PartialView();
        }*/


        public ActionResult GetFavoritePhone()
        {
            string user_id = Session["UserID"].ToString();
            List<TelecomPostPaidViewModel> accas = new List<TelecomPostPaidViewModel>();
            accas = data.getFavPhones(user_id);
            //Session["phonesList"] = accas;
            return PartialView(accas);
        }

        public ActionResult ChoosePhone(int id, ManagePhonesViewModel model)
        {
            TelecomPostPaidViewModel modell = new TelecomPostPaidViewModel();
            List<TelecomPostPaidViewModel> accas = new List<TelecomPostPaidViewModel>();
            accas = (List<TelecomPostPaidViewModel>)Session["phonesList"];

            foreach (var item in accas)
            {
                if (item.FavoritePhoneID.Equals(id.ToString()))
                    model.FavoritePhoneNo = item.ToPhoneNo;
            }

            /*Session["metersList"] = "";*/
            Session["FavPhone"] = true;
            Session["FavPhoneID"] = id;

            return RedirectToAction("TelecomPostPaid", modell.ToPhoneNo);
        }
    }
}