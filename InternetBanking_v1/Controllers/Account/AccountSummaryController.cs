using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.Models.Account;

namespace InternetBanking_v1.Controllers.Account
{
	public class AccountSummaryController : Controller
	{
		//
		// GET: /AccountSummary/
		public ActionResult Index()
		{
			var accSummary = new AccountSummary();
			List<AccountSummary> acc = new List<AccountSummary>()
			{
				new AccountSummary{AccountNumber = "2882907",AccountType = "Saving Account",BranchName = "فرع الخرطوم",AvailableBalance = "500 SDG"},
				new AccountSummary{AccountNumber = "2882124",AccountType = "Saving Account",BranchName = "فرع الخرطوم",AvailableBalance = "700 SDG"},
				new AccountSummary{AccountNumber = "2882123",AccountType = "Saving Account",BranchName = "فرع الخرطوم",AvailableBalance = "500 SDG"},
				new AccountSummary{AccountNumber = "2882234",AccountType = "Current Account",BranchName = "فرع الخرطوم",AvailableBalance = "220 SDG"},
				new AccountSummary{AccountNumber = "2882543",AccountType = "Saving Account",BranchName = "فرع الخرطوم",AvailableBalance = "100 SDG"},
				new AccountSummary{AccountNumber = "2882889",AccountType = "Current Account",BranchName = "فرع الخرطوم",AvailableBalance = "10500 SDG"}
				
			};
			return View(acc);
		}
	}
}