using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InternetBanking_v1.Models.DB;
using InternetBanking_v1.Models.EntityManager;
using InternetBanking_v1.Models.ViewModels;

namespace InternetBanking_v1.Controllers
{
    public class UserAccountController : Controller
    {
        //
        // GET: /UserAccount/
        public ActionResult SignUp()
        {
            return View();
        }

                //SIGN-UP
        [HttpPost]
        public ActionResult SignUp(UserSignUpView USV)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();

                if (!UM.IsLoginNameExist(USV.LoginName))
                {
                    UM.AddUserAccount(USV);
                    FormsAuthentication.SetAuthCookie(USV.FirstName, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","Login Name already Taken");
                }
            }
            return View();
        }
	}
}