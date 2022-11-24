using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class SalaryTransferViewModel
    {
        public string AccountNumber { get; set; }
        public string excelFile { get; set; }


        [Display(Name = "SalaryAccountNo", ResourceType = typeof(GlobalRes))]
        public string salary_account_no { get; set; }

        //[Display(Name = "Amount")]
        [Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        public string salary_amount { get; set; }

        // [Display(Name = "From File")]
        [Display(Name = "FromFile", ResourceType = typeof(GlobalRes))]
        public string salary_file_name { get; set; }

        //[Display(Name = "From Account No.")]
        [Display(Name = "FromAccountNo", ResourceType = typeof(GlobalRes))]
        public string salary_comp_act { get; set; }


        //[Display(Name = "Account No.")]
        [Display(Name = "AccountNo", ResourceType = typeof(GlobalRes))]
        public string successfulAccount { get; set; }


        //[Display(Name = "Status")]
        [Display(Name = "Status", ResourceType = typeof(GlobalRes))]
        public string successfulStatus { get; set; }



    }
}