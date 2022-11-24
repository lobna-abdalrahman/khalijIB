using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class ReceiptModel
    {
        public String fromaccount { get; set; }
        public String toaccount { get; set; }
        public String acuirerTransactionFees { get; set; }
        public String TransactionDateTime { get; set; }
        public String Date { get; set; }
        public String Time { get; set; }
        public String dynamicFees { get; set; }
        public String toCard { get; set; }
        public String expDate { get; set; }
        public String issuerTransactionFees { get; set; }
        public String fromCard { get; set; }
        public String TransactionAmount { get; set; }
        public String currency { get; set; }
        public String newvariable { get; set; }
        public String receiptserial { get; set; }
        public JObject billInfo;
        //--------------------------------------------------------
        [Display(Name = "Form Number")]
        public String formNo { get; set; }
        [Display(Name = "Student Number")]
        public String studentNo { get; set; }

        [Display(Name = "Receipt Number")]
        public String ReceiptNumber { get; set; }
        [Display(Name = "English Name")]
        public String englishName { get; set; }
        [Display(Name = "Arabic Name")]
        public String arabicName { get; set; }

        [Display(Name = "Amount")]
        public String Amount { get; set; }
        [Display(Name = "Declarant NAME")]
        public String DeclarantNAME { get; set; }
        [Display(Name = "Instance ID")]
        public String InstanceID { get; set; }
        [Display(Name = "Proc Status")]
        public String ProcStatus { get; set; }
        [Display(Name = "Proc Error")]
        public String ProcError { get; set; }
        [Display(Name = "Office")]
        public String Office { get; set; }
        [Display(Name = "Declarant")]
        public String Declarant { get; set; }
        [Display(Name = "DECNBER")]
        public String DECNBER { get; set; }
        [Display(Name = "Transaction")]
        public String Transaction { get; set; }
        [Display(Name = "Year")]
        public String Year { get; set; }
        [Display(Name = "DECSER")]
        public String DECSER { get; set; }

        [Display(Name = "E-15 Receipt Number")]
        public String E15ReceiptNumber { get; set; }
        [Display(Name = "Declarant Number")]
        public String DeclarantNumber { get; set; }
        [Display(Name = "BankCode")]
        public String BankCode { get; set; }
        [Display(Name = "Declarant Name")]
        public String Declarant_Name { get; set; }
        [Display(Name = "DecNo")]
        public String DecNo { get; set; }
        [Display(Name = "Meter Fees ")]
        public String meterFees { get; set; }
        [Display(Name = "Net Amount")]
        public String netAmount { get; set; }
        [Display(Name = "Customer Name")]
        public String customerName { get; set; }
        [Display(Name = "Reciver Name")]
        public String ReciverName { get; set; }
        [Display(Name = "Account Number ")]
        public String accountNo { get; set; }
        [Display(Name = "Meter Number ")]
        public String meterNumber { get; set; }
        [Display(Name = "Token Number ")]
        public String token { get; set; }
        [Display(Name = "Units In KWh")]
        public String unitsInKWh { get; set; }
        [Display(Name = "Water Fees")]
        public String waterFees { get; set; }
        [Display(Name = "Opertor Message")]
        public String opertorMessage { get; set; }
        [Display(Name = "Reference ID")]
        public String ReferenceId { get; set; }
        [Display(Name = "Unit Name")]
        public String UnitName { get; set; }
        [Display(Name = "Service Name")]
        public String ServiceName { get; set; }
        [Display(Name = "Payer Name")]
        public String PayerName { get; set; }
        [Display(Name = "Total Amount")]
        public String TotalAmount { get; set; }
        [Display(Name = "Receipt Number")]
        public String receiptNo { get; set; }
        [Display(Name = "Contract Number")]
        public String contractNumber { get; set; }
        [Display(Name = "Unbilled Amount")]
        public String unbilledAmount { get; set; }
        [Display(Name = "Total Amount")]
        public String total_Amount { get; set; }
        [Display(Name = "Last Invoice Date")]
        public String lastInvoiceDate { get; set; }
        [Display(Name = "Last 4 Digits")]
        public String last4Digits { get; set; }
        [Display(Name = "Billed Amount")]
        public String billedAmount { get; set; }
        [Display(Name = "Sub New Balance")]
        public String subNewBalance { get; set; }

        [Display(Name = "Transaction Refrence")]
        public String Transrefrence { get; set; }

        [Display(Name = "Bill Amount ")]
        public String BillAmount { get; set; }
        [Display(Name = "Total")]
        public String total { get; set; }
        [Display(Name = "SubscriberID")]
        public String SubscriberID { get; set; }
        [Display(Name = "Bill Amount")]
        public String bill_Amount { get; set; }
        [Display(Name = "Due Amount")]
        public String DueAmount { get; set; }
        [Display(Name = "Due Amount")]
        public String due_Amount { get; set; }
        [Display(Name = "Invoice Expiry")]
        public String InvoiceExpiry { get; set; }
        [Display(Name = "Invoice Status")]
        public String InvoiceStatus { get; set; }
        public String payeeid { get; set; }
        public String paymentinfo { get; set; }
        public int responseCode { get; set; }
        public String responseMessage { get; set; }
        public String responseStatus { get; set; }
        public JObject balance = new JObject();//Map contains two fields: "available", "leger"
        public String leger { get; set; }
        public String available { get; set; }

        public String frombranch { get; set; }
        public String fromaccounttype { get; set; }
        public String tobranch { get; set; }
        public String toaccounttype { get; set; }

        public void getbilldata(JObject res)
        {
            if (res.HasValues)
            {
                if (res.ContainsKey("tranDateTime"))
                {
                    TransactionDateTime = res.GetValue("tranDateTime").ToString();


                }
                else
                    TransactionDateTime = "";

                //----------------------------------------------------
                if (res.ContainsKey("dynamicFees"))
                {
                    dynamicFees = res.GetValue("dynamicFees").ToString();
                }
                else
                    dynamicFees = "";

                //----------------------------------------------------
                if (res.ContainsKey("PAN"))
                {
                    fromCard = res.GetValue("PAN").ToString();


                }
                else
                    fromCard = "";
                //----------------------------------------------------
                if (res.ContainsKey("toCard"))
                {
                    toCard = res.GetValue("toCard").ToString();


                }
                else
                    toCard = "";

                //----------------------------------------------------
                if (res.ContainsKey("expDate"))
                {
                    expDate = res.GetValue("expDate").ToString();


                }
                else
                    expDate = "";

                //----------------------------------------------------
                if (res.ContainsKey("issuerTranFee"))
                {
                    issuerTransactionFees = res.GetValue("issuerTranFee").ToString();


                }
                else
                    issuerTransactionFees = "";

                //----------------------------------------------------
                if (res.ContainsKey("tranAmount"))
                {
                    TransactionAmount = res.GetValue("tranAmount").ToString();


                }
                else
                    TransactionAmount = "";

                //----------------------------------------------------
                if (res.ContainsKey("acqTranFee"))
                {
                    acuirerTransactionFees = res.GetValue("acqTranFee").ToString();


                }
                else
                    acuirerTransactionFees = "";

                //----------------------------------------------------
                if (res.ContainsKey("studentNo"))
                {
                    studentNo = res.GetValue("studentNo").ToString();


                }
                else
                    studentNo = "";

                //----------------------------------------------------


                if (res.ContainsKey("arabicName"))
                {
                    arabicName = res.GetValue("arabicName").ToString();


                }
                else
                    arabicName = "";

                //----------------------------------------------------
                if (res.ContainsKey("englishName"))
                {
                    englishName = res.GetValue("englishName").ToString();


                }
                else
                    englishName = "";



                //----------------------------------------------------
                if (res.ContainsKey("ReceiptNumber"))
                {
                    ReceiptNumber = res.GetValue("ReceiptNumber").ToString();


                }
                else
                    ReceiptNumber = "";


                //----------------------------------------------------
                if (res.ContainsKey("formNo"))
                {
                    formNo = res.GetValue("formNo").ToString();


                }
                else
                    formNo = "";

                //----------------------------------------------------
                if (res.ContainsKey("E-15ReceiptNumber"))
                {
                    E15ReceiptNumber = res.GetValue("E-15ReceiptNumber").ToString();


                }
                else
                    E15ReceiptNumber = "";
                //----------------------------------------------------

                if (res.ContainsKey("Amount"))
                {
                    Amount = res.GetValue("Amount").ToString();


                }
                else
                    Amount = "";

                //----------------------------------------------------
                if (res.ContainsKey("DeclarantNAME"))
                {
                    DeclarantNAME = res.GetValue("DeclarantNAME").ToString();


                }
                else
                    DeclarantNAME = "";

                //----------------------------------------------------
                if (res.ContainsKey("InstanceID"))
                {
                    InstanceID = res.GetValue("InstanceID").ToString();


                }
                else
                    InstanceID = "";
                //----------------------------------------------------
                if (res.ContainsKey("ProcStatus"))
                {
                    ProcStatus = res.GetValue("ProcStatus").ToString();


                }
                else
                    ProcStatus = "";
                //----------------------------------------------------
                if (res.ContainsKey("ProcError"))
                {
                    ProcError = res.GetValue("ProcError").ToString();


                }
                else
                    ProcError = "";
                //----------------------------------------------------
                if (res.ContainsKey("Office"))
                {
                    Office = res.GetValue("Office").ToString();


                }
                else
                    Office = "";

                //----------------------------------------------------
                if (res.ContainsKey("Declarant"))
                {
                    Declarant = res.GetValue("Declarant").ToString();


                }
                else
                    Declarant = "";

                //----------------------------------------------------
                if (res.ContainsKey("DECNBER"))
                {
                    DECNBER = res.GetValue("DECNBER").ToString();


                }
                else
                    DECNBER = "";
                //----------------------------------------------------
                if (res.ContainsKey("Transaction"))
                {
                    Transaction = res.GetValue("Transaction").ToString();


                }
                else
                    Transaction = "";
                //----------------------------------------------------
                if (res.ContainsKey("Year"))
                {
                    Year = res.GetValue("Year").ToString();


                }
                else
                    Year = "";
                //----------------------------------------------------
                if (res.ContainsKey("DECSER"))
                {
                    DECSER = res.GetValue("DECSER").ToString();


                }
                else
                    DECSER = "";

                //----------------------------------------------------
                if (res.ContainsKey("BankCode"))
                {
                    BankCode = res.GetValue("BankCode").ToString();


                }
                else
                    BankCode = "";

                //----------------------------------------------------
                if (res.ContainsKey("DeclarantNumber"))
                {
                    DeclarantNumber = res.GetValue("DeclarantNumber").ToString();


                }
                else
                    DeclarantNumber = "";


                //----------------------------------------------------
                if (res.ContainsKey("DeclarantName"))
                {
                    Declarant_Name = res.GetValue("DeclarantName").ToString();


                }
                else
                    Declarant_Name = "";


                //----------------------------------------------------
                if (res.ContainsKey("Dec.No"))
                {
                    DecNo = res.GetValue("Dec.No").ToString();


                }
                else
                    DecNo = "";



                //----------------------------------------------------
                if (res.ContainsKey("subNewBalance"))
                {
                    subNewBalance = res.GetValue("subNewBalance").ToString();


                }
                else
                    subNewBalance = "";
                //----------------------------------------------------
                if (res.ContainsKey("meterFees"))
                {
                    meterFees = res.GetValue("meterFees").ToString();


                }
                else
                    meterFees = "";
                //----------------------------------------------------
                if (res.ContainsKey("meterNumber"))
                {
                    meterNumber = res.GetValue("meterNumber").ToString();


                }
                else
                    meterNumber = "";
                //----------------------------------------------------
                if (res.ContainsKey("waterFees"))
                {
                    waterFees = res.GetValue("waterFees").ToString();


                }
                else
                    waterFees = "";
                //----------------------------------------------------
                if (res.ContainsKey("billAmount"))
                {
                    bill_Amount = res.GetValue("billAmount").ToString();


                }
                else
                    bill_Amount = "";
                //----------------------------------------------------
                if (res.ContainsKey("billedAmount"))
                {
                    billedAmount = res.GetValue("billedAmount").ToString();


                }
                else
                    billedAmount = "";
                //----------------------------------------------------
                if (res.ContainsKey("BillAmount"))
                {
                    BillAmount = res.GetValue("BillAmount").ToString();


                }
                else
                    BillAmount = "";
                //----------------------------------------------------
                if (res.ContainsKey("accountNo"))
                {
                    accountNo = res.GetValue("accountNo").ToString();


                }
                else
                    accountNo = "";
                //----------------------------------------------------
                if (res.ContainsKey("DueAmount"))
                {
                    DueAmount = res.GetValue("DueAmount").ToString();


                }
                else
                    DueAmount = "";
                //----------------------------------------------------
                if (res.ContainsKey("dueAmount"))
                {
                    due_Amount = res.GetValue("dueAmount").ToString();


                }
                else
                    due_Amount = "";
                //----------------------------------------------------
                if (res.ContainsKey("netAmount"))
                {
                    netAmount = res.GetValue("netAmount").ToString();


                }
                else
                    netAmount = "";
                //----------------------------------------------------
                if (res.ContainsKey("customerName"))
                {
                    customerName = res.GetValue("customerName").ToString();


                }
                else
                    customerName = "";
                //----------------------------------------------------
                if (res.ContainsKey("unitsInKWh"))
                {
                    unitsInKWh = res.GetValue("unitsInKWh").ToString();


                }
                else
                    unitsInKWh = "";
                //----------------------------------------------------
                if (res.ContainsKey("token"))
                {
                    token = res.GetValue("token").ToString();


                }
                else
                    token = "";
                //----------------------------------------------------
                if (res.ContainsKey("tranCurrency"))
                {
                    currency = res.GetValue("tranCurrency").ToString();


                }
                else
                    currency = "";
                //----------------------------------------------------
                if (res.ContainsKey("total"))
                {
                    total = res.GetValue("total").ToString();


                }
                else
                    total = "";
                //----------------------------------------------------
                if (res.ContainsKey("totalAmount"))
                {
                    total_Amount = res.GetValue("totalAmount").ToString();


                }
                else
                    total_Amount = "";

                //----------------------------------------------------
                if (res.ContainsKey("TotalAmount"))
                {
                    TotalAmount = res.GetValue("TotalAmount").ToString();


                }
                else
                    TotalAmount = "";
                //----------------------------------------------------
                if (res.ContainsKey("opertorMessage"))
                {
                    opertorMessage = res.GetValue("opertorMessage").ToString();


                }
                else
                    opertorMessage = "";

                //----------------------------------------------------
                if (res.ContainsKey("receiptNo"))
                {
                    receiptNo = res.GetValue("receiptNo").ToString();


                }
                else
                    receiptNo = "";
                //----------------------------------------------------
                if (res.ContainsKey("ReferenceId"))
                {
                    ReferenceId = res.GetValue("ReferenceId").ToString();


                }
                else
                    ReferenceId = "";
                //----------------------------------------------------
                if (res.ContainsKey("UnitName"))
                {
                    UnitName = res.GetValue("UnitName").ToString();


                }
                else
                    UnitName = "";
                //----------------------------------------------------
                if (res.ContainsKey("paymentInfo"))
                {
                    payeeid = res.GetValue("paymentInfo").ToString();
                }
                else
                    payeeid = "";
                //----------------------------------------------------
                if (res.ContainsKey("ServiceName"))
                {
                    ServiceName = res.GetValue("ServiceName").ToString();


                }
                else
                    ServiceName = "";
                //----------------------------------------------------
                if (res.ContainsKey("unbilledAmount"))
                {
                    unbilledAmount = res.GetValue("unbilledAmount").ToString();


                }
                else
                    unbilledAmount = "";
                //----------------------------------------------------
                if (res.ContainsKey("PayerName"))
                {
                    PayerName = res.GetValue("PayerName").ToString();


                }
                else
                    PayerName = "";
                //----------------------------------------------------
                if (res.ContainsKey("payeeid"))
                {
                    payeeid = res.GetValue("payeeid").ToString();
                }
                else
                    payeeid = "";
                //----------------------------------------------------
                if (res.ContainsKey("contractNumber"))
                {
                    contractNumber = res.GetValue("contractNumber").ToString();


                }
                else
                    contractNumber = "";
                //----------------------------------------------------
                if (res.ContainsKey("lastInvoiceDate"))
                {
                    lastInvoiceDate = res.GetValue("lastInvoiceDate").ToString();


                }
                else
                    lastInvoiceDate = "";
                //----------------------------------------------------
                if (res.ContainsKey("last4Digits"))
                {
                    last4Digits = res.GetValue("last4Digits").ToString();


                }
                else
                    last4Digits = "";
                //----------------------------------------------------
                if (res.ContainsKey("InvoiceExpiry"))
                {
                    InvoiceExpiry = res.GetValue("InvoiceExpiry").ToString();


                }
                else
                    InvoiceExpiry = "";
                //----------------------------------------------------
                if (res.ContainsKey("SubscriberID"))
                {
                    SubscriberID = res.GetValue("SubscriberID").ToString();


                }
                else
                    SubscriberID = "";
                //----------------------------------------------------
                if (res.ContainsKey("InvoiceStatus"))
                {
                    InvoiceStatus = res.GetValue("InvoiceStatus").ToString();
                    if (InvoiceStatus == "0")
                    { InvoiceStatus = "Invoice Canceled "; }
                    else
                         if (InvoiceStatus == "1")
                    { InvoiceStatus = "Invoice Pending"; }

                    else
                         if (InvoiceStatus == "2")
                    { InvoiceStatus = "Invoice Already Paid"; }

                    else
                    {
                        InvoiceStatus = InvoiceStatus;


                    }

                }
                else
                    InvoiceStatus = "";
            }

        }

        public void getResponseval(JObject res)
        {
            if (res.HasValues)
            {
                //----------------------------------------------------
                if (res.ContainsKey("responseCode"))
                {
                    responseCode = Convert.ToInt32(res.GetValue("responseCode").ToString());


                }
                else
                    responseCode = -1;

                //----------------------------------------------------
                if (res.ContainsKey("responseMessage"))
                {
                    responseMessage = res.GetValue("responseMessage").ToString();


                }
                else
                    responseMessage = null;
                //----------------------------------------------------
                if (res.ContainsKey("responseStatus"))
                {
                    responseStatus = res.GetValue("responseStatus").ToString();


                }
                else
                    responseStatus = null;
                //-----------------------------------------------
                if (res.ContainsKey("balance"))
                {
                    String s = res.GetValue("balance").ToString();
                    if (res.GetValue("balance").ToString() != "")
                    {
                        balance = new JObject();
                        balance = JObject.Parse(res.GetValue("balance").ToString());
                        dynamic bal = balance;
                        available = bal.available;
                        leger = bal.leger;
                    }
                    else
                        balance = null;
                }
                else
                    balance = null;
                if (res.ContainsKey("billInfo"))
                {
                    if (res.GetValue("billInfo").ToString() != "")
                    {
                        billInfo = new JObject();
                        billInfo = JObject.Parse(res.GetValue("billInfo").ToString());
                        dynamic bill = billInfo;
                        //E15ReceiptNumber = bill.E-15ReceiptNumber;
                        ReceiptNumber = bill.ReceiptNumber;
                        receiptserial = bill.ReceiptSerial;
                        BankCode = bill.BankCode;
                        ReceiptNumber = bill.ReceiptNumber;
                        Declarant = bill.DeclarantCode;
                        ProcError = bill.ProcError;
                        waterFees = bill.waterFees;
                        unitsInKWh = bill.unitsInKWh;
                        meterNumber = bill.meterNumber;
                        netAmount = bill.netAmount;
                        opertorMessage = bill.opertorMessage;
                        accountNo = bill.accountNo;
                        meterFees = bill.meterFees;
                        customerName = bill.customerName;
                        token = bill.token;
                    }
                    else
                        billInfo = null;
                }
                else
                    billInfo = null;
            }
        }
    }
}