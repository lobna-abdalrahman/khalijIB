using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;

namespace InternetBanking_v1.Controllers
{
    public class ManageCardsController : Controller
    {

        DataSource data = new DataSource();
        //
        // GET: /ManageCards/
        public ActionResult ManageCards()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();
            if (Session["TempData"] != null)
            {
                TempData["Success"] = Session["TempData"].ToString();
                ViewBag.ResponseStat = Session["ResponseStat"].ToString();
                ViewBag.ResponseMSG = Session["ResponseMSG"].ToString();

                Session["TempData"] = "";
                Session["ResponseStat"] = "";
                Session["ResponseMSG"] = "";
            }
            List<ManageCardsViewModel> accas = new List<ManageCardsViewModel>();
            accas = data.getUserCards(user_id);


            return View(accas);
        }



        public ActionResult Edit(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            ManageCardsViewModel model = new ManageCardsViewModel();
            var user_ID = Session["UserID"].ToString();
            int userID = Convert.ToInt32(user_ID);
            model = data.getCardInfo(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, ManageCardsViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var user_ID = Session["UserID"].ToString();
            int userID = Convert.ToInt32(user_ID);

            if (ModelState.IsValid)
            {
                string CardID = id.ToString();
                int res = data.updateCardInfo(CardID, model.CardName, model.CardNo);

                if (res == 1)
                {
                   
                    TempData["Success"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Successful";
                    ViewBag.ResponseMSG = "Your Infromation has been updated successfully";
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



        public ActionResult Delete(int id)
        {
            string CardID = id.ToString();
            int res = data.DeleteCardInfo(CardID);

            if (res == 1)
            {

                TempData["Success"] = true;
                //ModelState.AddModelError("", tranRes.ToString());
                ViewBag.ResponseStat = "Successful";
                ViewBag.ResponseMSG = "Your Card has been Deleted";
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
                return RedirectToAction("ManageCards", new{lang = "ar"});
            }
            else
            {
                return RedirectToAction("ManageCards");
            }
            
        }


        
	}
}