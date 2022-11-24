using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class MOHESudViewModel
    {
        [Display(Name = "PAN")]
        public string CardNumber { get; set; }


        public string BillerName { get; set; }


        [Display(Name = "MOHE_StudentNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "MOHE_StudentNoRequired")]
        public string SETNUMBER { get; set; }
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

       
        //public string SETNUMBER { get; set; }
        [Display(Name = "CourseID", ResourceType = typeof(GlobalRes))]
        public string STUDCOURSEID { get; set; }

        [Display(Name = "FormKind", ResourceType = typeof(GlobalRes))]
        public string STUDFORMKIND { get; set; }



        //----
        [Display(Name = "formNo", ResourceType = typeof(GlobalRes))]
        public string formNo { get; set; }

        [Display(Name = "receiptNo", ResourceType = typeof(GlobalRes))]
        public string receiptNo { get; set; }

        [Display(Name = "englishName", ResourceType = typeof(GlobalRes))]
        public string englishName { get; set; }

        [Display(Name = "arabiName", ResourceType = typeof(GlobalRes))]
        public string arabiName { get; set; }
        [Display(Name = "dueAmount", ResourceType = typeof(GlobalRes))]
        public string dueAmount { get; set; }
    }
}