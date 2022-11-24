using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class InvestmentViewModel
    {
        public string AccountNumber { get; set; }
        public string CHEQ_C_NO { get; set; }
        public string CHEQ_D_DATE { get; set; }
        public string CHEQ_C_STS { get; set; }
        public string CHEQ_F_AMOUNT { get; set; }


        public string BranchCode { get; set; }
        public string AccountTypeCode { get; set; }
        public string CurrencyCode { get; set; }
        //public string CHEQ_F_AMOUNT { get; set; }
        
        
    }
}