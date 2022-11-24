using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 

namespace InternetBanking_v1.Models
{
    public class OrderDatials
    {
        public string CustomerFound { get; set; }
        [Display(Name = "Invoice No")]

        public string InvoiceNo { get; set; }

        [Display(Name = "Service type")]

        public string servicetype { get; set; }
        [Display(Name = "Customer Code")]

        public string PayCustomerCode { get; set; }

        [Display(Name = "Collector Name")]

        public string PayCustomerName { get; set; }
        [Display(Name = "Required Amount")]

        public string RequiredAmount { get; set; }
        [Display(Name = "Fees Amount")]

        public string FeesAmount { get; set; }
        [Display(Name = "Total Amount")]

        public string TotalAmount { get; set; }
        [Display(Name = "Collector Number")]

        public string CollectorNumber { get; set; }
        [Display(Name = "Currency Code")]

        public string CurrencyCode { get; set; }
        public string status { get; set; }
        [Display(Name = "Payment Method")]
        public List<SelectListItem> PaymentMethod { get; set; }
        public String paytype { get; set; }

    }
    public class BroojModel
    {

        public string CustomerFound { get; set; }
        [Display(Name = "Invoice No")]

        public string InvoiceNo { get; set; }

        [Display(Name = "Service type")]

        public string servicetype { get; set; }
        [Display(Name = "Customer Code")]

        public string PayCustomerCode { get; set; }

        [Display(Name = "Collector Name")]

        public string PayCustomerName { get; set; }
        [Display(Name = "Required Amount")]

        public string RequiredAmount { get; set; }
        [Display(Name = "Fees Amount")]

        public string FeesAmount { get; set; }
        [Display(Name = "Total Amount")]

        public string TotalAmount { get; set; }
        [Display(Name = "Collector Number")]

        public string CollectorNumber { get; set; }
        [Display(Name = "Currency Code")]

        public string CurrencyCode { get; set; }
        public string status { get; set; }
        [Display(Name = "Payment Method")]
        public List<SelectListItem> PaymentMethod { get; set; }
        public String paytype { get; set; }
        [Display(Name = "Customer Branch")]
        public string Branch { get; set; }
        //[Required]
        [Display(Name = "Account Currency")]
        public string Currency { get; set; }
        //[Required]
        [Display(Name = "Customer Account Type")]
        public string AccountType { get; set; }
        [Display(Name = "Customer Name")]
        public String Customername { get; set; }
        [Display(Name = "Customer Account")]
        public String Customeraccount { get; set; }
        [Display(Name = "Customer ID")]
        public String CustomerID { get; set; }

        [Display(Name = "Payment Status")]
        public string Paymentstatus { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }
        [Display(Name = "Payment Date")]
        public string PaymentDate { get; set; }
        [Display(Name = "Payment VoucherNo")]
        public string PaymentVoucherNo { get; set; }

    }
    public class PayDetails
    {

        public string CustomerFound { get; set; }
        [Display(Name = "Invoice No")]

        public string InvoiceNo { get; set; }

        [Display(Name = "Service type")]

        public string servicetype { get; set; }
        [Display(Name = "Customer Code")]

        public string PayCustomerCode { get; set; }

        [Display(Name = "Collector Name")]

        public string PayCustomerName { get; set; }
        [Display(Name = "Required Amount")]

        public string RequiredAmount { get; set; }
        [Display(Name = "Fees Amount")]

        public string FeesAmount { get; set; }
        [Display(Name = "Total Amount")]

        public string TotalAmount { get; set; }
        [Display(Name = "Collector Number")]

        public string CollectorNumber { get; set; }
        [Display(Name = "Currency Code")]

        public string CurrencyCode { get; set; }
        public string status { get; set; }
        [Display(Name = "Payment Method")]
        public List<SelectListItem> PaymentMethod { get; set; }
        public String paytype { get; set; }
        [Display(Name = "Customer Branch")]
        public string Branch { get; set; }
        //[Required]
        [Display(Name = "Account Currency")]
        public string Currency { get; set; }
        //[Required]
        [Display(Name = "Customer Account Type")]
        public string AccountType { get; set; }
        [Display(Name = "Customer Name")]
        public String Customername { get; set; }
        [Display(Name = "Customer Account")]
        public String Customeraccount { get; set; }
        [Display(Name = "Customer ID")]
        public String CustomerID { get; set; }

        [Display(Name = "Payment Status")]
        public string Paymentstatus { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }
        [Display(Name = "Payment Date")]
        public string PaymentDate { get; set; }
        [Display(Name = "Payment VoucherNo")]
        public string PaymentVoucherNo { get; set; }

    }

}