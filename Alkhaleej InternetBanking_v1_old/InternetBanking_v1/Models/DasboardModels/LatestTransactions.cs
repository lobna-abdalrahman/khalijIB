using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models.DasboardModels
{
    public class LatestTransactions 
    {
        public int TranId { get; set; }
        public string TranName { get; set; }
        public string TranStatus { get; set; }

        public string TranAmount { get; set; }

        public string TranResult { get; set; }

        
    }
}