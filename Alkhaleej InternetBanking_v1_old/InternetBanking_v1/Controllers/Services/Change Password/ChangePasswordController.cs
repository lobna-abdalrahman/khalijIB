using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using InternetBanking_v1.Providers;

namespace InternetBanking_v1.Controllers.Services.Change_Password
{
    public class ChangePasswordController : BaseController
    {

        DataSource data = new DataSource();
        //
        // GET: /ChangePassword/
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

    }
}