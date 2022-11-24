using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;
using Spire.Pdf.Exporting.XPS.Schema;

namespace InternetBanking_v1.Models
{
    public class TelecomPostPaidViewModel
    {

        [Display(Name = "CardNumber", ResourceType = typeof(GlobalRes))]
        public string CardNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "BillerNameRequired")]
        public string BillerName { get; set; }


        [Display(Name = "ToPhoneNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ToPhoneNoRequired")]

        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NumericValidationError")]
        [StringLength(10, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "invalidphone", MinimumLength = 10)]
        public string ToPhoneNo { get; set; }

        public string FavoritePhoneID { get; set; }
        public string FavoritePhoneName { get; set; }
        public string CardExp { get; set; }


        //[Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        ////[Required(ErrorMessage = "Amount is Required")]

        //[RegularExpression("^[0-9]*(\\.[0-9]{0,6})$", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NumericValidationError")]
    
        //public string TranAmount { get; set; }

        //[Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        //[Required(ErrorMessageResourceType = typeof(GlobalRes),
        //    ErrorMessageResourceName = "IPINRequired")]
        //[RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NumericValidationError")]
        //[StringLength(4, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ipinLength", MinimumLength = 4)]
        //[DataType(DataType.Password)]
        [Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NumericValidationError")]
        //[Required(ErrorMessage = "Amount is Required")]
        public string TranAmount { get; set; }

      
        [Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "IPINRequired")]
        //[DataType(DataType.Password)]
        //[RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NumericValidationError")]
        //[StringLength(4, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ipinLength", MinimumLength = 4)]
        public string IPIN { get; set; }
      //  public string IPIN { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }
        [Display(Name = "BillAmount", ResourceType = typeof(GlobalRes))]
        public string BillAmount { get; set; }

        [Display(Name = "TotalAmount", ResourceType = typeof(GlobalRes))]
        public string total { get; set; }

        [Display(Name = "ContractNo", ResourceType = typeof(GlobalRes))]
        public string contractNumber { get; set; }

        [Display(Name = "unbilledAmount", ResourceType = typeof(GlobalRes))]
        public string unbilledAmount { get; set; }

        [Display(Name = "LastInvoiceDate", ResourceType = typeof(GlobalRes))]
        public string LastInvoiceDate { get; set; }
        [Display(Name = "LastFourDigits", ResourceType = typeof(GlobalRes))]
        public string LastFourDigits { get; set; }

        [Display(Name = "billAmount", ResourceType = typeof(GlobalRes))]
        public string billAmount1 { get; set; }
        [Display(Name = "SubscriberID", ResourceType = typeof(GlobalRes))]
        public string SubscriberID { get; set; }
        //public string BillAmount { get; set; }
       // public string total { get; set; }
        //public string contractNumber { get; set; }
        //public string unbilledAmount { get; set; }

        //[Display(Name = "Last Invoice Date")]
        //public string LastInvoiceDate { get; set; }

        //[Display(Name = "Last 4 Digits")]
        //public string LastFourDigits { get; set; }
    }
}