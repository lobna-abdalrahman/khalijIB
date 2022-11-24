using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;
using Spire.Pdf.Exporting.XPS.Schema;

namespace InternetBanking_v1.Models
{
    public class FavoriteCardModel
    {
        public int CardID { get; set; }

        [Display(Name = "CardName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "CardNameRequired")]
        public string CardName { get; set; }

        [Display(Name = "CardNumber", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "CardNumberRequired")]
        public string CardNumber { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }
        public string CardExp { get; set; }
        public int UserID { get; set; }
    }
}