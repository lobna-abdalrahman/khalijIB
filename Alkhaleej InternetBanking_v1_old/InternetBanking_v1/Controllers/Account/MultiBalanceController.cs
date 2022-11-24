using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using InternetBanking_v1.Models.Account;
using InternetBanking_v1.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers.Account
{
    public class MultiBalanceController : BaseController //Controller
    {
        AccountStatementViewModel model = new AccountStatementViewModel();
        AccountSummaryViewModel multi = new AccountSummaryViewModel();
        DataSource ds = new DataSource();
        MyChequeStatus obj = new MyChequeStatus();
         

        //
        // GET: /MultiBalance/
        public ActionResult Accountsummary()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            List<AccountSummaryViewModel> accas = new List<AccountSummaryViewModel>();
            string user_id = Session["UserID"].ToString();
            try
            {
                string accounts = ds.getaccount(user_id);

                MyChequeStatus check = new MyChequeStatus();
                string res = check.GetMultiBalance(accounts);

                JObject jobj = new JObject();
                jobj = JObject.Parse(res);
                dynamic result = jobj;

                string responseStatus = result.responseStatus;
                string responseMessage = result.responseMessage;
                string bal = result.bal;
                string[] separators = { "-"};
                string value =bal;
                string[] acc = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);

              
                  
               for(var i=0;i<acc.Length;i+=1)
               {
                   accas.Add(
                       new AccountSummaryViewModel
                       {
                           AccountNumber = Convert.ToInt32( acc[i].ToString().Substring(13)).ToString(),
                           AccountType = ds.getaccounttype(acc[i].Substring(5, 5)),
                           BranchName = ds.getbranchnameenglish(acc[i].Substring(2,3)),
                           Balance = acc[i + 1].ToString() + " " + ds.GetCurrencyName(acc[i].Substring(10, 3).ToString())
                       });
                   i = i + 1;
               }

               
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }

            return View(accas);
        }

        //[HttpPost]
        //public ActionResult Accountsummary(AccountSummaryViewModel accSummary)
        //{
        //    string user_id = Session["UserID"].ToString();

        //    return View();
        //}

        public ActionResult Details(int id)
        {
            string user_id = Session["UserID"].ToString();
            int actno = id;
            string fromDate = "";
            string toDate = "";
            int noOfTrans = 20;
            string tranType = "NumberOfTransaction";// ASVM.tranType;
            String AccountNo = ds.getspfaccount(user_id, actno.ToString());
            List<AccountStatementViewModel> accas = new List<AccountStatementViewModel>();
            List<AccountSummaryViewModel> details = new List<AccountSummaryViewModel>();
             

             
           
            try
            {
                string statement = obj.GetStatement( AccountNo, noOfTrans, fromDate, toDate, tranType);
                JObject jobj = new JObject();

                //CreateReport(ASVM.fromDate,ASVM.toDate);
                //Response.Redirect("~/ShowReports.aspx");
                jobj = JObject.Parse(statement);
                dynamic result = jobj;

                //
                string mystatement = result.Statement;
                JObject statObj = new JObject();
                statObj = JObject.Parse(mystatement);
                dynamic statementObj = statObj;

                int index = statementObj.NoOftran;
                //string[] sep = { "{", "}" };

                //string[] stat = mystatement.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                //-------
                if (index > 0)
                {
                    for (int i = 1; i <= index; i++)
                    {


                        //string singlerow = stat[i];
                        //singlerow = "{" + singlerow + "}";
                        JToken singlerow = statementObj[i.ToString()];
                        JObject newObj = new JObject();
                      //  newObj = JObject.Parse(singlerow);
                    //    dynamic singleObj = newObj;
                        dynamic singleObj = singlerow;
                        string Amount = singleObj.Amount;
                        string TranscationDirection = singleObj.TranscationDirection;
                        string TranscationNarration = singleObj.TranscationNarration;
                        string Date = singleObj.Date;
                        string BalanceAfterTransaction = singleObj.BalanceAfterTransaction;
                        int stid = i;
                        details.Add(new AccountSummaryViewModel
                        {
                            StateID = stid,
                            StateAmount = Amount,
                            TranscationDirection = TranscationDirection,
                            TranscationNarration = TranscationNarration,
                            Date = Date,
                            BalanceAfterTransaction = BalanceAfterTransaction + " "+ds.GetCurrencyName(AccountNo.Substring(10,3).ToString())
                            
                        });

                        accas.Add(new AccountStatementViewModel
                        {
                            StateID = stid,
                            StateAmount = Amount,
                            TranscationDirection = TranscationDirection,
                            TranscationNarration = TranscationNarration,
                            Date = Date,
                            BalanceAfterTransaction = BalanceAfterTransaction + " " + ds.GetCurrencyName(AccountNo.Substring(10, 3).ToString())

                        });

                      //  i = i + 1;

                    }

                    
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);

            }
            accas = (List<AccountStatementViewModel>)Session["myStatement"]; 
            return View(details);
        }


    }
}
