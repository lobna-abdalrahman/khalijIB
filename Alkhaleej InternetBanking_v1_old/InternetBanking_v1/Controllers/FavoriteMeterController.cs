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
    public class FavoriteMeterController : Controller
    {
        LoginLogic obj = new LoginLogic();
        //
        // GET: /FavoriteMeter/
        public ActionResult AddMeter()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddMeter(FavoriteMeterModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                string meterName = model.MeterName;
                string meterNo = model.MeterNumber;

                var user_ID = Session["UserID"].ToString();
                int userID = Convert.ToInt32(user_ID);

                try
                {
                    obj.AddMeter(meterName,meterNo,userID);
                    TempData["Success"] = true;

                    ViewBag.ResponseStat = GlobalRes.SuccesHeader;
                    ViewBag.ResponseMSG = GlobalRes.SuccessMeterMessage;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if (e.Message.Contains("ORA-00001: unique constraint"))
                    {
                        ModelState.AddModelError("", GlobalRes.MeterAlreadyExistsError);
                    }
                    /*ModelState.AddModelError("", e.Message);*/
                    
                }
            }

            return View();
        }
	}
}