using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace InternetBanking_v1.Controllers.Payments.ALBrooj_Payment
{
    public class BroojController : Controller
    {
        private static Random random;
        DataSource ds = new DataSource();
        MyChequeStatus ConnectenUPI = new MyChequeStatus();
        //
        // GET: /EPort/
        public ActionResult GetOrderInfo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetOrderInfo(BroojModel model)
        {
            String message, response;
            try
            {
                if (ModelState.IsValid)
                {
                    Session["InvoiceNo"] = model.InvoiceNo;
                    Session["servicetype"] = model.servicetype;
                    String SessionID = "33";// random.Next(50, 101).ToString();
                    response = ConnectenUPI.GetOrderinfo(model.InvoiceNo, model.servicetype, "2", model.servicetype, SessionID, "1");
                    JObject jobj = new JObject();
                    jobj = JObject.Parse(response);
                    dynamic result = jobj;

                    OrderDatials model1 = new OrderDatials();
                    model1.CustomerFound = result.CustomerFound;
                    if (model1.CustomerFound.Equals("1"))
                    {
                        model1.PayCustomerCode = result.PayCustomerCode;
                        model1.PayCustomerName = result.PayCustomerName;
                        model1.RequiredAmount = result.RequiredAmount;

                        model1.FeesAmount = result.FeesAmount;
                        model1.TotalAmount = result.TotalAmount;
                        model1.InvoiceNo = model.InvoiceNo;

                        model1.servicetype = model.servicetype;
                        List<SelectListItem> items = new List<SelectListItem>();
                        items.Add(new SelectListItem { Text = "Cash", Value = "C" });
                        items.Add(new SelectListItem { Text = "Transfer", Value = "F" });
                        model1.PaymentMethod = items;
                        Session["OrderDatials"] = model1;

                        return RedirectToAction("OrderDetails");
                    }
                    else
                    {
                        message = "Please Check Invoice Number ";
                        ModelState.AddModelError("", message);
                    }


                }
                else
                {
                    message = "All Fields are required ";
                    ModelState.AddModelError("", "Something is missing" + message);

                }
            }
            catch (Exception ex)
            {
                message = "Please Contact for Support";
                ModelState.AddModelError("", "Something is missing" + message);

            }
            return View(model);
        }
     //   [HttpGet]
     //   public ActionResult pay(String paytype)
     //   {
     //       OrderDatials model2 = (OrderDatials)Session["OrderDatialsPay"];

     //       CustomerBankinfo model = new CustomerBankinfo();
     //       model.Branches = ds.PopulateBranchs();
     //       model.AccTypes = ds.PopulateAccountTypes();
     //       model.Currencies = ds.PopulateCurrencies();
     //       model2.paytype = paytype;
     //       if (model2.paytype.Equals("C"))
     //       {
     //           return RedirectToAction("index");
     //       }
     //       else if (model2.paytype.Equals("A"))
     //       {


     //           Session["regmodel"] = model2;


     //       }
     //       else
     //       {
     //           return RedirectToAction("OrderDetails");
     //       }
     //       return View(model);


     //   }

     //   [HttpPost]
     //   public ActionResult pay(CustomerBankinfo model, string command)
     //   {
     //       OrderDatials model2 = (OrderDatials)Session["regmodel"];


     //       String message, response;

     //       try
     //       {
     //           model.Branches = ds.PopulateBranchs();
     //           model.AccTypes = ds.PopulateAccountTypes();
     //           model.Currencies = ds.PopulateCurrencies();

     //           var selectedBranch = model.Branches.Find(p => p.Value == model.BranchCode.ToString());
     //           var selectedAccType = model.AccTypes.Find(p => p.Value == model.AccountTypecode.ToString());
     //           var selectedCurrency = model.Currencies.Find(p => p.Value == model.CurrencyCode.ToString());
     //           if (selectedBranch != null)
     //           {
     //               selectedBranch.Selected = true;

     //           }
     //           if (selectedAccType != null)
     //           {
     //               selectedAccType.Selected = true;

     //           }
     //           if (selectedCurrency != null)
     //           {
     //               selectedCurrency.Selected = true;

     //           }




     //           if (ModelState.IsValid)
     //           {
     //               if (command == "Check")
     //               {
     //                   response = Connecttocore.GetCustinfo(model.AccountNumber, model.BranchCode, model.CurrencyCode, model.AccountTypecode);

     //                   JObject jobj = new JObject();
     //                   jobj = JObject.Parse(response);
     //                   dynamic result = jobj;



     //                   String name = result.CustomerName;
     //                   String balance = result.CustomerBalance;
     //                   String id = result.CustomerId;
     //                   String mobile = result.CustomerMobile;
     //                   ViewBag.name = name;
     //                   ViewBag.balance = balance;
     //                   ViewBag.id = id;
     //                   ViewBag.mobile = mobile;



     //               }
     //               else
     //               {


     //                   PayDetails paydet = new PayDetails();
     //                   paydet.PayCustomerCode = model2.PayCustomerCode;
     //                   paydet.PayCustomerName = model2.PayCustomerName;
     //                   paydet.RequiredAmount = model2.RequiredAmount;

     //                   paydet.FeesAmount = model2.FeesAmount;
     //                   paydet.TotalAmount = model2.TotalAmount;
     //                   paydet.InvoiceNo = model2.InvoiceNo;

     //                   paydet.servicetype = model2.servicetype;
     //                   paydet.paytype = "Transfer";
     //                   paydet.Customeraccount = model.AccountNumber;
     //                   response = Connecttocore.PayOrder(model2.InvoiceNo, model2.servicetype, "1", model2.servicetype, "33", "1", model2.FeesAmount, model2.CustomerFound, model2.PayCustomerName, model2.TotalAmount, model2.RequiredAmount, model.AccountNumber
     //, model.BranchCode, model.CurrencyCode, model.AccountTypecode, "A");

     //                   JObject jobj = new JObject();
     //                   jobj = JObject.Parse(response);
     //                   dynamic result = jobj;

     //                   paydet.Paymentstatus = result.Paymentstatus;

     //                   if (paydet.Paymentstatus.Equals("Successful"))
     //                   {

     //                       paydet.PaymentType = "Transfer";
     //                       paydet.PaymentDate = result.PaymentDate;
     //                       paydet.Paymentstatus = result.Paymentstatus;
     //                       paydet.PaymentVoucherNo = result.PaymentVoucherNo;

     //                   }
     //                   else
     //                   {

     //                       paydet.PaymentType = "Transfer";
     //                       paydet.PaymentDate = result.PaymentDate;
     //                       paydet.Paymentstatus = result.Paymentstatus;
     //                       paydet.PaymentVoucherNo = result.PaymentVoucherNo;


     //                   }

     //                   Session["PayDatials"] = paydet;

     //                   return RedirectToAction("PayDetails");
     //               }

     //           }
     //           else
     //           {
     //               message = "All Fields are required ";
     //               ModelState.AddModelError("", "Something is missing" + message);

     //           }
     //       }
     //       catch (Exception ex)
     //       {
     //           message = "Please Contact for Support";
     //           ModelState.AddModelError("", "Something is missing" + message);

     //       }

     //       return View(model);


     //   }

        //[HttpPost]
        //public ActionResult Check(CustomerBankinfo model)
        //{
        //    OrderDatials model2 = (OrderDatials)Session["regmodel"];


        //    String message, response;

        //    try
        //    {
        //        model.Branches = ds.GetBranchs();
        //        model.AccTypes = ds.GetAccountType();
        //        model.Currencies = ds.GetCurrency();

        //        var selectedBranch = model.Branches.Find(p => p.Value == model.BranchCode.ToString());
        //        var selectedAccType = model.AccTypes.Find(p => p.Value == model.AccountTypecode.ToString());
        //        var selectedCurrency = model.Currencies.Find(p => p.Value == model.CurrencyCode.ToString());
        //        if (selectedBranch != null)
        //        {
        //            selectedBranch.Selected = true;

        //        }
        //        if (selectedAccType != null)
        //        {
        //            selectedAccType.Selected = true;

        //        }
        //        if (selectedCurrency != null)
        //        {
        //            selectedCurrency.Selected = true;

        //        }




        //        if (ModelState.IsValid)
        //        {

        //            response = Connecttocore.GetCustinfo(model.AccountNumber, model.BranchCode, model.CurrencyCode, model.AccountTypecode);

        //            JObject jobj = new JObject();
        //            jobj = JObject.Parse(response);
        //            dynamic result = jobj;



        //            String name = result.CustomerName;
        //            String balance = result.CustomerBalance;
        //            String id = result.CustomerId;
        //            String mobile = result.CustomerMobile;
        //            ViewBag.name = name;
        //            ViewBag.balance = balance;
        //            ViewBag.id = id;
        //            ViewBag.mobile = mobile;






        //        }
        //        else
        //        {
        //            message = "All Fields are required ";
        //            ModelState.AddModelError("", "Something is missing" + message);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = "Please Contact for Support";
        //        ModelState.AddModelError("", "Something is missing" + message);

        //    }

        //    return View(model);


        //}


        public ActionResult PayDetails()
        {
            PayDetails model = (PayDetails)Session["PayDatials"];

            return View(model);
        }
        public ActionResult OrderDetails()
        {
            Session["OrderDatialsPay"] = "";
            OrderDatials model = (OrderDatials)Session["OrderDatials"];
            Session["OrderDatialsPay"] = model;
            return View(model);
        }


        public ActionResult Print()
        {
            PayDetails model = new PayDetails();

            model = (PayDetails)Session["OrderDatialsPay"];
            return View(model);
        }


        public FileResult SavePDF()
        {
            //List < Employee > employees = _context.employees.ToList < Employee > ();  
            PayDetails model = new PayDetails();

            model = (PayDetails)Session["PayDatials"];
            //model.AccountType = "83";
            //model.PayCustomerName = "ADAM AHMED HUSIEN ELHAMARI";
            //model.PaymentDate = "07/03/2019";
            //model.PayCustomerCode = "10119013443";
            //model.InvoiceNo = "10119013443";
            //model.PaymentDate = "07/03/2019";
            //model.FeesAmount = "5";
            //model.PaymentVoucherNo = "201900951305";
            //model.TotalAmount = "17";
            //model.Paymentstatus = "Successful";
            //model.RequiredAmount = "12";

            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("EPorts- " + model.InvoiceNo.ToString() + " - " + dTime.ToString("yyyyMMddHHmmss") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(10f, 10f, 10f, 10f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(2);
            doc.SetMargins(10f, 10f, 10f, 10f);
            //Create PDF Table  

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   

            doc.Add(Add_Content_To_PDF(tableLayout));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            DateTime dTime = DateTime.Now;

            float[] headers = { 50, 50 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  




            tableLayout.AddCell(new PdfPCell(new Phrase("ALkhaleej Bank", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("Branch PortSudan", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            tableLayout.AddCell(new PdfPCell(new Phrase("Sudan Sea Ports", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            PayDetails model = new PayDetails();

            model = (PayDetails)Session["PayDatials"];

            ////Add header 

            //AddCellToHeader(tableLayout, "Customer Account");
            //AddCellToHeader(tableLayout, "Customer Invoice");
            //AddCellToHeader(tableLayout, "Amount");
            //AddCellToHeader(tableLayout, "Voucher");
            //AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "");
            AddCellToHeader(tableLayout, "");
            ////Add body  



            AddCellToBody(tableLayout, "Customer Account : " + model.Customeraccount.ToString());
            AddCellToBody(tableLayout, "Invoice : " + model.PayCustomerCode.ToString());
            AddCellToBody(tableLayout, "Amount : " + model.RequiredAmount.ToString());
            AddCellToBody(tableLayout, "Fees : " + model.FeesAmount.ToString());
            AddCellToBody(tableLayout, "TotalAmount : " + model.TotalAmount.ToString());
            AddCellToBody(tableLayout, "Customer Name : " + model.PayCustomerName.ToString());
            AddCellToBody(tableLayout, "VoucherNo : " + model.PaymentVoucherNo.ToString());
            AddCellToBody(tableLayout, "PaymentDate : " + model.PaymentDate.ToString());
            AddCellToBody(tableLayout, "Status : " + model.Paymentstatus.ToString());

            AddCellToBody(tableLayout, "Date : " + dTime.ToString("dd-MM-yyyy HH:mm:ss"));
            // AddCellToBody(tableLayout, "Time : "+ dTime.ToString("HH:mm:ss"));

            return tableLayout;
        }


        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 128, 128)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
    }
}
