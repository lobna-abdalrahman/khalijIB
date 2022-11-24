using InternetBanking_v1.App_LocalResources;
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

        public string OrderStatus { get; set; }
        public string AccountTypecode { get; set; }
        public string CurrCode { get; set; }
        public string BranchCode { get; set; }
        [Display(Name = "AccountNo", ResourceType = typeof(GlobalRes))]
        public String AccountNumber { get; set; }

        public String CustomerFound { get; set; }
         [Display(Name = "order_no", ResourceType = typeof(GlobalRes))]


        public String InvoiceNo { get; set; }

        [Display(Name = "Service type")]

        public String servicetype { get; set; }
        [Display(Name = "Customer Code")]

        public String PayCustomerCode { get; set; }

        [Display(Name = "Customer Name")]

        public String PayCustomerName { get; set; }
        [Display(Name = "Required Amount")]

        public String RequiredAmount { get; set; }
        [Display(Name = "Fees Amount")]

        public String FeesAmount { get; set; }
        [Display(Name = "Total Amount")]

        public String TotalAmount { get; set; }
        [Display(Name = "Collector Number")]

        public String CollectorNumber { get; set; }
        [Display(Name = "Currency Code")]

        public String CurrencyCode { get; set; }
        public String status { get; set; }
        [Display(Name = "Payment Method")]
        public List<SelectListItem> PaymentMethod { get; set; }
        public String paytype { get; set; }

    }
    public class BroojModel
    {
        public string AccountTypecode { get; set; }
        public string CurrCode { get; set; }
        public string BranchCode { get; set; }
        [Display(Name = "choose_your_account", ResourceType = typeof(GlobalRes))]
        public String AccountNumber { get; set; }

        public String CustomerFound { get; set; }
       [Display(Name = "order_no", ResourceType = typeof(GlobalRes))]

        public String InvoiceNo { get; set; }

        [Display(Name = "Service type")]

        public String servicetype { get; set; }
        [Display(Name = "Customer Code")]

        public String PayCustomerCode { get; set; }

        [Display(Name = "Customer Name")]

        public String PayCustomerName { get; set; }
        [Display(Name = "Required Amount")]

        public String RequiredAmount { get; set; }
        [Display(Name = "Fees Amount")]

        public String FeesAmount { get; set; }
        [Display(Name = "Total Amount")]

        public String TotalAmount { get; set; }
        [Display(Name = "Collector Number")]

        public String CollectorNumber { get; set; }
        [Display(Name = "Currency Code")]

        public String CurrencyCode { get; set; }
        public String status { get; set; }
        [Display(Name = "Payment Method")]
        public List<SelectListItem> PaymentMethod { get; set; }
        public String paytype { get; set; }
        [Display(Name = "Customer Branch")]
        public String Branch { get; set; }
        //[Required]
        [Display(Name = "Account Currency")]
        public String Currency { get; set; }
        //[Required]
        [Display(Name = "Customer Account Type")]
        public String AccountType { get; set; }
        [Display(Name = "Customer Name")]
        public String Customername { get; set; }
        [Display(Name = "Customer Account")]
        public String Customeraccount { get; set; }
        [Display(Name = "Customer ID")]
        public String CustomerID { get; set; }

        [Display(Name = "Payment Status")]
        public String Paymentstatus { get; set; }

        [Display(Name = "Payment Type")]
        public String PaymentType { get; set; }
        [Display(Name = "Payment Date")]
        public String PaymentDate { get; set; }
        [Display(Name = "Payment VoucherNo")]
        public String PaymentVoucherNo { get; set; }

    }
    public class PayDetails
    {

        public String CustomerFound { get; set; }
        [Display(Name = "Invoice No")]

        public String InvoiceNo { get; set; }

        [Display(Name = "Service type")]

        public String servicetype { get; set; }
        [Display(Name = "Customer Code")]

        public String PayCustomerCode { get; set; }

        [Display(Name = "Customer Name")]

        public String PayCustomerName { get; set; }
        [Display(Name = "Required Amount")]

        public String RequiredAmount { get; set; }
        [Display(Name = "Fees Amount")]

        public String FeesAmount { get; set; }
        [Display(Name = "Total Amount")]

        public String TotalAmount { get; set; }
        [Display(Name = "Collector Number")]

        public String CollectorNumber { get; set; }
        [Display(Name = "Currency Code")]

        public String CurrencyCode { get; set; }
        public String status { get; set; }
        [Display(Name = "Payment Method")]
        public List<SelectListItem> PaymentMethod { get; set; }
        public String paytype { get; set; }
        [Display(Name = "Customer Branch")]
        public String Branch { get; set; }
        //[Required]
        [Display(Name = "Account Currency")]
        public String Currency { get; set; }
        //[Required]
        [Display(Name = "Customer Account Type")]
        public String AccountType { get; set; }
        [Display(Name = "Customer Name")]
        public String Customername { get; set; }
        [Display(Name = "Customer Account")]
        public String Customeraccount { get; set; }
        [Display(Name = "Customer ID")]
        public String CustomerID { get; set; }

        [Display(Name = "Payment Status")]
        public String Paymentstatus { get; set; }

        [Display(Name = "Payment Type")]
        public String PaymentType { get; set; }
        [Display(Name = "Payment Date")]
        public String PaymentDate { get; set; }
        [Display(Name = "Payment VoucherNo")]
        public String PaymentVoucherNo { get; set; }

    }

}