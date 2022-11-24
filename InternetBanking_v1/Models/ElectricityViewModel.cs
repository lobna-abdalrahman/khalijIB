using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ElectricityViewModel
    {
        [Display(Name = "PAN")]
        public string CardNumber { get; set; }

        public string BillerName { get; set; }


      

        [RegularExpression(@"\d{11}", ErrorMessage = "Meter # must be 11 digits")]
        [Display(Name = "MeterNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "MeterNoRequired")]
        [StringLength(11, ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "MeterNoLong")]
        public string MeterNo { get; set; }

        public string MeterID { get; set; }
        public string MeterName { get; set; }
        public string CardExp { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "AmountRequired")]
        public string TranAmount { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "IPINRequired")]
        public string IPIN { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }

        //
        [Display(Name = "MeterNo", ResourceType = typeof(GlobalRes))]
        public string myMeterNumber { get; set; }

        [Display(Name = "NetAmount", ResourceType = typeof(GlobalRes))]
        public string netAmount { get; set; }

        [Display(Name = "Token", ResourceType = typeof(GlobalRes))]
        public string token { get; set; }

        [Display(Name = "WaterFees", ResourceType = typeof(GlobalRes))]
        public string waterFees { get; set; }

        [Display(Name = "MeterFees", ResourceType = typeof(GlobalRes))]
        public string meterFees { get; set; }

        [Display(Name = "CustomerName", ResourceType = typeof(GlobalRes))]
        public string customerName { get; set; }

        [Display(Name = "UnitsInKwh", ResourceType = typeof(GlobalRes))]
        public string unitsInKwh { get; set; }

        [Display(Name = "OperatorMsg", ResourceType = typeof(GlobalRes))]
        public string operatorMessage { get; set; }

        [Display(Name = "AccountNo", ResourceType = typeof(GlobalRes))]
        public string accountNo { get; set; }

        public int tranID { get; set; }

        [Display(Name = "Transaction_Status", ResourceType = typeof(GlobalRes))]
        public string tranStatus { get; set; }
    }
}