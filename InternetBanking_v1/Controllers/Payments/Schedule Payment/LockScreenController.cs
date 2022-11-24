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
namespace InternetBanking_v1.Controllers.Payments.Schedule_Payment
{
    public class LockScreenController : Controller
    {
        DataSource ds = new DataSource();
        public String mySessionUsername;
	   public UserAccount userAcc;
	    public User userModel;
	    public static string DataRead;
        
        LoginLogic userBL = new LoginLogic();
        BalanceLogic balanceLogic = new BalanceLogic();
        MyChequeStatus data = new MyChequeStatus();
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
            string message = "";
            if (ModelState.IsValid)
            {
                model.Username = Session["LockedUser"].ToString();
                model.Password = pwdEncrypt(model.Password);

                int res = ds.UnlockUserSession(model.Username, model.Password);
                if (res == 1)
                {
                    try
                    {
                        /////////////////////////
                        User User = new User();
                        User.Password = model.Password;
                        User.Username = model.Username;
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
                            ModelState.AddModelError("", "Wrong username or password");
                            res = ds.insertFailedUserLoginActivity(User.Username, User.Password, current_IP);
                            return View();
                        }
                        if (userStatus.Equals("B"))
                        {
                            ModelState.AddModelError("", "This User is Blocked. Please Contact Your Branch.");
                            res = ds.insertFailedUserLoginActivity(User.Username, User.Password, current_IP);
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
                                Menu = (IEnumerable<Menu>)Session["_Menu"];
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
                                res = ds.insertSuccessUserLoginActivity(User.Username, User.Password, current_IP, UserID.ToString());

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
                                        new { lang = "ar" });
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Home", new { lang = "en" });
                                }

                                //return RedirectToAction("Index", "Home");


                            }
                            else
                            {
                                res = ds.insertFailedUserLoginActivity(User.Username, User.Password, current_IP);

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





                    // return RedirectToAction("Login", "Login");
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