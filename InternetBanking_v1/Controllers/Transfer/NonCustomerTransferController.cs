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

namespace InternetBanking_v1.Controllers.Transfer
{
    public class NonCustomerTransferController : Controller
    {
        //
        // GET: /NonCustomerTransfer/
        DataSource data = new DataSource();
        MyChequeStatus myCheque = new MyChequeStatus();
        public ActionResult NonCustomer()
        {
            
         
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

            List<NonCustomerTransferViewModel> pcontent = new List<NonCustomerTransferViewModel>();
            {
                pcontent = data.NonCustomerTransferAccount(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {

                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,//item.FromAccount,
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




            ViewBag.clientList = AccountList;
            ViewBag.branchList = branchList;
            return  View();

        }

        [HttpPost]
        public ActionResult NonCustomer(NonCustomerTransferViewModel model)
        {
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string user_id = Session["UserID"].ToString();

            List<NonCustomerTransferViewModel> pcontent = new List<NonCustomerTransferViewModel>();
            {
                pcontent = data.NonCustomerTransferAccount(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.FromAccount.Substring(13));
                var AccountType = data.getaccounttype(item.FromAccount.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.FromAccount.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {

                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,//item.FromAccount,
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




            ViewBag.clientList = AccountList;
            ViewBag.branchList = branchList;
            if (ModelState.IsValid)
            {

                string tel = Session["UserTel"].ToString();
                //----------------Doing the Service--------------------
                //Get Selected Account...
                model.FromAccount = Request["FromAccount"];
                model.BranchCode = Request["BranchList"];
                string toAccount = "";

                toAccount = "99" + model.BranchCode +"00000"+ "001"+"000";
            

                Random rng = new Random();

                
                string OtpCharacters = OTPGenerate.OTPCharacters();
                OTPGenerate.getsmsconfig();
                //Createing More Secure OTP Password by Using MD5 algorithm
                string OTPPassword = OTPGenerate.OTPGenerator(OtpCharacters, rng.Next(10).ToString());
                String Out = HTTP_GET(OTPGenerate.smspath, "username=" + OTPGenerate.smsusername + "&password=" + OTPGenerate.smspassword + "&to=" + tel + "&text=Your Verification Code " + OTPPassword + "  ,Thanks For Making Us Your Choice");
                if (Out.Equals("<h3>OP is: 24990</h3>") || Out.Equals("<h3>OP is: 24991</h3>") || Out.Equals("<h3>OP is: 24992</h3>") || Out.Equals("<h3>OP is: 24993</h3>") || Out.Equals("<h3>OP is: 24996</h3>") || Out.Equals("<h3>OP is: 24999</h3>") || Out.Equals("<h3>OP is: 24910</h3>") || Out.Equals("<h3>OP is: 24911</h3>") || Out.Equals("<h3>OP is: 24912</h3>"))
                {
                    Session["countercusttryno"] = 0;
                    Session["tocounterOTPPassword"] = OTPPassword;
                    Session["tobranch"] = toAccount;
                    Session["toname"] = model.Name;
                    Session["tophone"] = model.ToPhoneNo;
                    Session["noncutfromacount"] = model.FromAccount;
                    Session["fromAccount"] = model.FromAccount;                   
                    Session["toamount"] = model.Amount;
                    if (CultureHelper.IsRighToLeft())
                    {
                        return RedirectToAction("Verify",  new { lang = "ar" });
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

        }




        public ActionResult Verify()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //info about Transfer
            var benifitiaryName = Session["toname"].ToString();
            var benifitiaryPhone = Session["tophone"].ToString();
            var FromAcc = Session["fromAccount"].ToString();
            

            var FromAccountNumber = Convert.ToInt64(FromAcc.Substring(13));
            var FromAccountType = data.getaccounttype(FromAcc.ToString().Substring(5, 5));
            var FromBranchName = data.getbranchnameenglish(FromAcc.ToString().Substring(2, 3));

            ViewBag.FromAccount = FromBranchName + "-" + FromAccountType + "-" + FromAccountNumber;
            ViewBag.BenifitiaryName = benifitiaryName.ToString();
            ViewBag.BenifitiaryPhone = benifitiaryPhone.ToString();
            ViewBag.Amount = Session["toamount"].ToString();

            return View();

        }
        [HttpPost]
        public ActionResult Verify(NonCustomerTransferViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int tryno = Convert.ToInt32(Session["countercusttryno"].ToString());

            //info about Transfer
            var benifitiaryName = Session["toname"].ToString();
            var benifitiaryPhone = Session["tophone"].ToString();
            var FromAcc = Session["fromAccount"].ToString();


            var FromAccountNumber = Convert.ToInt64(FromAcc.Substring(13));
            var FromAccountType = data.getaccounttype(FromAcc.ToString().Substring(5, 5));
            var FromBranchName = data.getbranchnameenglish(FromAcc.ToString().Substring(2, 3));

            ViewBag.FromAccount = FromBranchName + "-" + FromAccountType + "-" + FromAccountNumber;
            ViewBag.BenifitiaryName = benifitiaryName.ToString();
            ViewBag.BenifitiaryPhone = benifitiaryPhone.ToString();
            ViewBag.Amount = Session["toamount"].ToString();

            string user_id = Session["UserID"].ToString();
            if (tryno < 3)
            {
                string vCode = model.VerificatioCode;
                String OTPPassword = Session["tocounterOTPPassword"].ToString();
                if (vCode.Equals(OTPPassword))
                {
                    String tobranch = Session["tobranch"].ToString();
                    String toname = Session["toname"].ToString();
                    String tophone = Session["tophone"].ToString();

                    model.Amount = Session["toamount"].ToString();
                    model.FromAccount = Session["noncutfromacount"].ToString();
                
                    try
                    {
                        TempData["btnclick"] = true;
                        ViewBag.btnclick = "true";
                        string res = myCheque.DocounterTransfer(model.FromAccount, tobranch, toname, tophone, model.Amount);
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



                        
                        //data.InsertTranLog(user_id, "To Counter Transfer", req, res, errormsg.ToString(), tranRes.ToString(), model.Amount, responseStatus);

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

                            TempData["Success"] = true;
                            //ModelState.AddModelError("", tranRes.ToString());
                            ViewBag.ResponseStat = errormsg;
                            ViewBag.ResponseMSG = responseStatus;

                            //insert into TranLog
                            string req = model.FromAccount + "," + tobranch + "," + toname + "," + tophone + "," + model.Amount;

                            data.InsertTransferToTranLog(user_id, "To Counter Transfer", req, res, errormsg.ToString(),
                                tranRes.ToString(), model.Amount, responseStatus, senderName.ToString(), recieverName.ToString());

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
                            senderName = result.sendername;
                            recieverName = result.receivername;
                            string req = model.FromAccount + "," + tobranch + "," + toname + "," + tophone + "," + model.Amount;
                            data.InsertTranLog(user_id, "To Counter Transfer", req, res, errormsg.ToString(), tranRes.ToString(), model.Amount, "Timed Out");

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
                        ModelState.AddModelError("", GlobalRes.support);

                        string req = model.FromAccount + "," + tobranch + "," + toname + "," + tophone + "," + model.Amount;
                        data.InsertTranLog(user_id, "To Counter Transfer", req, "", "failed", "", model.Amount,"");
                        return View();
                    }

                }
                else
                {
                    tryno = tryno + 1;
                    Session["countercusttryno"] = tryno;
                    ModelState.AddModelError("", "Please Enter Valid Verfication Code..!");
                    return View();
                }

            }
            else
            {

                ModelState.AddModelError("", "Please try again later..!");
                return View();
            }

            return  View();

        }
        public ActionResult Reciept()
        {

            NonCustomerTransferViewModel model = new NonCustomerTransferViewModel();
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


            return View(model);
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