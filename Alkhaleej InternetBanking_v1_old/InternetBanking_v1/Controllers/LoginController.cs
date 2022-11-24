using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
 
using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Helper;
using InternetBanking_v1.Models;
using InternetBanking_v1.Models.Account;
using InternetBanking_v1.Models.Login;
using InternetBanking_v1.Providers;
using InternetBanking_v1.Repository;
using LinqToDB.Common;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebGrease;

namespace InternetBanking_v1.Controllers
{

	public class LoginController : Controller
	{
	    public String mySessionUsername;
	   public UserAccount userAcc;
	    public User userModel;
	    public static string DataRead;
        DataSource ds = new DataSource();
        LoginLogic userBL = new LoginLogic();
        BalanceLogic balanceLogic = new BalanceLogic();
        MyChequeStatus data = new MyChequeStatus();
		//
		// GET: /Login/
	    
		public ActionResult Login()
		{
            data.getconfig();

		    ViewBag.Title = "Login";
			return View();
		}

        [HttpPost]
        public async Task<ActionResult> Login(User User)
        {
            
            string message = "";
            if (ModelState.IsValid)
            {
                try{
                /////////////////////////
                User.Password = pwdEncrypt(User.Password);
                int UserID = userBL.checkUserLogin(User);

                string thisDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                string thisIPAddress = userBL.IPAdd; //userBL.GetLanIPAddress().ToString();
                string current_IP = userBL.currentIP;
                string primaryAccount = userBL.PrimaryAccount;
                string userTel = userBL.userTel;
                string useremail = userBL.useremail;
                string userStatus = userBL.userStatus;
                string FirstLogin = userBL.FirstLogin;
                string LoginStatus = userBL.loginstat;
                //store accNo in model
                MyAccounts myAcc = new MyAccounts();
                myAcc.AccountNumber = primaryAccount;

                if (userStatus.IsNullOrEmpty())
                {
                    ModelState.AddModelError("","Wrong username or password");
                    int res = ds.insertFailedUserLoginActivity(User.Username, User.Password, current_IP);
                    return View();
                }
                if (userStatus.Equals("B"))
                {
                    ModelState.AddModelError("", "This User is Blocked. Please Contact Your Branch.");
                    int res = ds.insertFailedUserLoginActivity(User.Username, User.Password, current_IP);
                    return View();
                }
                
                if (userStatus.Equals("A"))
                {
                    if (LoginStatus.Equals("1"))
                    {
                        Session["LockedUser"] = User.Username;
                        //ModelState.AddModelError("", "This user is online Already...");
                        return RedirectToAction("LockScreen", "LockScreen");
                    }

                    //Getting the menu from database...
                    IEnumerable<Menu> Menu = null;

                    if (Session["_Menu"] != null)
                    {
                        Menu = (IEnumerable<Menu>) Session["_Menu"];
                    }
                    else
                    {
                        if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
                        {
                            Menu = MenuData.GetArabicMenus(UserID.ToString(), userBL.userRole.ToString()); // pass employee id here
                            Session["_Menu"] = Menu;
                        }
                        else
                        {
                            Menu = MenuData.GetMenus(UserID.ToString(), userBL.userRole.ToString()); // pass employee id here
                            Session["_Menu"] = Menu;
                        }
                        
                    }

          
                    if (UserID > 0)
                    {

                        //var m = x.bal;

                        //FormsAuthentication.SetAuthCookie(userModel.Username,userModel.RememberMe);
                        Session["myMenu"] = null;
                        //Session["myBalance"] = x.bal;
                        Session["status"] = "1";
                        message = "sucess";
                        Session["UserID"] = UserID;
                        Session["date"] = thisDate;
                        Session["name"] = userBL.name;
                        Session["userName"] = User.Username;
                        Session["CurrentIP"] = current_IP;
                        Session["IPAddress"] = thisIPAddress;
                        Session["PrimaryAccount"] = primaryAccount;
                        Session["UserTel"] = userTel;
                        Session["email"] = useremail;
                        mySessionUsername = Session["userName"].ToString();
                        int res = ds.insertSuccessUserLoginActivity(User.Username, User.Password, current_IP, UserID.ToString());
                        
                        //Check First Login
                        if (FirstLogin.Equals("T"))
                        {
                            return RedirectToAction("ResetPassword", "FirstLogin");
                        }


                        //if (loginstat.Equals("1"))
                        //{
                        //    int ress = ds.insertFailedUserLoginActivity(User.Username, User.Password, current_IP);

                        //   String message = "Username Or password is wrong ";
                        //    ModelState.AddModelError("", "Your Already login ");
                        //    return View();
                        //}
                        FormsAuthentication.SetAuthCookie(User.Username, false);


                        if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
                        {
                            return RedirectToAction("Index", "Home",
                                new {lang = "ar"});
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { lang = "en" });
                        }

                        //return RedirectToAction("Index", "Home");
                        

                    }
                    else
                    {
                        int res = ds.insertFailedUserLoginActivity(User.Username, User.Password, current_IP);
                       
                        message = "Username Or password is wrong ";
                        ModelState.AddModelError("", "Username Or password is wrong");
                    }

                }
                else if (userStatus.Equals("S"))
                {
                    var unSuccessfulLogin = ds.updateLastUnsuccessfulLogin(UserID.ToString());
                    Session["unSuccessfulLogin"] = unSuccessfulLogin;
                    ModelState.AddModelError("", GlobalRes.BlockedAccountMessage);
                }

                else if (userStatus.Equals("D"))
                {
                    var unSuccessfulLogin = ds.updateLastUnsuccessfulLogin(UserID.ToString());
                    Session["unSuccessfulLogin"] = unSuccessfulLogin;
                    ModelState.AddModelError("", GlobalRes.DeActivatedAccountMessage);
                }
                else 
                {
                    var unSuccessfulLogin = ds.updateLastUnsuccessfulLogin(UserID.ToString());
                    Session["unSuccessfulLogin"] = unSuccessfulLogin;
                    ModelState.AddModelError("", GlobalRes.IssueWithAccountMessage);
                }
            }
                catch (Exception EX)
                {
                    message = "System Error";
                    ModelState.AddModelError("", "System Error");
                }
            }
	        else
	        {
	            message = "All Fields are required ";
	            ModelState.AddModelError("", "Something is missing");
	        }


            if (Request.IsAjaxRequest())
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
            }
	        return View();
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