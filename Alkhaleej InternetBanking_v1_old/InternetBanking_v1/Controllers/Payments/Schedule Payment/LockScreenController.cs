using System;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Providers;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers.Payments.Schedule_Payment
{
    public class LockScreenController : Controller
    {
        DataSource ds = new DataSource();
        //
        // GET: /LockScreen/
        public ActionResult LockScreen()
        {
            if (Session["LockedUser"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string username = Session["LockedUser"].ToString();
            ViewBag.LockedUser = username;
            return View();
        }

        [HttpPost]
        public ActionResult LockScreen(LockScreenViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Username = Session["LockedUser"].ToString();
                model.Password = pwdEncrypt(model.Password);

                int res = ds.UnlockUserSession(model.Username, model.Password);
                if (res == 1)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ModelState.AddModelError("", "User Not Found..! ");
                }


            }

            return View();
        }




        /**
         * Encrypt PWD         
         **/

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