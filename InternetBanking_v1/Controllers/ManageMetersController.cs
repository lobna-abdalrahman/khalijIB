using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class ManageMetersController : Controller
    {

        DataSource data = new DataSource();
        //
        // GET: /ManageMeters/
        public ActionResult ManageMeters()
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

            List<ManageMetersViewModel> accas = new List<ManageMetersViewModel>();
            accas = data.getUserMeters(user_id);
            return View(accas);
        }

        //Edit
        public ActionResult EditMeter(int id)
        {
            ManageMetersViewModel model = new ManageMetersViewModel();
            var user_ID = Session["UserID"].ToString();
            int userID = Convert.ToInt32(user_ID);
            model = data.getMeterInfo(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditMeter(int id, ManageMetersViewModel model)
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var user_ID = Session["UserID"].ToString();
            int userID = Convert.ToInt32(user_ID);

            if (ModelState.IsValid)
            {
                string MeterID = id.ToString();
                int res = data.updateMeterInfo(MeterID, model.MeterName, model.MeterNo);

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

        //Delete Meter..
        public ActionResult Delete(int id)
        {
            string MeterID = id.ToString();
            int res = data.DeleteMeterInfo(MeterID);

            if (res == 1)
            {

                TempData["Success"] = true;
                //ModelState.AddModelError("", tranRes.ToString());
                ViewBag.ResponseStat = "Successful";
                ViewBag.ResponseMSG = "Your Chosen Meter has been Deleted";

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
                return RedirectToAction("ManageMeters", new { lang = "ar" });
            }
            else
            {
                return RedirectToAction("ManageMeters");
            }

        }
	}
}