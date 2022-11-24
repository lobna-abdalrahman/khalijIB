using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetBanking_v1.Controllers.AllTransfer
{
    public class AllTransferController : Controller
    {
        //
        // GET: /AllTransfer/
        public ActionResult AllTrasnfer()
        {
            return View();
        }

        // GET: /Own Transfer/
        public ActionResult OwnTransfer()
        {
            return PartialView();
        }

        // GET: /To other customers in the bank Transfer/
        public ActionResult BankCustomerTransfer()
        {
            return PartialView();
        }

        // GET: /Other bank's customers' Transfer/
        public ActionResult OtherBanksCustomerTransfer()
        {
            return PartialView();
        }
	}
}