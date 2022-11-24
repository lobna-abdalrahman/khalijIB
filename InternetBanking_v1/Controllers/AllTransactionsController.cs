using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers
{
    public class AllTransactionsController : Controller
    {
        DataSource ds = new DataSource();

        public ActionResult MyTransactions()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string user_id = Session["UserID"].ToString();
            List<AllTransfersViewModel> trans = new List<AllTransfersViewModel>();
            trans = ds.getAllTransactions(user_id);
            return View(trans);
        }

        public ActionResult TransactionDetails(string id)
        {
            ReceiptModel receipt = new ReceiptModel();
            JObject jobj = new JObject();
            JObject electricitybillinfo = new JObject();
            CustomerTransferReportViewModel transaction = ds.GetTransactionDetails(id);

            //extracting transaction details
            string[] words = transaction.TranFullReq.ToString().Split(',');
            string fromaccount = words[0];
            receipt.fromaccount = words[0];
            receipt.frombranch = ds.getbranchnameenglish(fromaccount.Substring(2, 3));
            receipt.fromaccounttype = ds.getaccounttype(fromaccount.Substring(5, 5));
            if (!String.IsNullOrEmpty(transaction.TranFullResp))
            {
                dynamic responsedata = JObject.Parse(transaction.TranFullResp);
                jobj = JObject.Parse(transaction.TranFullResp);
                receipt.getbilldata(jobj);
                receipt.getResponseval(jobj);
                if (responsedata.receivername != null)
                {
                    receipt.customerName = responsedata.sendername;
                    receipt.ReciverName = responsedata.recivername;
                }

                if (responsedata.status != null)
                {
                    string tempstatus = responsedata.status;
                    string[] words3 = tempstatus.Split(',');
                    receipt.Transrefrence = words3[1];
                    receipt.ReciverName = responsedata.recivername;
                }

                string transdatetime = receipt.TransactionDateTime;
                string expDate = receipt.expDate;
                string day = transdatetime.Substring(0, 2);
                string month = transdatetime.Substring(2, 2);
                string year = "20" + transdatetime.Substring(4, 2);
                string hour = transdatetime.Substring(6, 2);
                string minute = transdatetime.Substring(8, 2);
                string seconds = transdatetime.Substring(10, 2);
                receipt.Date = day + "/" + month + "/" + year;
                receipt.Time = hour + ":" + minute + ":" + seconds;

                receipt.customerName = transaction.CustomerName;
                receipt.ServiceName = transaction.TranName;
                receipt.responseStatus = transaction.TranResult;
                receipt.Amount = transaction.TranAmount;
            }

            return View(receipt);
        }
    }
}