using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class FavoritePhoneController : Controller
    {

        LoginLogic obj = new LoginLogic();
        //
        // GET: /FavoritePhone/
        public ActionResult AddPhone()
        {
            ModelState.Clear();
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddPhone(FavoritePhoneModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                string PhoneName = model.FavoritePhoneName;
                string PhoneNo = model.FavoritePhoneNumber;

                var user_ID = Session["UserID"].ToString();
                int userID = Convert.ToInt32(user_ID);

                try
                {
                    obj.AddPhone(PhoneName, PhoneNo, userID);
                    TempData["Success"] = true;

                    ViewBag.ResponseStat = GlobalRes.SuccesHeader;
                    ViewBag.ResponseMSG = GlobalRes.SuccessPhoneMessage;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if (e.Message.Contains("ORA-00001: unique constraint"))
                    {
                        ModelState.AddModelError("", GlobalRes.PhoneAlreadyExistsError);
                    }
                }
            }

            return View();
        }
	}
}