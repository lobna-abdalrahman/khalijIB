using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Helper;
using InternetBanking_v1.Models;
using LinqToDB.Common;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers.Payments.Telecom
{
    public class Telecom2Controller : Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /Telecom2/
        //public ActionResult TelecomPostPaid()
        //{
        //    return View();
        //}

        // GET: /Post Paid Telecom/
        public ActionResult TelecomPostPaid()
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
        public ActionResult TelecomPostPaid(TelecomPostPaidViewModel model)
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
                string desc = "Mobile Bill";

                ViewBag.flag = 0;

                try
                {
                   
                    string res = myCheque.EBSBillInquiry(model.CardNumber, model.CardExp, model.TranAmount, tel,
                        model.IPIN, model.ToPhoneNo, model.BillerName, user_id, desc);

                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    string responseStatus = result.responseStatus;
                    string responseMessage = result.responseMessage;


                    //insert into TranLog
                    string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.ToPhoneNo + "," + model.BillerName + "," + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Mobile Bill", req, res, responseMessage.ToString(), responseStatus.ToString(),model.TranAmount,"");

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
                            Session["Zain"] = "false";

                            if (model.BillerName.Equals("2"))
                            {
                                Session["Zain"] = "true";
                                model.BillAmount = billInfo.billedAmount;
                                model.total = billInfo.totalAmount;
                                model.contractNumber = billInfo.contractNumber;
                                model.unbilledAmount = billInfo.unbilledAmount;
                                model.LastInvoiceDate = billInfo.lastInvoiceDate;
                                model.LastFourDigits = billInfo.last4Digits;

                                Session["LastInvoiceDate"] = model.LastInvoiceDate;
                                Session["LastFourDigits"] = model.LastFourDigits;
                            }
                            else
                            {
                                model.BillAmount = billInfo.BillAmount;
                                model.total = billInfo.total;
                                model.contractNumber = billInfo.contractNumber;
                                model.unbilledAmount = billInfo.unbilledAmount;
                                model.billAmount1 = billInfo.billAmount;
                                model.SubscriberID = billInfo.SubscriberID;


                            }


                            string billAmount = model.BillAmount;
                            string total = model.total;
                            string contractNumber = model.contractNumber;
                            string unbilledAmount = model.unbilledAmount;

                            Session["billAmount"] = billAmount;
                            Session["SubscriberID"] = model.SubscriberID;
                            Session["billAmount1"] = model.billAmount1;
                            Session["total"] = total;
                            Session["contractNumber"] = contractNumber;
                            Session["unbilledAmount"] = unbilledAmount;

                            Session["CardNumber"] = model.CardNumber;
                            Session["CardExp"]    = model.CardExp;
                            Session["IPIN"]       = model.IPIN;
                            Session["ToPhoneNo"]  = model.ToPhoneNo;
                            Session["BillerName"] = model.BillerName;

                            if (CultureHelper.IsRighToLeft())
                            {
                                return RedirectToAction("DoMyBillPayment", new { lang = "ar" });
                            }
                            else
                            {
                                return RedirectToAction("DoMyBillPayment");
                            }

                        }

                    }

                }
                catch (Exception e)
                {

                    //insert into TranLog
                    string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.ToPhoneNo + "," + model.BillerName + "," + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Mobile Bill", req, "", "failed", "",model.TranAmount,"");

                    //Console.WriteLine(e);
                    ModelState.AddModelError("","Sorry Connectivity Issues, Please Try again later..");
                }
                

            }


            return View();
        }

        public ActionResult DoMyBillPayment()
        {
            TelecomPostPaidViewModel model = new TelecomPostPaidViewModel();
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();
            /*model.BillAmount = Session["billAmount"].ToString();
            model.total = Session["total"].ToString();
            model.unbilledAmount = Session["unbilledAmount"].ToString();
            model.contractNumber = Session["contractNumber"].ToString();
*/

            ViewBag.Zain = false;

            //if (Session["billAmount"] != null)
            //{
            //    model.BillAmount = Session["billAmount"].ToString();
            //}
            if (Session["billAmount"] != null)
            {
                model.BillAmount = Session["billAmount"].ToString();
            }
            else
            {
                if (Session["billAmount1"] != null)
                model.BillAmount = Session["billAmount1"].ToString();
            }

            if (Session["total"] != null)
            {
                model.total = Session["total"].ToString();
            }


            if (Session["unbilledAmount"] != null)
            {
                model.unbilledAmount = Session["unbilledAmount"].ToString();
            }
            if (Session["contractNumber"] != null)
            {
                model.contractNumber = Session["contractNumber"].ToString();
            }
            if (Session["SubscriberID"] != null)
            {
                model.SubscriberID = Session["SubscriberID"].ToString();
            }

            if (Session["LastInvoiceDate"] != null)
            {
                ViewBag.Zain = true;
                model.LastInvoiceDate = Session["LastInvoiceDate"].ToString();
                model.LastFourDigits = Session["LastFourDigits"].ToString();
            }

            if (Session["Zain"] != null)
            {
                ViewBag.Zain = true;

                Session["Zain"] = "true";
            }

            string tel = Session["UserTel"].ToString();
            string desc = "Mobile bill";

            model.CardNumber = Session["CardNumber"].ToString();
            model.CardExp = Session["CardExp"].ToString();
            model.IPIN = Session["IPIN"].ToString();
            model.ToPhoneNo = Session["ToPhoneNo"].ToString();
            model.BillerName = Session["BillerName"].ToString();

            return View(model);
        }

        //Do BillPayment
        [HttpPost]
        public ActionResult DoMyBillPayment(TelecomPostPaidViewModel model)
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();
           /* model.BillAmount = Session["billAmount"].ToString();
            model.total = Session["total"].ToString();
            model.unbilledAmount = Session["unbilledAmount"].ToString();
            model.contractNumber = Session["contractNumber"].ToString();*/
            

            //if (Session["billAmount"] != null)
            //{
            //    model.BillAmount = Session["billAmount"].ToString();
            //}
            //if (Session["total"] != null)
            //{
            //    model.total = Session["total"].ToString();
            //}


            //if (Session["unbilledAmount"] != null)
            //{
            //    model.unbilledAmount = Session["unbilledAmount"].ToString();
            //}
            //if (Session["contractNumber"] != null)
            //{
            //    model.contractNumber = Session["contractNumber"].ToString();
            //}

            //if (Session["LastInvoiceDate"] != null)
            //{
            //    ViewBag.Zain = true;
            //    model.LastInvoiceDate = Session["LastInvoiceDate"].ToString();
            //    model.LastFourDigits = Session["LastFourDigits"].ToString();
            //}
            if (Session["billAmount"] != null)
            {
                model.BillAmount = Session["billAmount"].ToString();
            }
            else
            {
                if (Session["billAmount1"] != null)
                    model.BillAmount = Session["billAmount1"].ToString();
            }

            if (Session["total"] != null)
            {
                model.total = Session["total"].ToString();
            }


            if (Session["unbilledAmount"] != null)
            {
                model.unbilledAmount = Session["unbilledAmount"].ToString();
            }
            if (Session["contractNumber"] != null)
            {
                model.contractNumber = Session["contractNumber"].ToString();
            }
            if (Session["SubscriberID"] != null)
            {
                model.SubscriberID = Session["SubscriberID"].ToString();
            }

            if (Session["LastInvoiceDate"] != null)
            {
                ViewBag.Zain = true;
                model.LastInvoiceDate = Session["LastInvoiceDate"].ToString();
                model.LastFourDigits = Session["LastFourDigits"].ToString();
            }
            if (Session["Zain"] != null)
            {
                ViewBag.Zain = true;

                Session["Zain"] = "true";
            }

       

            string tel = Session["UserTel"].ToString();
            string desc = "Mobile bill";

            model.CardNumber = Session["CardNumber"].ToString();
            model.CardExp = Session["CardExp"].ToString();  
            model.IPIN = Session["IPIN"].ToString();      
            model.ToPhoneNo = Session["ToPhoneNo"].ToString(); 
            model.BillerName = Session["BillerName"].ToString();

             

            try
            {
               
                string res = myCheque.EBSDoBill(model.CardNumber, model.CardExp, model.TranAmount, tel, model.IPIN,
                    model.ToPhoneNo, model.BillerName, user_id, desc);

                JObject jobj = new JObject();
                jobj = JObject.Parse(res);
                dynamic result = jobj;

                string responseStatus = result.responseStatus;
                string responseMessage = result.responseMessage;

                //insert into TranLog
                string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.ToPhoneNo + "," + model.BillerName + "," + user_id + "," + desc;
                data.InsertTranLog(user_id, "Mobile Bill Payment", req, res, responseMessage.ToString(), responseStatus.ToString(),model.TranAmount,"N/A");

                if (!responseMessage.IsNullOrEmpty())
                {
                    TempData["Success"] = true;
                    ViewBag.ResponseStat = responseStatus;
                    ViewBag.ResponseMSG = responseMessage;
                }
            }
            catch (Exception e)
            {

                //insert into TranLog
                string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.ToPhoneNo + "," + model.BillerName + "," + user_id + "," + desc;
                data.InsertTranLog(user_id, "Mobile Bill Payment", req, "", "failed", "",model.TranAmount,"");
                //Console.WriteLine(e);
                ModelState.AddModelError("","Connectivity Issues, Please try again Later ...");
            }

            

            return View(model);
        }


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
