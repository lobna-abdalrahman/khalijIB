using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class EGovViewModel
    {
        [Display(Name = "PAN")]
        public string CardNumber { get; set; }

        //public string BillerName { get; set; }


        //[Display(Name = "Meter Number")]
        //[Required(ErrorMessage = "Phone Number is Required")]
        //[RegularExpression(@"\d{11}", ErrorMessage = "Meter # must be 11 digits")]
        //public string MeterNo { get; set; }

        [RegularExpression(@"\d{10}", ErrorMessage = "Phone # must be 10 digits")]
        [Display(Name = "Mobile", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "MobileRequired")]
        public string PhoneNo { get; set; }
        public string CardExp { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "AmountRequired")]
        public string TranAmount { get; set; }

        [Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "IPINRequired")]
        [DataType(DataType.Password)]
        public string IPIN { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }


        [Display(Name = "EGov_InvoiceNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EGov_InvoiceNo_Required")]
        public string InvoiceNo { get; set; }

        //
        public string UnitName { get; set; }
        public string TotalAmount { get; set; }
        public string ServiceName { get; set; }
        public string InvoiceExpiry { get; set; }
        public string DueAmount { get; set; }
        public string ReferenceId { get; set; }
        public string InvoiceStatus { get; set; }
        public string PayerName { get; set; }
    }
}