using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;       
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using IBLogic;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using InternetBanking_v1.Models.ViewModels;
using LinqToDB.Common;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Oracle.DataAccess.Client;

using Font = iTextSharp.text.Font;
using System.Text.RegularExpressions;
using System.Globalization;

namespace InternetBanking_v1.Controllers
{
    public class AccountStatementLegacyController : BaseController
    {
        //populate accounts in dropdown list
        LoginLogic dataObj = new LoginLogic();
        MyChequeStatus obj = new MyChequeStatus();
        DataSource data = new DataSource();

        //
        // GET: /AccountStatementLegacy/  
        public ActionResult Statement()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            AccountStatementViewModel model = new AccountStatementViewModel();
            string user_id = Session["UserID"].ToString();
            //model.AccountNo = dataObj.populateAccounts(user_id);
            List<AccountStatementViewModel> pcontent = new List<AccountStatementViewModel>();
            {
                pcontent = data.DropStatementClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {

                var AccountNumber = Convert.ToInt64(item.AccountNo.Substring(13));
                var AccountType = data.getaccounttype(item.AccountType);
                var BranchName = data.getbranchnameenglish(item.AccountNo.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.AccountNo
                });
            }

            ViewBag.clientList = AccountList;
            return View();
        }

        [HttpPost]
        public ActionResult Statement(AccountStatementViewModel ASVM)
        {
            List<AccountStatementViewModel> accas = new List<AccountStatementViewModel>();
            string message = "";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string user_id = Session["UserID"].ToString();

            List<AccountStatementViewModel> pcontent = new List<AccountStatementViewModel>();
            {
                pcontent = data.DropStatementClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt64(item.AccountNo.Substring(13));
                var AccountType = data.getaccounttype(item.AccountNo.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.AccountNo.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.AccountNo
                });
            }

            ViewBag.clientList = AccountList;

            if (ModelState.IsValid)
            {

                string fromDate, tranType, toDate;
                int noOfTrans;
                //Get Selected Account...
                ASVM.AccountNo = Request["Clients"];
                String transactionumber = ASVM.tranno;
                String stattype = ASVM.StatementType;
                if (stattype.Equals("2"))
                {
                    fromDate = "";
                    toDate = "";
                    if (!String.IsNullOrEmpty(transactionumber) && !String.IsNullOrWhiteSpace(transactionumber))
                    {
                        noOfTrans = Convert.ToInt32(transactionumber);
                    }
                    else
                    {
                        message = "All Fields are required ";
                        ModelState.AddModelError("", "Please Choose the NumberOfTransaction");
                        return View();
                    }
                    tranType = "NumberOfTransaction"; // ASVM.tranType;
                    Session["fromDate"] = "---------";
                    Session["toDate"] = "---------";

                }
                else
                {
                    fromDate = ASVM.fromDate;//DateTime.ParseExact(ASVM.fromDate,"yyyy-mm-dd",CultureInfo.InvariantCulture).ToString();
                    toDate = ASVM.toDate;//DateTime.ParseExact(ASVM.toDate, "yyyy-mm-dd",CultureInfo.InvariantCulture).ToString();
                    noOfTrans = 0;
                    tranType = "Period"; // ASVM.tranType;
                    if (!String.IsNullOrEmpty(fromDate) && !String.IsNullOrWhiteSpace(fromDate) && !String.IsNullOrEmpty(toDate) && !String.IsNullOrWhiteSpace(toDate))
                    {
                        fromDate = DateTime.ParseExact(fromDate.ToString(), "dd/mm/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-mm-dd");
                        toDate = DateTime.ParseExact(toDate.ToString(), "dd/mm/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-mm-dd");
                        Session["fromDate"] = fromDate;
                        Session["toDate"] = toDate;
                    }
                    else
                    {
                        message = "All Fields are required ";
                        ModelState.AddModelError("", "Please Choose the Period Of Transaction");
                        return View();
                    }

                }

                Session["AccNo"] = ASVM.AccountNo;
                try
                {
                    string statement = obj.GetLegacyStatement(ASVM.AccountNo, noOfTrans, fromDate, toDate, tranType);//"{\"tranDateTime\": \"150721014212\",\"uuid\": \"81c98168-7b3d-4de1-9b81-e1d2f4d0e9dd\",\"errormsg\": \"Secussfully\",\"errorcode\": \"1\"}";//
                    JObject response = new JObject();
                    response = JObject.Parse(statement);

                    response = JObject.Parse(statement);
                    JArray statementitems = (JArray)response.GetValue("Statement");
                    int id = 1;
                    string amount = "", TranscationDirection = "N/A", TranscationNarration = "N/A", Date = "N/A", BalanceAfterTransaction = "N/A";
                    if (statementitems.Count > 0)
                    {
                        foreach (JObject item in statementitems)
                        {
                            if (item.GetValue("Amount") != null)
                            {
                                amount = item.GetValue("Amount").ToString();
                                Double amountcontainer = Convert.ToDouble(amount);
                                amount = String.Format("{0:N}", amountcontainer); //.Substring(1, amountcontainer.ToString().Length);
                            }
                            if (item.GetValue("TranscationDirection") != null)
                            {
                                TranscationDirection = item.GetValue("TranscationDirection").ToString();
                            }
                            if (item.GetValue("TranscationNarration") != null)
                            {
                                TranscationNarration = item.GetValue("TranscationNarration").ToString();
                            }
                            if (item.GetValue("Date") != null)
                            {
                                Date = item.GetValue("Date").ToString();
                            }
                            if (item.GetValue("BalanceAfterTransaction") != null)
                            {
                                BalanceAfterTransaction = item.GetValue("BalanceAfterTransaction").ToString();
                                Double BalanceAfterTransactioncontainer = Convert.ToDouble(BalanceAfterTransaction);
                                BalanceAfterTransaction = String.Format("{0:N}", BalanceAfterTransactioncontainer); //BalanceAfterTransactioncontainer.ToString("C2");//.Substring(1, BalanceAfterTransactioncontainer.ToString().Length);
                            }

                            accas.Add(new AccountStatementViewModel
                            {
                                StateID = id,
                                StateAmount = amount,
                                TranscationDirection = TranscationDirection,
                                TranscationNarration = TranscationNarration,
                                Date = Date,
                                BalanceAfterTransaction = BalanceAfterTransaction //+ " " + ds.GetCurrencyName(AccountNo.ToString())
                            });
                            id++;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", GlobalRes.noStatementRecords);
                        return View();
                    }
                }
                catch (Exception e)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("this is exception : ", e.Message);
                    return View();
                }

            }
            else
            {
                message = "All Fields are required ";
                ModelState.AddModelError("", "Something is missing");
                return View();
            }

            //TempData["Statement"] = accas;
            Session["myStatement"] = accas;
            string acas = accas.ToString();

            if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
            {
                return RedirectToAction("ViewStatement", new { lang = "ar" });
            }
            else
            {
                return RedirectToAction("ViewStatement", accas);
            }


            //return View();
        }


        //View Statement....
        public ActionResult ViewStatement()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            List<AccountStatementViewModel> accas = new List<AccountStatementViewModel>();
            accas = (List<AccountStatementViewModel>)Session["myStatement"]; //TempData["Statement"];

            IEnumerable<List<AccountStatementViewModel>> accaass = new[] { accas };
            ViewBag.Statement = accaass;

            return View(accas);
        }

        public FileResult SavePDF()
        {

            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("StatementReport" + "-" + dTime.ToString("yyyyMMddHHmmss") + ".pdf");
            Document doc = new Document();
            doc.SetMargins(10f, 10f, 10f, 10f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(5);

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

            float[] headers = { 40, 20, 30, 70, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 4;
            //Add Title to the PDF file at the top  

            //List < Employee > employees = _context.employees.ToList < Employee > ();  
            List<AccountStatementViewModel> employees = new List<AccountStatementViewModel>();
            employees = (List<AccountStatementViewModel>)Session["myStatement"];
            DateTime dTime = DateTime.Now;
            string UserName = Session["username"].ToString();
            string AccNo = Session["AccNo"].ToString();
            string fromDate = Session["fromDate"].ToString();
            string toDate = Session["toDate"].ToString();
            string AccountNumber = AccNo.Substring(13);
            string AccountType = data.getaccounttype(AccNo.ToString().Substring(5, 5));
            string BranchName = data.getbranchnameenglish(AccNo.ToString().Substring(2, 3));
            string currency = data.GetCurrencyName(AccNo.Substring(10, 3));
            string Name = Session["name"].ToString();
            tableLayout.AddCell(new PdfPCell(new Phrase("ALkhaleej Internet Banking", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            if (!fromDate.Equals("---------"))
            {
                tableLayout.AddCell(new PdfPCell(new Phrase("Statement of Account From  : " + fromDate.ToString() + " To " + toDate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                {
                    Colspan = 12,
                    Border = 0,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            }
            tableLayout.AddCell(new PdfPCell(new Phrase("Statement Date : " + dTime.ToString("dd-MMM-yyyy HH:mm:ss"), new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                PaddingRight = 10,
                PaddingLeft = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            //tableLayout.AddCell(new PdfPCell(new Phrase("Customer : " + Name, new Font(Font.FontFamily.TIMES_ROMAN, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            //{
            //    Colspan = 12,
            //    Border = 0,
            //    PaddingBottom = 5,
            //    HorizontalAlignment = Element.ALIGN_LEFT
            //});


            Paragraph accountType1 = new Paragraph("Account Type : " + AccountType,
               new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)));
            Paragraph accountType = new Paragraph(AccountType,
                new Font(Font.FontFamily.TIMES_ROMAN, 8, 1, new BaseColor(0, 0, 0)));

            Paragraph AccountNo = new Paragraph("Account No : " + AccountNumber,
                new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)));

            Paragraph Currency = new Paragraph("Currency : " + currency,
                new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)));

            Paragraph customerName1 = new Paragraph("Customer Name : ",
                new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)));
            Paragraph customerName = new Paragraph(Name,
                new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)));

            //  AddAcctypeCell(tableLayout, AccountType);

            //tableLayout.AddCell(new PdfPCell(new Phrase(accountType1))
            //{
            //    Colspan = 2,

            //    Rowspan = 1,
            //    Border = 0,
            //    PaddingBottom = 5,
            //    HorizontalAlignment = Element.ALIGN_LEFT
            //});


            tableLayout.AddCell(new PdfPCell(new Phrase(AccountNo))
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                Colspan = 12,
                PaddingRight = 10,
                PaddingLeft = 10,

                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase(Currency))
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                Colspan = 12,
                PaddingRight = 10,
                PaddingLeft = 10,
                Rowspan = 1,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase(accountType1))
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                Colspan = 12,
                PaddingLeft = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase(customerName1))
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                Colspan = 1,
                PaddingLeft = 10,

                Border = 0,
                PaddingBottom = 15,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            //tableLayout.AddCell(new PdfPCell(new Phrase(customerName))
            //{
            //    Colspan = 3,

            //    Rowspan = 1,
            //    Border = 0,
            //    PaddingBottom = 5,
            //    HorizontalAlignment = Element.ALIGN_LEFT
            //});
            AddCustomerCell(tableLayout, Name);


            //tableLayout.AddCell(new PdfPCell(new Phrase(Currency))
            //{
            //    RunDirection = PdfWriter.RUN_DIRECTION_LTR,
            //    Colspan = 12,
            //    PaddingRight = 10,
            //    PaddingLeft = 10,
            //    Rowspan = 1,
            //    Border = 0,
            //    PaddingBottom = 5,
            //    HorizontalAlignment = Element.ALIGN_RIGHT
            //});




            ////Add header 

            AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "Transaction Amount");
            AddCellToHeader(tableLayout, "Transaction Type");
            AddCellToHeader(tableLayout, "Transaction Narration");
            AddCellToHeader(tableLayout, "Remaining Balance");
            //  AddCellToHeader(tableLayout, "Currency");

            ////Add body  

            foreach (var emp in employees)
            {

                AddCellToBody(tableLayout, emp.Date.ToString());
                AddCellToBody(tableLayout, emp.StateAmount.ToString());
                AddCellToBody(tableLayout, emp.TranscationDirection.ToString());
                AddCellToBody(tableLayout, emp.TranscationNarration.ToString());
                AddCellToBody(tableLayout, emp.BalanceAfterTransaction.ToString());
                //  AddCellToBody(tableLayout, emp.AccountCurrency.ToString());

            }

            return tableLayout;
        }


        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            string fontpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\times.ttf";
            BaseFont basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);


            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(basefont, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(Color.Goldenrod)//BaseColor(128, 0, 0)  
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            string fontpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\times.ttf";
            BaseFont basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);

            const string regex_match_arabic_hebrew = @"[\u0600-\u06FF\u0590-\u05FF]+";
            if (Regex.IsMatch(cellText, regex_match_arabic_hebrew, RegexOptions.IgnoreCase))
            {
                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(basefont, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 5,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });

                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            }
            else
            {
                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(basefont, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 5,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }
        }








        //-------------Export To Excel-------------------------
        //

        [HttpPost]
        public FileResult Export()
        {
            DateTime dTime = DateTime.Now;
            DataTable dt = new DataTable("AlKhaleej e-banking " + dTime.ToString("yyyyMMdd"));
            dt.Columns.AddRange(new DataColumn[6] {
                new DataColumn("Date"),
                new DataColumn("Transaction Amount"),
                new DataColumn("Transaction Type"),
                new DataColumn("Transaction Narration"),
                new DataColumn("Remaining Balance"),
                new DataColumn("Currency")
                 });

            List<AccountStatementViewModel> employees = new List<AccountStatementViewModel>();

            employees = (List<AccountStatementViewModel>)Session["myStatement"];

            foreach (var state in employees)
            {
                dt.Rows.Add(state.Date, state.StateAmount, state.TranscationDirection, state.TranscationNarration, state.BalanceAfterTransaction, state.AccountCurrency);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AlKhaleej Internet Banking " + dTime.ToString("yyyyMMddHHmmss") + ".xlsx");
                }
            }
        }

        /*[HttpGet]*/
        public FileContentResult ExportToExcell()
        {
            List<AccountStatementViewModel> employees = new List<AccountStatementViewModel>();

            employees = (List<AccountStatementViewModel>)Session["myStatement"];

            if (employees.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Cannot Save Empty File");
                //return View("ViewStatement"); 
            }
            DateTime dTime = DateTime.Now;
            string[] columns = { "Statement ID", "Statement Amount", "Direction", "Balance After Transaction", "Date" };
            byte[] filecontent = ExportExcel(employees, "Statement", true, columns);

            return File(filecontent, ExcelContentType, "AlKhaleej-Internetbanking-statement" + dTime.ToString("yyyyMMddHHmmss") + ".xlsx");
        }

        //-----------------------------------------------------------------------------------------------------
        public IList<AccountStatementViewModel> GetEmployeeList()
        {

            var employeeList = (List<AccountStatementViewModel>)Session["myStatement"];
            return employeeList;
        }
        public ActionResult ExportToExcel()
        {
            //List < Employee > employees = _context.employees.ToList < Employee > ();  
            List<AccountStatementViewModel> employees = new List<AccountStatementViewModel>();

            employees = (List<AccountStatementViewModel>)Session["myStatement"];

            if (employees.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Cannot Save Empty File");
                return View("ViewStatement");
            }

            GridView gv = new GridView();
            gv.DataSource = employees;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Product.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.End();

            return View("ViewStatement");

        }

        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        //this is for arabic headers
        private static void AddAcctypeCell(PdfPTable tableLayout, string cellText)
        {

            string fontpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\times.ttf";
            BaseFont basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);

            const string regex_match_arabic_hebrew = @"[\u0600-\u06FF\u0590-\u05FF]+";
            if (Regex.IsMatch(cellText, regex_match_arabic_hebrew, RegexOptions.IgnoreCase))
            {
                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(basefont, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 2,

                    Rowspan = 1,
                    Border = 0,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            }
            else
            {
                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(basefont, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 2,

                    Rowspan = 1,
                    Border = 0,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }

        }

        private static void AddCustomerCell(PdfPTable tableLayout, string cellText)
        {

            string fontpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\times.ttf";
            BaseFont basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);

            const string regex_match_arabic_hebrew = @"[\u0600-\u06FF\u0590-\u05FF]+";
            if (Regex.IsMatch(cellText, regex_match_arabic_hebrew, RegexOptions.IgnoreCase))
            {
                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(basefont, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 11,


                    Border = 0,
                    PaddingBottom = 15,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            }
            else
            {
                tableLayout.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(basefont, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 11,


                    Border = 0,
                    PaddingBottom = 15,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }

        }




        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }
        public ActionResult PdfTest()
        {
            Session["AccNo"] = "9900420105001527";
            Session["fromDate"] = "01/06/2017";
            Session["toDate"] = "27/11/2018";
            List<AccountStatementViewModel> accas = new List<AccountStatementViewModel>();

            accas.Add(new AccountStatementViewModel
            {
                StateID = 1,
                StateAmount = "20000",
                TranscationDirection = "dr",
                TranscationNarration = "ddd",
                Date = "15-jan-2018",
                BalanceAfterTransaction = "50000" + " SDG",



            });

            Session["myStatement"] = accas;
            if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
            {
                return RedirectToAction("SavePDF", new { lang = "ar" });
            }
            else
            {
                return RedirectToAction("SavePDF", accas);
            }


            return RedirectToAction("Statement");
        }

    }


}




