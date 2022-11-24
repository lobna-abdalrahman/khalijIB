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

namespace InternetBanking_v1.Controllers.Services_Ctrls.ChangePIN
{
    public class ChangePINController : BaseController
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /ChangePIN/
        public ActionResult ChangePin()
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
        public ActionResult ChangePin(ChangeIPINViewModel model)
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
                    Text = item.CardNumber,
                    Value = item.CardNumber.ToString()
                });
            }

            ViewBag.clientList = AccountList;

            //Get Selected Account...
            model.PAN = Request["Clients"];
            model.expDate = model.Year + model.Month;

            if (ModelState.IsValid)
            {
                string tel = Session["UserTel"].ToString();
                string desc = "Change IPIN";

                try
                {
                    string res = myCheque.DoChangeIPIN(model.PAN, model.expDate, tel, model.CurrentIPIN, model.NewIPIN,
                        user_id);
                    

                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    string responseStatus = result.responseStatus;
                    string responseMessage = result.responseMessage;
                    ViewBag.ResponseStat = responseStatus;
                    ViewBag.ResponseMSG = responseMessage;

                    //insert into TranLog
                    string req = model.PAN + "," + model.expDate + "," + tel + "," + model.CurrentIPIN + "," + model.NewIPIN + ","  + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Change IPIN", req, res, responseMessage.ToString(), responseStatus.ToString(),"N/A","N/A");


                    if (!responseMessage.IsNullOrEmpty())
                    {

                        TempData["Success"] = true;
                        //if (responseStatus.Equals("Failed"))
                        //{
                            
                        //    return View();
                        //}
                    }

                }
                catch (Exception e)
                {

                    //insert into TranLog
                    string req = model.PAN + "," + model.expDate + "," + tel + "," + model.CurrentIPIN + "," + model.NewIPIN + "," + user_id + "," + desc;
                    data.InsertTranLog(user_id, "Change IPIN", req, "", "failed", "","N/A","N/A");

                    //Console.WriteLine(e);
                    ModelState.AddModelError("",e.Message);
                }
            }

            return View();
        }
	}
}