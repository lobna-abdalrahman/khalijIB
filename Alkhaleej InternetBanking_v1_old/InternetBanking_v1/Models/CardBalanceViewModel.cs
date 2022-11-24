using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class CardBalanceViewModel
    {
        public string PAN { get; set; }
        public string expDate { get; set; }
        public string IPIN { get; set; }

    }
}