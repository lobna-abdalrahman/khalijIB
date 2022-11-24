using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ManageMetersViewModel
    {

        [Display(Name = "MeterName", ResourceType = typeof(GlobalRes))]
        public string MeterName { get; set; }

        [Display(Name = "MeterNo", ResourceType = typeof(GlobalRes))]
        public string MeterNo { get; set; }

        [Display(Name = "MeterID", ResourceType = typeof(GlobalRes))]
        public string MeterID { get; set; }

        public string sts { get; set; }
    }
}