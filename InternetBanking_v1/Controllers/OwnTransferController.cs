using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Helper;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;
using Rotativa;

namespace InternetBanking_v1.Controllers
{
    public class OwnTransferController : BaseController
    {

        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /OwnTransfer/
        public ActionResult OwnTransfer()
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

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

                    Text = BranchName + " - " + AccountType + " - " + item.Additional_Reference,//item.FromAccount,
                    Value = item.Additional_Reference + "-" + item.fromCurrencyCode
                });
            }

            ViewBag.clientList = AccountList;

            //-----------------------------------------------
            //////////////
            //To Account
            List<OwnTransferViewModel> pcontent2 = new List<OwnTransferViewModel>();
            {
                pcontent2 = data.DropFromOwnTransferClientSdgOnly(user_id);
            };
            List<SelectListItem> AccountList2 = new List<SelectListItem>();
            foreach (var item in pcontent2)
            {

                var AccountNumber = Convert.ToInt64(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList2.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + item.Additional_Reference,
                    Value = item.Additional_Reference + "-" + item.toCurrencyCode
                });
            }

            ViewBag.clientList2 = AccountList2;



            return View();
        }




        [HttpPost]
        public ActionResult OwnTransfer(OwnTransferViewModel model)
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string tel = Session["UserTel"].ToString();
            string user_id = Session["UserID"].ToString();

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

            ViewBag.clientList = AccountList;

            //To Account
            List<OwnTransferViewModel> pcontent2 = new List<OwnTransferViewModel>();
            {
                pcontent2 = data.DropFromOwnTransferClientSdgOnly(user_id);
            };
            List<SelectListItem> AccountList2 = new List<SelectListItem>();
            foreach (var item in pcontent2)
            {
                var AccountNumber = Convert.ToInt64(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList2.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + item.Additional_Reference,
                    Value = item.Additional_Reference + "-" + item.toCurrencyCode
                });
            }

            ViewBag.clientList2 = AccountList2;


            if (ModelState.IsValid)
            {

                //------Get Selected Account----------
                model.FromAccount = Request["FromAccount"];
                String[] words = model.FromAccount.Split('-');
                model.FromAccount = words[0];
                model.fromCurrencyCode = words[1];

                //////////////

                //Get Selected Account...
                model.ToAccount = Request["ToAccount"];
                String[] words2 = model.ToAccount.Split('-');
                model.ToAccount = words2[0];
                model.toCurrencyCode = words2[1];

                Random rng = new Random();


                //Session["tryno"] = 0;
                //Session["OTPPassword"] = "123456";
                //Session["ownFromAccount"] = model.FromAccount;
                //Session["ownfromcurrency"] = model.fromCurrencyCode;
                //Session["ownToAccount"] = model.ToAccount;
                //Session["owntocurrency"] = model.toCurrencyCode;
                //Session["ownAmount"] = model.Amount;
                string OtpCharacters = OTPGenerate.OTPCharacters();
                OTPGenerate.getsmsconfig();
                //Createing More Secure OTP Password by Using MD5 algorithm
                string OTPPassword = OTPGenerate.OTPGenerator(OtpCharacters, rng.Next(10).ToString());
                String Out = HTTP_GET(OTPGenerate.smspath, "username=" + OTPGenerate.smsusername + "&password=" + OTPGenerate.smspassword + "&to=" + tel + "&text= Your Verification Code  " + OTPPassword + " ,Thanks For Making Us Your Choice");
                if (Out.Equals("<h3>OP is: 24990</h3>") || Out.Equals("<h3>OP is: 24991</h3>") || Out.Equals("<h3>OP is: 24992</h3>") || Out.Equals("<h3>OP is: 24993</h3>") || Out.Equals("<h3>OP is: 24996</h3>") || Out.Equals("<h3>OP is: 24999</h3>") || Out.Equals("<h3>OP is: 24910</h3>") || Out.Equals("<h3>OP is: 24911</h3>") || Out.Equals("<h3>OP is: 24912</h3>"))
                {
                    Session["tryno"] = 0;
                    Session["OTPPassword"] = OTPPassword;
                    Session["ownFromAccount"] = model.FromAccount;
                    Session["ownfromcurrency"] = model.fromCurrencyCode;
                    Session["ownToAccount"] = model.ToAccount;
                    Session["owntocurrency"] = model.toCurrencyCode;
                    Session["ownAmount"] = model.Amount;
                    if (CultureHelper.IsRighToLeft())
                    {
                        return RedirectToAction("Verify", new { lang = "ar" });
                    }
                    else
                    {
                        return RedirectToAction("Verify");
                    }
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
            if (CultureHelper.IsRighToLeft())
            {
                return RedirectToAction("Verify", new { lang = "ar" });
            }
            else
            {
                return RedirectToAction("Verify");
            }
            //return View();
        }

        public ActionResult Verify()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //info about Transfer
            var FromAcc = Session["ownFromAccount"].ToString();
            var ToAcc = Session["ownToAccount"].ToString();
            var fromcurrencycode = Session["ownfromcurrency"].ToString();
            var tocurrencycode = Session["owntocurrency"].ToString();

            ViewBag.FromAccount = FromAcc;
            ViewBag.FromCurrency = data.GetCurrencyName(fromcurrencycode);
            ViewBag.ToAccount = ToAcc;
            ViewBag.ToCurrency = data.GetCurrencyName(tocurrencycode);
            ViewBag.Amount = Session["ownAmount"].ToString();

            return View();

        }
        [HttpPost]
        public ActionResult Verify(OwnTransferViewModel model)
        {
            int tryno = Convert.ToInt32(Session["tryno"].ToString());


            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            model.FromAccount = Session["ownFromAccount"].ToString();
            model.fromCurrencyCode = Session["ownfromcurrency"].ToString();
            model.ToAccount = Session["ownToAccount"].ToString();
            model.toCurrencyCode = Session["owntocurrency"].ToString();
            model.Amount = Session["ownAmount"].ToString();

            //info about Transfer
            var FromAcc = Session["ownFromAccount"].ToString();
            var ToAcc = Session["ownToAccount"].ToString();

            ViewBag.FromAccount = FromAcc;
            ViewBag.ToAccount = ToAcc;
            ViewBag.Amount = Session["ownAmount"].ToString();


            string user_id = Session["UserID"].ToString();
            if (tryno < 3)
            {

                string vCode = model.VerificatioCode;
                String OTPPassword = Session["OTPPassword"].ToString();
                if (vCode.Equals(OTPPassword))
                {

                    try
                    {

                        if (CultureHelper.IsRighToLeft())
                            TempData["btnclick"] = true;
                        ViewBag.btnclick = "true";
                        string res = myCheque.DoAccountTransfer(model.FromAccount, model.ToAccount, model.Amount, model.fromCurrencyCode, Session["userName"].ToString(), Session["userName"].ToString());
                        //string res = "{\"receivername\":\"HUSAM ALDIEN HAFIZ ABDALRHHMEAN MOHAMED\",\"sendername\":\"HUSAM ALDIEN HAFIZ ABDALRHHMEAN MOHAMED\",\"tranDateTime\":\"141018124523\",\"uuid\":\"ca88edf2-4f85-4a3b-98a7-70c9f1711880\",\"errormsg\":\"Secussfully\",\"errorcode\":\"0\",\"status\":\"The transactionfalid, OutUtiNumber = 201800464499\"}";
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



                        /*//insert into TranLog
                        string req = model.FromAccount + "," + model.ToAccount + "," + model.Amount;
                        data.InsertTranLog(user_id, "Own Transfer", req, res, errormsg.ToString(), tranRes.ToString(), model.Amount, responseMessage);*/

                        Session["TranResponse"] = false;
                        Session["ResponseStat"] = "";
                        Session["ResponseMSG"] = "";

                        if (errorCode != null && !tranRes.ToString().Equals("0"))
                        {
                            Session["TranResponse"] = true;
                            Session["ResponseStat"] = errormsg;
                            Session["ResponseMSG"] = responseStatus;

                            senderName = result.sendername;
                            recieverName = result.receivername;
                            TempData["Success"] = false;
                            //ModelState.AddModelError("", tranRes.ToString());
                            ViewBag.ResponseStat = errormsg;
                            ViewBag.ResponseMSG = responseStatus;

                            //insert into TranLog
                            string req = model.FromAccount + "," + model.ToAccount + "," + model.Amount;
                            //Session["Sender"] = senderName;
                            //Session["Reciever"] = recieverName;
                            //data.InsertTranLog(user_id, "Own Transfer", req, res, errormsg.ToString(), tranRes.ToString(), model.Amount, responseMessage);
                            data.InsertTransferToTranLog(user_id, "Own Transfer", req, res, errormsg.ToString(),
                                tranRes.ToString(), model.Amount, responseStatus, senderName.ToString(), recieverName.ToString());
                        }
                        else
                        {
                            Session["TranResponse"] = true;
                            Session["ResponseStat"] = "Not Successful";
                            Session["ResponseMSG"] = responseStatus;
                            senderName = result.sendername;
                            recieverName = result.receivername;
                            TempData["Success"] = false;
                            //ModelState.AddModelError("", tranRes.ToString());
                            ViewBag.ResponseStat = "Not Successful";
                            ViewBag.ResponseMSG = responseStatus;
                            //insert into TranLog
                            string req = model.FromAccount + "," + model.ToAccount + "," + model.Amount;
                            data.InsertTranLog(user_id, "Own Transfer", req, res, errormsg.ToString(), tranRes.ToString(), model.Amount, "Timed Out");
                            data.InsertTransferToTranLog(user_id, "Own Transfer", req, res, errormsg.ToString(),
                                tranRes.ToString(), model.Amount, responseStatus, senderName.ToString(), recieverName.ToString());
                        }



                        Session["ownSender"] = senderName;
                        Session["ownReciever"] = recieverName;
                        Session["ownTranAmount"] = model.Amount + "SDG";
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
                        //   Console.WriteLine(e);
                        ModelState.AddModelError("", GlobalRes.ConnectivityIssue);

                        //insert into TranLog
                        string req = model.FromAccount + "," + model.ToAccount + "," + model.Amount;
                        data.InsertTranLog(user_id, "Own Transfer", req, "", "failed", "", model.Amount, "Timed Out");
                        return View();
                    }

                }
                else
                {
                    tryno = tryno + 1;
                    Session["tryno"] = tryno;
                    ModelState.AddModelError("", "Please Enter Valid Verfication Code..!");
                    return View();
                }

            }
            else
            {

                ModelState.AddModelError("", "Please try again later..!");
                return View();
            }
            return View();
        }

        public ActionResult Reciept()
        {
            //ViewBag.Sender = Session["Sender"].ToString();
            //ViewBag.Reciever = Session["Reciever"].ToString();
            //ViewBag.Amount = Session["TranAmount"].ToString();
            //ViewBag.Reference = Session["ResponseMSG"].ToString();
            //return View();
            ToBanCustomerViewModel model = new ToBanCustomerViewModel();
            //Session["Senderr"] = Session["ownSender"].ToString();
            //Session["Recieverr"] = Session["Reciever"].ToString();
            //Session["TranAmountt"] = Session["TranAmount"].ToString();
            //Session["ResponseMSGG"] = Session["ResponseMSG"].ToString();

            model.SenderName = Session["ownSender"].ToString();
            model.RecieverName = Session["ownReciever"].ToString();
            model.Amount = Session["ownTranAmount"].ToString();
            model.TranReference = Session["ResponseMSG"].ToString();
            Session["TranResponse"] = false;
            Session["ResponseStat"] = "";
            Session["ResponseMSG"] = "";
            TempData["Success"] = "";

            return View(model);
        }

        public ActionResult PrintSalarySlip()
        {
            var report = new ActionAsPdf("Reciept");
            return report;
        }

        //
        // GET: /SendSMS/Details/5

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