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
                pcontent = data.DropFromOwnTransferClientSdgOnly(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.fromAccountType);
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + item.Additional_Reference,
                    Value = item.Additional_Reference + "-" + item.fromCurrencyCode
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
                pcontent = data.DropFromOwnTransferClientSdgOnly(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + item.Additional_Reference,
                    Value = item.Additional_Reference + "-" + item.fromCurrencyCode
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
                string[] words = model.FromAccount.Split('-');
                model.FromAccount = words[0];
                model.fromCurrencyCode = words[1];
                model.BranchCode = Request["BranchList"];
                model.AccountTypeCode = Request["AccTypeList"];
                model.toCurrencyCode = Request["CurrencyList"];
                string toAccount = "";

                toAccount = model.ToAccount;//"99" + model.BranchCode + model.AccountTypeCode + model.CurrencyCode + model.ToAccount;

                //TODO Add The ReceiverName Retreival HERE ..
                try
                {
                   // string res = "{\"NoOfAct\":4,\"Accounts\":[{\"ACT_C_NAME\":\"HUSAM\",\"CURRENCY_C_CODE\":\"USD\",\"CUST_I_NO\":\"004031852\",\"MOBILE_C_NO\":\"\",\"BRANCH_C_CODE\":\"AfricaStreet\",\"ACT_C_TYPE\":\"213104\"},{\"ACT_C_NAME\":\"HUSAM\",\"CURRENCY_C_CODE\":\"SDG\",\"CUST_I_NO\":\"0040583\",\"MOBILE_C_NO\":\"\",\"BRANCH_C_CODE\":\"AfricaStreet\",\"ACT_C_TYPE\":\"211106\"},{\"ACT_C_NAME\":\"HUSAM\",\"CURRENCY_C_CODE\":\"SDG\",\"CUST_I_NO\":\"00403271\",\"MOBILE_C_NO\":\"\",\"BRANCH_C_CODE\":\"AfricaStreet\",\"ACT_C_TYPE\":\"213104\"},{\"ACT_C_NAME\":\"HUSAM\",\"CURRENCY_C_CODE\":\"SDG\",\"CUST_I_NO\":\"00403286\",\"MOBILE_C_NO\":\"\",\"BRANCH_C_CODE\":\"AfricaStreet\",\"ACT_C_TYPE\":\"213104\"}]}";
                    string res = myCheque.GetCustinfoByID(toAccount);
                    JObject jobj = new JObject();
                    jobj = JObject.Parse(res);
                    dynamic response = jobj;

                    int NoOfAct = response.NoOfAct;
                    JArray accountsarray = null;
                    accountsarray = response.Accounts;
                    string AccountNumber, AccountType, BranchName, Currency;
                    List<SelectListItem> items = new List<SelectListItem>();

                    if (NoOfAct > 0)
                    {
                        for (int i = 0; i < NoOfAct; i++)
                        {
                            JArray accountobject = accountsarray;
                            dynamic accountdetails = accountobject[i];
                            Session["custID"] = accountdetails.CUST_I_NO;
                            Session["custname"] = accountdetails.ACT_C_NAME;
                            Session["custphone"] = accountdetails.MOBILE_C_NO;
                            BranchName = accountdetails.BRANCH_C_CODE;
                            AccountType = accountdetails.ACT_C_TYPE;
                            AccountType = data.getaccounttype(AccountType.Substring(1, AccountType.Length - 1));
                            Currency = accountdetails.CURRENCY_C_CODE;
                            AccountNumber = accountdetails.CUST_I_NO;
                            if (Currency == "SDG")
                            {
                                items.Add(new SelectListItem
                                {
                                    Text = BranchName + " - " + AccountType + " - " + Currency + " - " + AccountNumber,
                                    Value = AccountNumber
                                });
                            }
                        }
                        Session["CustomerAccounts"] = items;
                    }
                    else
                    {
                        message = "Please check customer information something wrong ";
                        ModelState.AddModelError("", message);
                        return View(model);
                    }

                    //if (result.result != "NoCustomerInfoFound")
                    //{
                    //    Session["custID"] = result.CustomerId;
                    //    Session["custname"] = result.CustomerName;
                    //    Session["custphone"] = result.CustomerMobile;
                    //    //model.CustomerPhone = result.CustomerMobile;
                    //    //if(result.CustomerNameAR != null)
                    //    //{
                    //    //    model.arabiccustomername = result.CustomerNameAR;
                    //    //}
                    //    //if (result.CustomerName != null)
                    //    //{
                    //    //    model.Customername = result.CustomerName;
                    //    //}
                    //    //model.CustomerAccounts = new List<SelectListItem>();
                    //    //string customeraccounts = result.CustomerAccounts;
                    //    ////string[] accounts = customeraccounts.Split('-');
                    //    ////for (int i = 0; i < accounts.Length; i++)
                    //    ////{
                    //    ////    model.CustomerAccounts.Add(new SelectListItem
                    //    ////    {
                    //    ////        Text = accounts[i].ToString(),
                    //    ////        Value = accounts[i].ToString()
                    //    ////    });
                    //    ////}
                    //    //Session["custname"] = model.Customername;
                    //    //Session["custnamearabic"] = model.arabiccustomername;
                    //    //Session["custphone"] = model.CustomerPhone;
                    //    //Session["CustomerAccounts"] = model.CustomerAccounts;
                    //    //ViewBag.CustomerAccounts = model.CustomerAccounts;
                    //}
                    //else
                    //{
                    //    message = "Please check customer information something wrong ";
                    //    ModelState.AddModelError("", message);
                    //    return View(model);
                    //}


                }
                catch (Exception e)
                {
                    // Console.WriteLine(e);
                    ModelState.AddModelError("", "Customer not found");
                    return View(model);
                }
                Random rng = new Random();
                string OtpCharacters = OTPGenerate.OTPCharacters();
                //OTPGenerate.getsmsconfig();
                //Createing More Secure OTP Password by Using MD5 algorithm
                 string OTPPassword = OTPGenerate.OTPGenerator(OtpCharacters, rng.Next(10).ToString());
                //String Out = HTTP_GET(OTPGenerate.smspath, "username=" + OTPGenerate.smsusername + "&password=" + OTPGenerate.smspassword + "&to=" + tel + "&text=Your Verification Code " + OTPPassword + " ,Thanks For Making Us Your Choice");


                string msg = "رمز التحقق  : " + OTPPassword + " .";

                string UserLog = Session["userName"].ToString();
                string responseSMS = myCheque.SendSMS(msg, UserLog);

                // string response = "{\"Response\":\"{\\\"success\\\":true,\\\"info\\\":\\\"unexpected_error\\\",\\\"details\\\":null}\",\"Request\":{\"flag\":\"Internetbanking\",\"Message\":\"تم إعادة تعين كلمه المرور الخاص بك. ويمكنك الدخول عن طريق كلمة السر : 68940957 .\",\"Authentication\":\"Card\",\"Channel\":\"InternetBanking\",\"lang\":1,\"uuid\":\"ba4d3c30-350f-44f7-8e84-4400430eb204\",\"Phone_No\":null}}";



                //JObject jobj1 = new JObject();
                //jobj1 = JObject.Parse(responseSMS);
                //dynamic result1 = jobj1;

                //var errorCode = result1.errorcode;
                //var errormsg = result1.errormsg;
                //string ress = result1.Response;
                //JObject jobj2 = new JObject();
                //jobj2 = JObject.Parse(ress);
                //dynamic result2 = jobj2;
                //// var Status = result.success;
                //var Status = result2.success;




                //Random rng = new Random();
                // string OtpCharacters = OTPGenerate.OTPCharacters();
                // OTPGenerate.getsmsconfig();
                //Createing More Secure OTP Password by Using MD5 algorithm
                // string OTPPassword = OTPGenerate.OTPGenerator(OtpCharacters, rng.Next(10).ToString());
                //String Out = HTTP_GET(OTPGenerate.smspath, "username=" + OTPGenerate.smsusername + "&password=" + OTPGenerate.smspassword + "&to=" + tel + "&text=Your Verification Code " + OTPPassword + " ,Thanks For Making Us Your Choice");
                //if (Out.Equals("<h3>OP is: 24990</h3>") || Out.Equals("<h3>OP is: 24991</h3>") || Out.Equals("<h3>OP is: 24992</h3>") || Out.Equals("<h3>OP is: 24993</h3>") || Out.Equals("<h3>OP is: 24996</h3>") || Out.Equals("<h3>OP is: 24999</h3>") || Out.Equals("<h3>OP is: 24910</h3>") || Out.Equals("<h3>OP is: 24911</h3>") || Out.Equals("<h3>OP is: 24912</h3>"))
                //{



                //if (Status == "true")
                //{
                Session["bnkcusttryno"] = 0;
                    Session["tobankOTPPassword"] = OTPPassword;
                    Session["fromAccount"] = model.FromAccount;
                    Session["fromCurrency"] = model.fromCurrencyCode;
                    Session["toAccount"] = toAccount;
                    Session["toCurrency"] = model.toCurrencyCode;
                    Session["Amount"] = model.Amount;
                    return RedirectToAction("Verify", model);
                //}
                //else
                //{

                //    message = "Connect to SMS Server Problem ";
                //    ModelState.AddModelError("", "Sorry Please Try again later ...." + message);
                //    return View();
                //}


                //Session["bnkcusttryno"] = 0;
                //Session["tobankOTPPassword"] = "123456";
                //Session["fromAccount"] = model.FromAccount;
                //Session["fromCurrency"] = model.fromCurrencyCode;
                //Session["toAccount"] = toAccount;
                //Session["toCurrency"] = model.toCurrencyCode;
                //Session["Amount"] = model.Amount;
                //return RedirectToAction("Verify", model);
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
            //var ToAcc = Session["toAccount"].ToString();

            //var FromAccountNumber = Convert.ToInt64(FromAcc.Substring(13));
            //var FromAccountType = data.getaccounttype(FromAcc.ToString().Substring(5, 5));
            //var FromBranchName = data.getbranchnameenglish(FromAcc.ToString().Substring(2, 3));

            //var ToAccountNumber = Convert.ToInt64(ToAcc.Substring(13));
            //var ToAccountType = data.getaccounttype(ToAcc.ToString().Substring(5, 5));
            //var ToBranchName = data.getbranchnameenglish(ToAcc.ToString().Substring(2, 3));

            ViewBag.FromAccount = FromAcc;
            ViewBag.fromCurrency = data.GetCurrencyName(Session["fromCurrency"].ToString());
            ViewBag.Amount = Session["Amount"].ToString();
            ViewBag.ReceiverName = Session["custname"].ToString();
            return View();

        }

        [HttpPost]
        public ActionResult Verify(ToBanCustomerViewModel model)
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
            //var ToAcc = Session["toAccount"].ToString();

            //var FromAccountNumber = Convert.ToInt64(FromAcc.Substring(13));
            //var FromAccountType = data.getaccounttype(FromAcc.ToString().Substring(5, 5));
            //var FromBranchName = data.getbranchnameenglish(FromAcc.ToString().Substring(2, 3));

            //var ToAccountNumber = Convert.ToInt64(ToAcc.Substring(13));
            //var ToAccountType = data.getaccounttype(ToAcc.ToString().Substring(5, 5));
            //var ToBranchName = data.getbranchnameenglish(ToAcc.ToString().Substring(2, 3));

            ViewBag.FromAccount = FromAcc;//FromBranchName + "-" + FromAccountType + "-" + FromAccountNumber;
            //ViewBag.ToAccount = ToAcc;//ToBranchName + "-" + ToAccountType + "-" + ToAccountNumber;       
            ViewBag.Amount = Session["Amount"].ToString();
            model.fromCurrencyCode = Session["fromCurrency"].ToString();
            int tryno = Convert.ToInt32(Session["bnkcusttryno"].ToString());

            string user_id = Session["UserID"].ToString();
            if (tryno < 3)
            {
                string vCode = model.VerificatioCode;
                String OTPPassword = Session["tobankOTPPassword"].ToString();
                if (vCode.Equals(OTPPassword))
                {
                    //String toAccount = ToAcc;
                    try
                    {
                        TempData["btnclick"] = true;
                        ViewBag.btnclick = "true";
                        string res = myCheque.DoAccountTransfer(model.FromAccount, model.CustomerAccount, model.Amount, model.fromCurrencyCode, Session["userName"].ToString(), Session["custname"].ToString());
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
                            string req = model.FromAccount + "," + model.CustomerAccount + "," + model.Amount;
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
                            string req = model.FromAccount + "," + model.CustomerAccount + "," + model.Amount;
                            data.InsertTranLog(user_id, "To Bank Customer Transfer ", req, res, errormsg.ToString(), tranRes.ToString(), model.Amount, "Timed Out");
                            data.InsertTransferToTranLog(user_id, "To Bank Customer Transfer", req, res, errormsg.ToString(),
                                tranRes.ToString(), model.Amount, responseStatus, senderName.ToString(), recieverName.ToString());

                        }

                        Session["Sender"] = senderName;
                        Session["Reciever"] = recieverName;
                        Session["TranAmount"] = model.Amount + "SDG";

                        if (CultureHelper.IsRighToLeft())
                        {
                            return RedirectToAction("Reciept", new { lang = "ar" });
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
                        data.InsertTranLog(user_id, "To Bank Customer Transfer", req, "", "failed", "", model.Amount, "Timed Out");
                    }

                }
                else
                {
                    tryno = tryno + 1;
                    Session["bnkcusttryno"] = tryno;
                    ModelState.AddModelError("", "Please Enter Valid Verfication Code..!");
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