using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models.Account;
using Newtonsoft.Json.Linq;
using Oracle.DataAccess.Client;
using OracleCommand = System.Data.OracleClient.OracleCommand;
using OracleDataReader = System.Data.OracleClient.OracleDataReader;

namespace InternetBanking_v1.Controllers.Account.ChequeStatus
{
    public class ChequeStatusController : BaseController//Controller
    {
        //ConnectionString....
        private string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //LoginLogic dataObj = new LoginLogic();

        DataSource data = new DataSource();

        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /ChequeStatus/
        public ActionResult ChequeStatus()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            MyAccounts model = new MyAccounts();
            string user_id = Session["UserID"].ToString();

            List<MyAccounts> pcontent = new List<MyAccounts>();
            {
                pcontent = data.DropClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.AccountNumber.Substring(13));
                var AccountType = data.getaccounttype(item.AccountNumber.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.AccountNumber.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.AccountNumber.ToString()
                });
            }

            ViewBag.clientList = AccountList;

            return View();
        }

        [HttpPost]
        public ActionResult ChequeStatus(MyAccounts myAccounts)
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {

                 string user_id = Session["UserID"].ToString();
                //myAccounts.AccountNumbers = dataObj.populateAccounts(user_id);

                //var selectedItem = myAccounts.AccountNumbers.Find(p => p.Value == myAccounts.AccountNumbers.ToString());

                //if (selectedItem != null)
                //{
                //    selectedItem.Selected = true;
                    //string status = myCheque.getChequeStatus(selectedItem, myAccounts.ChequeNumber);

                List<MyAccounts> pcontent = new List<MyAccounts>();
                {
                    pcontent = data.DropClient(user_id);
                };
                List<SelectListItem> AccountList = new List<SelectListItem>();
                foreach (var item in pcontent)
                {
                    var AccountNumber = Convert.ToInt64(item.AccountNumber.Substring(13));
                    var AccountType = data.getaccounttype(item.AccountNumber.ToString().Substring(5, 5));
                    var BranchName = data.getbranchnameenglish(item.AccountNumber.ToString().Substring(2, 3));

                    AccountList.Add(new SelectListItem
                    {
                        Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                        Value = item.AccountNumber.ToString()
                    });
                }

                ViewBag.clientList = AccountList;

                //Get Selected Account...
                myAccounts.AccountNumber = Request["Clients"];

                try
                {
                    String chqsts = "";
                    String chqfro, chqto;
                    chqfro = myAccounts.ChequeNumber1;
                    chqto = myAccounts.ChequeNumber2;

                    List<MyAccounts> accas = new List<MyAccounts>();
           
                    if (!chqfro.Equals(chqto))
                    {
                        long range = Convert.ToInt64(chqto) - Convert.ToInt64(chqfro);
                        if (range > 0)
                        {

                            for (long index = Convert.ToInt64(chqfro); index <=  Convert.ToInt64(chqto); index++)
                            {
                               
                                string status = myCheque.getChequeStatus(myAccounts.AccountNumber, index.ToString());
                                JObject jobj = new JObject();
                                jobj = JObject.Parse(status);
                                dynamic result = jobj;

                                var errorCode = result.errorcode;
                                var errormsg = result.errormsg;
                                var chequeStatus = result.ChequeStatus;
                                var ChequeDate = result.ChequeDate;
                                if (errorCode == 1)
                                {
                                    chqsts=chequeStatus.ToString();
                                    //return View(chequeStatus);
                                }
                                else
                                {
                                    chqsts="No status found ..";
                                }
                                accas.Add(new MyAccounts
                                {
                                    CheqNum = index.ToString(),
                                    Status = chqsts,
                                    Date = ChequeDate
                                });
                               // index = index + 1;
                            }
                            Session["myCheques"] = accas;
                            if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
                            {
                                return RedirectToAction("ViewCheque",new{lang = "ar"});
                            }
                            else
                            {
                                return RedirectToAction("ViewCheque");
                            }
                            
                        }
                        else
                        {
                            ModelState.AddModelError("", GlobalRes.ChequeRangeError);
                        }
                    }
                    else
                    {

                       
                    string status = myCheque.getChequeStatus(myAccounts.AccountNumber, myAccounts.ChequeNumber1);
                    JObject jobj = new JObject();
                    jobj = JObject.Parse(status);
                    dynamic result = jobj;

                    var errorCode = result.errorcode;
                    var errormsg = result.errormsg;
                    var chequeStatus = result.ChequeStatus;
                        var ChequeDate = result.ChequeDate;
                    if (errorCode==1)
                    {
                      //  ModelState.AddModelError("", chequeStatus.ToString());
                          chqsts = chequeStatus.ToString();
                        //return View(chequeStatus);
                    }
                    else
                    {
                        //ModelState.AddModelError("", "No status found ..");
                        chqsts = "No status found ..";
                    }
                        accas.Add(new MyAccounts
                        {
                            CheqNum = myAccounts.ChequeNumber1,
                            Status = chqsts,
                            Date = ChequeDate
                        });
                        Session["myCheques"] = accas;
                        if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
                        {
                            return RedirectToAction("ViewCheque", new { lang = "ar" });
                        }
                        else
                        {
                            return RedirectToAction("ViewCheque");
                        }
                        /*return RedirectToAction("ViewCheque");*/
                    }
                  
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    
                }               


            }
            else
            {
                message = "All Fields are required ";
                ModelState.AddModelError("", "Something is missing");
                
            }

            return View();
            

        }


        public ActionResult ViewCheque()
        {
            List<MyAccounts> accas = new List<MyAccounts>();
            accas = (List<MyAccounts>)Session["myCheques"];

            return View(accas);

        }

       


	}
}