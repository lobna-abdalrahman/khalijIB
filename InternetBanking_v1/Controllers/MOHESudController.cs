using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers
{
    public class MOHESudController : BaseController
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();

        //
        // GET: /MOHESud/
        public ActionResult MOHESud()
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
        public ActionResult MOHESud(MOHESudViewModel model)
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
            if (ModelState.IsValid)
            {
                string tel = Session["UserTel"].ToString();
                string desc = "MOHE";

                //
                if (model.STUDFORMKIND == "1"  || model.STUDFORMKIND == "6" || model.STUDFORMKIND == "8")
                {
                    model.TranAmount = "50";
                }
                else if (model.STUDFORMKIND == "2" || model.STUDFORMKIND == "3" || model.STUDFORMKIND == "7" || model.STUDFORMKIND == "9")
                {
                    model.TranAmount = "200";
                }            
                else
                {
                    model.TranAmount = "200";
                }


                try
                {
                    model.BillerName = "8";
                    string res = myCheque.EBSBillInquiryMOHE(model.CardNumber, model.CardExp, model.TranAmount, tel, model.IPIN,
                        model.SETNUMBER, model.BillerName, user_id, desc,model.STUDCOURSEID,model.STUDFORMKIND);

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

                        //insert into TranLog
                        string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.SETNUMBER + "," + model.BillerName + "," + user_id + "," + desc;
                        data.InsertTranLog(user_id, "MOHE Bill inquery", req, "", "Failed", "", model.TranAmount,"");
                    }
                    else
                    {
                        if (result.billInfo != null)
                        {
                            string info = result.billInfo.ToString();
                            JObject infoObj = JObject.Parse(info);
                            dynamic billInfo = infoObj;

                            model.formNo = billInfo.formNo;
                            model.receiptNo = billInfo.receiptNo;
                            model.englishName = billInfo.englishName;
                            model.arabiName = billInfo.arabicName;
                            model.dueAmount = billInfo.dueAmount ;

                            //insert into TranLog
                            string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.SETNUMBER + "," + model.BillerName + "," + user_id + "," + desc;
                            data.InsertTranLog(user_id, "MOHE Bill inquery", req, res, responseMessage.ToString(), responseStatus.ToString(), model.TranAmount, model.formNo);
                            Session["MOHESudViewModel"] = model;
         
                            return RedirectToAction("ViewData");

                        }
                    }

                }
                catch (Exception e)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", string.Format(" Sorry we are facing Problem here {0}", e.Message));
                    ViewBag.Message = string.Format(" Sorry we are facing Problem here {0}", e.Message);
                    
                }
            }

            return View();
        }

        public ActionResult ViewData()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            MOHESudViewModel model = (MOHESudViewModel)Session["MOHESudViewModel"];
         
            return View(model);
        }
    [HttpPost]
        public ActionResult ViewData(MOHESudViewModel model2)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            MOHESudViewModel model = (MOHESudViewModel)Session["MOHESudViewModel"];
      
          
            
            
            if (model!=null)
            {
                string tel = Session["UserTel"].ToString();
                string desc = "MOHE";

                //
                if (model.STUDFORMKIND == "1" || model.STUDFORMKIND == "6" || model.STUDFORMKIND == "8")
                {
                    model.TranAmount = "50";
                }
                else if (model.STUDFORMKIND == "2" || model.STUDFORMKIND == "3" || model.STUDFORMKIND == "7" || model.STUDFORMKIND == "9")
                {
                    model.TranAmount = "200";
                }
                else
                {
                    model.TranAmount = "200";
                }


                try
                {
                    model.BillerName = "8";
                    string res = myCheque.EBSDoBillMOHE(model.CardNumber, model.CardExp, model.dueAmount, tel, model.IPIN,
                        model.SETNUMBER, model.BillerName, user_id, desc, model.STUDCOURSEID, model.STUDFORMKIND);

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

                        //insert into TranLog
                        string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.SETNUMBER + "," + model.BillerName + "," + user_id + "," + desc;
                        data.InsertTranLog(user_id, "MOHE Payment", req, "", "Failed", "", model.TranAmount, "");
                    }
                    else
                    {
                        if (result.billInfo != null)
                        {
                            string info = result.billInfo.ToString();
                            JObject infoObj = JObject.Parse(info);
                            dynamic billInfo = infoObj;

                            model.formNo = billInfo.formNo;
                            model.receiptNo = billInfo.receiptNo;
                            model.englishName = billInfo.englishName;
                            model.arabiName = billInfo.arabicName;
                            model.dueAmount = billInfo.dueAmount;

                            //insert into TranLog
                            string req = model.CardNumber + "," + model.CardExp + "," + model.TranAmount + "," + tel + "," + model.IPIN + "," + model.SETNUMBER + "," + model.BillerName + "," + user_id + "," + desc;
                            data.InsertTranLog(user_id, "MOHE Payment", req, res, responseMessage.ToString(), responseStatus.ToString(), model.TranAmount, model.formNo);

                            return RedirectToAction("Result", model);

                        }
                    }

                }
                catch (Exception e)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", string.Format(" Sorry we are facing Problem here {0}", e.Message));
                    ViewBag.Message = string.Format(" Sorry we are facing Problem here {0}", e.Message);

                }
            }

            return View(model);
        }
        public ActionResult Result(MOHESudViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View(model);
        }
	}
}