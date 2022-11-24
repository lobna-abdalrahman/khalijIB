using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using InternetBanking_v1.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers
{

    public class InvestmentController : BaseController
    {
        DataSource data = new DataSource();
        MyChequeStatus obj = new MyChequeStatus();
        InvestmentViewModel model = new InvestmentViewModel();

        private double total = 0;
        private double paid = 0;
        private double nopiad = 0;

        //
        // GET: /Investment/
        public ActionResult Investment()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            InvestmentViewModel  model = new InvestmentViewModel();
            string user_id = Session["UserID"].ToString();
            //model.AccountNo = dataObj.populateAccounts(user_id);
            List<AccountStatementViewModel> pcontent = new List<AccountStatementViewModel>();
            {
                pcontent = data.DropStatementClient(user_id);
            };
            //--------get branchList----------
            List<ToBanCustomerViewModel> branchs = new List<ToBanCustomerViewModel>();
            {
                branchs = data.GetBranchs();
            };

            List<SelectListItem> branchList = new List<SelectListItem>();
            foreach (var item in branchs)
            {
                branchList.Add(new SelectListItem
                {
                    Text = item.BranchName,
                    Value = item.BranchCode.ToString()
                });
            }

            //-------------------Get Account Types-------------
            List<ToBanCustomerViewModel> accTypes = new List<ToBanCustomerViewModel>();
            {
                accTypes = data.InvGetAccountType();
            };

            List<SelectListItem> AccTypeList = new List<SelectListItem>();
            foreach (var item in accTypes)
            {
                AccTypeList.Add(new SelectListItem
                {
                    Text = item.AccountTypeName,
                    Value = item.AccountTypeCode.ToString()
                });
            }

            //----------------------Get Currency----------------
            List<ToBanCustomerViewModel> currency = new List<ToBanCustomerViewModel>();
            {
                currency = data.GetCurrency();
            };

            List<SelectListItem> CurrencyList = new List<SelectListItem>();
            foreach (var item in currency)
            {
                CurrencyList.Add(new SelectListItem
                {
                    Text = item.CurrencyName,
                    Value = item.CurrencyCode.ToString()
                });
            }

            ViewBag.branchList = branchList;
            ViewBag.AccTypesList = AccTypeList;
            ViewBag.CurrencyList = CurrencyList;


            return View();

        }

        [HttpPost]
        public ActionResult Investment(InvestmentViewModel ASVM)
        {
            List<InvestmentViewModel> accas = new List<InvestmentViewModel>();
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                string user_id = Session["UserID"].ToString();

                //--------get branchList----------
                List<ToBanCustomerViewModel> branchs = new List<ToBanCustomerViewModel>();
                {
                    branchs = data.GetBranchs();
                };

                List<SelectListItem> branchList = new List<SelectListItem>();
                foreach (var item in branchs)
                {
                    branchList.Add(new SelectListItem
                    {
                        Text = item.BranchName,
                        Value = item.BranchCode.ToString()
                    });
                }

                //-------------------Get Account Types-------------
                List<ToBanCustomerViewModel> accTypes = new List<ToBanCustomerViewModel>();
                {
                    accTypes = data.InvGetAccountType();
                };

                List<SelectListItem> AccTypeList = new List<SelectListItem>();
                foreach (var item in accTypes)
                {
                    AccTypeList.Add(new SelectListItem
                    {
                        Text = item.AccountTypeName,
                        Value = item.AccountTypeCode.ToString()
                    });
                }

                //----------------------Get Currency----------------
                List<ToBanCustomerViewModel> currency = new List<ToBanCustomerViewModel>();
                {
                    currency = data.GetCurrency();
                };

                List<SelectListItem> CurrencyList = new List<SelectListItem>();
                foreach (var item in currency)
                {
                    CurrencyList.Add(new SelectListItem
                    {
                        Text = item.CurrencyName,
                        Value = item.CurrencyCode.ToString()
                    });
                }

                ViewBag.branchList = branchList;
                ViewBag.AccTypesList = AccTypeList;
                ViewBag.CurrencyList = CurrencyList;

                model.BranchCode = Request["BranchList"];
                model.AccountTypeCode = Request["AccTypeList"];
                model.CurrencyCode = Request["CurrencyList"];
                string wholeAccount = "";
                wholeAccount = "99" + model.BranchCode + model.AccountTypeCode + model.CurrencyCode + ASVM.AccountNumber;
              
 

                try
                {
                    string statement = obj.GetInvestmentInstalment(wholeAccount);
                    //string statement = obj.GetInvestmentInstalment(ASVM.AccountNumber);
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
                    string[] sep = { "{", "}" };

                    string[] stat = mystatement.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                    //-------
                    if (index > 0)
                    {
                        for (int i = 1; i <= index; i++)
                        {


                            string singlerow = stat[i];
                            singlerow = "{" + singlerow + "}";

                            JObject newObj = new JObject();
                            newObj = JObject.Parse(singlerow);
                            dynamic singleObj = newObj;

                            string CHEQ_C_NO = singleObj.CHEQ_C_NO;
                            string CHEQ_D_DATE = singleObj.CHEQ_D_DATE;
                            string CHEQ_C_STS = singleObj.CHEQ_C_STS;
                            string CHEQ_F_AMOUNT = singleObj.CHEQ_F_AMOUNT;

                            total = total + Convert.ToDouble(CHEQ_F_AMOUNT);
                            if (CHEQ_C_STS.Equals("Not Paid"))
                            {
                                nopiad = nopiad + Convert.ToDouble(CHEQ_F_AMOUNT);
                            }
                            if (CHEQ_C_STS.Equals("Paid"))
                            {
                                paid  = paid + Convert.ToDouble(CHEQ_F_AMOUNT);
                            }
                            accas.Add(new InvestmentViewModel
                            {
                                CHEQ_C_NO = CHEQ_C_NO,
                                CHEQ_D_DATE = CHEQ_D_DATE,
                                CHEQ_C_STS = CHEQ_C_STS,
                                CHEQ_F_AMOUNT = CHEQ_F_AMOUNT + " SDG"

                            });

                            i = i + 1;

                        }
                        /*accas.Add(new InvestmentViewModel
                        {
                            CHEQ_C_NO ="Total Amount = "+ total.ToString() + " SDG",
                            CHEQ_D_DATE ="Paid Amount = "+ paid.ToString() + " SDG",
                            CHEQ_C_STS = "Not Paid Amount = " + nopiad.ToString() + " SDG",
                            CHEQ_F_AMOUNT =   " "

                        });*/

                        Session["MyTotal"] = total.ToString() + " SDG";
                        Session["MyPaid"] =  paid.ToString() + " SDG";
                        Session["MyNotPaid"] = nopiad.ToString() + " SDG";

                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);

                }

            }
            else
            {
                message = "All Fields are required ";
                ModelState.AddModelError("", "Something is missing");
            }

            //TempData["Statement"] = accas;
            Session["Investment"] = accas;
            string acas = accas.ToString();

            if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
            {
                return RedirectToAction("ViewInvestment", new { lang = "ar" });
            }
            else
            {
                return RedirectToAction("ViewInvestment", accas);
            }
           
            //return View();
        }
        public ActionResult ViewInvestment()
        {
            ViewBag.MyTotal = Session["MyTotal"].ToString();
            ViewBag.MyPaid = Session["MyPaid"].ToString();
            ViewBag.MyNotPaid = Session["MyNotPaid"].ToString();
            Session["MyTotal"] = "";
            Session["MyPaid"] = "";
            Session["MyNotPaid"] = "";

            List<InvestmentViewModel> accas = new List<InvestmentViewModel>();
            accas = (List<InvestmentViewModel>)Session["Investment"]; //TempData["Statement"];

            //accas.Add(new AccountStatementViewModel
            //{
            //    StateAmount = 

            //});
            return View(accas);
        }

    }
}