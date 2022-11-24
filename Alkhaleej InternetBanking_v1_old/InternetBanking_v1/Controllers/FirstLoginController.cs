using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using InternetBanking_v1.Providers;

namespace InternetBanking_v1.Controllers
{
    public class FirstLoginController : Controller
    {

        DataSource data = new DataSource();
        //
        // GET: /FirstLogin/
        public ActionResult ResetPassword()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }


        [HttpPost]
        public ActionResult ResetPassword(FirstLoginViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            var user_ID = Session["UserID"].ToString();

            if (ModelState.IsValid)
            {
                string currentPassword = pwdEncrypt(model.CurrentPassword);
                string newPassword = pwdEncrypt(model.NewPassword);
                int res = data.FirstLoginUpdateUserPassword(user_ID, currentPassword, newPassword);


                if (res == 1)
                {
                    TempData["Success"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Successful";
                    ViewBag.ResponseMSG = "Your Password has been updated successfully";

                    return RedirectToAction("Index", "Home");
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



        //---------
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




        //-------------------Encrypt Pass--------------------------
        protected string Encrypt(string clearText)
        {
            string EncryptionKey = "IBAZ2TWTQS77898";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
	}
}