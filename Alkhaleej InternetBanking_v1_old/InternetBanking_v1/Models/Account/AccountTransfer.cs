using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace InternetBanking_v1.Models.Account
{
    public class AccountTransfer
    {
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string PhoneNumber { get; set; }
       public string Amount { get; set; }
    }
}