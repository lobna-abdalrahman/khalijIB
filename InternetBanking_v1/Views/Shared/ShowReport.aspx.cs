

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace InternetBanking_v1.Views.Shared
{
    public partial class ShowReport : System.Web.UI.Page
    {
        protected void page_init(object sender, EventArgs e)
        {


            String s = "";
            //try
            //{
            //    if (Session["log"] == null)
            //    {
                    
            //        Response.Redirect("Login/Login.cshtml");

            //    }
            //    var o = Session["log"];
            //    if (o != null) s = o.ToString();
            //    if (!s.Equals("true"))
            //    {
            //        Response.Redirect("Login/Login.cshtml");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.Data.Clear();
            //    lblconfirm.Text = "Error";
            //    //ex.Message;
            //    Response.Redirect("default.aspx");
            //}
            String sqlstr, tname, rname;
            sqlstr = (String) Session["Sql"];
            tname = (String) Session["TableName"];
            rname = (String) Session["ReportName"];
            Spark.HBIdb.repDoc = Spark.HBIdb.createReport(sqlstr, tname, rname);

            try
            {
                if (Spark.HBIdb.reportsRows > 0)
                {


                    Spark.HBIdb.repDoc.SetParameterValue("kh", Session["RepName"]);
                    Spark.HBIdb.repDoc.SetParameterValue("brch", Session["BrName"]);
                    Spark.HBIdb.repDoc.SetParameterValue("fromd", Session["FromDate"]);
                    Spark.HBIdb.repDoc.SetParameterValue("tod", Session["ToDate"]);
                    Spark.HBIdb.repDoc.SetParameterValue("brcode", "");



                    Viewer.ReportSource = Spark.HBIdb.repDoc;
                    Viewer.SeparatePages = true;
                    Viewer.DisplayGroupTree = false;
                    Viewer.LogOnInfo[0].ConnectionInfo.ServerName = "orcl";
                    Viewer.LogOnInfo[0].ConnectionInfo.UserID = "ibanking";
                    Viewer.LogOnInfo[0].ConnectionInfo.Password = "admin";




                }
                else
                {
                    lblconfirm.Text = "No data found on this date";
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                lblconfirm.Text = "Error";
                //ex.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


    }
}