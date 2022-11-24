using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers.CardTransfer
{
    public class CardTransferController : BaseController//Controller
    {
        CardTransferViewModel model = new CardTransferViewModel();
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /CardTransfer/
        public ActionResult CardTransfer()
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
        public ActionResult CardTransfer(CardTransferViewModel model)
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
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
                string tel = Session["UserTel"].ToString();

                ViewBag.flag = 0;
                try
                {
                    string res = myCheque.DoCardTransfer(model.CardNumber, model.CardExp, model.Amount, tel,
                        model.IPIN, model.ToCard);

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
                    string req = model.CardNumber + "," + model.CardExp + "," + model.Amount + "," + tel + "," + model.IPIN + "," + model.ToCard;
                    data.InsertTranLog(user_id, "Card Transfer", req, res, responseMessage.ToString(), responseStatus.ToString(), model.Amount,"N/A");


                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    ModelState.AddModelError("", e.Message);

                    //insert into TranLog
                    string req = model.CardNumber + "," + model.CardExp + "," + model.Amount + "," + tel + "," + model.IPIN + "," + model.ToCard;
                    data.InsertTranLog(user_id, "Card Transfer", req, "", "failed", "",model.Amount, "N/A");
                }
            }
            else
            {
                ModelState.AddModelError("", "Something is missing");
            }

            return View();
            
        }


        public ActionResult GetFavoriteCard()
        {
            string user_id = Session["UserID"].ToString();

            List<CardTransferViewModel> accas = new List<CardTransferViewModel>();

            accas = data.DropCardClient(user_id);


            return PartialView(accas);

        }
	}
}