using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers
{
    public class CardlessTransferController : Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /CardlessTransfer/
        public ActionResult CardlessTransfer()
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

            return View();
        }


        [HttpPost]
        public ActionResult CardlessTransfer(CardlessTransferViewModel model)
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

            //Get Selected Account...
            model.CardNumber = Request["Clients"];
            foreach (var item in pcontent)
            {
                if (item.CardNumber.Equals(model.CardNumber.ToString()))
                {
                    model.CardExp = item.CardExp;
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
                string desc = "Cardless Transfer";
                /* is this truee ==> */
                model.BillerName = "7";

                ViewBag.flag = 0;

                try
                {
                    string res = myCheque.DoCardLessTransfer(model.CardNumber, model.CardExp, model.TranAmount, tel,
                        model.IPIN, model.VoucherNo);
                    //string res = "{\"responseStatus\":\"Successful\",\"responseCode\":0,\"balance\":{\"available\":3575.46,\"leger\":3575.46},\"voucherNumber\":\"0904018283\",\"responseMessage\":\"Approved\",\"tranAmount\":10,\"voucherCode\":\"788351\"}";

                    
                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;


                    string responseMessage = result.responseMessage;
                    string responseStatus = result.responseStatus;
                    if (responseMessage.Equals("Approved"))
                    {

                        string voucher_no = result.voucherNumber;
                        string voucher_code = result.voucherCode;
                        string voucher_amount = result.tranAmount;

                        //insert into TranLog
                        string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," +
                                     model.IPIN + "," + model.VoucherNo;
                        data.InsertTranLog(user_id, "CardLess Transfer", req, res, responseMessage.ToString(),
                            responseStatus.ToString(), voucher_amount, voucher_code);


                        Session["voucher_no"] = voucher_no;
                        Session["voucher_code"] = voucher_code;
                        Session["voucher_amount"] = voucher_amount;

                        if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
                        {
                            return RedirectToAction("ViewVoucher", new {lang = "ar"});
                        }
                        else
                        {
                            return RedirectToAction("ViewVoucher", model);
                        }

                    }
                    else
                    {
                        //insert into TranLog
                        string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," +
                                     model.IPIN + "," + model.VoucherNo;
                        data.InsertTranLog(user_id, "CardLess Transfer", req, res, "Failed",
                            "TimedOut", model.TranAmount, "N/A");
                        ModelState.AddModelError("",GlobalRes.ConnectivityErrorMessage);
                        return View();
                    }
                    


                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    ModelState.AddModelError("", "Sorry, Connectivity Issues..Please try again later");
                    string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," +
                                 model.IPIN + "," + model.VoucherNo;
                    data.InsertTranLog(user_id, "CardLess Transfer", req, "Failed", "Failed",
                        "TimedOut", model.TranAmount, "N/A");
                }
            }

            return View();
        }

        public ActionResult ViewVoucher()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            CardlessTransferViewModel model = new CardlessTransferViewModel();
            model.voucherCode = Session["voucher_code"].ToString();
            model.voucherNumber = Session["voucher_no"].ToString();
            model.transactionAmount = Session["voucher_amount"].ToString() + "  SDG";

            Session["voucher_code"] = "";
            Session["voucher_no"] = "";
            Session["voucher_amount"] = "";

            return View(model);
        }
	}
}