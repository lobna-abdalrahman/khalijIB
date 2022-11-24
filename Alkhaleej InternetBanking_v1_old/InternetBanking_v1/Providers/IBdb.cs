using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OracleClient;
using System.Diagnostics;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using InternetBanking_v1.Controllers;
using InternetBanking_v1.Models.Account;
using InternetBanking_v1.Models.Login;
using ReportDocument = CrystalDecisions.CrystalReports.Engine.ReportDocument;

namespace InternetBanking_v1.Providers
{
    public class IBdb: DbContext
    {
        public DbSet<UserAccount> UserAccount { get; set; }

        public static OracleConnection dbConn;
        public static String ConStr;
        public static String bankCode;
        public static String brahoCode;
        public static String bankAcc;
        public static String intBranch;
        public static String intBank;
   //     public static ValueList list;
        public static int reportsRows;
        public static String SettPath;
        public static String ProSettPath;
        public static String backup;
        public static ReportDocument repDoc = new ReportDocument();

        public static String getConnect()
        {
            String errMsg = "";
            try
            {
                // 
                //StreamReader reader = File.OpenText("C:\\QNB Journal\\QNB\\HBI12142016\\HBI\\log.txt");
                StreamReader reader =
                    File.OpenText("D:\\MVC demo Apps\\InternetBanking_v1\\InternetBanking_v1\\Log.txt");
                ConStr = reader.ReadLine();
                ConStr = ConStr.Substring(ConStr.IndexOf("=") + 1).Trim();

                bankCode = reader.ReadLine();
                bankCode = bankCode.Substring(bankCode.IndexOf("=") + 1).Trim();

                brahoCode = reader.ReadLine();
                brahoCode = brahoCode.Substring(brahoCode.IndexOf("=") + 1).Trim();

                bankAcc = reader.ReadLine();
                bankAcc = bankAcc.Substring(bankAcc.IndexOf("=") + 1).Trim();

                intBranch = reader.ReadLine();
                intBranch = intBranch.Substring(intBranch.IndexOf("=") + 1).Trim();

                intBank = reader.ReadLine();
                intBank = intBank.Substring(intBank.IndexOf("=") + 1).Trim();

                SettPath = reader.ReadLine();
                SettPath = SettPath.Substring(SettPath.IndexOf("=") + 1).Trim();

                ProSettPath = reader.ReadLine();
                ProSettPath = ProSettPath.Substring(ProSettPath.IndexOf("=") + 1).Trim();

                backup = reader.ReadLine();
                backup = backup.Substring(backup.IndexOf("=") + 1).Trim();

                reader.Close();
            }
            catch (Exception ex1)
            {
                return ex1.Message;
            }
            dbConn = new OracleConnection(ConStr);
            try
            {
                if (dbConn.State.Equals(ConnectionState.Closed))
                    dbConn.Open();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }

        public static OracleConnection getBranchConnect(String brCode)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlstr;
            sqlstr = "Select BR_USER_NAME,BR_PWD,BR_TNS_NAME from CONNECT_BRANCH where BRANCH_CODE ='" + brCode + "'";
            cmd = new OracleCommand(sqlstr, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    con = new OracleConnection("User ID=" + dr[0].ToString() + ";Data Source=" + dr[2].ToString() + ";Password=" + dr[1].ToString());
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                }
            }
            catch (Exception ex)
            {
                String s = ex.Message;
            }
            return con;
        }

        public static String dropList(DropDownList list, String sqlStr, String valField, String textField)
        {
            String errMsg;
            OracleCommand cmd;
            OracleDataReader dr;
            cmd = new OracleCommand(sqlStr, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list.DataSource = dr;
                    list.DataValueField = valField;
                    list.DataTextField = textField;
                    list.DataBind();
                }
                dr.Close();
                errMsg = "";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }

        public static String dropList1(DropDownList list1, String sqlStr, String valField, String textField)
        {
            String errMsg;
            OracleCommand cmd;
            OracleDataReader dr;
            cmd = new OracleCommand(sqlStr, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    // list.DataSource = dr;
                    list1.DataSource = dr;
                    list1.DataTextField = textField;
                    list1.DataValueField = valField;
                    list1.DataBind();

                    //// list.DataValueField = "";
                    // list.DataTextField = "ATM_ID";
                    // list.DataBind();
                }
                dr.Close();
                errMsg = "";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }


        public static String dropList(DropDownList list, String sqlStr, String valField, String textField, OracleConnection con)
        {
            String errMsg;
            OracleCommand cmd;
            OracleDataReader dr;
            cmd = new OracleCommand(sqlStr, con);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list.DataSource = dr;
                    list.DataValueField = valField;
                    list.DataTextField = textField;
                    list.DataBind();
                }
                dr.Close();
                errMsg = "";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }

        public static String bindgrid(GridView grid, String sqlStr, String tableName)
        {
            String errMsg;
            OracleDataAdapter adapter;
            DataSet dSet = new DataSet();
            int rows = 0;
            adapter = new OracleDataAdapter(sqlStr, dbConn);
            try
            {
                rows = adapter.Fill(dSet, tableName);
                grid.DataSource = dSet.Tables[0].DefaultView.Table;
                grid.DataBind();
                errMsg = "";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }

        public static String bindgrid1(GridView grid, String sqlStr, String tableName)
        {
            String errMsg;
            OracleDataAdapter adapter;
            DataSet dSet = new DataSet();
            int rows = 0;
            adapter = new OracleDataAdapter(sqlStr, dbConn);
            try
            {
                rows = adapter.Fill(dSet, 0, 20, tableName);
                grid.DataSource = dSet;//.Tables[0].DefaultView.Table;
                grid.DataBind();
                errMsg = "";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }

        public static ReportDocument createReport(String sqlstr, String tableName, String Path)
        {
            DataSet Dset = new DataSet();

            try
            {
                repDoc.Load(Path);

            }
            catch (Exception ex)
            {
                String s = ex.Message;
                return null;
            }
            Dset = fillReport(sqlstr, tableName);
            repDoc.SetDataSource(Dset);
            return repDoc;
        }

        private static DataSet fillReport(String sqlstr, String tableName)
        {
            String s = "";
            DataSet Dset = new DataSet();
            OracleDataAdapter adapter;
            adapter = new OracleDataAdapter(sqlstr, dbConn);
            try
            {
                reportsRows = adapter.Fill(Dset, tableName);
                return Dset;
            }
            catch (Exception ex)
            {
                s = ex.Message;
                return null;
            }
        }

        public static String getMonth(String month)
        {
            String monthval = "";
            switch (month)
            {
                case "01":
                    monthval = "Jan";
                    break;
                case "02":
                    monthval = "Feb";
                    break;
                case "03":
                    monthval = "Mar";
                    break;
                case "04":
                    monthval = "Apr";
                    break;
                case "05":
                    monthval = "May";
                    break;
                case "06":
                    monthval = "Jun";
                    break;
                case "07":
                    monthval = "Jul";
                    break;
                case "08":
                    monthval = "Aug";
                    break;
                case "09":
                    monthval = "Sep";
                    break;
                case "10":
                    monthval = "Oct";
                    break;
                case "11":
                    monthval = "Nov";
                    break;
                case "12":
                    monthval = "Dec";
                    break;
            }
            return monthval;

        }

        public static String BindInterGrid(GridView Grid, String Sqlstr, String TableName)
        {
            String str = "";
            OracleDataAdapter adapter;
            DataSet datas = new DataSet();
            int counter;
            adapter = new OracleDataAdapter(Sqlstr, dbConn);
            try
            {
                adapter.Fill(datas, TableName);
                Grid.DataSource = datas.Tables[0].DefaultView.Table;
                Grid.DataBind();
                Grid.Columns[1].Visible = false;
                Grid.Columns[2].Visible = false;
                for (counter = 0; counter < Grid.Rows.Count; counter++)
                {
                    if (Grid.Rows[counter].Cells[1].Text.Equals("0"))
                    {
                        if (!Grid.Rows[counter].Cells[2].Text.Equals("-1"))
                        {
                            if (!Grid.Rows[counter].Cells[2].Text.Equals("-4"))
                            {
                                Grid.Rows[counter].BackColor = System.Drawing.Color.LightGreen;
                                Grid.Rows[counter].Cells[0].Text = Grid.Rows[counter].Cells[0].Text;
                            }
                            else
                            {
                                Grid.Rows[counter].BackColor = System.Drawing.Color.LightCyan;
                                Grid.Rows[counter].Cells[0].Text = Grid.Rows[counter].Cells[0].Text;
                            }
                        }
                        else
                        {
                            Grid.Rows[counter].BackColor = System.Drawing.Color.Yellow;
                            Grid.Rows[counter].Cells[0].Text = Grid.Rows[counter].Cells[0].Text;
                        }
                    }
                    else
                    {
                        if (!Grid.Rows[counter].Cells[2].Text.Equals("-1"))
                        {
                            if (!Grid.Rows[counter].Cells[2].Text.Equals("-4"))
                            {
                                Grid.Rows[counter].BackColor = System.Drawing.Color.Red;
                                Grid.Rows[counter].Cells[0].Text = Grid.Rows[counter].Cells[0].Text;
                            }
                            else
                            {
                                Grid.Rows[counter].BackColor = System.Drawing.Color.LightCyan;
                                Grid.Rows[counter].Cells[0].Text = Grid.Rows[counter].Cells[0].Text;
                            }
                        }
                        else
                        {
                            Grid.Rows[counter].BackColor = System.Drawing.Color.Yellow;
                            Grid.Rows[counter].Cells[0].Text = Grid.Rows[counter].Cells[0].Text;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public static double calcSum(String sqlstr)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String s = "";
            double sum = 0;
            cmd = new OracleCommand(sqlstr, dbConn);

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    sum = double.Parse(dr[0].ToString());

                }
                dr.Close();
            }
            catch (Exception ex)
            {

                s = ex.Message;
            }
            return sum;
        }

        public static double getTotal(String sqlstr)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            double total = 0;
            String s = "";
            cmd = new OracleCommand(sqlstr, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    total = Double.Parse(dr[0].ToString());
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            return total;
        }

        public static String getLastUser()
        {
            String str;
            OracleCommand cmd;
            OracleDataReader dr;
            String s = "";
            String lastUser = "-1";
            str = "Select nvl(max(USE_CODE),0) + 1 from users123";
            cmd = new OracleCommand(str, IBdb.dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lastUser = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            return lastUser;
        }

        public static String getLastProfile()
        {
            String str;
            OracleCommand cmd;
            OracleDataReader dr;
            String s = "";
            String lastUser = "-1";
            str = "Select nvl(max(PROFILE_ID),0) + 1 from M_Profile";
            cmd = new OracleCommand(str, IBdb.dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lastUser = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                s = ex.Message;
                //return "-1";
            }
            return lastUser;
        }

        public static String getUserId(String UserName, String usersts, ref String Uname)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String userid = "";
            String s = "";
            cmd = new OracleCommand("select USE_CODE,USE_STS,USE_FIRS_NAME from users123 where USE_LOGI = '" + UserName + "'", dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    userid = dr[0].ToString();
                    usersts = dr[1].ToString();
                    Uname = dr[2].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            return userid;
        }

        public static String getUserData(String UserId)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String Uname = "";
            String s = "";
            cmd = new OracleCommand("select USE_FIRS_NAME from users123 where USE_CODE = " + UserId, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Uname = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }


            return Uname;
        }

        public static bool runBatchFiles(Label lblconfirm, String FilePath)
        {
            bool flag = false;
            ProcessStartInfo psi = new ProcessStartInfo(@FilePath);
            Process proc;
            try
            {
                proc = new Process();
                proc.StartInfo = psi;
                proc.Start();

            }
            catch (Exception ex)
            {
                lblconfirm.Text = ex.Message;
            }
            return flag;
        }

        public static void getBranchData(String brcode, ref String Uname, ref String passwrd, ref String tns, String flag)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String str;
            String Sqlstr = "";
            if (flag.Equals("BRANCH"))
            {
                Sqlstr = "Select BR_USER_NAME,BR_PWD,BR_TNS_NAME from connect_branch where branch_code = '" + brcode + "'";
            }
            else if (flag.Equals("HBI"))
            {
                Sqlstr = "Select HBI_USER_NAME,HBI_PWD,HBI_TNS_NAME from connect_branch where branch_code = '" + brcode + "'";
            }
            cmd = new OracleCommand(Sqlstr, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Uname = dr[0].ToString();
                    passwrd = dr[1].ToString();
                    tns = dr[2].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
        }

        public static String getBranchData(String brcode)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String DbLink = "";
            String str;
            cmd = new OracleCommand("Select BR_DB_LINK from connect_branch where branch_code = '" + brcode + "'", dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    DbLink = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return DbLink;
        }

        public static String getActType(String act_type)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String Str;
            try
            {
                cmd = new OracleCommand("select acc_code from account_link where acc_code_hbi = '" + act_type + "'", dbConn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Str = (String)dr[0];
                }
                else
                    Str = "";
                dr.Close();

            }
            catch (Exception ex)
            {

                Str = ex.Message;
            }
            return Str;
        }

        public static String getMaxDate(String Sqlstr, Label lblconfirm)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String str = "";
            cmd = new OracleCommand(Sqlstr, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    str = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                lblconfirm.Text = ex.Message;
            }
            return str;
        }

        public static String getBranchName(String brCode, Label lblconfirm)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlstr;
            String brName = "";
            sqlstr = "Select BRA_DESC from Branch where BRA_BRANCH_CODE ='" + brCode + "'";
            cmd = new OracleCommand(sqlstr, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    brName = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                lblconfirm.Text = ex.Message;
            }
            return brName;
        }

        public static String getProfile(String ProNo)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String str = "";
            String ProName = "";
            cmd = new OracleCommand("Select PROFILE_NAME from m_profile where PROFILE_ID=" + ProNo, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ProName = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return ProName;
        }

        public static bool checkDate(String date)
        {
            bool flag = false;
            String s = "";
            try
            {
                String day = date.Substring(0, 2);
                String month = date.Substring(3, 2);
                String year = date.Substring(6, 4);
                if (Convert.ToInt32(day) <= 31)
                    if (Convert.ToInt32(month) <= 12)
                        if (Convert.ToInt32(year) >= 2000)
                            flag = true;
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            return flag;
        }

        ///////////////////////////////////////////////
        public static String IBlogin(String uName, String pwd)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            

            String Sqlstr = "Select user_pwd , user_id , user_name,last_login,last_log_ip from users where user_log = '" + uName + "' and user_status = 'A'";
            cmd = new OracleCommand(Sqlstr, dbConn);

            try
            {

                dr = cmd.ExecuteReader();
                Login login = new Login();
                if (!dr.Read())
                {
                    login.FailureText = "not Connected";
                }
                else
                {
                    LoginController.DataRead = dr.ToString();
                    
                }
                

            }
            catch (Exception e)
            {
                /*Console.WriteLine(e);
                throw;*/
            }
            String s = "sss";
            return s;
        }


      
        
    }
}