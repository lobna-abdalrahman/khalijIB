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

namespace InternetBanking_v1.Controllers.Payments.Customs_Payment
{
    public class CustomsPaymentController : BaseController//Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /CustomsPayment/
        public ActionResult CustomsPay()
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
        public ActionResult CustomsPay(CustomsViewModel model)
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
            model.CardExp = model.Year + model.Month;

            if (ModelState.IsValid)
            {
                string tel = Session["UserTel"].ToString();
                string desc = "Customs";

                ViewBag.flag = 0;

                try
                {
                    string amount = "0";
                    model.TranAmount = amount;
                    model.BillerName = "9"; //Customs Biller...
                    string res =  myCheque.EBSCustomsBillInquiry(model.CardNumber, model.CardExp, model.TranAmount, tel,
                        model.IPIN, model.PolicyNo,model.DeclarantCode, model.BillerName, user_id, desc);
                     
                        


                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    string responseStatus = result.responseStatus;
                    string responseMessage = result.responseMessage;


            

                    if (responseStatus.Equals("Failed"))
                    {
                        TempData["Success"] = true;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Not Successful";
                        ViewBag.ResponseMSG = responseMessage;

                        string req = model.CardNumber + "," + model.CardExp + "," + model.AmountToBePaid + "," + tel + "," + model.IPIN + "," + model.PolicyNo + "," +model.DeclarantCode+","+ model.BillerName + "," + user_id + "," + desc;
                        data.InsertTranLog(user_id, "Customs Bill", req, "", "failed", "",model.AmountToBePaid,"");
                    }
                    else
                    {

                        //get bill info
                        if (result.billInfo != null)
                        {
                            string info = result.billInfo.ToString();
                            JObject infoObj = JObject.Parse(info);
                            dynamic billInfo = infoObj;

                            model.BankCode = billInfo.BankCode;
                            model.BillAmount = billInfo.Amount;
                            model.DeclarantNAME = billInfo.DeclarantNAME;
                            model.InstanceID = billInfo.InstanceID;
                            model.Office = billInfo.Office;
                            model.Declarant = billInfo.Declarant;
                            model.DECNBER = billInfo.DECNBER;
                            model.Transaction = billInfo.Transaction;
                            model.BillYear = billInfo.Year;
                            model.DECSER = billInfo.DECSER;

                             model.BankCode = model.PolicyNo;
                            model.BillAmount = billInfo.Amount;
                            model.DeclarantNAME = billInfo.DeclarantNAME;
                            model.RegistrationNumber = billInfo.RegistrationNumber;
                            model.RegistrationSerial = billInfo.RegistrationSerial;
                            model.Declarant = billInfo.Declarant;
                            model.ProcError = billInfo.ProcError;
                             
                            model.DECSER = billInfo.DECSER;
                            model.Status = billInfo.Status;
                            model.DeclarantCode = billInfo.DeclarantCode;
                            model.AmountToBePaid = billInfo.AmountToBePaid;

                            Session["BankCode"] = model.BankCode;
                            Session["Amount"] = model.BillAmount;
                            Session["DeclarantNAME"] = model.DeclarantNAME;
                            Session["RegistrationNumber"] = model.RegistrationNumber;
                            Session["RegistrationSerial"] = model.RegistrationSerial;
                            Session["Declarant"] = model.Declarant;
                            Session["ProcError"] = model.ProcError;
                            Session["Transaction"] = model.Transaction;
                            Session["Year"] = model.BillYear;
                            Session["DECSER"] = model.DECSER;

                            Session["Status"] = model.Status;
                            Session["AmountToBePaid"] = model.AmountToBePaid;
                            Session["DeclarantCode"] = model.DeclarantCode;
                            Session["Model"] = model;
                            

                            
                            Session["DeclarantNAME"] = model.DeclarantNAME ;
                          //  Session["InstanceID"] = model.InstanceID;
                           // Session["Office"] = model.Office;
                            Session["Declarant"] = model.Declarant;
                           // Session["DECNBER"] = model.DECNBER;
                            Session["Transaction"] = model.Transaction;
                            Session["Year"] = model.BillYear;
                            Session["DECSER"] = model.DECSER;

                            //insert into TranLog
                            string req = model.CardNumber + "," + model.CardExp + "," + model.AmountToBePaid + "," + tel + "," + model.IPIN + "," + model.PolicyNo + ","+model.DeclarantCode+","  + model.BillerName + "," + user_id + "," + desc;
                            data.InsertTranLog(user_id, "Customs Bill", req, res, responseMessage.ToString(), responseStatus.ToString(), model.AmountToBePaid, model.RegistrationNumber);

                            return RedirectToAction("DoCustomPayment");


                        }
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    ModelState.AddModelError("", "Sorry Couldn't Complete the Request, please try again later");
                    //insert into TranLog
                    string req = model.CardNumber + "," + model.CardExp + "," + model.AmountToBePaid + "," + tel + "," + model.IPIN + "," + model.PolicyNo+ ","+model.DeclarantCode+","+ model.BillerName + "," + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Customs Bill", req, "", "failed", "", model.AmountToBePaid,"");
                }
            }

            return View();
        }


        public ActionResult DoCustomPayment()
        {
            CustomsViewModel model = (CustomsViewModel)Session["Model"];
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();



            model.BankCode = Session["BankCode"] == null ? "" : Session["BankCode"].ToString();
            model.BillAmount = Session["Amount"] == null ? "" : Session["Amount"].ToString();
            model.DeclarantNAME = Session["DeclarantNAME"] == null ? "" : Session["DeclarantNAME"].ToString();
            model.RegistrationNumber = Session["RegistrationNumber"] == null ? "" : Session["RegistrationNumber"].ToString();
            model.RegistrationSerial = Session["RegistrationSerial"] == null ? "" : Session["RegistrationSerial"].ToString();
            model.Declarant = Session["Declarant"] == null ? "" : Session["Declarant"].ToString();
            model.ProcError = Session["ProcError"] == null ? "" : Session["ProcError"].ToString();
            model.Transaction = Session["Transaction"] == null ? "" : Session["Transaction"].ToString();
            model.BillYear = Session["Year"] == null ? "" : Session["Year"].ToString();
            model.DECSER = Session["DECSER"] == null ? "" : Session["DECSER"].ToString();


            model.Status = Session["Status"] == null ? "" : Session["Status"].ToString();
            model.AmountToBePaid = Session["AmountToBePaid"] == null ? "" : Session["AmountToBePaid"].ToString();
            model.DeclarantCode = Session["DeclarantCode"] == null ? "" : Session["DeclarantCode"].ToString();

            return View(model);
        }




        [HttpPost]
        public ActionResult DoCustomPayment(CustomsViewModel model)
        {
            //string message = "";
            //if (Session["username"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}

            //string user_id = Session["UserID"].ToString();



            //model.BankCode = Session["BankCode"].ToString();
            //model.BillAmount = Session["Amount"].ToString();
            //model.DeclarantNAME = Session["DeclarantNAME"].ToString();
            //model.InstanceID = Session["InstanceID"].ToString();
            //model.Office = Session["Office"].ToString();
            //model.Declarant = Session["Declarant"].ToString();
            //model.DECNBER = Session["DECNBER"].ToString();
            //model.Transaction = Session["Transaction"].ToString();
            //model.BillYear = Session["Year"].ToString();
            //model.DECSER = Session["DECSER"].ToString();

            //string tel = Session["UserTel"].ToString();
            model = (CustomsViewModel)Session["Model"];
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            model.BankCode = Session["BankCode"] == null ? "" : Session["BankCode"].ToString();
            model.BillAmount = Session["Amount"] == null ? "" : Session["Amount"].ToString();
            model.DeclarantNAME = Session["DeclarantNAME"] == null ? "" : Session["DeclarantNAME"].ToString();
            model.RegistrationNumber = Session["RegistrationNumber"] == null ? "" : Session["RegistrationNumber"].ToString();
            model.RegistrationSerial = Session["RegistrationSerial"] == null ? "" : Session["RegistrationSerial"].ToString();
            model.Declarant = Session["Declarant"] == null ? "" : Session["Declarant"].ToString();
            model.ProcError = Session["ProcError"] == null ? "" : Session["ProcError"].ToString();
            model.Transaction = Session["Transaction"] == null ? "" : Session["Transaction"].ToString();
            model.BillYear = Session["Year"] == null ? "" : Session["Year"].ToString();
            model.DECSER = Session["DECSER"] == null ? "" : Session["DECSER"].ToString();


            model.Status = Session["Status"] == null ? "" : Session["Status"].ToString();
            model.AmountToBePaid = Session["AmountToBePaid"] == null ? "" : Session["AmountToBePaid"].ToString();
            model.DeclarantCode = Session["DeclarantCode"] == null ? "" : Session["DeclarantCode"].ToString();
            string tel = Session["UserTel"].ToString();
            string desc = "Customs bill";

            try
            {
                string res = myCheque.EBSCustomsDoBill(model.CardNumber, model.CardExp, model.AmountToBePaid, tel, model.IPIN,
                    model.PolicyNo, model.DeclarantCode, model.BillerName, user_id, desc);

                JObject jobj = new JObject();
                jobj = JObject.Parse(res);
                dynamic result = jobj;

                string responseStatus = result.responseStatus;
                string responseMessage = result.responseMessage;

                //insert into TranLog
                string req = model.CardNumber + "," + model.CardExp + "," + model.AmountToBePaid + "," + tel + "," + model.IPIN + "," + model.PolicyNo + "," + model.DeclarantCode + "," + model.BillerName + "," + user_id + "," + desc;
                data.InsertTranLog(user_id, "Customs Bill", req, res, responseMessage.ToString(), responseStatus.ToString(), model.AmountToBePaid,"N/A" );

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

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                ModelState.AddModelError("",GlobalRes.ConnectivityIssue);
                //insert into TranLog
                string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.PolicyNo + "," + model.BillerName + "," + user_id + "," + desc;
                data.InsertTranLog(user_id, "Customs Bill", req, "", "failed", "",model.AmountToBePaid,"");
            }



            return View(model);

        }
	}
}