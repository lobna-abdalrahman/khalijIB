using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Helper;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;
using Rotativa;

namespace InternetBanking_v1.Controllers
{
    public class ToBankCustomerTransferController : BaseController
    {
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        ToBanCustomerViewModel model = new ToBanCustomerViewModel();
        //
        // GET: /ToBankCustomerTransfer/
        public ActionResult ToBankCustomerTransfer()
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

            //------------------Get Accounts----------------
            List<OwnTransferViewModel> pcontent = new List<OwnTransferViewModel>();
            {
                pcontent = data.DropFromOwnTransferClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt32(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.FromAccount.ToString()
                });
            }

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
                accTypes = data.GetAccountType();
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
                currency = data.GetSDGCurrency();
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


            ViewBag.clientList = AccountList;
            ViewBag.branchList = branchList;
            ViewBag.AccTypesList = AccTypeList;
            ViewBag.CurrencyList = CurrencyList;

            return View();
        }


        [HttpPost]
        public ActionResult ToBankCustomerTransfer(ToBanCustomerViewModel model)
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

            //------------------Get Accounts----------------
            List<OwnTransferViewModel> pcontent = new List<OwnTransferViewModel>();
            {
                pcontent = data.DropFromOwnTransferClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt32(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.FromAccount.ToString()
                });
            }

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
                accTypes = data.GetAccountType();
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
                currency = data.GetSDGCurrency();
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

            ViewBag.clientList = AccountList;
            ViewBag.branchList = branchList;
            ViewBag.AccTypesList = AccTypeList;
            ViewBag.CurrencyList = CurrencyList;

            if (ModelState.IsValid)
            {

                string tel = Session["UserTel"].ToString();
                //----------------Doing the Service--------------------
                //Get Selected Account...
                model.FromAccount = Request["FromAccount"];
                model.BranchCode = Request["BranchList"];
                model.AccountTypeCode = Request["AccTypeList"];
                model.CurrencyCode = Request["CurrencyList"];
                string toAccount = "";

                toAccount = "99" + model.BranchCode + model.AccountTypeCode + model.CurrencyCode + model.ToAccount;

                //TODO Add The ReceiverName Retreival HERE ..
                try
                {
                    string res = myCheque.GetReceiverName(toAccount);
                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    string responseStatus = result.responseStatus;
                    string responseMessage = result.responseMessage;
                    string bal = result.result;
                    string[] separators = { ",", ":" };
                    string value = bal;
                    string[] acc = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    String custname;
                    String custID;
                    String custphone;

                    if (acc.Length == 6)
                    {
                        custID = acc[1].ToString();
                        custname = acc[3].ToString();

                        custphone = acc[5].ToString();
                        Session["custID"] = custID;
                        Session["custname"] = custname;
                        Session["custphone"] = custphone;
                        //Session["custcat"] = model.CategoryCode;


                        //return RedirectToAction("custinfo");
                    }

                    else
                    {
                        message = "Please check customer information something wrong ";
                        ModelState.AddModelError("", message);
                        return View(model);
                    }


                }
                catch (Exception e)
                {
                    // Console.WriteLine(e);
                    ModelState.AddModelError("", e.Message);
                }
                

                Random rng = new Random();


                string OtpCharacters = OTPGenerate.OTPCharacters();
                OTPGenerate.getsmsconfig();
                //Createing More Secure OTP Password by Using MD5 algorithm
                string OTPPassword = OTPGenerate.OTPGenerator(OtpCharacters, rng.Next(10).ToString());
                String Out = HTTP_GET(OTPGenerate.smspath, "username=" + OTPGenerate.smsusername + "&password=" + OTPGenerate.smspassword + "&to=" + tel + "&text=Your Verification Code " + OTPPassword + " ,Thanks For Making Us Your Choice");
                if (Out.Equals("<h3>OP is: 24990</h3>") || Out.Equals("<h3>OP is: 24991</h3>") || Out.Equals("<h3>OP is: 24992</h3>") || Out.Equals("<h3>OP is: 24993</h3>") || Out.Equals("<h3>OP is: 24996</h3>") || Out.Equals("<h3>OP is: 24999</h3>") || Out.Equals("<h3>OP is: 24910</h3>") || Out.Equals("<h3>OP is: 24911</h3>") || Out.Equals("<h3>OP is: 24912</h3>"))
                {
                    Session["bnkcusttryno"] = 0;
                    Session["tobankOTPPassword"] = OTPPassword;
                    Session["fromAccount"] = model.FromAccount;
                    Session["toAccount"] = toAccount;
                    Session["Amount"] = model.Amount;
                    return RedirectToAction("Verify", model);
                }
                else
                {
                   
                    message = "Connect to SMS Server Problem ";
                    ModelState.AddModelError("", "Sorry Please Try again later ...." + message);
                    return View();
                }
            }

            else
            {
                message = "All Fields are required ";
                ModelState.AddModelError("", "Something is missing" + message);
                return View();
            }

        }




        public ActionResult Verify()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            
            
                
            //info about Transfer
            var FromAcc = Session["fromAccount"].ToString();
            var ToAcc = Session["toAccount"].ToString();

            var FromAccountNumber = Convert.ToInt32(FromAcc.Substring(13));
            var FromAccountType = data.getaccounttype(FromAcc.ToString().Substring(5, 5));
            var FromBranchName = data.getbranchnameenglish(FromAcc.ToString().Substring(2, 3));

            var ToAccountNumber = Convert.ToInt32(ToAcc.Substring(13));
            var ToAccountType = data.getaccounttype(ToAcc.ToString().Substring(5, 5));
            var ToBranchName = data.getbranchnameenglish(ToAcc.ToString().Substring(2, 3));

            ViewBag.FromAccount = FromBranchName + "-" + FromAccountType + "-" + FromAccountNumber;
            ViewBag.ToAccount = ToBranchName + "-" + ToAccountType + "-" +ToAccountNumber ;
            ViewBag.Amount = Session["Amount"].ToString();
            ViewBag.ReceiverName = Session["custname"].ToString();
            return View();

        }
        [HttpPost]
        public ActionResult Verify(ToBanCustomerViewModel  model  )
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //info about Transfer
            var FromAcc = Session["fromAccount"].ToString();
            var ToAcc = Session["toAccount"].ToString();

            var FromAccountNumber = Convert.ToInt32(FromAcc.Substring(13));
            var FromAccountType = data.getaccounttype(FromAcc.ToString().Substring(5, 5));
            var FromBranchName = data.getbranchnameenglish(FromAcc.ToString().Substring(2, 3));

            var ToAccountNumber = Convert.ToInt32(ToAcc.Substring(13));
            var ToAccountType = data.getaccounttype(ToAcc.ToString().Substring(5, 5));
            var ToBranchName = data.getbranchnameenglish(ToAcc.ToString().Substring(2, 3));

            ViewBag.FromAccount = FromBranchName + "-" + FromAccountType + "-" + FromAccountNumber;
            ViewBag.ToAccount = ToBranchName + "-" + ToAccountType + "-" + ToAccountNumber;       
            ViewBag.Amount = Session["Amount"].ToString();
            ViewBag.ReceiverName = Session["custname"].ToString();

            int tryno = Convert.ToInt32(Session["bnkcusttryno"].ToString());

            string user_id = Session["UserID"].ToString();
             if (tryno<3){
            string vCode = model.VerificatioCode;
            String OTPPassword = Session["tobankOTPPassword"].ToString();
            if (vCode.Equals(OTPPassword))
            {
               String toAccount= Session["toAccount"].ToString() ;
                try
                {
                    TempData["btnclick"] = true;
                    ViewBag.btnclick = "true";
                    string res = myCheque.DoAccountTransfer(model.FromAccount, toAccount, model.Amount);
                    //string res = "{\"receivername\":\"HUSAM ALDIEN HAFIZ ABDALRHHMEAN MOHAMED\",\"sendername\":\"HUSAM ALDIEN HAFIZ ABDALRHHMEAN MOHAMED\",\"tranDateTime\":\"141018124523\",\"uuid\":\"ca88edf2-4f85-4a3b-98a7-70c9f1711880\",\"errormsg\":\"Secussfully\",\"errorcode\":\"0\",\"status\":\"The transaction Faild, OutUtiNumber = 201800464499\"}";
                    
                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic result = jobj;

                    var errorCode = result.errorcode;
                    var errormsg = result.errormsg;
                    var tranRes = result.status;
                    var senderName = "";
                    var recieverName = "";
                    

                    string responseStatus = result.status;
                    string responseMessage = result.responseMessage;


                    Session["TranResponse"] = false;
                    Session["ResponseStat"] = "";
                    Session["ResponseMSG"] = "";

                    if (errorCode != null && !errorCode.ToString().Equals("0"))
                    {
                        senderName = result.sendername;
                        recieverName = result.receivername;
                        Session["TranResponse"] = true;
                        Session["ResponseStat"] = errormsg;
                        Session["ResponseMSG"] = responseStatus;

                        TempData["Success"] = false;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = errormsg;
                        ViewBag.ResponseMSG = responseStatus;

                        //insert into TranLog
                        string req = model.FromAccount + "," + toAccount + "," + model.Amount;
                        //data.InsertTranLog(user_id, "To Bank Customer Transfer", req, res, errormsg.ToString(), tranRes.ToString(), model.Amount, responseMessage);
                        data.InsertTransferToTranLog(user_id, "To Bank Customer Transfer", req, res, errormsg.ToString(),
                            tranRes.ToString(), model.Amount, responseStatus, senderName.ToString(), recieverName.ToString());

                    }
                    else
                    {
                        Session["TranResponse"] = true;
                        Session["ResponseStat"] = "Not Successful";
                        Session["ResponseMSG"] = responseStatus;

                        TempData["Success"] = false;
                        //ModelState.AddModelError("", tranRes.ToString());
                        ViewBag.ResponseStat = "Not Successful";
                        ViewBag.ResponseMSG = responseStatus;
                        senderName = result.sendername;
                        recieverName = result.receivername;
                        //insert into TranLog
                        string req = model.FromAccount + "," + toAccount + "," + model.Amount;
                        data.InsertTranLog(user_id, "To Bank Customer Transfer", req, res, errormsg.ToString(), tranRes.ToString(),model.Amount,"Timed Out");
                        data.InsertTransferToTranLog(user_id, "To Bank Customer Transfer", req, res, errormsg.ToString(),
                            tranRes.ToString(), model.Amount, responseStatus, senderName.ToString(), recieverName.ToString());

                    }

                    Session["Sender"] = senderName;
                    Session["Reciever"] = recieverName;
                    Session["TranAmount"] = model.Amount+"SDG";

                    if (CultureHelper.IsRighToLeft())
                    {
                        return RedirectToAction("Reciept",  new { lang = "ar" });
                    }
                    else
                    {
                        return RedirectToAction("Reciept");
                    }



                }
                catch (Exception e)
                {
                   // Console.WriteLine(e);
                    ModelState.AddModelError("", e.Message);

                    string req = model.FromAccount + model.ToAccount + model.Amount;
                    data.InsertTranLog(user_id, "To Bank Customer Transfer", req, "", "failed", "",model.Amount,"Timed Out");
                }
            
            }
            else
            {
                tryno = tryno + 1;
                Session["bnkcusttryno"] = tryno;
                ModelState.AddModelError("","Please Enter Valid Verfication Code..!");
            }

        }
        else
        {
                 
            ModelState.AddModelError("", "Please try again later..!");
        }

            return View();

        }


        public ActionResult Reciept()
        {

            ToBanCustomerViewModel model = new ToBanCustomerViewModel();
            Session["Senderr"] = Session["Sender"].ToString();
            Session["Recieverr"] = Session["Reciever"].ToString();
            Session["TranAmountt"] = Session["TranAmount"].ToString();
            Session["ResponseMSGG"] = Session["ResponseMSG"].ToString();

            model.SenderName = Session["Senderr"].ToString();
            model.RecieverName = Session["Recieverr"].ToString();
            model.Amount = Session["TranAmountt"].ToString();
            model.TranReference = Session["ResponseMSGG"].ToString();
            Session["TranResponse"] = false;
            Session["ResponseStat"] = "";
            Session["ResponseMSG"] = "";
            TempData["Success"] = "";

            return View(model);
        }

        public ActionResult PrintSalarySlip()
        {
            /*var report = new ActionAsPdf("Reciept");*/
            var report = new ViewAsPdf("Reciept");
            return report;
        }

        public static string HTTP_GET(string Url, string Data)
        {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url + (string.IsNullOrEmpty(Data) ? "" : "?" + Data));
            try
            {
                System.Net.WebResponse resp = req.GetResponse();
                using (System.IO.Stream stream = resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        Out = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }
	
    }
}