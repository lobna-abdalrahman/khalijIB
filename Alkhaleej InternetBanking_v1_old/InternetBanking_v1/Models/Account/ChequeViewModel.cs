using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models.Account
{
    public class ChequeViewModel
    {
        public IEnumerable<Bal> Bals { get; set; }
        public IEnumerable<MyAccounts> MyAccounts { get; set; }
    }
}