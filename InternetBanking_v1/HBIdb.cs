using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OracleClient;
using System.IO;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Net;




namespace Spark
{
    public class HBIdb
    {
        public static byte[] bytes = new byte[1024];
        public static Socket senderSock;
        public static StringBuilder TRANGETACCT;
        public static StringBuilder POSTTRAN;
        public static StringBuilder CONNECT;
        public static StringBuilder GETDATE;
        public static StringBuilder hosttement;
        public static StringBuilder hosttementnet;
        private const string initVector = "tu89geji340t89u2";
        private const int keysize = 256;
        public static OracleConnection dbConn;
        public static String ConStr;
        public static String SparkConStr;
        public static string SparkFlag;
        public static String ConStrDecrypted;
        public static String bankCode;
        public static String brahoCode;
        public static String bankAcc;
        public static String intBranch;
        public static String intBank;
        public static int reportsRows;
        public static String SettPath;
        public static String ProSettPath;
        public static String backup;
        public static String BoIp;
        public static String BoPort;
        public static ReportDocument repDoc = new ReportDocument();

        public static ReportDocument Repo = new ReportDocument();

        //getsettaccounting parameters
        static String BIN = "";

        private static string srcACt = "";
        private static string dstACt = "";
        private static string narr = "";
        public static string response = "";

        public static String getConnect()
        {
            String errMsg = "";
            try
            {
                StreamReader SparkProtiesReader = File.OpenText("\\Inetpub\\wwwroot\\HBI\\Bin\\SparkProperties.txt");
                SparkConStr = SparkProtiesReader.ReadLine();
                SparkFlag = SparkConStr.ToString();
                SparkProtiesReader.Dispose();
                SparkProtiesReader.Close();

                string skullandbones = "%AZ$tEcH@^_^2361";
                StreamReader reader = File.OpenText("\\Inetpub\\wwwroot\\HBI\\log.txt");
                ConStr = reader.ReadLine();
                ConStr = ConStr.Substring(ConStr.IndexOf("=") + 1).Trim();

                if (SparkFlag == "322")
                {
                    string UserIDO = "";
                    int index = ConStr.IndexOf("=");
                    UserIDO = ConStr.Substring(index + 1);
                    string UserIDOO = UserIDO.Substring(0, UserIDO.IndexOf(";"));
                    string UserIDOOO = UserIDOO.Remove(UserIDOO.Length - 2, 2) + "==";
                    string UserIDOOOO = Decrypt(UserIDOOO, skullandbones);

                    string DataSourceo = "";
                    int indexDataSourceo = UserIDO.IndexOf("=");
                    DataSourceo = UserIDO.Substring(indexDataSourceo + 1);
                    string DataSourceoo = DataSourceo.Substring(0, DataSourceo.IndexOf(";"));
                    string DataSourceooo = DataSourceoo.Remove(DataSourceoo.Length - 2, 2) + "==";
                    string DataSourceoooo = Decrypt(DataSourceooo, skullandbones);

                    string Passo = "";
                    int indexPasso = DataSourceo.IndexOf("=");
                    Passo = DataSourceo.Substring(indexPasso + 1);
                    string Passoo = Passo.Remove(Passo.Length - 2, 2) + "==";
                    string passooo = Decrypt(Passoo, skullandbones);

                    ConStrDecrypted = "User ID=" + UserIDOOOO + ";Data Source=" + DataSourceoooo + ";Password=" +
                                      passooo + "";

                }
                else
                {
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

                    BoIp = reader.ReadLine();
                    BoIp = BoIp.Substring(BoIp.IndexOf("=") + 1).Trim();

                    BoPort = reader.ReadLine();
                    BoPort = BoPort.Substring(BoPort.IndexOf("=") + 1).Trim();




                    reader.Close();
                }
            }
            catch (Exception ex1)
            {
                ex1.Data.Clear();
                string eroronto = "Error" + ex1.Message;
                return eroronto;
                /// return ex1.Message;

            }
            if (SparkFlag == "322")
            {
                dbConn = new OracleConnection(ConStrDecrypted);
            }
            else
            {
                dbConn = new OracleConnection(ConStr);
            }

            try
            {
                if (dbConn.State.Equals(ConnectionState.Closed))
                    dbConn.Open();
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                string eroronto = "Error" + ex.Message;
                errMsg = eroronto;
                // errMsg = ex.Message;
            }
            return errMsg;
        }

        public static string getRoutingInt()
        {

            OracleCommand cmd;
            OracleDataReader dr;
            String sqlstr;
            String rou_int = "";
            sqlstr = "Select rou_int from routing where rou_code ='" + 1 + "'";
            cmd = new OracleCommand(sqlstr, dbConn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    rou_int = dr[0].ToString();

                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                rou_int = "Error" + ex.Message;
                String s = rou_int;
            }
            return rou_int;
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
                    con = new OracleConnection("User ID=" + dr[0].ToString() + ";Data Source=" + dr[2].ToString() +
                                               ";Password=" + dr[1].ToString());
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                string eroronto = "Error" + ex.Message;
                String s = eroronto;
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
                ex.Data.Clear();
                string eroronto = "Error" + ex.Message;
                errMsg = eroronto;
                //errMsg = ex.Message;
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
                ex.Data.Clear();
                string eroronto = "Error" + ex.Message;
                //errMsg = ex.Message;
                errMsg = eroronto;
            }
            return errMsg;
        }

        public static String dropList(DropDownList list, String sqlStr, String valField, String textField,
            OracleConnection con)
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
                ex.Data.Clear();
                string eroronto = "Error" + ex.Message;
                //errMsg = ex.Message;
                errMsg = eroronto;
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
                ex.Data.Clear();
                string eroronto = "Error" + ex.Message;
                // errMsg = ex.Message;
                errMsg = eroronto;
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
                grid.DataSource = dSet; //.Tables[0].DefaultView.Table;
                grid.DataBind();
                errMsg = "";
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                string eroronto = "Error" + ex.Message;
                //errMsg = ex.Message;
                errMsg = eroronto;
            }
            return errMsg;
        }

        public static String bindGridInf(GridView grid, String sqlStr, String tableName)
        {
            String errMsg;
            OracleDataAdapter adapter;
            DataSet dSet = new DataSet();
            int rows = 0;
            adapter = new OracleDataAdapter(sqlStr, dbConn);
            try
            {
                if (tableName == "")
                {
                    rows = adapter.Fill(dSet);
                    grid.DataSource = dSet;
                }
                else
                {
                    rows = adapter.Fill(dSet, tableName);
                    grid.DataSource = dSet.Tables[0].DefaultView.Table;
                }
                grid.DataBind();
                errMsg = "";
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                string eroronto = "Error";
                //errMsg = ex.Message;
                errMsg = eroronto;
            }
            return errMsg;
        }

        public static ReportDocument createReport(String sqlstr, String tableName, String Path)
        {
            DataSet Dset = new DataSet();

            try
            {
                string cc = Path;
                repDoc.Load(Path);

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                string eroronto = "Error";
                // String s = ex.Message;
                String s = eroronto;
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
                ex.Data.Clear();
                string eroronto = "Error";
                // s = ex.Message;
                s = eroronto;
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
                ex.Data.Clear();
                string eroronto = "Error";
                //str = ex.Message;
                str = eroronto;
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
                ex.Data.Clear();
                string eroronto = "Error";
                // s = ex.Message;
                s = eroronto;
            }
            return sum;
        }

        public static double getTotal(String sqlstr)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            double total = 0;
            // String s = "";
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
                ex.Data.Clear();
                //  string eroronto = "Error";
                // s = ex.Message;
                //   s = eroronto;
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
            str = "Select nvl(max(USE_CODE),0) + 1 from users";
            cmd = new OracleCommand(str, Spark.HBIdb.dbConn);
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
                ex.Data.Clear();
                string eroronto = "Error";
                //s = ex.Message;
                s = eroronto;
            }
            return lastUser;
        }

        public static String getBranchName(String brCode, Label lblconfirm)
        {
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlstr;
            String brName = "";
            sqlstr = "Select BRA_DESC from Branch where bra_code ='" + brCode + "'";
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
                ex.Data.Clear();
                string eroronto = "Error";
                lblconfirm.Text = eroronto;
                //ex.Message;
            }
            return brName;
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
                ex.Data.Clear();
                string eroronto = "Error";
                s = eroronto;
                //ex.Message;
            }
            return flag;
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public static String getSettAccounting(string flag, string adviceno, string trandate, string amount,
            string nooftran, string settfile, string atm_act, string atm_id)
        {
            String resp = "196";
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlstr;
            TRANGETACCT = new StringBuilder("getaccct :");
            OracleCommand CmdSP = new OracleCommand("GETSETTACCOUNTING", Spark.HBIdb.dbConn);
            CmdSP.CommandType = System.Data.CommandType.StoredProcedure;
            CmdSP.Parameters.AddWithValue("FLAG", flag);
            CmdSP.Parameters.AddWithValue("ADVICENO", adviceno);
            CmdSP.Parameters.AddWithValue("trandate", trandate);
            CmdSP.Parameters.AddWithValue("amount", amount);
            CmdSP.Parameters.AddWithValue("atm_act", atm_act);
            CmdSP.Parameters.AddWithValue("atm_id", atm_id);


            CmdSP.Parameters.Add(new OracleParameter("v_SRC_ACT", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;
            CmdSP.Parameters.Add(new OracleParameter("V_DST_ACT", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;
            CmdSP.Parameters.Add(new OracleParameter("Narr", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;
            CmdSP.Parameters.Add(new OracleParameter("ERRCODE", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;
            CmdSP.Parameters.Add(new OracleParameter("errmsg", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;
            CmdSP.Parameters.Add(new OracleParameter("Resp", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;

            TRANGETACCT.AppendLine("aacct prepare ");
            try
            {
                if (Spark.HBIdb.dbConn.State == ConnectionState.Closed)
                {
                    Spark.HBIdb.dbConn.Open();
                }
                TRANGETACCT.AppendLine("aacct  excurte1");
                CmdSP.ExecuteNonQuery();
                TRANGETACCT.AppendLine("aacct  excurte2");
                srcACt = Convert.ToString(CmdSP.Parameters["v_SRC_ACT"].Value);
                dstACt = Convert.ToString(CmdSP.Parameters["V_DST_ACT"].Value);
                narr = Convert.ToString(CmdSP.Parameters["Narr"].Value);
                resp = Convert.ToString(CmdSP.Parameters["Resp"].Value);
                CmdSP.Dispose();

            }

            catch (Exception ex)
            {
                //ex.Data.Clear();
                TRANGETACCT.AppendLine("aacct  error " + ex.Message);

                resp = "196";
            }
            TRANGETACCT.AppendLine("aacct  finsh " + resp);
            return resp;
        }

        public static String InsertActivityLog(string action, string page, string user)
        {
            String resp = "196";
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlstr;

            OracleCommand CmdSP = new OracleCommand("INSERT_ACTIVITY_LOG", Spark.HBIdb.dbConn);
            CmdSP.CommandType = System.Data.CommandType.StoredProcedure;
            CmdSP.Parameters.AddWithValue("ACTION", action);
            CmdSP.Parameters.AddWithValue("USR", user);
            CmdSP.Parameters.AddWithValue("acti_page", page);

            CmdSP.Parameters.Add(new OracleParameter("Resp", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;
            CmdSP.Parameters.Add(new OracleParameter("ERRCODE", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;
            CmdSP.Parameters.Add(new OracleParameter("errmsg", OracleType.VarChar, 2000)).Direction =
                ParameterDirection.Output;



            try
            {
                if (Spark.HBIdb.dbConn.State == ConnectionState.Closed)
                {
                    Spark.HBIdb.dbConn.Open();
                }

                CmdSP.ExecuteNonQuery();
                resp = Convert.ToString(CmdSP.Parameters["Resp"].Value);
                CmdSP.Dispose();

            }

            catch (Exception ex)
            {
                //ex.Data.Clear();
                //TRANGETACCT.AppendLine("aacct  error " + ex.Message);

                resp = "196";
            }
            //   TRANGETACCT.AppendLine("aacct  finsh " + resp);
            return resp;
        }



        /*   public static String postTransaction(String flag, String trnDate, String terminal, String proprietaryATM, String refNo, String settFile, String actNo, double trnAmount, String deviceLocation, String trnDesc, String branchCode)
           {
               String resp = "196";
               //System.out.println(" PostTransaction...........");
               POSTTRAN = new StringBuilder("PostTransaction............");
          
               //System.out.println("13-1 get Connection");
               String avlbal = "";
   
               //Date d= new Date();
               //SimpleDateFormat sdt= new SimpleDateFormat("yyMMdd");
               String effdate = DateTime.Now.ToString("yyMMdd");//sdt.format(d);
               //System.out.println("effictiv date" + effdate);
   
               //Get Accounting 
               getSettAccounting(flag, "", trnDate, trnAmount.ToString(), "", settFile, "", "");
               //Sending XMl here
   
               Connect();
               String xml =Utility.G2G(refNo, "N", "Sett", srcACt, dstACt, "", "", "", "", "", "",
                   "", "","" , refNo, "", "", "", "", "", "",
                   "", trnDate, "", "", "", "", narr, "", "",
                   "");
   
               Send(xml);
   
               resp = response;
               
               return resp;
           }
   
           public static void insertTrack(String msg, String adviceno, String from,
                                     String resp, String source)
           {
               OracleCommand pstmtinsert = null;
   
               String insertsettelment = null;
               int resultinsert = 0;
   
   
   
               insertsettelment = "insert into track(TRA_CODE, TRA_DATE, TRA_MSG, TRA_ADVICE,TRA_FROM,TRA_RESP,tra_source)values(S_TRACK.NEXTVAL,SYSDATE,'" + msg + "','" + adviceno + "','" + from + "','" + resp + "','" + source + "')";
               pstmtinsert = new OracleCommand(insertsettelment, Spark.HBIdb.dbConn);
               if (Spark.HBIdb.dbConn.State == ConnectionState.Closed)
               {
                   Spark.HBIdb.dbConn.Open();
               }
               resultinsert = pstmtinsert.ExecuteNonQuery();
               pstmtinsert.Dispose();
               //Spark.HBIdb.dbConn.Close();
   
           }
           
           public static void processHOSettlement()
           {
               int counter = 0, resultinsert = 0, nooftrn = 0;
               double amount = 0, errcode = 0;
               String getactresp = "", ATMact = "", settsummtype = "", settsummcode = "", insertsettelment = "", brdate = "", reval = "", errmsg = "", adviceno = "", trandate = "", sts = "", srcbranch = "", dstbranch = "", settfile = "";
   
               OracleCommand pstmtinsert = null, pstmtatmbr = null,
                       pstmtUpdatesett = null;
               OracleDataReader resultatmbr = null;
               hosttement = new StringBuilder("HO starting  ");
   
             //  getactresp = getSettAccounting();
               bool x = true;
               hosttement.AppendLine("ho response get account" + getactresp);
               try
               {
                   pstmtatmbr = new OracleCommand(" select SETT_TRN_ADVICE_NO,SETT_TRN_ENTERY_DATE,SETT_TRN_SOURCE_BRANCH,SETT_TRN_DESTINATION_BRANCH,SETT_TRN_AMOUNT,SETT_TRN_STATUS,SETT_TRN_NUMOFTRN,SETT_TRN_SETTFILE,SETT_TRN_SETT_SUMM_CODE,SETT_TRN_ATM_ACCOUNT from settlement_transaction where SETT_TRN_STATUS in ('ATMACQHOSCS','ATMACQCOMMHOSCS','ATMNECCOMMHOSCS') AND SETT_TRN_TYPE in ('ATM','ATMCOMM') and SETT_TRN_NUMOFSETTTRN < SETT_TRN_NUMOFTRN ");
                   // Edited 20/11/2012 POSISS POSSTDISS
                   pstmtatmbr.Connection = Spark.HBIdb.dbConn;
                   if (Spark.HBIdb.dbConn.State == ConnectionState.Closed)
                   {
                       Spark.HBIdb.dbConn.Open();
                   }
                   hosttement.AppendLine("ho excute 1");
                   resultatmbr = pstmtatmbr.ExecuteReader();
                   if (resultatmbr.HasRows)
                   {
                       while (resultatmbr.Read())
                       {
                           //System.out.println("50-2 while ...");
                           adviceno = "";
                           trandate = "";
                           srcbranch = "";
                           dstbranch = "";
                           amount = 0;
                           sts = "";
                           nooftrn = 0;
                           settfile = "";
                           reval = "";
                           errcode = 0;
                           errmsg = "";
                           settsummcode = "";
                           settsummtype = "";
                           ATMact = "";
   
                           adviceno = resultatmbr[0].ToString();
                           trandate = resultatmbr[1].ToString();
                           srcbranch = resultatmbr[2].ToString();
                           dstbranch = resultatmbr[3].ToString();
                           amount = Double.Parse(resultatmbr[4].ToString()) / 1000;
                           sts = resultatmbr[5].ToString();
                           nooftrn = int.Parse(resultatmbr[6].ToString());
                           settfile = resultatmbr[7].ToString();
                           settsummcode = resultatmbr[8].ToString();
   
                           try
                           {
   
                               if (sts.Equals("ATMACQHOSCS"))
                               {
                                   hosttement.AppendLine("1- ho ATM ACQ call Post");
                                   ATMact = resultatmbr[9].ToString().Trim();
                                   // System.out.println("50-488888888 ATM ACQ call Post ...");
                                   reval = postTransaction(sts, trandate,
                                       "ATM Acquirer Settlement", "Y",
                                       adviceno, settfile,
                                       ATMact, amount, adviceno,
                                       "ATM Acquirer Sett" + settfile,
                                       dstbranch);
                               }
                               else
                               {
                                   if (sts.Equals("ATMACQCOMMHOSCS"))
                                   {
                                       hosttement.AppendLine("2- ho ACQ COMM  call Post");
                                       //System.out.println("9999999999999999 ACQ COMM call Post ...");
                                       reval = postTransaction(
                                           sts,
                                           trandate,
                                           "BBA Charges Frm OthrBank Sett",
                                           "Y", adviceno + "A",
                                           settfile,
                                           
                                           "",
                                           amount, adviceno,
                                           "BBA Charge Sett Trn" +
                                           settfile, "804"
                                           );
                                   }
                                   else
                                   {
                                       if (sts.Equals("ATMNECCOMMHOSCS"))
                                       {
   
                                           hosttement.AppendLine("3- ho NEC COMM  call Post");
                                           //System.out.println("50-6 NEC COMM call Post ...");
                                           reval = postTransaction(
                                               sts,
                                               trandate,
                                               "ADIB Charges Frm OthrBank Sett",
                                               "Y", adviceno + "A",
                                               settfile,
                                               
                                               "",
                                               amount, adviceno,
                                               "ADIB Charge Sett Trn" +
                                               settfile, "804"
                                               );
                                       }
                                   }
                               }
                               //System.out.println("50-6  Post Return to HO...:"+reval);
                               hosttement.AppendLine("6- Post Return to HO...:" + reval);
                               if (reval.Equals("000"))
                               {
                                   if (sts.Equals("ATMACQHOSCS"))
                                   {
                                       sts = "ATMACQHOBRSCS";
                                       settsummtype = "ATMACQNET";
                                   }
                                   else
                                   {
                                       if (sts.Equals("ATMACQCOMMHOSCS"))
                                       {
                                           sts = "ATMACQCOMMHOBRSCS";
                                           settsummtype = "ATMACQCOMMNET";
                                       }
                                       else
                                       {
                                           if (sts.Equals("ATMNECCOMMHOSCS"))
                                           {
                                               sts = "ATMNECCOMMHOBRSCS";
                                               settsummtype = "ATMNECCOMMNET";
                                           }
   
                                       }
                                   }
                                   hosttement.AppendLine("7- HO  Update Transaction...+" + sts);
                                   //System.out.println("50-7  Update Transaction...+"+sts);
                                   pstmtUpdatesett =
                                       new OracleCommand("update SETTLEMENT_TRANSACTION set SETT_TRN_STATUS = '" + sts +
                                                         "', SETT_TRN_NUMOFSETTTRN = SETT_TRN_NUMOFTRN where SETT_TRN_ADVICE_NO = '" +
                                                         adviceno + "' AND SETT_TRN_AMOUNT = '" +
                                                         Double.Parse(resultatmbr[4].ToString()) +
                                                         "' AND SETT_TRN_SETTFILE = '" + settfile + "'");
                                   pstmtUpdatesett.Connection = Spark.HBIdb.dbConn;
                                   if (Spark.HBIdb.dbConn.State == ConnectionState.Closed)
                                   {
                                       Spark.HBIdb.dbConn.Open();
                                   }
                                   pstmtUpdatesett.ExecuteNonQuery();
                                   pstmtUpdatesett.Dispose();
                                   hosttement.AppendLine("8- HO Update Summary...+" + sts);
                                   //System.out.println("50-8  Update Summary...+"+sts);
                                   pstmtUpdatesett =
                                       new OracleCommand(
                                           "update SETTLEMENT_SUMMARY set SETT_SUMM_NOOFSETTADV = SETT_SUMM_NOOFSETTADV + 1 where SETT_SUMM_CODE = '" +
                                           settsummcode + "' and SETT_SUMM_TYPE = '" + settsummtype +
                                           "' AND SETT_SUMM_SETTFILE = '" + settfile + "'");
                                   pstmtUpdatesett.Connection = Spark.HBIdb.dbConn;
                                   if (Spark.HBIdb.dbConn.State == ConnectionState.Closed)
                                   {
                                       Spark.HBIdb.dbConn.Open();
                                   }
                                   pstmtUpdatesett.ExecuteNonQuery();
                                   pstmtUpdatesett.Dispose();
                                   hosttement.AppendLine("9- HO Update Temp...+" + sts);
                                   //System.out.println("50-9  Update Temp...+"+sts);
                                   pstmtUpdatesett =
                                       new OracleCommand("update SETTELMENT_temp set SETT_ADVICE_STATUS = '" + sts +
                                                         "' where SETT_ADVICE = '" + adviceno + "' and SETT_FILE_NAME  = '" +
                                                         settfile + "'");
                                   pstmtUpdatesett.Connection = Spark.HBIdb.dbConn;
                                   if (Spark.HBIdb.dbConn.State == ConnectionState.Closed)
                                   {
                                       Spark.HBIdb.dbConn.Open();
                                   }
                                   pstmtUpdatesett.ExecuteNonQuery();
                                   pstmtUpdatesett.Dispose();
                                   //  Spark.HBIdb.dbConn.Close();
                                   counter++;
                               }
   
                           }
                           catch (Exception ex)
                           {
                               try
                               {
                                   insertTrack(ex.Message, adviceno, "processHOSettlement", sts, settfile);
                                   resultatmbr.Close();
                                   pstmtatmbr.Dispose();
                                   //Spark.HBIdb.dbConn.Close();
   
                               }
                               catch (Exception e)
                               {
                                   //  System.out.println("Exception :" + e);
                               }
                               hosttement.AppendLine("processHOSettlement Method Error While System Try To Process advice:" + adviceno);
                               //System.out.println("processHOSettlement Method Error While System Try To Process advice: " +adviceno);
                               //System.out.println("Exception :" + ex);
                               hosttement.AppendLine("processHOSettlement Method Exception :" + ex);
   
                           }
                       } //end whil resultbr
                   }
                   hosttement.AppendLine("System Processed " + counter + " ATM Settelment Advice ");
                   hosttement.AppendLine("********************************************************************");
                   hosttement.AppendLine(" ");
                   resultatmbr.Close();
                   pstmtatmbr.Dispose();
                   //     Spark.HBIdb.dbConn.Close();
   
               }
               catch (Exception uex)
               { //end try
                   try
                   {
                       resultatmbr.Close();
                       pstmtatmbr.Dispose();
                       //    Spark.HBIdb.dbConn.Close();
                   }
                   catch (Exception ue)
                   {
                       hosttement.AppendLine("Exception :" + ue.Message);
                   }
   
                   try
                   {
                       insertTrack(uex.Message, adviceno, "processHOSettlement", sts, settfile);
   
                   }
                   catch (Exception mex)
                   {
                           
                   }
                   hosttement.AppendLine("processHOSettlement Method Error While System Try To Process advice: " + adviceno);
                   hosttement.AppendLine("Exception :" + uex);
               }
           } //END METHOD processHOSettlement()
   
           public  static void Connect()
           {
               try
               {
                   CONNECT = new StringBuilder("Connect............");
                   // Create one SocketPermission for socket access restrictions 
                   SocketPermission permission = new SocketPermission(
                       NetworkAccess.Connect, // Connection permission 
                       TransportType.Tcp, // Defines transport types 
                       GetIPAddress(), // Gets the IP addresses 
                       SocketPermission.AllPorts // All ports 
                       );
   
                   // Ensures the code to have permission to access a Socket 
                   permission.Demand();
   
                   // Resolves a host name to an IPHostEntry instance    
                   CONNECT.AppendLine("IP Adrress="+BoIp);
   
                   var ipHost = Dns.GetHostAddresses(BoIp);
                 
                   // Gets first IP address associated with a localhost 
                   IPAddress ipAddr = IPAddress.Parse(BoIp); ;
                   
                   // Creates a network endpoint 
                   IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Int16.Parse(BoPort));
                   CONNECT.AppendLine("PORT=" + BoPort);
   
                   // Create one Socket object to setup Tcp connection 
                   senderSock = new Socket(
                       ipAddr.AddressFamily, // Specifies the addressing scheme 
                       SocketType.Stream, // The type of socket  
                       ProtocolType.Tcp // Specifies the protocols  
                       );
   
                   senderSock.NoDelay = false; // Using the Nagle algorithm 
   
                   // Establishes a connection to a remote host 
                   senderSock.Connect(ipEndPoint);
   
               }
               catch (Exception exc)
               {
                   CONNECT.AppendLine(exc.Message);
   
               }
   
           }
   
           public static void Send(String xml)
           {
               try
               {
                   // Sending message 
                   //<Client Quit> is the sign for end of data 
                   string theMessageToSend = xml;
                   byte[] msg = FormatMessae(xml);
   
                   // Sends data to a connected Socket. 
                   int bytesSend = senderSock.Send(msg);
                   ReceiveDataFromServer();
                }
   
               catch(Exception exc)
                {
                   
                }
           }
   
           public static String ReceiveDataFromServer()
           {
               try
               {
                   // Receives data from a bound Socket. 
                   int bytesRec = senderSock.Receive(bytes);
   
                   // Converts byte array to string 
                   String theMessageToReceive = Encoding.UTF8.GetString(bytes, 0, bytesRec);
   
                   // Continues to read the data till data isn't available 
                   while (senderSock.Available > 0)
                   {
                       bytesRec = senderSock.Receive(bytes);
                       theMessageToReceive += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                   }
                   var xd = new XmlDocument();
                   xd.LoadXml(theMessageToReceive.Substring(4));
   
                   var node = xd.SelectSingleNode("/HBI/Response/ResponseCode");
                   if (node != null)
                   {
                        response = node.InnerText;
                   }
               
                   return response;
               }
               catch (Exception exc)
               {
                   return "";
               }
           }
   
           private void Disconnect()
           {
               try
               {
                   // Disables sends and receives on a Socket. 
                   senderSock.Shutdown(SocketShutdown.Both);
   
                   //Closes the Socket connection and releases all resources 
                   senderSock.Close();
               }
               catch (Exception exc)
               {
                   
               }
           }
   
           protected static string GetIPAddress()
           {
               System.Web.HttpContext context = System.Web.HttpContext.Current;
               string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
   
               if (!string.IsNullOrEmpty(ipAddress))
               {
                   string[] addresses = ipAddress.Split(',');
                   if (addresses.Length != 0)
                   {
                       return addresses[0];
                   }
               }
   
               return context.Request.ServerVariables["REMOTE_ADDR"];
           }
   
           public static byte[] FormatMessae(String messa)
           {
               String messageLenght = String.Format(messa.Length.ToString());
               return Encoding.UTF8.GetBytes(messageLenght + messa); ;
           }
   
           ////////////////////////////////////////////////////////////////////
           
       }*/



    }
}
 
   
