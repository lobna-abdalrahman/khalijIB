using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class ChequeRequestViewModel
    {
        public string AccountNumber { get; set; }

        public string ChequeBookSize { get; set; }
    }
}