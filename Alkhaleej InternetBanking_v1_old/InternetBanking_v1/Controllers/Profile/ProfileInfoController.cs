using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers.Profile
{
    public class ProfileInfoController : BaseController
    {
        DataSource data = new DataSource();
        //
        // GET: /ProfileInfo/
        public ActionResult UpdateProfileInfo()
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

            ProfileInfoViewModel model=new ProfileInfoViewModel();
            var user_ID = Session["UserID"].ToString();
            int userID = Convert.ToInt32(user_ID);
            model = data.getUserInfo(userID);
            return View(model);
        }



        [HttpPost]
        public ActionResult UpdateProfileInfo(ProfileInfoViewModel model)
        {
            var user_ID = Session["UserID"].ToString();
            int userID = Convert.ToInt32(user_ID);

            if (ModelState.IsValid)
            {

                int res = data.updateUserInfo(user_ID, model.Email, model.Address);

                if (res == 1)
                {
                    //Session["UserTel"] = model.Mobile;
                    Session["email"] = model.Email;

                    //TempData["Success"] = true;
 
                    //ViewBag.ResponseStat = "Successful";
                    //ViewBag.ResponseMSG = "Your Infromation has been updated successfully";

                    Session["ResponseStat"] = "Secussfully";
                    Session["ResponseMSG"] = "Your Infromation has been updated successfully";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["updateSuccess"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Not Successful";
                    ViewBag.ResponseMSG = "Please Try again later";
                }
            }


            return View();
        }
	}
}