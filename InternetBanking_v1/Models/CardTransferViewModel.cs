using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.App_LocalResources;
using Spire.Pdf.Exporting.XPS.Schema;

namespace InternetBanking_v1.Models
{
    public class CardTransferViewModel
    {
        public int CardID { get; set; }

        [Display(Name = "CardNumber", ResourceType = typeof(GlobalRes))]
        public string CardNumber { get; set; }

        public string CardName { get; set; }

        [NotMapped]
        public SelectList AccountList { get; set; }  

        public string CardExp { get; set; }

        /*[Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount is Required")]*/
        [Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "AmountRequired")]
        public string Amount { get; set; }

        [Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "IPINRequired")]
        [DataType(DataType.Password)]
        public string IPIN { get; set; }


        [Display(Name = "ToCardNumber", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ToCardNumberRequired")]
        public string ToCard { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }

    }
}