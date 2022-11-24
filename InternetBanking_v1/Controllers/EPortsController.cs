using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetBanking_v1.Controllers
{
    public class EPortsController : Controller
    {
        // GET: EPorts
        DataSource datasource = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();

        [HttpGet]
        public ActionResult EPorts()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            EPortsViewModel model = new EPortsViewModel();

            model.Ports.Add(new SelectListItem
            {
                Text = GlobalRes.southport,
                Value = "9"
            });
            model.Ports.Add(new SelectListItem
            {
                Text = GlobalRes.northport,
                Value = "8"
            });

            return View(model);
        }

        public ActionResult EPorts(EPortsViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            model.sessionid = "28";
            model.PayServiceID = "3";
            model.PayOrgID = "1";
            //string response = "{\"AdditionalRefrence\": \"8\",\"PaymentVoucherNo\": \"202104873729\",\"CustomerMobile\": \"00249912310750\",\"CustomerBalance\": \"655222.73\",\"PayCustomerCode\": \"10121043251\",\"PaymentDate\": \"20/05/2021\",\"PayServiceID\": \"8\",\"ChannelCode\": \"1\",\"PayCustomerName\": \"null\",\"TotalAmount\": \"293.0\",\"CustomerId\": \"4400212313\",\"FeesAmount\": \"10\",\"OrderStatus\": \"Processing\",\"CustomerFound\": \"-1\",\"PayOrgID\":\"1\",\"Paymentstatus\": \"Successful\",\"CustomerName\": \"FAKKI AHMED MAKKAWI MUSA\",\"SessionID\": \"33\",\"RequiredAmount\": \"0.0\"}";
            string response = "{\"AdditionalRefrence\":\"9\",\"ResponseCode\":\"1\",\"PaymentVoucherNo\":\"\",\"PayCustomerCode\":\"10121042041\",\"PayServiceID\":\"3\",\"ChannelCode\":\"44\",\"PayCustomerName\":\"null\",\"ResponseMessage\":\"1 - null\",\"FeesAmount\":\"\",\"OrderStatus\":\"Not Paid\",\"CustomerFound\":\" - 1\",\"PayOrgID\":\"1\",\"SessionID\":\"28\",\"RequiredAmount\":\"0.0\"}";
            //string response = myCheque.GetOrderinfo(model.PayCustomerCode, model.PayServiceID, model.PayOrgID, model.selectedport, model.sessionid, "44");
            JObject responseobject = new JObject();
            try
            {
                responseobject = JObject.Parse(response);
                dynamic result = responseobject;
                string orderstatus = result.OrderStatus;
                if (!String.IsNullOrEmpty(orderstatus))
                {
                    EPortsViewModel inquerydata = new EPortsViewModel();
                    inquerydata.PaymentVoucherNo = result.PaymentVoucherNo;
                    inquerydata.CustomerMobile = result.CustomerMobile;
                    inquerydata.CustomerBalance = result.CustomerBalance;
                    inquerydata.PaymentDate = result.PaymentDate;
                    inquerydata.TotalAmount = result.TotalAmount;
                    inquerydata.CustomerId = result.CustomerId;
                    inquerydata.FeesAmount = result.FeesAmount;
                    inquerydata.OrderStatus = result.OrderStatus;
                    inquerydata.PaymentStatus = result.Paymentstatus;
                    inquerydata.CustomerName = Session["name"].ToString();
                    inquerydata.RequiredAmount = result.RequiredAmount;
                    inquerydata.PayCustomerCode = model.PayCustomerCode;
                    inquerydata.paycustomername = result.PayCustomerName;
                    inquerydata.CustomerFound = result.CustomerFound;
                    inquerydata.channelCode = result.ChannelCode;
                    inquerydata.PayServiceID = model.PayServiceID;
                    inquerydata.PayOrgID = model.PayOrgID;
                    inquerydata.AdditionalRefrence = model.selectedport;
                    Session["inquerydata"] = inquerydata;
                    return RedirectToAction("ViewInquery", inquerydata);
                }
                else
                {
                    EPortsViewModel model2 = new EPortsViewModel();

                    model2.Ports.Add(new SelectListItem
                    {
                        Text = GlobalRes.southport,
                        Value = "9"
                    });
                    model2.Ports.Add(new SelectListItem
                    {
                        Text = GlobalRes.northport,
                        Value = "8"
                    });
                    ModelState.AddModelError("", "Invoice not found.");
                    return View(model2);
                }

            }
            catch (Exception e)
            {
                EPortsViewModel model2 = new EPortsViewModel();

                model2.Ports.Add(new SelectListItem
                {
                    Text = GlobalRes.southport,
                    Value = "9"
                });
                model2.Ports.Add(new SelectListItem
                {
                    Text = GlobalRes.northport,
                    Value = "8"
                });
                ModelState.AddModelError("", "Invoice not found.");
                return View(model2);
            }
        }

        public ActionResult ViewInquery(EPortsViewModel model)
        {
            string user_id = Session["UserID"].ToString();

            List<OwnTransferViewModel> pcontent = new List<OwnTransferViewModel>();
            {
                pcontent = datasource.DropFromOwnTransferClientSdgOnly(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.FromAccount.Substring(13));
                var AccountType = datasource.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = datasource.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {

                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,//item.FromAccount,
                    Value = item.FromAccount.ToString()
                });
            }

            ViewBag.clientList = AccountList;

            return View(model);
        }

        [HttpPost]
        public ActionResult CompletePayment(EPortsViewModel model)
        {
            EPortsViewModel data = new EPortsViewModel();
            data = (EPortsViewModel)Session["inquerydata"];
            data.sessionid = "33";
            data.PayServiceID = "2";
            data.channelCode = "44";
            string account = model.FromAccount.ToString().Substring(13);
            string branch = model.FromAccount.ToString().Substring(2, 3);
            string account_type = model.FromAccount.ToString().Substring(6, 6);
            string currency = model.FromAccount.ToString().Substring(11, 2);
            string user_id = Session["UserID"].ToString();
            string req = "";

            string response = myCheque.PayOrder(data.PayCustomerCode, data.PayServiceID, data.PayOrgID, data.AdditionalRefrence, data.sessionid, data.channelCode, data.FeesAmount, data.CustomerFound, data.paycustomername, data.TotalAmount, data.RequiredAmount, account,branch,currency,account_type,"Payment");

            // inserting transaction into trans_log
            datasource.InsertTranLog(user_id, "EPorts Payment", req, response, data.CustomerFound, data.OrderStatus, data.RequiredAmount, "N/A");

            return View();
        }
    }
}