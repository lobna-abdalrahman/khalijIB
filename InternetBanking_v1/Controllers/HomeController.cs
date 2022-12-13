using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Helper;
using InternetBanking_v1.Models.Account;
using InternetBanking_v1.Models.DasboardModels;
using InternetBanking_v1.Models.Login;
using InternetBanking_v1.Models.Profile;
using InternetBanking_v1.Models.ViewModels;
using InternetBanking_v1.Providers;
using InternetBanking_v1.Repository;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Menu = InternetBanking_v1.Models.Menu;

namespace InternetBanking_v1.Controllers
{
    public class HomeController : BaseController //Controller
    {

        //Hosted web API REST Service base url  
        string Baseurl = "http://192.168.0.119:8080/";

        BalanceLogic balanceLogic = new BalanceLogic();

        LoginLogic obj = new LoginLogic();
        DataSource ds = new DataSource();
        string primaryiban;

        //IBdb con = new IBdb();

        #region Fields

        private readonly string tranID = "TR9842";
        LoginLogic userBL = new LoginLogic();
        //BalanceLogic balanceLogic = new BalanceLogic();


        #endregion

        public  ActionResult Index()
        {

            DataTable dt = new DataTable();

            dt = userBL.GetMenuData(0);
            userBL.PopulateMenu(dt, 0, null);

            if (Session["ResponseStat"] != null)
            {
                TempData["Success"] = true;
                ViewBag.ResponseStat = Session["ResponseStat"].ToString();
                ViewBag.ResponseMSG = Session["ResponseMSG"].ToString();

                Session["ResponseStat"] = null;
                Session["ResponseMSG"] = null;
            }

            



            ProfileModelInfo info = new ProfileModelInfo();
            var info_custID = 1;
            String status = "";

            Session["status"] = "";
            status = Session["status"].ToString();

            if (Session.IsNewSession)
            {
                //if(!status.Equals("1"))
                return RedirectToAction("Login", "Login");
            }

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string mySessionUserName = Session["name"].ToString();
            string info_username = mySessionUserName;

            var info_currentIP = Session["CurrentIP"].ToString();
            var info_avatar_url = "~/Content/dist/img/avatar.png";

            string info_successful_login = Session["date"].ToString();

            var info_unsuccessful_login = "";

            if (Session["unSuccessfulLogin"] == null)
            {
                info_unsuccessful_login = "--";
            }
            else
            {
                 info_unsuccessful_login = Session["unSuccessfulLogin"].ToString();//"27/7/2017";
            }

            var info_sumAccounts = "6";
            var info_name = "";
            if (Session["name"] == null)
            {
                info_name = "";
            }
            else
            {
                info_name = Session["name"].ToString();
            }

            string UserBalance = "{\"tranDateTime\": \"170322061748\",\"IBAN\": \"SD9044000002780001\",\"Currency\": \"938\",\"bal\": \"6835.3\",\"uuid\": \"someuuid\",\"errormsg\": \"Secussfully\",\"errorcode\": \"1\"}"; //
            //string UserBalance = balanceLogic.GetUserBalance(Session["additionalReference"].ToString());
            //string UserBalance = "{\"tranDateTime\":\"021021125855\",\"IBAN\":\"SD4244000000830001\",\"bal\":\"27174.48\",\"uuid\":\"someuuid\",\"errormsg\":\"Secussfully\",\"errorcode\":\"1\"}";
            //string UserBalance = "{\"tranDateTime\":\"201021012336\",\"IBAN\":\"SD1544020011500001\",\"Currency\":\"938\",\"bal\":\"6.21968486E7\",\"uuid\":\"anything\",\"errormsg\":\"Secussfully\",\"errorcode\":\"1\"}";
            JObject balanceresponse = JObject.Parse(UserBalance);

            var mydata = new ProfileModelInfo
            {
                Username = info_username,
                CustomerProfileId = info_custID,
                CurrentIP = info_currentIP,
                AvatarUrl = info_avatar_url,
                LastSuccessfulLogin = info_successful_login,
                LastUnsuccessfulLogin = info_unsuccessful_login,
                name = info_name,
                primaryiban = balanceresponse.GetValue("IBAN").ToString()
            };


            ViewBag.Title = "Dashboard";
            //if (connected.Equals(""))
            //{
            //    ViewBag.Title = "My Dashboard";
            //}
            ViewBag.SumAccount = info_sumAccounts;

            return View(mydata);
            //return View();
        }




        //////////////////////////////Zainab's try run/////////////////////////

        public JsonResult CheckType(AccountStatementViewModel Model)
        {

            if (Model.StatementType == "2")
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json("", JsonRequestBehavior.AllowGet);
        }


        /// 



        /**
         * Primary Balance Get from Webservice
         */
  //      [ChildActionOnly]
        public ActionResult PrimaryBalance()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

           // List<AccountSummary> accInfo = new List<AccountSummary>();
            Bal accSummary = new Bal();

            string additionalReference = Session["additionalReference"].ToString();
            string UserBalance = balanceLogic.GetUserBalance(additionalReference);
            //string UserBalance = "{\"tranDateTime\":\"201021012336\",\"IBAN\":\"SD1544020011500001\",\"Currency\":\"938\",\"bal\":\"6.21968486E7\",\"uuid\":\"anything\",\"errormsg\":\"Secussfully\",\"errorcode\":\"1\"}";
            JObject jobj = new JObject();

            try
            {
                jobj = JObject.Parse(UserBalance);
                dynamic x = jobj;
                var m = x.bal;
                accSummary.AvailableBalance = m; //+ " " + ds.GetCurrencyName(primaryAccount.Substring(10, 3));
                if(accSummary.AvailableBalance != "N/A")
                {
                    Double value = Convert.ToDouble(accSummary.AvailableBalance);
                    accSummary.AvailableBalance = String.Format("{0:N}", value);
                }
            }
            catch (Exception e)
            {
                accSummary.AvailableBalance = "0"; //+ " " + ds.GetCurrencyName(primaryAccount.Substring(10, 3));
                //Console.WriteLine(e);

            }

           

            return PartialView(accSummary);
           // return View(accSummary);
        }


        /**
         * Number Of Transfers...
         */
        public ActionResult NumberOfTransfers()
        {
            string user_id = Session["UserID"].ToString();

            string count = ds.GetTransferCount(user_id);

            ViewBag.TranCount = count;


            return PartialView();
        }


        /**
         * Number Of Transfers...
         */
        public ActionResult NumberOfAccounts()
        {
            string user_id = Session["UserID"].ToString();

            string count = ds.GetAccountsCount(user_id);

            ViewBag.AccCount = count;


            return PartialView();
        }




        /**
         * Latest Transactions
         */
        public virtual ActionResult LatestTransactions()
        {
            string user_id = Session["UserID"].ToString();
            List<LatestTransactions> trans = new List<LatestTransactions>();
            //{
            //    new LatestTransactions {TranId = "TR9842", TranName = "Electricity", TranStatus = "Completed"},
            //    new LatestTransactions {TranId = "TR1848", TranName = "Custom Payment", TranStatus = "Pending"},
            //    new LatestTransactions {TranId = "TR7429", TranName = "Bill Payment", TranStatus = "Completed"},
            //    new LatestTransactions {TranId = "OR7420", TranName = "Card Transfer", TranStatus = "Cancelled"}
            //};
            trans = ds.getTransactions(user_id);

            return PartialView(trans);
        }




        //[Authorize(Roles = "Admin")]
        public virtual ActionResult TopMenu()
        {
            int myRole = obj.userRole;
            if (myRole == 1)
            {
                //Roles.AddUserToRole("user", "Admin");
            }
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.username = Session["username"].ToString();

            IEnumerable<Menu> Menu = null;

            if (Session["_Menu"] != null)
            {
                Menu = (IEnumerable<Menu>)Session["_Menu"];
            }
            else
            {
                return RedirectToAction("Login", "Login");
                //Menu = MenuData.GetMenus(userBL.userRole.ToString());// pass employee id here
                //Session["_Menu"] = Menu;
            }
            return PartialView(Menu);
        }

        public ActionResult Logout()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string user_id = Session["UserID"].ToString();

                int res = ds.UserLogout(user_id);
                Session["status"] = "0";
                Session.Abandon();

                return RedirectToAction("Login", "Login");
            }

            
        }


        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }    




        
    }
}
