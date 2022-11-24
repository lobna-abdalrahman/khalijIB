using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{

    public class FavoriteCardController : BaseController
    {

        FavoriteCardModel favCard = new FavoriteCardModel();
        LoginLogic obj = new LoginLogic();

        //
        // GET: /FavoriteCard/
        public ActionResult AddCard()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddCard(FavoriteCardModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid) { 
                string cardName = model.CardName;
                string cardNumber = model.CardNumber;

                model.CardExp = model.Year + model.Month;
                string cardExp = model.CardExp;
                //DateTime dt = Convert.ToDateTime(cardExp);
                //string st = dt.ToString("yyMM");

                var user_ID = Session["UserID"].ToString();
                int userID = Convert.ToInt32(user_ID);

                try
                {
                    obj.AddCard(cardName, cardNumber, cardExp, userID);

                    TempData["Success"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Successful";
                    ViewBag.ResponseMSG = "Your Card has been Added successfully";
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("",e.Message);
                }
            }

            return View();      
        }
	}
}