using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers
{
    public class TransferAuthorizeController : Controller
    {
        DataSource data = new DataSource();
        MyChequeStatus obj = new MyChequeStatus();
        MyChequeStatus myCheque = new MyChequeStatus();
        //
        // GET: /TransferAuthorize/
        public ActionResult UnAuthorizedTransfers()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            List<TransferAuthorizeViewModel> accas = new List<TransferAuthorizeViewModel>();
            accas = data.getUnAuthorizedTransfers();

            return View(accas);
        }




        public ActionResult Details(int id)
        {
            String message;
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string tranID = id.ToString();
            Session["TranID"] = tranID;
            List<TransferAuthorizeViewModel> accas = new List<TransferAuthorizeViewModel>();
            accas = data.getSpecificTransfer(tranID);

            TransferAuthorizeViewModel model = new TransferAuthorizeViewModel();

            foreach (var item in accas)
            {
                //TODO Add The ReceiverName Retreival HERE ..
                try
                {
                    string res = myCheque.GetReceiverName(item.ToAccountNumber);
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

                        model.ReceiverName=custname;
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
                    // Console.WriteLine(e);item.FromAccountNumber
                    ModelState.AddModelError("", e.Message);
                }
                var FromAccountNumber = Convert.ToInt32(item.FromAccountNumber.Substring(13));
                var FromAccountType = data.getaccounttype(item.FromAccountNumber.ToString().Substring(5, 5));
                var FromBranchName = data.getbranchnameenglish(item.FromAccountNumber.ToString().Substring(2, 3));

                var ToAccountNumber = Convert.ToInt32(item.ToAccountNumber.Substring(13));
                var ToAccountType = data.getaccounttype(item.ToAccountNumber.ToString().Substring(5, 5));
                var ToBranchName = data.getbranchnameenglish(item.ToAccountNumber.ToString().Substring(2, 3));
                
                model.UserID = item.UserID;
                model.UserName = item.UserName;
                //Mapping the from-Account to its branch-accType-AccNumber
                model.FromAccountName = FromBranchName + " - " + FromAccountType + " - " + FromAccountNumber;//item.FromAccountNumber;
                //Mapping the To-Account to its branch-accType-AccNumber
                model.ToAccountName = ToBranchName + " - " + ToAccountType + " - " + ToAccountNumber;//item.ToAccountNumber;
                model.TransferAmount = item.TransferAmount;
                model.TransferStatus = item.TransferStatus;
                model.TransferDate = item.TransferDate;

            }

            Session["TransferUserId"]            = model.UserID;
            Session["TransferUserName"]          = model.UserName;
            Session["TransferFromAccountNumber"] = model.FromAccountName;
            Session["TransferToAccountNumber"]   = model.ToAccountName;
            Session["TransferAmount"]            = model.TransferAmount;
            Session["TransferStatus"]            = model.TransferStatus;
            Session["TransferDate"]              = model.TransferDate;

            return View(model);
        }

        public ActionResult Authorize()
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            int id = Convert.ToInt32(Session["TranID"].ToString());

            return RedirectToAction("Details",new{id = id});
            /*return View("Details");*/
        }

        [HttpPost]
        public ActionResult Authorize(TransferAuthorizeViewModel model)
        {

            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();
            string tel = Session["UserTel"].ToString();

            model.UserID = Session["TransferUserId"].ToString();
            model.UserName =             Session["TransferUserName"].ToString();      
            model.FromAccountNumber =     Session["TransferFromAccountNumber"].ToString();
            model.ToAccountNumber =       Session["TransferToAccountNumber"].ToString();  
            model.TransferAmount =        Session["TransferAmount"].ToString();   
            model.TransferStatus =        Session["TransferStatus"].ToString();
            model.TransferDate = Session["TransferDate"].ToString();
            model.ReceiverName = Session["custname"].ToString();
                
            Random rng = new Random();


            string OtpCharacters = OTPGenerate.OTPCharacters();
            OTPGenerate.getsmsconfig();

            //Createing More Secure OTP Password by Using MD5 algorithm
            string OTPPassword = OTPGenerate.OTPGenerator(OtpCharacters, rng.Next(10).ToString());
            String Out = HTTP_GET(OTPGenerate.smspath, "username=" + OTPGenerate.smsusername + "&password=" + OTPGenerate.smspassword + "&to=" + tel + "&text=Your Verification Code " + OTPPassword + "  ,Thanks For Making Us Your Choice");
            if (Out.Equals("<h3>OP is: 24990</h3>") || Out.Equals("<h3>OP is: 24991</h3>") || Out.Equals("<h3>OP is: 24992</h3>") || Out.Equals("<h3>OP is: 24993</h3>") || Out.Equals("<h3>OP is: 24996</h3>") || Out.Equals("<h3>OP is: 24999</h3>") || Out.Equals("<h3>OP is: 24910</h3>") || Out.Equals("<h3>OP is: 24911</h3>") || Out.Equals("<h3>OP is: 24912</h3>"))
            {
                Session["bnkcusttryno"] = 0;
                Session["tobankOTPPassword"] = OTPPassword;
                Session["toAccount"] = model.ToAccountNumber;

                string tranID = Session["TranID"].ToString();
                /*int status = data.updateTransferStatus(tranID);*/
                return RedirectToAction("Verify", model);
            }
            else
            {
                   
                message = "Connect to SMS Server Problem ";
                ModelState.AddModelError("", "Sorry Please Try again later ...." + message);

                int id = Convert.ToInt32(Session["TranID"].ToString());

                return RedirectToAction("Details",new{id = id});

                /*return View("Details");*/
            }

        }


        public ActionResult Verify()
        {
            return View();

        }


        [HttpPost]
        public ActionResult Verify(TransferAuthorizeViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            model.UserID = Session["TransferUserId"].ToString();
            model.UserName = Session["TransferUserName"].ToString();
            model.FromAccountNumber = Session["TransferFromAccountNumber"].ToString();
            model.ToAccountNumber = Session["TransferToAccountNumber"].ToString();
            model.TransferAmount = Session["TransferAmount"].ToString();
            model.TransferStatus = Session["TransferStatus"].ToString();
            model.TransferDate = Session["TransferDate"].ToString();
            model.ReceiverName = Session["custname"].ToString();
            int tryno = Convert.ToInt32(Session["bnkcusttryno"].ToString());

            string user_id = Session["UserID"].ToString();
            if (tryno < 3)
            {
                string vCode = model.VerificatioCode;
                String OTPPassword = Session["tobankOTPPassword"].ToString();
                if (vCode.Equals(OTPPassword))
                {
                    String toAccount = Session["toAccount"].ToString();
                    try
                    {
                        //Update the Status
                        string tranID = Session["TranID"].ToString();
                        int status = data.updateTransferStatus(tranID);

                        //Response...
                        TempData["btnclick"] = true;
                        ViewBag.btnclick = "true";
                        string res = myCheque.DoAccountTransfer(model.FromAccountNumber, model.ToAccountNumber, model.TransferAmount);
                        JObject jobj = new JObject();
                        jobj = JObject.Parse(res);
                        dynamic result = jobj;

                        var errorCode = result.errorcode;
                        var errormsg = result.errormsg;
                        var tranRes = result.status;


                        string responseStatus = result.status;
                        string responseMessage = result.responseMessage;



                        /*//insert into TranLog
                        string req = model.FromAccountNumber + "," + model.ToAccountNumber + "," + model.TransferAmount;
                        data.InsertTranLog(user_id, "To Bank Customer Transfer", req, res, errormsg.ToString(), tranRes.ToString(), model.TransferAmount, responseMessage);*/

                        Session["TranResponse"] = false;
                        Session["ResponseStat"] = "";
                        Session["ResponseMSG"] = "";

                        if (errorCode != null && !errorCode.ToString().Equals("0"))
                        {
                            Session["TranResponse"] = true;
                            Session["ResponseStat"] = errormsg;
                            Session["ResponseMSG"] = responseStatus;

                            TempData["Success"] = true;
                            //ModelState.AddModelError("", tranRes.ToString());
                            ViewBag.ResponseStat = errormsg;
                            ViewBag.ResponseMSG = responseStatus;

                            //insert into TranLog
                            string req = model.FromAccountNumber + "," + model.ToAccountNumber + "," + model.TransferAmount;
                            data.InsertTranLog(user_id, "To Bank Customer Transfer", req, res, errormsg.ToString(), tranRes.ToString(), model.TransferAmount, responseMessage);


                        }
                        else
                        {
                            Session["TranResponse"] = true;
                            Session["ResponseStat"] = "Not Successful";
                            Session["ResponseMSG"] = responseStatus;

                            TempData["Success"] = true;
                            //ModelState.AddModelError("", tranRes.ToString());
                            ViewBag.ResponseStat = "Not Successful";
                            ViewBag.ResponseMSG = responseStatus;

                            string req = model.FromAccountNumber + model.ToAccountNumber + model.TransferAmount;
                            data.InsertTranLog(user_id, "To Bank Customer Transfer", req, "", "failed", "", model.TransferAmount, "");

                        }

                        if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
                        {
                            return RedirectToAction("Index", "Home", new { lang = "ar" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        



                    }
                    catch (Exception e)
                    {
                        // Console.WriteLine(e);
                        ModelState.AddModelError("", e.Message);

                        string req = model.FromAccountNumber + model.ToAccountNumber + model.TransferAmount;
                        data.InsertTranLog(user_id, "To Bank Customer Transfer", req, "", "failed", "", model.TransferAmount,"");
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





        //Reject Transfer...
        public ActionResult RejectTransfer()
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string tranID = Session["TranID"].ToString();
            int status = data.RejectTransfer(tranID);

            if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
            {
                return RedirectToAction("UnAuthorizedTransfers", "TransferAuthorize", new { lang = "ar" });
            }
            else
            {
                return RedirectToAction("UnAuthorizedTransfers", "TransferAuthorize");
            }

            
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