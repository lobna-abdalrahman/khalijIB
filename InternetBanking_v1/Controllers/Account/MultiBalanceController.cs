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
                List<AccountStatementViewModel> accounts = ds.getAccountsArray(user_id);
                JArray accountsarray = new JArray();
                for (int i = 0; i < accounts.Count; i++)
                {
                    JObject singleaccount = new JObject();
                    singleaccount.Add(i.ToString(), accounts[i].AdditionalRefrence);
                    if(accounts[i].AdditionalRefrence.ToString() != "")
                    {
                        accountsarray.Add(singleaccount);
                    }
                }

                MyChequeStatus check = new MyChequeStatus();
               // string res = "{\"tranDateTime\":\"201021015851\",\"CustomerAccounts\":[{\"Account\":\"00301635\",\"IBAN\":\"SD1544020011500001\",\"Currency\":\"938\",\"Balance\":\"6.21968486E7\"},{\"Account\":\"003012987\",\"IBAN\":\"SD8544020011500002\",\"Currency\":\"840\",\"Balance\":\"500\"}],\"uuid\":\"417e7297 - 7e37 - 49e9 - 8618 - f83f7a21b526\",\"errormsg\":\"Secussfully\",\"errorcode\":\"1\"}";
                string res = check.GetMultiBalance(accountsarray);

                JObject jobj = new JObject();
                jobj = JObject.Parse(res);
                dynamic result = jobj;
                int counter = 0;

                int errorcode = result.errorcode;
                if (errorcode == 1)
                {
                    JArray CustomerAccounts = result.CustomerAccounts;
                    foreach (JObject account in CustomerAccounts)
                    {
                        Double balancecontainer = Convert.ToDouble(account.GetValue("Balance").ToString());
                        string formattedbalance = balancecontainer.ToString("C5").Substring(1, balancecontainer.ToString().Length);
                        accas.Add(new AccountSummaryViewModel
                        {
                            AccountNumber = account.GetValue("Account").ToString(),
                            Balance = formattedbalance,
                            IBAN = account.GetValue("IBAN").ToString(),
                            Currency = ds.GetCurrencyName(account.GetValue("Currency").ToString()),
                            AccountType = accounts[counter].AccountType,
                            BranchName = accounts[counter].BranchCode
                        });
                        counter++;
                    }
                }
                else
                {
                    TempData["fail"] = "Cannot connect to server.";
                }

                //string responseStatus = result.responseStatus;
                //string responseMessage = result.responseMessage;
                //string bal = result.bal;
                //string[] separators = {"-"};
                //string value =bal;
                //string[] acc = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                //string[] acc_types = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                //string[] currenciesvalues = currencies.Split(separators, StringSplitOptions.RemoveEmptyEntries);



                // for (var i=0;i<acc.Length;i+=1)
                //{
                //    accas.Add(
                //        new AccountSummaryViewModel
                //        {
                //            AccountNumber = acc[i].ToString(),
                //            //AccountType = ds.getaccounttype(acc[i].Substring(5, 5)),
                //            //BranchName = ds.getbranchnameenglish(acc[i].Substring(2,3)),
                //            Balance = acc[i + 1].ToString()// + " " +  ds.GetCurrencyName(currenciesvalues[i])
                //        });
                //    i = i + 1;
                //}


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

        public ActionResult Details(string id)
        {
            string user_id = Session["UserID"].ToString();
            //int actno = id;
            string fromDate = "";
            string toDate = "";
            int noOfTrans = 20;
            string tranType = "NumberOfTransaction";// ASVM.tranType;
            //String AccountNo = ds.getspfaccount(user_id, actno.ToString());
            List<AccountStatementViewModel> accas = new List<AccountStatementViewModel>();
            List<AccountSummaryViewModel> details = new List<AccountSummaryViewModel>();

            try
            {
                string statement = obj.GetStatement(id, noOfTrans, fromDate, toDate, tranType);
                //string statement = "{\"tranDateTime\":\"131021030931\",\"Statement\":[{\"Amount\":276773.9,\"TranscationDirection\":\"CR\",\"TranscationNarration\":\" - PAYSLIP FOR EXTRA SALARY # 00000001\",\"Date\":\"2021-10-13\",\"BalanceAfterTransaction\":291316.13},{\"Amount\":50,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"Account Transfer-Mobile Transfer  \",\"Date\":\"2021-10-11\",\"BalanceAfterTransaction\":291266.13},{\"Amount\":1,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"Account Transfer-Mobile Transfer  \",\"Date\":\"2021-10-11\",\"BalanceAfterTransaction\":291265.13},{\"Amount\":15000,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\" -09/10/2021--14003099\",\"Date\":\"2021-10-09\",\"BalanceAfterTransaction\":276265.13},{\"Amount\":5,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"F POS Puch-09/10/2021--14003099\",\"Date\":\"2021-10-09\",\"BalanceAfterTransaction\":276260.13},{\"Amount\":500,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\" -09/10/2021--AZCon\",\"Date\":\"2021-10-09\",\"BalanceAfterTransaction\":275760.13},{\"Amount\":5000,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"P2P Debit-07/10/2021--AZCon\",\"Date\":\"2021-10-07\",\"BalanceAfterTransaction\":270760.13},{\"Amount\":1,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"F P2P Deb-07/10/2021--AZCon\",\"Date\":\"2021-10-07\",\"BalanceAfterTransaction\":270759.13},{\"Amount\":40000,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"Cash Withdrawal-Cash Withdrawal  # 3192,?????\",\"Date\":\"2021-10-07\",\"BalanceAfterTransaction\":230759.13},{\"Amount\":110000,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"Account Transfer-Mobile Transfer\",\"Date\":\"2021-10-07\",\"BalanceAfterTransaction\":120759.13},{\"Amount\":20000,\"TranscationDirection\":\"CR\",\"TranscationNarration\":\"P2P Credit-07/10/2021--SYBERCon\",\"Date\":\"2021-10-07\",\"BalanceAfterTransaction\":140759.13},{\"Amount\":100000,\"TranscationDirection\":\"CR\",\"TranscationNarration\":\"P2P Credit-07/10/2021--SYBERCon\",\"Date\":\"2021-10-07\",\"BalanceAfterTransaction\":240759.13},{\"Amount\":40000,\"TranscationDirection\":\"CR\",\"TranscationNarration\":\"P2P Credit-07/10/2021--23000318\",\"Date\":\"2021-10-07\",\"BalanceAfterTransaction\":280759.13},{\"Amount\":1500,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\" -06/10/2021--AZCon\",\"Date\":\"2021-10-06\",\"BalanceAfterTransaction\":279259.13},{\"Amount\":5,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\" -06/10/2021--AZCon\",\"Date\":\"2021-10-06\",\"BalanceAfterTransaction\":279254.13},{\"Amount\":10,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"Account Transfer-Mobile Transfer\",\"Date\":\"2021-10-04\",\"BalanceAfterTransaction\":279244.13},{\"Amount\":1,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"F P2P Deb-03/10/2021--06000234\",\"Date\":\"2021-10-03\",\"BalanceAfterTransaction\":279243.13},{\"Amount\":2370,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"P2P Debit-03/10/2021--06000234\",\"Date\":\"2021-10-03\",\"BalanceAfterTransaction\":276873.13},{\"Amount\":10,\"TranscationDirection\":\"CR\",\"TranscationNarration\":\"Account Transfer-Mobile Transfer\",\"Date\":\"2021-10-03\",\"BalanceAfterTransaction\":276883.13},{\"Amount\":20,\"TranscationDirection\":\"DR\",\"TranscationNarration\":\"Account Transfer-Mobile Transfer\",\"Date\":\"2021-10-03\",\"BalanceAfterTransaction\":276863.13}],\"uuid\":\"a2b8d083-4366-4513-95a0-512ad887576d\",\"errormsg\":\"Secussfully\",\"errorcode\":\"1\"}";
                JObject response = new JObject();

                response = JObject.Parse(statement);
                JArray statementitems = (JArray)response.GetValue("Statement");
                int stid = 1;
                string amount = "", TranscationDirection = "N/A", TranscationNarration = "N/A", Date = "N/A", BalanceAfterTransaction = "N/A";

                foreach (JObject item in statementitems)
                {
                    if(item.GetValue("Amount") != null)
                    {
                        amount = item.GetValue("Amount").ToString();
                        decimal amountcontainer = Convert.ToDecimal(amount);
                        amount = String.Format("{0:N}", amountcontainer); //.Substring(1, amountcontainer.ToString().Length);
                    }
                    if (item.GetValue("TranscationDirection") != null)
                    {
                        TranscationDirection = item.GetValue("TranscationDirection").ToString();
                    }
                    if (item.GetValue("TranscationNarration") != null)
                    {
                        TranscationNarration = item.GetValue("TranscationNarration").ToString();
                    }
                    if (item.GetValue("Date") != null)
                    {
                        Date = item.GetValue("Date").ToString();
                    }
                    if (item.GetValue("BalanceAfterTransaction") != null)
                    {
                        BalanceAfterTransaction = item.GetValue("BalanceAfterTransaction").ToString();
                        Double BalanceAfterTransactioncontainer = Convert.ToDouble(BalanceAfterTransaction);
                        if (TranscationDirection == "CR")
                        {
                            BalanceAfterTransactioncontainer = BalanceAfterTransactioncontainer * -1;
                        }
                        BalanceAfterTransaction = String.Format("{0:N}", BalanceAfterTransactioncontainer); //BalanceAfterTransactioncontainer.ToString("C2");//.Substring(1, BalanceAfterTransactioncontainer.ToString().Length);
                    }


                    details.Add(new AccountSummaryViewModel
                    {
                        StateID = stid,
                        StateAmount = amount,
                        TranscationDirection = TranscationDirection,
                        TranscationNarration = TranscationNarration,
                        Date = Date,
                        BalanceAfterTransaction = BalanceAfterTransaction //+ " SDG" //+ ds.GetCurrencyName(AccountNo.Substring(10,3).ToString())

                    });

                    accas.Add(new AccountStatementViewModel
                    {
                        StateID = stid,
                        StateAmount = amount,
                        TranscationDirection = TranscationDirection,
                        TranscationNarration = TranscationNarration,
                        Date = Date,
                        BalanceAfterTransaction = BalanceAfterTransaction //+ " " + ds.GetCurrencyName(AccountNo.ToString())

                    });

                    stid++;

                    //accas = (List<AccountStatementViewModel>)Session["myStatement"];

                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);

            }
            return View(details);
        }


    }
}
