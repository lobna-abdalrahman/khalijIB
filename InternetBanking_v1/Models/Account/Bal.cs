using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace InternetBanking_v1.Models.Account
{
    public class Bal
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string BranchName { get; set; }
        public string AvailableBalance { get; set; }
        public string SumAccounts { get; set; }
    }

   
}