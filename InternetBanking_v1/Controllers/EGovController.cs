using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using LinqToDB.Common;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers
{
    public class EGovController : BaseController
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /EGov/
        public ActionResult EGov()
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
        public ActionResult EGov(EGovViewModel model)
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
            model.CardNumber = Request["Clients"];
           // model.CardExp = model.Year + model.Month;
            foreach (var item in pcontent)
            {
                if (item.CardNumber.Equals(model.CardNumber.ToString()))
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
            string amount = "0";
            model.TranAmount = amount;

            if (ModelState.IsValid)
            {
                string tel = Session["UserTel"].ToString();

                try
                {
                    string res = myCheque.EBSGovBill(model.CardNumber, model.CardExp, model.IPIN, tel, model.InvoiceNo,
                        model.PhoneNo, user_id);

                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    string responseStatus = result.responseStatus;
                    string responseMessage = result.responseMessage;

                    /*//insert into TranLog
                    string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.InvoiceNo + "," + model.PhoneNo + "," + user_id ;
                    data.InsertTranLog(user_id, "EGov Bill Inquiry", req, res, responseMessage.ToString(), responseStatus.ToString(), model.TranAmount,);*/

                    if (responseStatus.Equals("Failed"))
                    {
                        TempData["Success"] = true;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Not Successful";
                        ViewBag.ResponseMSG = responseMessage;

                    }
                    else
                    {
                        //get bill info
                        if (result.billInfo != null)
                        {
                            string info = result.billInfo.ToString();
                            JObject infoObj = JObject.Parse(info);
                            dynamic billInfo = infoObj;

                            model.UnitName      = billInfo.UnitName;
                            model.TotalAmount   = billInfo.TotalAmount;
                            model.ServiceName   = billInfo.ServiceName;
                            model.InvoiceExpiry = billInfo.InvoiceExpiry;
                            model.DueAmount     = billInfo.DueAmount;
                            model.ReferenceId   = billInfo.ReferenceId;
                            model.InvoiceStatus = billInfo.InvoiceStatus;
                            model.PayerName     = billInfo.PayerName;

                            Session["UnitName"] = model.UnitName;
                            Session["TotalAmount"] = model.TotalAmount;
                            Session["ServiceName"] = model.ServiceName ;
                            Session["InvoiceExpiry"] = model.InvoiceExpiry;
                            Session["DueAmount"] = model.DueAmount;
                            Session["ReferenceId"] = model.ReferenceId ;
                            Session["InvoiceStatus"] = model.InvoiceStatus;
                            Session["PayerName"] = model.PayerName;

                            //insert into TranLog
                            string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.InvoiceNo + "," + model.PhoneNo + "," + user_id ;
                            data.InsertTranLog(user_id, "EGov Bill Inquiry", req, res, responseMessage.ToString(), responseStatus.ToString(), model.TranAmount, model.ReferenceId);

                            return RedirectToAction("GovBillPayment");

                        }
                        
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    ModelState.AddModelError("","Sorry.. Connection Issue, Please try again later");
                    //insert into TranLog
                    string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.InvoiceNo + "," + model.PhoneNo + "," + user_id;
                    data.InsertTranLog(user_id, "EGov Bill Inquiry", req, "", "failed", "",model.TranAmount,"");
                }
            }


            return View();
        }





        public ActionResult GovBillPayment()
        {
            EGovViewModel model = new EGovViewModel();
            string user_id = Session["UserID"].ToString();

            model.UnitName = Session["UnitName"].ToString();
            model.TotalAmount = Session["TotalAmount"].ToString();
            model.ServiceName = Session["ServiceName"].ToString();
            model.InvoiceExpiry = Session["InvoiceExpiry"].ToString();
            model.DueAmount = Session["DueAmount"].ToString();
            model.ReferenceId = Session["ReferenceId"].ToString();
            model.InvoiceStatus = Session["InvoiceStatus"].ToString();
            model.PayerName = Session["PayerName"].ToString();

            string tel = Session["UserTel"].ToString();
            string desc = "E15 billPayment";


            return View(model);
        }





        [HttpPost]
        public ActionResult GovBillPayment(EGovViewModel model)
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string user_id = Session["UserID"].ToString();

            model.UnitName = Session["UnitName"].ToString();
            model.TotalAmount = Session["TotalAmount"].ToString();
            model.ServiceName = Session["ServiceName"].ToString();
            model.InvoiceExpiry = Session["InvoiceExpiry"].ToString();
            model.DueAmount = Session["DueAmount"].ToString();
            model.ReferenceId = Session["ReferenceId"].ToString();
            model.InvoiceStatus = Session["InvoiceStatus"].ToString();
            model.PayerName = Session["PayerName"].ToString();

            string tel = Session["UserTel"].ToString();
            string desc = "E15 billPayment";

            try
            {
                string res = myCheque.EBSGovBillPayment(model.CardNumber, model.CardExp, model.IPIN, tel,
                    model.InvoiceNo, model.PhoneNo, model.TranAmount, user_id);

                JObject jobj = new JObject();
                jobj = JObject.Parse(res);
                dynamic result = jobj;

                string responseStatus = result.responseStatus;
                string responseMessage = result.responseMessage;

                //insert into TranLog
                string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + tel + "," + model.InvoiceNo + "," + model.PhoneNo + "," + model.TranAmount +"," + user_id + "," + desc;
                data.InsertTranLog(user_id, "E15 BillPayment", req, res, responseMessage.ToString(), responseStatus.ToString(), model.TranAmount, responseMessage);

                if (!responseMessage.IsNullOrEmpty())
                {
                    TempData["Success"] = true;
                    ViewBag.ResponseStat = responseStatus;
                    ViewBag.ResponseMSG = responseMessage;
                }


            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                ModelState.AddModelError("","Sorry, Connectivity Issue, Please try again Later..");
                //insert into TranLog
                string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + tel + "," + model.InvoiceNo + "," + model.PhoneNo + "," + model.TranAmount + "," + user_id + "," + desc;
                data.InsertTranLog(user_id, "E15 BillPayment", req, "", "failed", "", model.TranAmount,"");
            }

            return View(model);
        }
	}
}