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

namespace InternetBanking_v1.Controllers.Payments.Electricity_Payment
{
    public class ElectricityPaymentController : BaseController//Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /ElectricityPayment/
        public ActionResult ElectricityPay()
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
        public ActionResult ElectricityPay(ElectricityViewModel model)
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
            model.CardExp = model.Year + model.Month;

            if (ModelState.IsValid)
            {
                string tel = Session["UserTel"].ToString();
                string desc = "Electricity";
                model.BillerName = "7";

                ViewBag.flag = 0;

                try
                {

                    string res = myCheque.EBSDoBill(model.CardNumber, model.CardExp, model.TranAmount, tel,
                        model.IPIN, model.MeterNo, model.BillerName, user_id, desc);

                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    string responseStatus = result.responseStatus;
                    string responseMessage = result.responseMessage;
                    ViewBag.ResponseStat = responseStatus;
                    ViewBag.ResponseMSG = responseMessage;
                    ViewBag.flag = 1;

                    string info = result.billInfo.ToString();
                    JObject jInfo = new JObject();
                    jInfo = JObject.Parse(info);
                    dynamic Eres = jInfo;

                    model.myMeterNumber = Eres.meterNumber;
                    model.netAmount = Eres.netAmount;
                    model.token = Eres.token;
                    model.waterFees = Eres.waterFees;
                    model.meterFees = Eres.meterFees;
                    model.customerName = Eres.customerName;
                    model.unitsInKwh = Eres.unitsInKwh;
                    model.operatorMessage = Eres.operatorMessage;
                    model.accountNo = Eres.accountNo;


                    if (!responseMessage.IsNullOrEmpty())
                    {

                        //insert into TranLog
                        string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.MeterNo + "," + model.BillerName + "," + user_id + "," + desc;
                        //insert into electricity log
                        data.NECTranLog(user_id, "Electricity", req, res, responseMessage, responseStatus,
                            model.CardNumber, model.MeterNo, model.token, model.TranAmount, model.waterFees,
                            model.meterFees, model.netAmount, model.customerName, model.unitsInKwh,
                            model.operatorMessage);
                         data.InsertTranLog(user_id, "Electricity", req, res, responseMessage.ToString(), responseStatus.ToString(), model.TranAmount, model.token);

                        if (responseStatus.Equals("Failed"))
                        {                            

                            TempData["Success"] = true;
                            return View();
                        }



                        return RedirectToAction("ViewElectricity", model);
                    }

                }
                catch (Exception e)
                {

                    //insert into TranLog
                    string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.MeterNo + "," + model.BillerName + "," + user_id + "," + desc;
                  data.InsertTranLog(user_id, "Electricity", req, "", "failed", "",model.TranAmount,"");
                
                    data.NECTranLog(user_id, "Electricity", req, "N/A", "Connectivity Error", "N/A",
                        model.CardNumber, model.MeterNo, "N/A", model.TranAmount, "N/A",
                        "N/A", "N/A", "N/A", "N/A",
                        "N/A");

                    //Console.WriteLine(e);
                    ModelState.AddModelError("",GlobalRes.support);
                    return View();

                }

            }



            return View();
        }

        public ActionResult ViewElectricity(ElectricityViewModel model)
        {

            return View(model);

        }

        public ActionResult GetFavoriteMeter()
        {
            string user_id = Session["UserID"].ToString();

            List<ElectricityViewModel> accas = new List<ElectricityViewModel>();
            accas = data.getFavMeters(user_id);
            Session["metersList"] = accas;

            return PartialView(accas);

        }

        public ActionResult GetFavoriteCard()
        {
            string user_id = Session["UserID"].ToString();

            List<CardTransferViewModel> accas = new List<CardTransferViewModel>();

            accas = data.DropCardClient(user_id);


            return PartialView(accas);

        }

        public ActionResult ChooseMeter(int id, ManageMetersViewModel model)
        {
            ElectricityViewModel modell = new ElectricityViewModel();
            List<ElectricityViewModel> accas = new List<ElectricityViewModel>();
            accas = (List<ElectricityViewModel>)Session["metersList"];

            foreach (var item in accas)
            {
                if (item.MeterID.Equals(id.ToString()))
                    model.MeterNo = item.MeterNo;
            }

            /*Session["metersList"] = "";*/
            Session["FavMeter"] = true;
            Session["FavMeterID"] = id;
            return RedirectToAction("ElectricityPay", modell.MeterNo);

        }

        public ActionResult ElectricityHistory()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

            List<ElectricityViewModel> trans = new List<ElectricityViewModel>();
            trans = data.getElectricityTransactions(user_id);

            return View(trans);
        }





	}
}