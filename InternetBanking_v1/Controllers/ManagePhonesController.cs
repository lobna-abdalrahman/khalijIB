using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class ManagePhonesController : Controller
    {
        DataSource data = new DataSource();
        //
        // GET: /ManagePhones/
        public ActionResult ManagePhones()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Session["TempData"] != null)
            {
                TempData["Success"] = Session["TempData"].ToString();
                ViewBag.ResponseStat = Session["ResponseStat"].ToString();
                ViewBag.ResponseMSG = Session["ResponseMSG"].ToString();

                Session["TempData"] = "";
                Session["ResponseStat"] = "";
                Session["ResponseMSG"] = "";
            }


            string user_id = Session["UserID"].ToString();

            List<ManagePhonesViewModel> accas = new List<ManagePhonesViewModel>();
            accas = data.getUserPhones(user_id);

            return View(accas);
        }

        //Edit..
        public ActionResult EditPhone(int id)
        {
            ManagePhonesViewModel model = new ManagePhonesViewModel();
            var user_ID = Session["UserID"].ToString();
            int userID = Convert.ToInt32(user_ID);
            model = data.getPhoneInfo(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPhone(int id,ManagePhonesViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var user_ID = Session["UserID"].ToString();
            int userID = Convert.ToInt32(user_ID);

            if (ModelState.IsValid)
            {
                string PhoneID = id.ToString();
                int res = data.updatePhoneInfo(PhoneID, model.FavoritePhoneName, model.FavoritePhoneNo);

                if (res == 1)
                {

                    TempData["Success"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Successful";
                    ViewBag.ResponseMSG = "Your Information has been updated successfully";
                }
                else
                {
                    TempData["Success"] = true;
                    ViewBag.ResponseStat = "Not Successful";
                    ViewBag.ResponseMSG = "Please Try again later";
                }
            }

            return View();
        }


        //Delete Phone Number...
        public ActionResult Delete(int id)
        {
            string PhoneID = id.ToString();
            int res = data.DeletePhoneInfo(PhoneID);

            if (res == 1)
            {

                TempData["Success"] = true;
                //ModelState.AddModelError("", tranRes.ToString());
                ViewBag.ResponseStat = "Successful";
                ViewBag.ResponseMSG = "Your Chosen Phone Number has been Deleted";

                Session["TempData"] = TempData["Success"].ToString();
                Session["ResponseStat"] = ViewBag.ResponseStat;
                Session["ResponseMSG"] = ViewBag.ResponseMSG;
            }
            else
            {
                TempData["Success"] = true;
                ViewBag.ResponseStat = "Not Successful";
                ViewBag.ResponseMSG = "Please Try again later";

                Session["TempData"] = TempData["Success"].ToString();
                Session["ResponseStat"] = ViewBag.ResponseStat;
                Session["ResponseMSG"] = ViewBag.ResponseMSG;
            }

            if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
            {
                return RedirectToAction("ManagePhones", new { lang = "ar" });
            }
            else
            {
                return RedirectToAction("ManagePhones");
            }
        }
	}
}