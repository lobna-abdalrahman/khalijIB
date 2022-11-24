using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using InternetBanking_v1.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers.Services.Change_Password
{
    public class ChangePasswordController : BaseController
    {

        DataSource data = new DataSource();
        //
        // GET: /ChangePassword/

        public ActionResult passwordotp()
        {
            if (Session.IsNewSession)
            {
                //if(!status.Equals("1"))
                return RedirectToAction("Login", "Login");
            }

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult passwordotp(ChangePasswordViewModel model)
        {
            if (Session.IsNewSession)
            {
                //if(!status.Equals("1"))
                return RedirectToAction("Login", "Login");
            }

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //
            Random rng = new Random();
            String tel = Session["UserTel"].ToString();
            string OtpCharacters = OTPGenerate.OTPCharacters();
            string OTPPassword = OTPGenerate.OTPGenerator(OtpCharacters, rng.Next(10).ToString());
            Session["otp"] = OTPPassword;
            string message = "Your Verification Code " + OTPPassword + " ,Thanks For Making Us Your Choice";
            string response = sentotp(tel, message);
            if (response == "OK")
            {
                return View("otpverification");
            }
            else
            {
                message = "Connect to SMS Server Problem ";
                ModelState.AddModelError("", "Sorry Please Try again later ...." + message);
                return View();
            }
        }

        public ActionResult otpverification()
        {
            if (Session.IsNewSession)
            {
                //if(!status.Equals("1"))
                return RedirectToAction("Login", "Login");
            }

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult otpverification(ChangePasswordViewModel model)
        {
            string message = "";
            if (Session.IsNewSession)
            {
                //if(!status.Equals("1"))
                return RedirectToAction("Login", "Login");
            }

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string otp = Session["otp"].ToString();
            if (model.otp == otp)
            {
                return View("ChangePassword");
            }
            else
            {
                message = " incorrect otp.";
                ModelState.AddModelError("", "Sorry Please Try again," + message);
                return View();
            }
        }

        public ActionResult ChangePassword()
        {
            if (Session.IsNewSession)
            {
                //if(!status.Equals("1"))
                return RedirectToAction("Login", "Login");
            }

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (Session.IsNewSession)
            {
                //if(!status.Equals("1"))
                return RedirectToAction("Login", "Login");
            }

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var user_ID = Session["UserID"].ToString();

            if (ModelState.IsValid)
            {
                string currentPassword = pwdEncrypt(model.CurrentPassword);
                string newPassword = pwdEncrypt(model.NewPassword);

                // adding otp update on 22/11/2020
                string message = "";
                Random rng = new Random();


                string OtpCharacters = OTPGenerate.OTPCharacters();
                OTPGenerate.getsmsconfig();
                //Createing More Secure OTP Password by Using MD5 algorithm
                string OTPPassword = OTPGenerate.OTPGenerator(OtpCharacters, rng.Next(10).ToString());
                //String Out = HTTP_GET(OTPGenerate.smspath, "username=" + OTPGenerate.smsusername + "&password=" + OTPGenerate.smspassword + "&to=" + tel + "&text=Your Verification Code " + OTPPassword + " ,Thanks For Making Us Your Choice");
                //if (Out.Equals("<h3>OP is: 24990</h3>") || Out.Equals("<h3>OP is: 24991</h3>") || Out.Equals("<h3>OP is: 24992</h3>") || Out.Equals("<h3>OP is: 24993</h3>") || Out.Equals("<h3>OP is: 24996</h3>") || Out.Equals("<h3>OP is: 24999</h3>") || Out.Equals("<h3>OP is: 24910</h3>") || Out.Equals("<h3>OP is: 24911</h3>") || Out.Equals("<h3>OP is: 24912</h3>"))
                //{
                //    Session["bnkcusttryno"] = 0;
                //    Session["tobankOTPPassword"] = OTPPassword;
                //    Session["fromAccount"] = model.FromAccount;
                //    Session["toAccount"] = toAccount;
                //    Session["Amount"] = model.Amount;
                //    return RedirectToAction("Verify", model);
                //}
                //else
                //{

                //    message = "Connect to SMS Server Problem ";
                //    ModelState.AddModelError("", "Sorry Please Try again later ...." + message);
                //    return View();
                //}

                int res = data.updateUserPassword(user_ID, currentPassword, newPassword);


                if (res == 1)
                {
                    TempData["Success"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Successful";
                    ViewBag.ResponseMSG = "Your Password has been updated successfully";


                }
                else
                {
                    TempData["Success"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Not Successful";
                    ViewBag.ResponseMSG = "Please Try again later";

                }
            }

            return View();
        }

        public string pwdEncrypt(string clearText)
        {
            CryptLib _crypt = new CryptLib();

            String key = "b16920894899c7780b5fc7161560a412";//CryptLib.SHA256("my secret key", 32); //32 bytes = 256 bit

            String iv = "e77886746a9b416d";
            //String iv = CryptLib.GenerateRandomIV(16); //16 bytes = 128 bits
            //string key = CryptLib.getHashSha256("my secret key", 31); //32 bytes = 256 bits
            String cypherText = _crypt.encrypt(clearText, key, iv);

            //Console.WriteLine("Plain text =" + _crypt.decrypt(cypherText, key, iv));
            return cypherText;
        }

        public string sentotp(string tel,string message)
        {
            MyChequeStatus connect = new MyChequeStatus();
            string url = connect.getsmsconfig();
            Uri requestUri = new Uri(url + "&to=" + tel + "&text=" + message);
            HttpResponseMessage respon = null;

            dynamic dynamicJson = new ExpandoObject();

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                     respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception e)
                {
                    /* Console.WriteLine(e);
                     throw;*/
                }

                return respon.StatusCode.ToString();

            }

        }
    }
}