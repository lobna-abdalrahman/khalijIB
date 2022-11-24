using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1; 
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;
using System.IO;


namespace IBLogic
{
    public class MyChequeStatus
    {
        public static  string configip = null, configport = null, configpath =null;
        //Base Url
        public void getconfig() {
            try
            {
                
                using (StreamReader sr = new StreamReader("C:/Configuration/api.txt"))
                {
                    string line;

                    
                    while ((line = sr.ReadLine()) != null)
                    {
                        configip = line;
                          configport = sr.ReadLine();
                          configpath = sr.ReadLine();
                          BASE_URL = "http://" + configip + ":" + configport + "/" + configpath;
                    }
                }
            }
            catch (Exception e)
            {
                 String s=e.Message;
            }
        }

        public string BASE_URL;// = "http://"+configip+":"+configport+configpath;
        // public string BASE_URL = "http://192.168.66.9:8080/IBMiddleware/webresources/IBWebservices";
       // public string BASE_URL = "http://192.168.0.126:8080/IBMiddleware/webresources/IBWebservices";


        public string getKey()
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/getkey");
            var responJsonText = "";
            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient.GetAsync(requestUri).Result;


                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);

                }

                return responJsonText;
                
            }
        }

      /*  public string getKey()
        {
            string key = "abcdefg123434535";
            return key;
        }*/

        //----------------------------------------------------------------------------------------------------------
        //GET STATEMENT
        public string GetInvestmentInstalment(string accountNo)
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/GetInvestmentInstalment");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.account = accountNo;//"990042010593883".ToString();
         
            dynamicJson.lang = "1";
            dynamicJson.uuid = Guid.NewGuid();

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception e)
                {
                    /*Console.WriteLine(e);
                    throw;*/
                }

                return responJsonText;

            }

        }
        public string getChequeStatus(string accountNo, string chequeNo)
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/GetChqstat");
                
            dynamic dynamicJson = new ExpandoObject();
            
            dynamicJson.account = accountNo;//"990042010593883".ToString();
            dynamicJson.ChequeNumber = Convert.ToInt32(chequeNo);//"10";
            dynamicJson.lang = 1;
            dynamicJson.uuid = Guid.NewGuid();

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                   // throw;
                }

                return responJsonText;
                
            }
        }
        //----------------------------------------------------------------------------------------------------------
        //GET STATEMENT
        public string GetStatement(string accountNo, int noOfTrans, string fromDate, string toDate,
            string selectedMethod)
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/Getstat");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.account = accountNo;//"990042010593883".ToString();
            dynamicJson.NoOFtransaction =  noOfTrans;//"10";
            dynamicJson.DateFrom = fromDate;
            dynamicJson.DateTo = toDate;
            dynamicJson.selectmethod = selectedMethod;
            dynamicJson.lang = "1";
            dynamicJson.uuid = Guid.NewGuid(); 

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception e)
                {
                   /* Console.WriteLine(e);
                    throw;*/
                }

                return responJsonText;

            }

        }
        //------------------------------------------------------------------------------------------
        //GET MultiBalance
        public string GetMultiBalance(string accountNo)
        {

            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/GetMultiBal");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.account = accountNo;//"990042010593883".ToString();
            dynamicJson.lang = "0";
            dynamicJson.uuid = Guid.NewGuid(); 

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    /*Console.WriteLine(e);
                    Console.Error.Write(e.Message);*/
                }

                return responJsonText;
            }
            
        }

        //------------------------------------------GetReceiverName--------------------------------------------
        //
        public string GetReceiverName(string AccountNo)
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/GetCustinfo");
            DateTime time = DateTime.Now;
            string mydate = time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //mydate = mydate.Substring(1,10);
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.account = AccountNo;                 
            dynamicJson.trandate = mydate;
            dynamicJson.lang = 1;
            dynamicJson.uuid = Guid.NewGuid();

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    /*Console.WriteLine(e);
                    Console.Error.Write(e.Message);*/
                }

                return responJsonText;
            }
        }

        //------------------------------------------AccountTransfer--------------------------------------------
        //
        public string DoAccountTransfer(string Accountfrom, string Accountto, string amount)
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/AccountTransfer");

            DateTime time = DateTime.Now;
            string mydate = time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //mydate = mydate.Substring(1,10);
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.accountfrom = Accountfrom;//"990042010593883".ToString();
            dynamicJson.accountto = Accountto;
            dynamicJson.amount = amount;
            dynamicJson.trandate = mydate;//time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); //= 09/06/2014   //DateTime.Now.ToString("dd/MM/yyyy"); 
            dynamicJson.lang = 1;
            dynamicJson.uuid = Guid.NewGuid();

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    /*Console.WriteLine(e);
                    Console.Error.Write(e.Message);*/
                }

                return responJsonText;
            }
            
        }

        //------------------------------------------DomultiAccountTransfer--------------------------------------------
        //
        public string DomultiAccountTransfer(string Accountfrom, string Accountto, string amount)
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/Multitransfer");

            DateTime time = DateTime.Now;
            string mydate = time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //mydate = mydate.Substring(1,10);
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.accountfrom = Accountfrom;//"990042010593883".ToString();
            dynamicJson.accountto = Accountto;
            dynamicJson.amount = amount;
            dynamicJson.trandate = mydate;//time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); //= 09/06/2014   //DateTime.Now.ToString("dd/MM/yyyy"); 
            dynamicJson.lang = 1;
            dynamicJson.uuid = Guid.NewGuid();

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    /*Console.WriteLine(e);
                    Console.Error.Write(e.Message);*/
                }

                return responJsonText;
            }

        }



//=============================================EBS Services=============================================================================

        //----------------------------------------GET EBS BALANCE-------------------------------------
        //
        public string getEBSBalance(string PAN, string expDate, string tel, string IPIN)
        {
            getconfig();

            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();

            string encryptedIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock(IPIN, publickey, uuid);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }

             Uri requestUri = new Uri(BASE_URL + "/EBSGetbal");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.uuid = uuid;
            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }

        }


        //------------------------------------------EBS CARD TRANSFER-------------------------------------------
        //
        public string DoCardTransfer(string PAN, string expDate, string Amount, string tel , string IPIN,string toCard)
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();
            string desc = "Card transfer";
            string fromAccountType = "00";
            string toAccountTupe = "00";

            string encryptedIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(IPIN, publickey, uuid);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            //EBSCardTransfer
            Uri requestUri = new Uri(BASE_URL + "/EBSCardTransfer");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.tranAmount = Amount;
            dynamicJson.toCard = toCard;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.uuid = uuid;
            dynamicJson.fromAccountType = fromAccountType;
            dynamicJson.toAccountType = toAccountTupe;
            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }
                    
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }
            
        }

        //------------------------------------------EBS CARDLESS TRANSFER-------------------------------------------
        //
        public string DoCardLessTransfer(string PAN, string expDate, string Amount, string tel, string IPIN, string voucherNumber)
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();
            string desc = "Cardless transfer";
            string fromAccountType = "00";
            string toAccountTupe = "00";

            string encryptedIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(IPIN, publickey, uuid);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            //EBSCardTransfer
            Uri requestUri = new Uri(BASE_URL + "/EBSCardless");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.tranAmount = Amount;
            dynamicJson.voucherNumber = voucherNumber;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.uuid = uuid;
            dynamicJson.fromAccountType = fromAccountType;
            dynamicJson.toAccountType = toAccountTupe;
            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }

        }


        //------------------------------------------EBS Do Customes Bill Payment-------------------------------------------
        //
        public string EBSCustomsBillInquiry(string PAN, string expDate, string Amount, string tel, string IPIN, string payInfo,string declarant, string payId, string Serial, string desc )
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();



            string encryptedIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(IPIN, publickey, uuid);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            //EBSCardTransfer
            Uri requestUri = new Uri(BASE_URL + "/EBSBillInqurie");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.tranAmount = Amount;
            dynamicJson.payInfo = payInfo;
            dynamicJson.DeclarantNumber = declarant;
            dynamicJson.payId = payId;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.Serial = Serial;
            dynamicJson.uuid = uuid;

            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }

        }


        //------------------------------------------EBS Do Bill Payment-------------------------------------------
        //
        public string EBSBillInquiry(string PAN, string expDate, string Amount, string tel, string IPIN, string payInfo, string payId, string Serial, string desc)
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();
            


            string encryptedIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(IPIN, publickey, uuid);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            //EBSCardTransfer
            Uri requestUri = new Uri(BASE_URL + "/EBSBillInqurie");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.tranAmount = Amount;
            dynamicJson.payInfo = payInfo;
            dynamicJson.payId = payId;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.Serial = Serial;
            dynamicJson.uuid = uuid;

            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }

        }


        //------------------------------------------EBS Do Bill Payment-------------------------------------------
        //
        public string EBSDoBill(string PAN, string expDate, string Amount, string tel, string IPIN, string payInfo, string payId, string Serial,string desc)
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();
      


            string encryptedIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(IPIN, publickey, uuid);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            //EBSCardTransfer
            Uri requestUri = new Uri(BASE_URL + "/EBSDobill");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.tranAmount = Amount;
            dynamicJson.payInfo = payInfo;
            dynamicJson.payId = payId;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.Serial = Serial;
            dynamicJson.uuid = uuid;

            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }

        }

        //------------------------------------------EBS Custom Do Bill Payment-------------------------------------------
        //
        public string EBSCustomsDoBill(string PAN, string expDate, string Amount, string tel, string IPIN, string payInfo, string declarant, string payId, string Serial, string desc)
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();



            string encryptedIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(IPIN, publickey, uuid);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //EBSCardTransfer
            Uri requestUri = new Uri(BASE_URL + "/EBSDobill");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.tranAmount = Amount;
            dynamicJson.payInfo = payInfo;
            dynamicJson.payId = payId;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.Serial = Serial;
            dynamicJson.uuid = uuid;
            dynamicJson.DeclarantNumber = declarant;
            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return responJsonText;
            }

        }
        //------------------------------------------EBS ChangeIPIN-------------------------------------------
        //
        public string DoChangeIPIN(string PAN, string expDate, string tel, string CurrentIPIN, string newIPIN,string Serial)
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();
            string uuid2 = Guid.NewGuid().ToString();
            string desc = "ChangeIPIN";


            string encryptedIPIN = "";
            string encryptedNewIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(CurrentIPIN, publickey, uuid);
                encryptedNewIPIN = getIPINBlock2(newIPIN, publickey, uuid2);

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            //EBSChangeIPIN
            Uri requestUri = new Uri(BASE_URL + "/EBSChangeIPIN");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.newIPIN = encryptedNewIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.uuid1 = uuid;
            dynamicJson.uuid2 = uuid2;
            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }

        }

        //------------------------------------------EBS ChangeIPIN-------------------------------------------
        //
        /// <summary>
        /// /
        /// </summary>
        /// <param name="PAN"></param>
        /// <param name="expDate"></param>
        /// <param name="ipin"></param>
        /// <param name="tel"></param>
        /// <param name="invoiceNo"></param>
        /// <param name="PhoneNo"></param>
        /// <param name="Serial"></param>
        /// <returns>Json String response</returns>
        public string EBSGovBill(string PAN, string expDate, string ipin, string tel, string invoiceNo, string PhoneNo, string Serial)
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();
            //string uuid2 = Guid.NewGuid().ToString();
            string desc = "E15 Bill Inquiry";
            string SERVICEID = "2";
            string payId = ("6");


            string encryptedIPIN = "";
            //string encryptedNewIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(ipin, publickey, uuid);
               // encryptedNewIPIN = getIPINBlock2(newIPIN, publickey, uuid2);

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            //EBSChangeIPIN
            Uri requestUri = new Uri(BASE_URL + "/EBSgovbill");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.uuid = uuid;
            dynamicJson.SERVICEID = SERVICEID;
            dynamicJson.INVOICENUMBER = invoiceNo;
            dynamicJson.PHONENUMBER = PhoneNo;
            dynamicJson.PayId = payId;
            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }

        }




        public string EBSGovBillPayment(string PAN, string expDate, string ipin, string tel, string invoiceNo, string PhoneNo,string tranAmount, string Serial)
        {
            getconfig();
            string tranCurrency = "SDG";
            string authenticationType = "1";
            string uuid = Guid.NewGuid().ToString();
            //string uuid2 = Guid.NewGuid().ToString();
            string desc = "E15 BillPayment";
            string SERVICEID = "2";
            string payId = ("6");


            string encryptedIPIN = "";
            //string encryptedNewIPIN = "";
            //trying to parse the key to get the IPINBlock
            try
            {
                //GET Encrypted IPIN Block
                string myKey = getKey();
                JObject jobj = new JObject();
                jobj = JObject.Parse(myKey);
                dynamic result = jobj;

                string publickey = result.key;
                encryptedIPIN = getIPINBlock2(ipin, publickey, uuid);
                // encryptedNewIPIN = getIPINBlock2(newIPIN, publickey, uuid2);

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            //EBSChangeIPIN
            Uri requestUri = new Uri(BASE_URL + "/EBSgovpayment");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.PAN = PAN;//"990042010593883".ToString();
            dynamicJson.desc = desc;
            dynamicJson.expDate = expDate;
            dynamicJson.tel = tel;
            dynamicJson.IPIN = encryptedIPIN;
            dynamicJson.tranCurrency = tranCurrency;
            dynamicJson.authenticationType = authenticationType;
            dynamicJson.uuid = uuid;
            dynamicJson.SERVICEID = SERVICEID;
            dynamicJson.INVOICENUMBER = invoiceNo;
            dynamicJson.PHONENUMBER = PhoneNo;
            dynamicJson.PayId = payId;
            dynamicJson.tranAmount = tranAmount;
            dynamicJson.lang = "0";
            dynamicJson.uuid = uuid;//"fffff";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                return responJsonText;
            }

        }


        //----------------------------GET IPIN Block-------------------------------------
        public string getIPINBlock(string ipin, string publicKey, string uuid)
        {
             
            string clearIPIN = uuid + ipin;

            byte[] keyByte = Base64.Decode(publicKey);
            //byte[] keyByte2;
            //X509Certificate2 s = new X509Certificate2(keyByte);

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);

            
           // PublicKey pub = s.PublicKey;

            UnicodeEncoding data = new UnicodeEncoding();
           // keyByte2 =  data.GetBytes(pub.ToString() );
            PublicKeyFactory factory = null;

          //  factory = (RSACryptoServiceProvider)s.PublicKey;
           
         
            RSACryptoServiceProvider myRSAProvide = new RSACryptoServiceProvider();
            RSAParameters RSAKeyInfo = RSA.ExportParameters(false);
          ////  RSAKeyInfo.Modulus = keyByte;
            //Import key parameters into RSA.
           //// RSA.ImportParameters(RSAKeyInfo);
            RSA.FromXmlString(publicKey);
            string strCrypt = null;
            byte[] bteCrypt = null;
            byte[] bteResult = null;
            try
            {
               
                strCrypt = clearIPIN;
                bteCrypt = Encoding.ASCII.GetBytes(strCrypt);
                bteResult = myRSAProvide.Encrypt(bteCrypt, false);
                Console.WriteLine(Encoding.ASCII.GetString(bteResult));
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex.Message);
            }


            return Convert.ToBase64String(bteResult);
        }

        public string getIPINBlock2(string ipin, string stringPublicKey, string uuid)
        {
             
            string clearIPIN = uuid + ipin;

            Asn1Object obj = Asn1Object.FromByteArray(Convert.FromBase64String(stringPublicKey));
            DerSequence publicKeySequence = (DerSequence)obj;

            DerBitString encodedPublicKey = (DerBitString)publicKeySequence[1];
            DerSequence publicKey = (DerSequence)Asn1Object.FromByteArray(encodedPublicKey.GetBytes());

            DerInteger modulus = (DerInteger)publicKey[0];
            DerInteger exponent = (DerInteger)publicKey[1];
            RsaKeyParameters keyParameters = new RsaKeyParameters(false, modulus.PositiveValue, exponent.PositiveValue);
            RSAParameters parameters = DotNetUtilities.ToRSAParameters(keyParameters);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);
            byte[] dataToEncrypt = Encoding.Default.GetBytes(clearIPIN);
            byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);
            String eee = Convert.ToBase64String(encryptedData);
            return eee;
        }

        public string DocounterTransfer(string Accountfrom, string tobranch, string toname, string tophone, string  amount)
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL + "/CounterTransfer");

            DateTime time = DateTime.Now;
            string mydate = time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //mydate = mydate.Substring(1,10);
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.accountfrom = Accountfrom;//"990042010593883".ToString();
            dynamicJson.accountto = tobranch;
            dynamicJson.phonenumber = tophone;
            dynamicJson.name = toname;
            dynamicJson.amount = amount;
            dynamicJson.trandate = mydate;//time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); //= 09/06/2014   //DateTime.Now.ToString("dd/MM/yyyy"); 
            dynamicJson.lang = 1;
            dynamicJson.uuid = Guid.NewGuid();

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage respon = objClient
                        .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (respon.IsSuccessStatusCode)
                    {
                        responJsonText = respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    /*Console.WriteLine(e);
                    Console.Error.Write(e.Message);*/
                }

                return responJsonText;
            }
            
        }

        public string GetOrderinfo(string p1, string p2, string p3, string p4, string SessionID, string p5)
        {
            throw new NotImplementedException();
        }
    }
}
