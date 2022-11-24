using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetBanking_v1.Models
{
    public class EPortsViewModel
    {
        // request components
        [Display(Name = "SessionID")]
        public string sessionid { get; set; } = "28";
        [Display(Name = "Pay Org ID")]
        public string PayOrgID { get; set; }
        [Display(Name = "Pay Service ID")]
        public string PayServiceID { get; set; }
        [Required]
        [Display(Name = "Invoice numebr")]
        public string PayCustomerCode { get; set; }
        [Display(Name = "channel Code")]
        public string channelCode { get; set; } = "44";
        [Display(Name = "Port")]
        [Required]
        public string selectedport { get; set; }
        [Display(Name = "Port Selection")]
        public List<SelectListItem> Ports = new List<SelectListItem>();

        //response components
        [Display(Name = "Voucher Number")]
        public string PaymentVoucherNo { get; set; }
        [Display(Name = "Customer Mobile")]
        public string CustomerMobile { get; set; }
        [Display(Name = "CustomerBalance")]
        public string CustomerBalance { get; set; }
        [Display(Name = "Payment Date")]
        public string PaymentDate { get; set; }
        [Display(Name = "Total Amount")]
        public string TotalAmount { get; set; }
        [Display(Name = "Customer ID")]
        public string CustomerId { get; set; }
        [Display(Name = "Fees Amount")]
        public string FeesAmount { get; set; }
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Required Amount")]
        public string RequiredAmount { get; set; }
        [Display(Name = "Pay Customer Name")]
        public string paycustomername { get; set; }
        [Display(Name = "Additional Refremce")]
        public string AdditionalRefrence { get; set; }
        [Display(Name = "Customer Found")]
        public string CustomerFound{ get; set; }
        [Display(Name = "From Account")]
        public string FromAccount { get; set; }
    }
}