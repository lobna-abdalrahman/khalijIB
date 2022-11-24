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
        public string ToPhoneNo { get; set; }

        public string FavoritePhoneID { get; set; }
        public string FavoritePhoneName { get; set; }
        public string CardExp { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        //[Required(ErrorMessage = "Amount is Required")]
        public string TranAmount { get; set; }

        [Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "IPINRequired")]
        //[DataType(DataType.Password)]
        public string IPIN { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }

        public string BillAmount { get; set; }
        public string total { get; set; }
        public string contractNumber { get; set; }
        public string unbilledAmount { get; set; }

        [Display(Name = "Last Invoice Date")]
        public string LastInvoiceDate { get; set; }

        [Display(Name = "Last 4 Digits")]
        public string LastFourDigits { get; set; }
    }
}