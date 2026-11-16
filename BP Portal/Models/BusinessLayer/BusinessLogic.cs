using DemoProject.Models.DomainClasses;
using DemoProject.Models.DomainClasses;
using DemoProject.Models.SqlConnections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;
using POS;
using RestSharp;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO;
using System.Linq;
using System.Net;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ResponseStatus = DemoProject.Models.DomainClasses.ResponseStatus;

namespace DemoProject.Models.BusinessLayer
{
    public class BusinessLogic
    {

        private SAPbobsCOM.Company oCompany;
        private StringBuilder _stringBuilder;
        private ResponseModel _responseModel;
        private SqlConnectionData sqlDataAccessLayer;
        private SqlCon sqlcon;

        private Dictionary<string, double> oTranBatchDetail = new Dictionary<string, double>();


        static string CN1, CN2, CV1, CV2;
        static string strFun = string.Empty;

        List<string> Data = new List<string>();

        public string Query { get; private set; }
        public string Query2 { get; private set; }
        internal BusinessLogic()
        {
            this._responseModel = new ResponseModel();
            this._stringBuilder = new StringBuilder();
            this.sqlDataAccessLayer = new SqlConnectionData();
            this.sqlcon = new SqlCon();
            this._stringBuilder.Clear();
        }


        public ResponseModel SalesQuatation(SalesQuatation model)
        {
            string Result = DIAPIConnectCompay();
            int RetInt;
            if (Result == "Company Connected Successfully")
            {
                try
                {

                    this._responseModel = new ResponseModel();
                    DateTime Date = DateTime.ParseExact(model.DocDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime DateDueDate = DateTime.ParseExact(model.DocDueDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    SAPbobsCOM.Documents oSQ = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                    oSQ.DocDate = Date;
                    oSQ.DocDueDate = DateDueDate;
                    oSQ.CardCode = model.CardCode;
                    oSQ.NumAtCard = model.NumAtCard;
                    oSQ.Comments = model.Comments;

                    foreach (var Items in model.SalesQuotationDetails)
                    {
                        oSQ.Lines.ItemCode = Items.ItemCode;
                        oSQ.Lines.Quantity = Convert.ToDouble(Items.Quantity);
                        oSQ.Lines.WarehouseCode = Items.Warehouse;
                        oSQ.Lines.UnitPrice = Convert.ToDouble(Items.UnitPrice);
                        oSQ.Lines.Add();
                    }

                    RetInt = oSQ.Add();

                    if (RetInt == 0)
                    {

                        string DocEntry = oCompany.GetNewObjectKey().ToString();
                        string DocNum = getSingleValue("select \"DocNum\" from OQUT Where \"DocEntry\" ='" + DocEntry.Trim() + "'");

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oSQ);
                        GC.Collect();

                        this._responseModel.ResponseData = "DocNumber: " + DocNum;
                        this._responseModel.StatusCode = (int)StatusCodes.Success;
                        this._responseModel.StatusMessage = StatusCodes.Success.ToString();
                        return this._responseModel;
                    }

                    else
                    {
                        this._responseModel.ResponseData = oCompany.GetLastErrorDescription().ToString();
                        this._responseModel.StatusCode = (int)StatusCodes.Failed;
                        this._responseModel.StatusMessage = StatusCodes.Failed.ToString();
                        return this._responseModel;
                    }

                }
                catch (Exception ex)
                {
                    if (oCompany.InTransaction == true)
                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                    this._responseModel.ResponseData = ex.Message.ToString();
                    this._responseModel.StatusCode = (int)StatusCodes.Failed;
                    this._responseModel.StatusMessage = StatusCodes.Failed.ToString();
                    LogTracker.ErrorLog.WriteLog("The Error is : " + ex.ToString());
                    return this._responseModel;

                }
            }
            else
            {
                this._responseModel.ResponseData = "Company connection failed : " + Result.ToString().Replace('"', ' ').Trim() + "";
                this._responseModel.StatusCode = (int)StatusCodes.Failed;
                this._responseModel.StatusMessage = StatusCodes.Failed.ToString();
                return this._responseModel;
            }
            return this._responseModel;
        }

        public ResponseModel SalesOrder(SalesOrder model)
        {
            string Result = DIAPIConnectCompay();
            int RetInt;
            if (Result == "Company Connected Successfully")
            {
                try
                {

                    this._responseModel = new ResponseModel();
                    DateTime Date = DateTime.ParseExact(model.DocDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime DateDueDate = DateTime.ParseExact(model.DocDueDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    SAPbobsCOM.Documents oSO = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                    oSO.DocDate = Date;
                    oSO.DocDueDate = DateDueDate;
                    oSO.CardCode = model.CardCode;
                    oSO.NumAtCard = model.NumAtCard;
                    oSO.Comments = model.Comments;

                    foreach (var Items in model.SalesOrderDetails)
                    {
                        oSO.Lines.ItemCode = Items.ItemCode;
                        oSO.Lines.Quantity = Convert.ToDouble(Items.Quantity);
                        oSO.Lines.WarehouseCode = Items.Warehouse;
                        oSO.Lines.UnitPrice = Convert.ToDouble(Items.UnitPrice);
                        oSO.Lines.Add();
                    }

                    RetInt = oSO.Add();

                    if (RetInt == 0)
                    {

                        string DocEntry = oCompany.GetNewObjectKey().ToString();
                        string DocNum = getSingleValue("select \"DocNum\" from ORDR Where \"DocEntry\" ='" + DocEntry.Trim() + "'");

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oSO);
                        GC.Collect();

                        this._responseModel.ResponseData = "DocNumber: " + DocNum;
                        this._responseModel.StatusCode = (int)StatusCodes.Success;
                        this._responseModel.StatusMessage = StatusCodes.Success.ToString();
                        return this._responseModel;
                    }

                    else
                    {
                        this._responseModel.ResponseData = oCompany.GetLastErrorDescription().ToString();
                        this._responseModel.StatusCode = (int)StatusCodes.Failed;
                        this._responseModel.StatusMessage = StatusCodes.Failed.ToString();
                        return this._responseModel;
                    }

                }
                catch (Exception ex)
                {
                    if (oCompany.InTransaction == true)
                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                    this._responseModel.ResponseData = ex.Message.ToString();
                    this._responseModel.StatusCode = (int)StatusCodes.Failed;
                    this._responseModel.StatusMessage = StatusCodes.Failed.ToString();
                    LogTracker.ErrorLog.WriteLog("The Error is : " + ex.ToString());
                    return this._responseModel;

                }
            }
            else
            {
                this._responseModel.ResponseData = "Company connection failed : " + Result.ToString().Replace('"', ' ').Trim() + "";
                this._responseModel.StatusCode = (int)StatusCodes.Failed;
                this._responseModel.StatusMessage = StatusCodes.Failed.ToString();
                return this._responseModel;
            }
            return this._responseModel;
        }

        public string DIAPIConnectCompay()
        {

            string server = "";
            string DBusername = "";
            string CompanyDB = "";
            string DbPassword = "";
            string UserName = "";
            string Password = "";
            string DBServerID = "";
            string Licenseserver = "";
            string strConnect = "";
            string sCon = "";
            string[] MyArr = null;

            //if (oCompany.Connected == true)
            //    oCompany.Disconnect();

            strConnect = "SAPConnect";
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            MyArr = sCon.Split(';');

            CompanyDB = MyArr[0].ToString();
            UserName = MyArr[1].ToString();
            Password = MyArr[2].ToString();
            server = MyArr[3].ToString();
            DBusername = MyArr[4].ToString();
            DbPassword = MyArr[5].ToString();
            Licenseserver = MyArr[6];
            DBServerID = "SAPbobsCOM.BoDataServerTypes.dst_HANADB";
            oCompany = new SAPbobsCOM.Company();
            oCompany.Server = server.ToString();
            oCompany.DbUserName = DBusername.ToString();
            oCompany.DbPassword = DbPassword.ToString();
            //oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
            oCompany.UserName = UserName.ToString();
            oCompany.Password = Password.ToString();
            oCompany.CompanyDB = CompanyDB.ToString();
            int connection = oCompany.Connect();
            if (connection != 0)
            {
                string Error = oCompany.GetLastErrorDescription().ToString();
                return Error.ToString();

            }
            else
            {
                return "Company Connected Successfully";
            }

            return "Company Connected Successfully";
        }



        #region "Service Layer Login"
        public static string Login(out string strRouteVal)
        {
            string str_Response = string.Empty;
            string ResponseMessage = string.Empty;
            string URL = string.Empty;
            string CompanyDB = string.Empty;
            string UserName = string.Empty;
            string Password = string.Empty;

            try
            {
                strFun = "Login";
                string sURL = Global.APIBaseURL + strFun;

                string json = "{\"CompanyDB\":\"" + Global.CompanyDB + "\",\"UserName\":\"" + Global.APIUserName + "\",\"Password\":\"" + Global.APIPassWord + "\"}";
                var client = new RestClient(sURL);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();
                if (value != null)
                {
                    str_Response = JsonStringToDataTable(value, "Login");
                }
                CookieContainer cookie = new CookieContainer();
                var cookie_1 = response.Cookies.FirstOrDefault();
                var cookie_2 = response.Cookies.LastOrDefault();
                CN1 = cookie_1.Name;
                CN2 = cookie_2.Name;
                CV1 = cookie_1.Value;
                CV2 = cookie_2.Value;

                if (str_Response == "Company Connected")
                {
                    strRouteVal = "Success";
                    ResponseMessage = CV1;
                    return ResponseMessage;
                }
                else
                {
                    strRouteVal = "Failed";
                    ResponseMessage = str_Response;
                }

                return ResponseMessage;
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        public static string JsonStringToDataTable(string jsonString, string strFun)
        {
            try
            {
                DataTable dt = new DataTable();
                var trgArray = new JArray();
                var cleanRow1 = new JObject();
                var cleanRow = new JObject();
                var Rows = new JObject();
                var js = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);
                var jsonLinq = JObject.Parse(jsonString);
                string Cnd = string.Empty;
                string errval = string.Empty;
                string DocEntry = string.Empty;
                string DocNum = string.Empty;
                string CardCode = string.Empty;
                string JdtNum = string.Empty;
                string Number = string.Empty;
                string CardName = string.Empty;
                string DocumentEntry = string.Empty;
                string DocumentNumber = string.Empty;
                foreach (var lin in jsonLinq)
                {
                    if (lin.Key == "error")
                    {
                        Rows.Add(lin.Key, lin.Key);
                        var srcArray = jsonLinq.Descendants().Where(d => d is JObject).First();
                        foreach (JObject row in srcArray.Last())
                        {
                            cleanRow = new JObject();
                            foreach (JProperty column in row.Properties())
                            {

                                if (column.Value is JValue)
                                {
                                    if (column.Name == "value")
                                    {
                                        cleanRow.Add(column.Name, column.Value);
                                        errval = Convert.ToString(column.Value);
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        if (lin.Key == "DocEntry")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "JdtNum")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "Number")
                        {
                            DocNum = lin.Value.ToString();
                        }

                        else if (lin.Key == "DocNum")
                        {
                            DocNum = lin.Value.ToString();
                        }
                        else if (lin.Key == "CardCode")
                        {
                            CardCode = lin.Value.ToString();
                        }
                        else if (lin.Key == "CardName")
                        {
                            CardName = lin.Value.ToString();
                        }
                        else if (lin.Key == "AbsEntry")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "DepositNumber")
                        {
                            DocNum = lin.Value.ToString();
                        }
                        else if (lin.Key == "ReconNum")
                        {
                            DocNum = lin.Value.ToString();
                        }
                        else if (lin.Key == "DocumentEntry")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "DocumentNumber")
                        {
                            DocNum = lin.Value.ToString();
                        }
                        if (strFun == "Login")
                        {
                            errval = "Company Connected";
                            break;
                        }
                        else if (strFun == "BPMasterCreation")
                        {
                            if (CardCode != "" && CardName != "")
                            {
                                errval = CardCode + "#" + CardName + "#" + "Customer created successfully";
                                break;
                            }

                        }
                        else if (strFun == "SalesInvoice")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "ARInvoice created successfully";
                                break;
                            }
                        }
                        else if (strFun == "SalesReturn")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "SalesReturn created successfully";
                                break;
                            }
                        }

                        else if (strFun == "SalesReturn")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "SalesReturn created successfully";
                                break;
                            }
                        }
                        else if (strFun == "Incoming")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Payment created successfully";
                                break;
                            }

                        }
                        else if (strFun == "SalesOrder")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "SalesOrder Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "Quotations")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "SalesQuotation Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "GRPO")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "GRPO created successfully";
                                break;
                            }
                        }
                        else if (strFun == "StockTransferRequest")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Stocktransferrequest created successfully";
                                break;
                            }
                        }
                        else if (strFun == "Stocktransfer")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Stocktransfer created successfully";
                                break;
                            }
                        }
                        else if (strFun == "OutgoingPayment")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "OutgoingPayment created Sucessfully";
                                break;
                            }
                        }
                        else if (strFun == "Deposit")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Deposit created Sucessfully";
                                break;
                            }
                        }

                        else if (strFun == "JournalEntries")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Journal Entry Created Sucessfully";
                                break;
                            }
                        }
                        else if (strFun == "InternalReconciliations")
                        {
                            errval = "Reconciliation(s) done successfully";
                            break;

                        }
                        else if (strFun == "InventoryCountings")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "InventoryCountings created Sucessfully";
                                break;
                            }

                        }
                        else if (strFun == "ODEF")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Defective Document Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "GIS_OSPN")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Shipping Note Document Created successfully";
                                break;
                            }
                        }
                        //DENOMINATION
                        else if (strFun == "DENOMINATION")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Denomination Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "PurchaseReturns")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "PurchaseReturns created successfully";
                                break;
                            }
                        }
                        else if (strFun == "Drafts")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Drafts created successfully";
                                break;
                            }
                        }
                        else if (strFun == "GIS_GATE")
                        {
                            if (DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "GateEntry created successfully";
                                break;
                            }
                        }
                        else if (strFun == "APInvoice")
                        {
                            if (DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "APInvoice created successfully";
                                break;
                            }
                        }
                    }
                }
                return errval;
            }
            catch
            {
                throw;
            }
        }

        public static bool GetToken()
        {


            string sFuncName = String.Empty;
            sFuncName = "GetToken";

            Global.Function = "GetToken";
            string json = "{\"CompanyDB\":\"" + Global.CompanyDB + "\",\"UserName\":\"" + Global.APIUserName + "\",\"Password\":\"" + Global.APIPassWord + "\"}";
            //  string json = "{\"UserName\":\"ELUSHDEMO\",\"Password\":\"SM1rW9ZzHGhNzYmy\"}";
            string URL = Global.APIBaseURL + "Login";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = Global.ContentType;
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Proxy = null;
            httpWebRequest.KeepAlive = false;
            //httpWebRequest.Headers.Add("ApiAuth", Global.ApiAuth);
            httpWebRequest.ContentLength = json.Length;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.DefaultConnectionLimit = 9999;

            StreamWriter requestWriter = new StreamWriter(httpWebRequest.GetRequestStream(), System.Text.Encoding.UTF8);
            requestWriter.Write(json);
            requestWriter.Close();

            try
            {
                WebResponse webResponse = httpWebRequest.GetResponse();
                //webResponse = httpWebRequest.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                int start = response.IndexOf(":") + 1;
                int end = response.IndexOf(",", start);
                Global.Token = response.Substring(start, end - start);
                Global.Token = Global.Token.Trim(new Char[] { '"' });
                responseReader.Close();
                return false;

            }
            catch (WebException ex)
            {
                Global.WriteToLogFile_Error("Error : " + ex.ToString(), sFuncName);
                return false;
            }
        }

        #endregion



        #region "Sales Quotation and Order Posting using service layer" 

        public ResponseModel SalesQuotation(SalesQuotationPosting model)
        {
            Global.ConfigFileget();
            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;

            sFuncName = "Sales Quotation Posting Using Service Layer";

            try
            {
                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);
                TransOutput = SalesQuotationPosting(SAPToken, model);

                JObject json = JObject.Parse(TransOutput);

                var errorValue = json.SelectToken("error.message.value");

                if (errorValue == null)
                {
                    var DocEntry = json.SelectToken("DocEntry");
                    var DocNum = json.SelectToken("DocNum");
                    _responseModel.StatusCode = (int)ResponseStatus.Success;
                    _responseModel.StatusMessage = ResponseStatus.Success.ToString();
                    _responseModel.ResponseData = "DocNum: " + DocNum.ToString();
                    return this._responseModel;

                }
                else
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Failed;
                    _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                    _responseModel.ResponseData = "Error: " + errorValue;
                    return this._responseModel;
                }


            }
            catch (Exception ex)
            {

                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }

            return this._responseModel;

        }

        public static string SalesQuotationPosting(string str_SessionID, SalesQuotationPosting model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            try
            {
                strFun = "Quotations";
                URL = Global.APIBaseURL + strFun;

                string CardCode = model.cardcode;
                string DocDate = (model.DocDate);
                string DocDueDate = (model.DocDueDate);
                string NumAtCard = model.NumAtCard;
                string Comments = model.Comments;


                AddSalesQuotation OQUT = new AddSalesQuotation
                {
                    CardCode = CardCode,
                    DocDate = DocDate,
                    DocDueDate = DocDueDate,
                    NumAtCard = NumAtCard,
                    Comments = Comments,
                    DocumentLines = new List<AddSalesQuotation_Details>()
                };

                foreach (var Items in model.SalesQuotationDetails)
                {
                    AddSalesQuotation_Details OQUTRows = new AddSalesQuotation_Details
                    {
                        ItemCode = Items.ItemCode.ToString(),
                        Quantity = Items.Quantity.ToString(),
                        UnitPrice = Items.UnitPrice.ToString(),
                        Warehouse = Items.Warehouse.ToString(),
                        UoMCode = Items.UoM
                    };

                    OQUT.DocumentLines.Add(OQUTRows);
                }

                string JSON = JsonConvert.SerializeObject(OQUT);

                CV1 = str_SessionID;
                strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;

            }
            catch (WebException ex)
            {
                str_Response = ex.ToString();
            }
            return str_Response;
        }


        public ResponseModel SalesOrder(SalesOrderPosting model)
        {
            Global.ConfigFileget();
            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;

            sFuncName = "Sales Order Posting Using Service Layer";

            try
            {
                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);
                TransOutput = SalesOrderPosting(SAPToken, model);

                JObject json = JObject.Parse(TransOutput);

                var errorValue = json.SelectToken("error.message.value");


                if (errorValue == null)
                {
                    var DocEntry = json.SelectToken("DocEntry");
                    var DocNum = json.SelectToken("DocNum");
                    _responseModel.StatusCode = (int)ResponseStatus.Success;
                    _responseModel.StatusMessage = ResponseStatus.Success.ToString();
                    _responseModel.ResponseData = "DocNum: " + DocNum.ToString();
                    return this._responseModel;

                }
                else
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Failed;
                    _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                    _responseModel.ResponseData = "Error: " + errorValue;
                    return this._responseModel;
                }


            }
            catch (Exception ex)
            {

                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }

            return this._responseModel;

        }

        public static string SalesOrderPosting(string str_SessionID, SalesOrderPosting model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            try
            {
                strFun = "Orders";
                URL = Global.APIBaseURL + strFun;

                string CardCode = model.cardcode;
                string DocDate = (model.DocDate);
                string DocDueDate = (model.DocDueDate);
                string NumAtCard = model.NumAtCard;
                string Comments = model.Comments;


                AddSalesOrder ORDR = new AddSalesOrder
                {
                    CardCode = CardCode,
                    DocDate = DocDate,
                    DocDueDate = DocDueDate,
                    NumAtCard = NumAtCard,
                    Comments = Comments,
                    DocumentLines = new List<AddSalesOrder_Details>()
                };

                foreach (var Items in model.SalesOrderDetails)
                {
                    AddSalesOrder_Details ORDRRows = new AddSalesOrder_Details
                    {
                        ItemCode = Items.ItemCode.ToString(),
                        Quantity = Items.Quantity.ToString(),
                        UnitPrice = Items.UnitPrice.ToString(),
                        Warehouse = Items.Warehouse.ToString(),
                        UoMCode = Items.UoM
                    };

                    ORDR.DocumentLines.Add(ORDRRows);
                }

                string JSON = JsonConvert.SerializeObject(ORDR);

                CV1 = str_SessionID;
                strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;

            }
            catch (WebException ex)
            {
                str_Response = ex.ToString();
            }
            return str_Response;
        }



        //Start Live
        public ResponseModel getARinvoiceList(Body model)
        {

            try
            {

                //model.Index = model.Index;
                //Int32 Fromrownumber = 0;
                //Int32 Torownumber = 0;
                //if (model.Index == 0)
                //{
                //    Fromrownumber = model.Index;
                //    Torownumber = Fromrownumber + 10000;
                //}
                //else
                //{
                //    Fromrownumber = (model.Index * 100) - 100;
                //    Torownumber = Fromrownumber + 100;
                //    Fromrownumber = Fromrownumber + 1;
                //}



                this._responseModel = new ResponseModel();
                //this._stringBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY T0.\"DocEntry\" ASC) AS \"RowNum\", T0.\"DocEntry\", T0.\"DocNum\", T0.\"DocDate\",T0.\"DocDueDate\", T0.\"CardCode\", T0.\"CardName\", T0.\"DocTotal\" FROM OINV T0 WHERE T0.\"CANCELED\" ='N' and  T0.\"DocStatus\" ='O' order by T0.\"DocEntry\")");
               // this._stringBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY T0.\"DocEntry\" ASC) AS \"RowNum\", T0.\"DocEntry\", T0.\"DocNum\", T0.\"DocCur\", T0.\"CardCode\", T0.\"CardName\", T0.\"LicTradNum\", T0.\"U_U_KRA_CU_No\",T0.\"PaidToDate\", T0.\"DocTotal\",(\"DocTotal\" - \"PaidToDate\") AS \"BalanceDue\",(T0.\"DocTotal\"+ T0.\"DiscSum\" - T0.\"VatSum\"- T0.\"TotalExpns\"+ T0.\"WTSum\") AS \"TotalBeforeDiscount\" FROM OINV T0 WHERE T0.\"CANCELED\" ='N' and  T0.\"DocStatus\" ='O' order by T0.\"DocEntry\")");
                this._stringBuilder.Append("SELECT  T0.\"DocEntry\",T0.\"DocDate\", T0.\"DocNum\", T0.\"DocCur\", T0.\"CardCode\", T0.\"CardName\", T0.\"LicTradNum\",T0.\"U_SERIAL\",T0.\"PaidToDate\", T0.\"DocTotal\",(\"DocTotal\" - \"PaidToDate\") AS \"BalanceDue\",(T0.\"DocTotal\"+ T0.\"DiscSum\" - T0.\"VatSum\"- T0.\"TotalExpns\"+ T0.\"WTSum\") AS \"TotalBeforeDiscount\" FROM OINV T0 WHERE T0.\"CANCELED\" ='N' and  T0.\"DocStatus\" ='O' AND T0.\"DocDate\" BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "'  order by T0.\"DocEntry\"");
                this.sqlDataAccessLayer = new SqlConnectionData();
                this.sqlDataAccessLayer.Query = this._stringBuilder.ToString();
                this.Query = this._stringBuilder.ToString();
                this.Query2 = "SELECT  T0.\"DocEntry\",T0.\"DocDate\", T0.\"DocNum\", T0.\"DocCur\", T0.\"CardCode\", T0.\"CardName\", T0.\"LicTradNum\",T0.\"U_SERIAL\",T0.\"PaidToDate\", T0.\"DocTotal\",(\"DocTotal\" - \"PaidToDate\") AS \"BalanceDue\",(T0.\"DocTotal\"+ T0.\"DiscSum\" - T0.\"VatSum\"- T0.\"TotalExpns\"+ T0.\"WTSum\") AS \"TotalBeforeDiscount\" FROM OINV T0 WHERE T0.\"CANCELED\" ='N' and  T0.\"DocStatus\" ='O' AND T0.\"DocDate\" BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "' order by T0.\"DocEntry\"";
               // this.Query2 = "SELECT CEIL(CAST(COUNT(*) as Decimal(18,2))/100.0) CNT FROM(SELECT ROW_NUMBER() OVER (ORDER BY T0.\"DocEntry\" ASC) AS \"RowNum\",  T0.\"DocEntry\", T0.\"DocNum\", T0.\"DocCur\", T0.\"CardCode\", T0.\"CardName\", T0.\"LicTradNum\", T0.\"U_U_KRA_CU_No\",T0.\"PaidToDate\", T0.\"DocTotal\",(\"DocTotal\" - \"PaidToDate\") AS \"BalanceDue\",(T0.\"DocTotal\"+ T0.\"DiscSum\" - T0.\"VatSum\"- T0.\"TotalExpns\"+ T0.\"WTSum\") AS \"TotalBeforeDiscount\" FROM OINV T0 WHERE T0.\"CANCELED\" ='N' and  T0.\"DocStatus\" ='O' order by T0.\"DocEntry\") A";

        //        Dictionary<string, string> databases = new Dictionary<string, string>
        //         {
        //{ "POSConnection", "ASLTRADING" },
        //{ "POSConnection1", "ASL_CHEM_LIVE" },
        //{ "POSConnection2", "ASL_IND" },
        //{ "POSConnection3", "ASLWC" }
        ////{ "POSConnection4", "SBK_ASL_SS_ACC" }

        //        };

        //        return this.sqlDataAccessLayer.responseDataMultiDatabase(Query, Query2, databases);
            }


            catch (Exception ex)
            {
                LogTracker.ErrorLog.WriteLog("The Error is : " + ex.ToString());
                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }
            return this.sqlDataAccessLayer.responseData("getINVList", Query, Query2);
        }

        public ResponseModel getBankDetails()
        {

            try
            {
                this._responseModel = new ResponseModel();
                this._stringBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY T0.\"BankCode\" ASC) AS \"RowNum\", T0.\"BankCode\", T0.\"BankName\" FROM ODSC T0) A");
                this.sqlDataAccessLayer = new SqlConnectionData();
                this.sqlDataAccessLayer.Query = this._stringBuilder.ToString();
                this.Query = this._stringBuilder.ToString();
                this.Query2 = "SELECT CEIL(CAST(COUNT(*) as Decimal(18,2))/100.0) CNT FROM(SELECT ROW_NUMBER() OVER (ORDER BY T0.\"BankCode\" ASC) AS \"RowNum\", T0.\"BankCode\", T0.\"BankName\" FROM ODSC T0) A";
            }
            catch (Exception ex)
            {
                LogTracker.ErrorLog.WriteLog("The Error is : " + ex.ToString());
                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }
            return this.sqlDataAccessLayer.responseData("getBnkList", Query, Query2);
        }
        public ResponseModel getCustomerDetails()
        {

            try
            {
                this._responseModel = new ResponseModel();
                this._stringBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY T0.\"BankCode\" ASC) AS \"RowNum\", T0.\"CardCode\", T0.\"CardName\", T0.\"LicTradNum\" FROM OCRD T0 WHERE  T0.\"CardType\" = 'C') A");
                this.sqlDataAccessLayer = new SqlConnectionData();
                this.sqlDataAccessLayer.Query = this._stringBuilder.ToString();
                this.Query = this._stringBuilder.ToString();
                this.Query2 = "SELECT CEIL(CAST(COUNT(*) as Decimal(18,2))/100.0) CNT FROM(SELECT ROW_NUMBER() OVER (ORDER BY T0.\"BankCode\" ASC) AS \"RowNum\", T0.\"CardCode\", T0.\"CardName\", T0.\"LicTradNum\" FROM OCRD T0 WHERE  T0.\"CardType\" = 'C') A";
            }
            catch (Exception ex)
            {
                LogTracker.ErrorLog.WriteLog("The Error is : " + ex.ToString());
                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }
            return this.sqlDataAccessLayer.responseData("getCustomerList", Query, Query2);
        }
        public ResponseModel getCurrencyDetails()
        {

            try
            {
                this._responseModel = new ResponseModel();
                this._stringBuilder.Append("SELECT T0.\"CurrCode\", T0.\"CurrName\" FROM OCRN T0");
                this.sqlDataAccessLayer = new SqlConnectionData();
                this.sqlDataAccessLayer.Query = this._stringBuilder.ToString();
                this.Query = this._stringBuilder.ToString();
                this.Query2 = "SELECT T0.\"CurrCode\", T0.\"CurrName\" FROM OCRN T0";
            }
            catch (Exception ex)
            {
                LogTracker.ErrorLog.WriteLog("The Error is : " + ex.ToString());
                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }
            return this.sqlDataAccessLayer.responseData("getCurrencyList", Query, Query2);
        }
        public ResponseModel PaymentAdd(List<PaymentAdd> payments)
        {

            if (payments == null)
            {
                _responseModel.StatusCode = (int)ResponseStatus.NotFound;
                _responseModel.StatusMessage = "Data Is Not Present";
                return this._responseModel;
            }

            try
            {
                string strConnect = "";
                string sCon = "";
                strConnect = "FilePath";
                sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
                StreamWriter oStreamWriter = null;
                try
                {
                    var Values = JsonConvert.SerializeObject(payments);
                    sCon = sCon + "PaymentDetails_" + System.DateTime.Now.ToString("MMddyyyyHHmmss") + ".txt";
                    oStreamWriter = File.AppendText(sCon);
                    oStreamWriter.WriteLine(Values.ToString());
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                    oStreamWriter = null;
                }

                bool IsHddAvil = false;
                bool IsHdAdd = false;
                bool IsRowAdd = false;


                if (payments.Count > 0)
                {
                    List<Int32> HD = new List<Int32>();
                    foreach (var payment in payments)
                    {
                        string _id = payment._id;
                        string user = payment.user;
                        string trans_date = payment.trans_date;
                        string payment_date = payment.payment_date;
                        double payment_amount = payment.payment_amount;
                        string CardName = payment.CardName.Replace("'", "''");
                        string paymentsTransIDs = payment.paymentsTransIDs;
                        double invoiceTotalDue = payment.invoiceTotalDue;
                        double remainingBalance = payment.remainingBalance;
                        string TruckNumber = payment.TruckNumber;
                        string SAPSyncStatus1 = payment.SAPSyncStatus;
                        string paymentAccount = payment.paymentAccount;
                        string paymentType = payment.paymentType;
                        string bankCode = payment.bankCode;
                        string checkNo = payment.checkNo;

                        string IN = GetSingleValue_HANA("select count(*) from \"PaymentHeader1\" where \"_id\" = '" + _id + "'");

                        if (IN == "0")
                        {
                            IsHddAvil = true;
                            string HeaderId = GetSingleValue_HANA("call \"sp_Pay_Insert_Header1\"('" + _id + "','" + user + "'," +
                                "'" + trans_date + "','" + payment_date + "', '" + payment_amount + "', '" + CardName + "', '" + paymentsTransIDs + "', '" + invoiceTotalDue + "', " +
                                "'" + remainingBalance + "','" + TruckNumber + "','" + SAPSyncStatus1 + "','" + paymentAccount + "','" + paymentType + "','" + bankCode + "','" + checkNo + "',  ?) ");

                            if (!string.IsNullOrEmpty(HeaderId))
                            {
                                HD.Add(Convert.ToInt32(HeaderId));
                                IsHdAdd = true;
                                string HeaderId1 = GetSingleValue_HANA("select count(\"HeaderID\") from \"InvoiceDetails1\" where \"HeaderID\" = '" + HeaderId + "'");
                                if (payment.invoices.Count > 0 && HeaderId1 == "0")
                                {
                                    List<string> mylist = new List<string>();
                                    foreach (var Inv in payment.invoices)
                                    {
                                        string _id1 = Inv._id;
                                        string trans_id = Inv.trans_id;
                                        string docEntry = Inv.docEntry;
                                        string docNum = Inv.docNum;
                                        string docDate = Inv.docDate;
                                        string docDueDate = Inv.docDueDate;
                                        string numAtCard = Inv.numAtCard;
                                        string cardCode = Inv.cardCode;
                                        string cardName = Inv.cardName.Replace("'", "''");
                                        double vatSum = Inv.vatSum;
                                        double docTotal = Inv.docTotal;
                                        double paidTotal = Inv.paidTotal;
                                        double balanceDue = Inv.balanceDue;
                                        string comments = Inv.comments;
                                        string SAPSyncStatus = Inv.SAPSyncStatus;
                                        double __v = Inv.__v;
                                        string createdAt = Inv.createdAt;
                                        string updatedAt = Inv.updatedAt;
                                        double appliedAmount = Inv.appliedAmount;

                                        bool Result1 = NonExecuteHANA("call \"sp_Inv_Insert_Details1\"('" + HeaderId + "','" + _id1 + "', '" + trans_id + "', '" + docEntry + "'," +
                                        "'" + docNum + "', '" + docDate + "', '" + docDueDate + "', '" + numAtCard + "', '" + cardCode + "', '" + cardName + "', " +
                                        "'" + vatSum + "', '" + docTotal + "', '" + paidTotal + "', '" + balanceDue + "','" + comments + "', '" + SAPSyncStatus + "', " +
                                        "'" + __v + "', '" + createdAt + "', '" + updatedAt + "','" + appliedAmount + "')");
                                        if (Result1)
                                        {
                                            IsRowAdd = true;
                                        }
                                        else
                                        {

                                            var RMdata = String.Join(",", HD);
                                            bool Result2 = NonExecuteHANA("delete from \"InvoiceDetails1\" where \"HeaderID\" in ('" + RMdata + "')");
                                            bool Result3 = NonExecuteHANA("delete from \"PaymentHeader1\" where \"ID\" in ('" + RMdata + "')");

                                            _responseModel.StatusCode = (int)StatusCodes.Failed;
                                            _responseModel.StatusMessage = "Error on Add Invoices Data";
                                            return this._responseModel;
                                        }
                                    }
                                }
                                else
                                {
                                    var RMdata = String.Join(",", HD);
                                    bool Result2 = NonExecuteHANA("delete from \"InvoiceDetails1\" where \"HeaderID\" in ('" + RMdata + "')");
                                    bool Result3 = NonExecuteHANA("delete from \"PaymentHeader1\" where \"ID\" in ('" + RMdata + "')");

                                    _responseModel.StatusCode = (int)StatusCodes.NotFound;
                                    _responseModel.StatusMessage = "Invoice Details Not Found or Error on Add Invoices Data";
                                    return this._responseModel;
                                }
                            }
                            else
                            {
                                var RMdata = String.Join(",", HD);
                                bool Result2 = NonExecuteHANA("delete from \"InvoiceDetails1\" where \"HeaderID\" in ('" + RMdata + "')");
                                bool Result3 = NonExecuteHANA("delete from \"PaymentHeader1\" where \"ID\" in ('" + RMdata + "')");

                                _responseModel.StatusCode = (int)StatusCodes.Failed;
                                _responseModel.StatusMessage = "Error on Add Payment Header Data";
                                return this._responseModel;
                            }
                        }
                        else
                        {
                            //_responseModel.StatusCode = (int)StatusCodes.IsPosted;
                            //_responseModel.StatusMessage = "Already Data Added for this ID : " + _id;
                            //return this._responseModel;
                        }

                    }

                    if (IsHdAdd == true && IsRowAdd == true)
                    {
                        _responseModel.StatusCode = (int)StatusCodes.Success;
                        _responseModel.StatusMessage = "Data Added Successfully";
                        return this._responseModel;
                    }
                    else if (!IsHddAvil)
                    {
                        _responseModel.StatusCode = (int)StatusCodes.Success;
                        _responseModel.StatusMessage = "Dublicate _id cannot Added";
                        return this._responseModel;
                    }
                }
                else
                {
                    _responseModel.StatusCode = (int)StatusCodes.NoData;
                    _responseModel.StatusMessage = "No Data";
                    return this._responseModel;
                }

            }
            catch (Exception ex)
            {
                _responseModel.StatusCode = (int)StatusCodes.Failed;
                _responseModel.StatusMessage = "Error on Data Insert" + ex.ToString();
                return this._responseModel;
            }

            return this._responseModel;

        }
        public ResponseModel ARInvoice(List<ARInvoiceUpdate> model)
        {
            Global.ConfigFileget();
            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;

            sFuncName = "AR Invoice Update Using Service Layer";

            try
            {

                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);
                List<ResponseModelList> Trans = ARInvoice(SAPToken, model);

                if (Trans.Count > 0)
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Success;
                    _responseModel.StatusMessage = ResponseStatus.Success.ToString();
                    _responseModel.ResponseData = Trans;
                    return this._responseModel;

                }
                else
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Failed;
                    _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                    _responseModel.ResponseData = "AR Invoice Update Return Count is Zero";
                    return this._responseModel;
                }


            }
            catch (Exception ex)
            {

                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }

            return this._responseModel;

        }
        public List<ResponseModelList> ARInvoice(string str_SessionID, List<ARInvoiceUpdate> model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            ResponseModelList BPRLData = new ResponseModelList();

            List<ResponseModelList> BPRL = new List<ResponseModelList>();

            if (model == null)
            {
                BPRLData = new ResponseModelList();
                BPRLData.StatusCode = (int)ResponseStatus.NotFound;
                BPRLData.StatusMessage = "Data Not Found";
                BPRLData.ResponseData = "Data is Not Present";
                BPRL.Add(BPRLData);
                return BPRL;
            }
            try
            {

                try
                {
                    var Values = JsonConvert.SerializeObject(model);
                    LogTracker.ErrorLog.WriteLog("Invoice JSON: " + Values.ToString());

                }
                catch (Exception ex)
                {

                }
                foreach (var invoice in model)
                {
                    BPRLData = new ResponseModelList();

                    string strFun = "Invoices(" + invoice.DocEntry + ")";
                    URL = Global.APIBaseURL + strFun;

                    var ARInvUpd = new
                    {
                        U_TruckNumber = invoice.TruckNumber,
                        U_DispatchDate = invoice.DispatchDate,
                        U_ReferenceNumber = invoice.ReferenceNumber,
                        U_LoadingStatus = invoice.LoadingStatus
                    };

                    string JSON = JsonConvert.SerializeObject(ARInvUpd);
                    //Global.WriteToLogFile_Debug_JSON("Invoice Update JSON: " + JSON.ToString(), "");
                    CV1 = str_SessionID;
                    strFun = TransactionType;
                    var client = new RestClient(URL + "?SessionId=" + CV1);
                    ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                    var request = new RestRequest(Method.PATCH);
                    //request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                    request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                    //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                    //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                    IRestResponse response = client.Execute(request);


                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        BPRLData.StatusCode = 0;
                        BPRLData.StatusMessage = "Success";
                        BPRLData.ResponseData = "AR Invoice Update Successfully";
                        BPRLData.DocEntry = invoice.DocEntry;
                    }
                    else if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
                    {
                        dynamic value = JsonConvert.DeserializeObject(response.Content);
                        value = (value == null) ? null : value.ToString();

                        str_Response = value;

                        JObject json = JObject.Parse(str_Response);

                        var errorValue = json.SelectToken("error.message.value");

                        BPRLData.StatusCode = 1;
                        BPRLData.StatusMessage = "Failed";
                        BPRLData.ResponseData = errorValue != null ? "AR Invoice Update Failed: " + errorValue.ToString() : "AR Invoice Update Failed.";
                        BPRLData.DocEntry = invoice.DocEntry;

                        try
                        {
                            LogTracker.ErrorLog.WriteLog("Invoice DocEntry: " + invoice.DocEntry.ToString() + "Truck Number: " + invoice.TruckNumber + " Error Msg: " + errorValue.ToString());

                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    //if (errorValue == null)
                    //{
                    //    BPRLData.StatusCode = 0;
                    //    BPRLData.StatusMessage = "Success";
                    //    BPRLData.ResponseData = ("AR Invoice Update Successfully : '" + invoice.DocEntry + "'");
                    //}
                    //else
                    //{
                    //    BPRLData.StatusCode = 1;
                    //    BPRLData.StatusMessage = "Failed";
                    //    BPRLData.ResponseData = ("AR Invoice Update Failed " + errorValue.ToString());
                    //}

                    BPRL.Add(BPRLData);


                }

            }
            catch (WebException ex)
            {
                BPRLData = new ResponseModelList();
                BPRLData.ResponseData = ex.ToString();
                BPRL.Add(BPRLData);
                return BPRL;
            }
            return BPRL;
        }
        public ResponseModel getPaymentStatus(PaymentStatus model)
        {

            try
            {

                if (!string.IsNullOrEmpty(model.DocNum))
                {
                    this._responseModel = new ResponseModel();
                    this._stringBuilder.Append("select T1.\"_id\", T1.\"DocNum\", T0.\"ReceiveDate\", T0.\"ErrMsg\", T0.\"Status\" from \"PaymentHeader1\" T0 inner join  \"InvoiceDetails1\" T1 on T0.\"ID\" = T1.\"HeaderID\" where T1.\"DocNum\" = '" + model.DocNum + "' order by T0.\"ID\", T1.\"HeaderID\"");
                    this.sqlDataAccessLayer = new SqlConnectionData();
                    this.sqlDataAccessLayer.Query = this._stringBuilder.ToString();
                    this.Query = this._stringBuilder.ToString();
                    this.Query2 = "select T1.\"_id\", T1.\"DocNum\", T0.\"ReceiveDate\", T0.\"ErrMsg\", T0.\"Status\" from \"PaymentHeader1\" T0 inner join  \"InvoiceDetails1\" T1 on T0.\"ID\" = T1.\"HeaderID\" where T1.\"DocNum\" = '" + model.DocNum + "' order by T0.\"ID\", T1.\"HeaderID\"";
                }
                else if (!string.IsNullOrEmpty(model.NumAtCard))
                {
                    this._responseModel = new ResponseModel();
                    this._stringBuilder.Append("select T1.\"_id\", T1.\"DocNum\", T0.\"ReceiveDate\", T0.\"ErrMsg\", T0.\"Status\" from \"PaymentHeader1\" T0 inner join  \"InvoiceDetails1\" T1 on T0.\"ID\" = T1.\"HeaderID\" where T1.\"NumAtCard\" = '" + model.NumAtCard + "' order by T0.\"ID\", T1.\"HeaderID\"");
                    this.sqlDataAccessLayer = new SqlConnectionData();
                    this.sqlDataAccessLayer.Query = this._stringBuilder.ToString();
                    this.Query = this._stringBuilder.ToString();
                    this.Query2 = "select T1.\"_id\", T1.\"DocNum\", T0.\"ReceiveDate\", T0.\"ErrMsg\", T0.\"Status\" from \"PaymentHeader1\" T0 inner join  \"InvoiceDetails1\" T1 on T0.\"ID\" = T1.\"HeaderID\" where T1.\"NumAtCard\" = '" + model.NumAtCard + "' order by T0.\"ID\", T1.\"HeaderID\"";
                }
                else
                {
                    this._responseModel = new ResponseModel();
                    this._stringBuilder.Append("select T1.\"_id\", T1.\"DocNum\", T0.\"ReceiveDate\", T0.\"ErrMsg\", T0.\"Status\" from \"PaymentHeader1\" T0 inner join  \"InvoiceDetails1\" T1 on T0.\"ID\" = T1.\"HeaderID\" order by T0.\"ID\", T1.\"HeaderID\"");
                    this.sqlDataAccessLayer = new SqlConnectionData();
                    this.sqlDataAccessLayer.Query = this._stringBuilder.ToString();
                    this.Query = this._stringBuilder.ToString();
                    this.Query2 = "select T1.\"_id\", T1.\"DocNum\", T0.\"ReceiveDate\", T0.\"ErrMsg\", T0.\"Status\" from \"PaymentHeader1\" T0 inner join  \"InvoiceDetails1\" T1 on T0.\"ID\" = T1.\"HeaderID\" order by T0.\"ID\", T1.\"HeaderID\"";
                }

            }
            catch (Exception ex)
            {
                LogTracker.ErrorLog.WriteLog("The Error is : " + ex.ToString());
                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }
            return this.sqlDataAccessLayer.responseData("getPayList", Query, Query2);
        }
        //End Live
        public ResponseModel ARInvoiceWithPayment_OLD(List<IncomingPaymentPosting> model)
        {
            Global.ConfigFileget();
            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;
            List<string> DocNums = new List<string>();
            List<string> Errors = new List<string>();
            sFuncName = "Incoming Payment Posting Using Service Layer";

            try
            {

                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);
                foreach (IncomingPaymentPosting payment in model)
                {
                    TransOutput = ARInvoiceWithPaymentPosting(SAPToken, payment);

                    JObject json = JObject.Parse(TransOutput);

                    var errorValue = json.SelectToken("error.message.value");


                    if (errorValue == null)
                    {
                        var DocEntry = json.SelectToken("DocEntry");
                        var DocNum = json.SelectToken("DocNum");
                        if (DocNum != null)
                        {
                             DocNums.Add(DocNum.ToString());
                           
                        }
                        //_responseModel.StatusCode = (int)ResponseStatus.Success;
                        //_responseModel.StatusMessage = ResponseStatus.Success.ToString();
                        //_responseModel.ResponseData = "DocNum: " + string.Join(", ", DocNums);
                        //return this._responseModel;

                    }
                    else
                    {
                        Errors.Add(payment.CardCode + ": " + errorValue.ToString());
                        // _responseModel.StatusCode = (int)ResponseStatus.Failed;
                        //_responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                        //_responseModel.ResponseData = "Error: " + errorValue;
                        //return this._responseModel;
                    }


                }
                _responseModel.StatusCode = (int)ResponseStatus.Success;
                _responseModel.StatusMessage = ResponseStatus.Success.ToString();
                _responseModel.ResponseData = "DocNum: " + string.Join(", ", DocNums) + (Errors.Count > 0 ? " | Failed Payments: " + string.Join(" ; ", Errors) : "");
                return this._responseModel;
            }
            catch (Exception ex)
            {

                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }



        }
        public ResponseModel ARInvoiceWithPayment(List<IncomingPaymentPosting> model)
        {
            Global.ConfigFileget();

            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;

            List<PaymentPostingResult> PaymentResults = new List<PaymentPostingResult>();

            sFuncName = "Incoming Payment Posting Using Service Layer";

            try
            {
                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);

                foreach (IncomingPaymentPosting payment in model)
                {
                    try
                    {
                        TransOutput = ARInvoiceWithPaymentPosting(SAPToken, payment);

                        JObject json = JObject.Parse(TransOutput);

                        var errorValue = json.SelectToken("error.message.value");

                        if (errorValue == null)
                        {
                            var DocNum = json.SelectToken("DocNum");

                            PaymentResults.Add(new PaymentPostingResult
                            {
                                CertificateNumber = payment.CertificateNumber,
                                PaymentDocNum = DocNum?.ToString(),
                                ErrorMsg = null
                            });
                        }
                        else
                        {
                            PaymentResults.Add(new PaymentPostingResult
                            {
                                CertificateNumber = payment.CertificateNumber,
                                PaymentDocNum = null,
                                ErrorMsg = errorValue.ToString()
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        PaymentResults.Add(new PaymentPostingResult
                        {
                            CertificateNumber = payment.CertificateNumber,
                            PaymentDocNum = null,
                            ErrorMsg = ex.Message
                        });
                    }
                }
                bool hasFailedPayments = PaymentResults.Any(x => x.ErrorMsg != null);

                if (hasFailedPayments)
                {
                    _responseModel.StatusCode = 1;
                    _responseModel.StatusMessage = "Failed";
                }
                else
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Success;
                    _responseModel.StatusMessage = "Success";
                }

                _responseModel.ResponseData = JsonConvert.SerializeObject(PaymentResults);

                return this._responseModel;
                //_responseModel.StatusCode = (int)ResponseStatus.Success;
                //_responseModel.StatusMessage = ResponseStatus.Success.ToString();
                //_responseModel.ResponseData = JsonConvert.SerializeObject(PaymentResults);

                //return this._responseModel;
            }
            catch (Exception ex)
            {
                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();

                return this._responseModel;
            }
        }
        public ResponseModel OnAccountPayment(List<OnAccountPaymentPosting> model)
        {
            Global.ConfigFileget();
            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;
            List<PaymentPostingResult> PaymentResults = new List<PaymentPostingResult>();
            sFuncName = "Incoming Payment Posting Using Service Layer";

            try
            {

                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);
                foreach (OnAccountPaymentPosting payment in model)
                {

                    TransOutput = OnAccountPaymentPosting(SAPToken, payment);

                    JObject json = JObject.Parse(TransOutput);

                    var errorValue = json.SelectToken("error.message.value");

                    if (errorValue == null)
                    {
                        var DocEntry = json.SelectToken("DocEntry");
                        var DocNum = json.SelectToken("DocNum");

                        PaymentResults.Add(new PaymentPostingResult
                        {
                            CertificateNumber = payment.CertificateNumber,
                            PaymentDocNum = DocNum.ToString(),
                            ErrorMsg = null
                        });


                    }
                    else
                    {
                        //_responseModel.StatusCode = (int)ResponseStatus.Failed;
                        //_responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                        //_responseModel.ResponseData = "Error: " + errorValue;
                        //return this._responseModel;
                        PaymentResults.Add(new PaymentPostingResult
                        {
                            CertificateNumber = payment.CertificateNumber,
                            PaymentDocNum = null,
                            ErrorMsg = errorValue.ToString()
                        });
                    }
                }
                bool hasFailedPayments = PaymentResults.Any(x => x.ErrorMsg != null);

                if (hasFailedPayments)
                {
                    _responseModel.StatusCode = 1;
                    _responseModel.StatusMessage = "Failed";
                }
                else
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Success;
                    _responseModel.StatusMessage = "Success";
                }

                _responseModel.ResponseData = JsonConvert.SerializeObject(PaymentResults);

                return this._responseModel;
                //_responseModel.StatusCode = (int)ResponseStatus.Success;
                //_responseModel.StatusMessage = ResponseStatus.Success.ToString();
                //_responseModel.ResponseData = JsonConvert.SerializeObject(PaymentResults);
                //return this._responseModel;
            }
            catch (Exception ex)
            {

                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }



        }
        public string ARInvoiceWithPaymentPosting_OLD(string str_SessionID, IncomingPaymentPosting model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            try
            {
                strFun = "IncomingPayments";
                URL = Global.APIBaseURL + strFun;
                string CashAccount = Global.GetSingleValue_HANA("SELECT T0.\"AcctCode\" FROM OACT T0 WHERE T0.\"BPLId\" = '" + model.BPLID + "' AND T0.\"LocManTran\" = 'N' AND T0.\"Postable\" = 'Y'");
                IncomingPayments ORCT = new IncomingPayments
                {
                    DocDate = DateTime.Now,
                    CardCode = model.CardCode,
                    //CounterReference = model.NumAtCard,
                    //Remarks = model.Comments,
                    // CashAccount = CashAccount,
                    //CashSum = model.CashAmount,
                    // BPLID = model.BPLID,
                    //TransferAccount = model.BankAccount,
                    //TransferSum = model.BankAmount,
                };

                List<IncomingPaymentInvoice> PAYINV = new List<IncomingPaymentInvoice>();

                foreach (var paymentDetail in model.InvoiceItemDetails)
                {
                    IncomingPaymentInvoice payinv = new IncomingPaymentInvoice();
                    {
                        //DataTable dt = Global.GetDataTable_HANA("Select \"DocEntry\" from OINV where \"DocNum\" = '" + paymentDetail.DocNum + "' ");
                        string docEntry = Global.GetSingleValue_HANA("Select \"DocEntry\" from OINV where \"DocNum\" = '' ");
                        payinv.DocEntry = Convert.ToInt32(docEntry);
                        payinv.SumApplied = paymentDetail.SumApplied;
                        payinv.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice.ToString();
                    }
                    ;
                    PAYINV.Add(payinv);
                }
                ORCT.PaymentInvoices = PAYINV;

                string JSON = JsonConvert.SerializeObject(ORCT);
                //Global.WriteToLogFile_Debug_JSON("Incoming Payment JSON: " + JSON.ToString(), "");
                CV1 = str_SessionID;
                strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;

            }
            catch (WebException ex)
            {
                str_Response = ex.Message.ToString();
            }
            return str_Response;
        }
        public string ARInvoiceWithPaymentPosting(string str_SessionID, IncomingPaymentPosting model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            try
            {
                strFun = "IncomingPayments";
                URL = Global.APIBaseURL + strFun;
                string DocCurrency = model.DocCurrency.Trim();
                double Amount = model.Amount;
                double ExchangeRate = model.ExchangeRate;
                double PayAmount = 0;
                double AppliedAmount = 0;
                double AppliedFC = 0;
                double TransferSum = 0;

                if (!DocCurrency.Equals("KES", StringComparison.OrdinalIgnoreCase) && ExchangeRate > 0)
                {
                    PayAmount = Amount / ExchangeRate;
                    AppliedAmount = Amount;
                    AppliedFC = PayAmount;
                    TransferSum = PayAmount;
                    DocCurrency = "USD";

                }
                if (DocCurrency.Equals("KES", StringComparison.OrdinalIgnoreCase) && ExchangeRate <= 0)
                {
                    PayAmount = Amount;
                    AppliedAmount = Amount;
                    AppliedFC = 0;
                    TransferSum = PayAmount;
                    DocCurrency = "KES";
                }

                IncomingPayments ORCT = new IncomingPayments
                {
                    DocDate = model.DocDate,
                    CardCode = model.CardCode.Trim(),
                    //ProjectCode = model.Site,
                    DocCurrency = DocCurrency,
                    CounterReference = "WHT-C",
                    Remarks = model.WithholderName,
                    JournalRemarks = model.CertificateNumber,
                    TransferReference = model.CertificateNumber,
                    TransferAccount = model.TransferAccount.Trim(),
                    TransferSum = PayAmount,
                };

                List<IncomingPaymentInvoice> PAYINV = new List<IncomingPaymentInvoice>();

                foreach (var paymentDetail in model.InvoiceItemDetails)
                {
                    IncomingPaymentInvoice payinv = new IncomingPaymentInvoice();
                    {
                        //DataTable dt = Global.GetDataTable_HANA("Select \"DocEntry\" from OINV where \"DocNum\" = '" + paymentDetail.DocNum + "' ");
                        string docEntry = paymentDetail.InvoiceDocEntry;//Global.GetSingleValue_HANA("Select \"DocEntry\" from OINV where \"DocNum\" = '" + paymentDetail.DocNum + "' ");
                        payinv.DocEntry = Convert.ToInt32(docEntry);
                        payinv.SumApplied = paymentDetail.SumApplied;
                        payinv.AppliedFC = paymentDetail.AppliedFC;
                        payinv.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice.ToString();
                    }
                    ;
                    PAYINV.Add(payinv);
                }
                ORCT.PaymentInvoices = PAYINV;

                string JSON = JsonConvert.SerializeObject(ORCT);
                //Global.WriteToLogFile_Debug_JSON("Incoming Payment JSON: " + JSON.ToString(), "");
                CV1 = str_SessionID;
                strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;

            }
            catch (WebException ex)
            {
                str_Response = ex.Message.ToString();
            }
            return str_Response;
        }
        public string OnAccountPaymentPosting(string str_SessionID, OnAccountPaymentPosting model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            try
            {
                strFun = "IncomingPayments";
                URL = Global.APIBaseURL + strFun;
                string DocCurrency = model.DocCurrency.Trim();
                double Amount = model.Amount;
                double ExchangeRate = model.ExchangeRate;
                double PayAmount = 0;
                double AppliedAmount = 0;
                double AppliedFC = 0;
                double TransferSum = 0;

                if (!DocCurrency.Equals("KES", StringComparison.OrdinalIgnoreCase) && ExchangeRate > 0)
                {
                    PayAmount = Amount / ExchangeRate;
                    AppliedAmount = Amount;
                    //AppliedFC = PayAmount;
                    TransferSum = PayAmount;
                    DocCurrency = "USD";

                }
                if (DocCurrency.Equals("KES", StringComparison.OrdinalIgnoreCase) && ExchangeRate <= 0)
                {
                    PayAmount = Amount;
                    AppliedAmount = Amount;
                    //AppliedFC = 0;
                    TransferSum = PayAmount;
                    DocCurrency = "KES";
                }

                OnAccountPayments ORCT = new OnAccountPayments
                {
                    DocDate = model.DocDate,
                    DocType = SAPbobsCOM.BoRcptTypes.rCustomer.ToString(),
                    CardCode = model.CardCode.Trim(),
                    //ProjectCode = model.Site,
                    DocCurrency = DocCurrency,
                    CounterReference = "WHT-C",
                    Remarks = model.WithholderName,
                    JournalRemarks = model.CertificateNumber,
                    TransferReference = model.CertificateNumber,
                    TransferAccount = model.TransferAccount.Trim(),
                    TransferSum = PayAmount,
                };


                string JSON = JsonConvert.SerializeObject(ORCT);
                //Global.WriteToLogFile_Debug_JSON("Incoming Payment JSON: " + JSON.ToString(), "");
                CV1 = str_SessionID;
                strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;

            }
            catch (WebException ex)
            {
                str_Response = ex.Message.ToString();
            }
            return str_Response;
        }

        public ResponseModel ARInvoicePosting(InvoicePosting model)
        {
            Global.ConfigFileget();
            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;

            sFuncName = "Invoice Posting Using Service Layer";

            try
            {

                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);
                TransOutput = ARInvoicePosting(SAPToken, model);

                JObject json = JObject.Parse(TransOutput);

                var errorValue = json.SelectToken("error.message.value");


                if (errorValue == null)
                {
                    var DocEntry = json.SelectToken("DocEntry");
                    var DocNum = json.SelectToken("DocNum");
                    _responseModel.StatusCode = (int)ResponseStatus.Success;
                    _responseModel.StatusMessage = ResponseStatus.Success.ToString();
                    _responseModel.ResponseData = "DocNum: " + DocNum.ToString();
                    return this._responseModel;

                }
                else
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Failed;
                    _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                    _responseModel.ResponseData = "Error: " + errorValue;
                    return this._responseModel;
                }


            }
            catch (Exception ex)
            {

                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }

            return this._responseModel;

        }

        public string ARInvoicePosting(string str_SessionID, InvoicePosting model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            try
            {
                strFun = "Invoices";
                URL = Global.APIBaseURL + strFun;

                Invoices OINV = new Invoices
                {
                    DocDate = DateTime.Now,
                    CardCode = model.CardCode,
                    DocDueDate = model.DocDueDate,
                    BPL_IDAssignedToInvoice = int.TryParse(model.BPL_IDAssignedToInvoice, out var bplId) ? bplId : 0,
                    NumAtCard = model.NumAtCard,
                    Remarks = model.Comments,

                };

                List<DeliveryNotes> PAYINV = new List<DeliveryNotes>();

                foreach (var InvoiceDetail in model.InvoiceLines)
                {
                    DeliveryNotes payinv = new DeliveryNotes();
                    {
                        //DataTable dt = Global.GetDataTable_HANA("Select \"DocEntry\" from OINV where \"DocNum\" = '" + paymentDetail.DocNum + "' ");
                        //string docEntry = GetSingleValue("Select \"DocEntry\" from OINV where \"DocNum\" = '" + paymentDetail.DocNum + "' ");
                        payinv.BaseEntry = InvoiceDetail.BaseEntry.ToString();
                        payinv.BaseType = InvoiceDetail.BaseType;
                        payinv.BaseLine = InvoiceDetail.BaseLine;
                        payinv.ItemCode = InvoiceDetail.ItemCode;
                        payinv.Quantity = InvoiceDetail.Quantity.ToString();
                        //payinv.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice.ToString();
                    }
                    ;
                    PAYINV.Add(payinv);
                }
                OINV.DocumentLines = PAYINV;

                string JSON = JsonConvert.SerializeObject(OINV);
                //Global.WriteToLogFile_Debug_JSON("Invoice JSON: " + JSON.ToString(), "");
                CV1 = str_SessionID;
                strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;

            }
            catch (WebException ex)
            {
                str_Response = ex.Message.ToString();
            }
            return str_Response;
        }

        public ResponseModel ReturnPosting(ReturnPosting model)
        {
            Global.ConfigFileget();
            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;

            sFuncName = "Return Posting Using Service Layer";

            try
            {

                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);
                TransOutput = ReturnPosting(SAPToken, model);

                JObject json = JObject.Parse(TransOutput);

                var errorValue = json.SelectToken("error.message.value");


                if (errorValue == null)
                {
                    var DocEntry = json.SelectToken("DocEntry");
                    var DocNum = json.SelectToken("DocNum");
                    _responseModel.StatusCode = (int)ResponseStatus.Success;
                    _responseModel.StatusMessage = ResponseStatus.Success.ToString();
                    _responseModel.ResponseData = "DocNum: " + DocNum.ToString();
                    return this._responseModel;

                }
                else
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Failed;
                    _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                    _responseModel.ResponseData = "Error: " + errorValue;
                    return this._responseModel;
                }


            }
            catch (Exception ex)
            {

                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }

            return this._responseModel;

        }

        public string ReturnPosting(string str_SessionID, ReturnPosting model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            try
            {
                strFun = "Returns";
                URL = Global.APIBaseURL + strFun;

                Returns ORDN = new Returns
                {
                    DocDate = DateTime.Now,
                    CardCode = model.CardCode,
                    DocDueDate = model.DocDueDate,
                    BPL_IDAssignedToInvoice = int.TryParse(model.BPL_IDAssignedToInvoice, out var bplId) ? bplId : 0,
                    NumAtCard = model.NumAtCard,
                    Remarks = model.Comments,

                };

                List<DeliveryNotes> PAYRDN = new List<DeliveryNotes>();

                foreach (var ReturnDetail in model.ReturnLines)
                {
                    DeliveryNotes payrdn = new DeliveryNotes();
                    {
                        //DataTable dt = Global.GetDataTable_HANA("Select \"DocEntry\" from OINV where \"DocNum\" = '" + paymentDetail.DocNum + "' ");
                        //string docEntry = GetSingleValue("Select \"DocEntry\" from OINV where \"DocNum\" = '" + paymentDetail.DocNum + "' ");
                        payrdn.BaseEntry = ReturnDetail.BaseEntry.ToString();
                        payrdn.BaseType = ReturnDetail.BaseType;
                        payrdn.BaseLine = ReturnDetail.BaseLine;
                        //payrdn.ItemCode = ReturnDetail.ItemCode;
                        payrdn.Quantity = ReturnDetail.Quantity.ToString();
                        //payinv.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice.ToString();
                    }
                    ;
                    PAYRDN.Add(payrdn);
                }
                ORDN.DocumentLines = PAYRDN;

                string JSON = JsonConvert.SerializeObject(ORDN,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                //Global.WriteToLogFile_Debug_JSON("Invoice JSON: " + JSON.ToString(), "");
                CV1 = str_SessionID;
                strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;

            }
            catch (WebException ex)
            {
                str_Response = ex.Message.ToString();
            }
            return str_Response;
        }


        public ResponseModel CreditNote(CreditNotePosting model)
        {
            Global.ConfigFileget();
            string sFuncName = String.Empty;
            string SAPToken = String.Empty;
            string strRouteVal = string.Empty;
            string TransOutput = string.Empty;

            sFuncName = "Credit Note Posting Using Service Layer";

            try
            {

                Global.ConfigFileget();
                SAPToken = Login(out strRouteVal);
                TransOutput = CreditNotePosting(SAPToken, model);

                JObject json = JObject.Parse(TransOutput);

                var errorValue = json.SelectToken("error.message.value");


                if (errorValue == null)
                {
                    var DocEntry = json.SelectToken("DocEntry");
                    var DocNum = json.SelectToken("DocNum");
                    _responseModel.StatusCode = (int)ResponseStatus.Success;
                    _responseModel.StatusMessage = ResponseStatus.Success.ToString();
                    _responseModel.ResponseData = "DocNum: " + DocNum.ToString();
                    return this._responseModel;

                }
                else
                {
                    _responseModel.StatusCode = (int)ResponseStatus.Failed;
                    _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                    _responseModel.ResponseData = "Error: " + errorValue;
                    return this._responseModel;
                }


            }
            catch (Exception ex)
            {

                _responseModel.StatusCode = (int)ResponseStatus.Failed;
                _responseModel.StatusMessage = ResponseStatus.Failed.ToString();
                _responseModel.ResponseData = "Error: " + ex.Message.ToString();
                return this._responseModel;
            }

            return this._responseModel;

        }

        public string CreditNotePosting(string str_SessionID, CreditNotePosting model)
        {
            string URL = string.Empty;
            string MasterData = string.Empty; string TransactionType = string.Empty; string strRoutevalue = string.Empty;
            string strCompDB = string.Empty;
            string str_Response = string.Empty;

            try
            {
                strFun = "CreditNotes";
                URL = Global.APIBaseURL + strFun;

                string docCurrency = GetSingleValue($"SELECT \"DocCur\" FROM OINV WHERE \"DocEntry\" = '{model.CreditNoteDetails.FirstOrDefault()?.InvoiceDocEntry}'");

                CreditNote ORIN = new CreditNote
                {
                    DocType = BoDocumentTypes.dDocument_Items.ToString(),
                    DocDate = model.DocDate,
                    DocDueDate = model.DocDueDate,
                    CardCode = model.CardCode,
                    DocCurrency = docCurrency,
                    CardName = GetSingleValue("Select \"CardName\" from OCRD where \"CardCode\" = '" + model.CardCode + "'"),
                    NumAtCard = model.NumAtCard,
                    Comments = model.Comments,
                };

                List<CreditNote_Details> RIN1 = new List<CreditNote_Details>();


                foreach (var item in model.CreditNoteDetails)
                {
                    CreditNote_Details rin1 = new CreditNote_Details
                    {
                        ItemCode = item.ItemCode,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        WarehouseCode = item.WarehouseCode,
                        TaxCode = item.TaxCode,
                        BaseType = 13,
                        BaseEntry = item.InvoiceDocEntry,
                        BaseLine = item.InvoiceLineNum
                    };

                    string ManBtchNum = GetSingleValue("SELECT T0.\"ManBtchNum\"  FROM OITM T0 WHERE T0.\"ItemCode\" ='" + item.ItemCode + "'");


                    List<BatchDetails> Batch = new List<BatchDetails>();
                    BatchDetails batch = new BatchDetails();

                    if (ManBtchNum == "Y")
                    {
                        string Batch_Qry = "SELECT T0.\"ItemCode\", T0.\"WhsCode\", T0.\"Quantity\", T0.\"SysNumber\",T1.\"DistNumber\",T1.\"InDate\" " +
                           " FROM OBTQ T0  INNER JOIN OBTN T1 ON T0.\"SysNumber\" = T1.\"SysNumber\" and  T1.\"ItemCode\"=T0.\"ItemCode\" WHERE  " +
                           " T0.\"ItemCode\" ='" + item.ItemCode + "' and T0.\"WhsCode\"='" + item.WarehouseCode + "' and T0.\"Quantity\" <>0 order by T1.\"InDate\" ASC ";

                        DataTable dt = GetDataTable_HANA(Batch_Qry);

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                string BatchNum = row["DistNumber"].ToString().Trim();
                                double BatchQty = Convert.ToDouble(row["Quantity"]);

                                string Concat = "_";
                                BatchNum = row["ItemCode"].ToString().Trim() + Concat + BatchNum;

                                if (oTranBatchDetail.ContainsKey(BatchNum))
                                {
                                    BatchQty -= oTranBatchDetail[BatchNum];
                                    if (BatchQty <= 0)
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    oTranBatchDetail.Add(BatchNum, 0);
                                }

                                BatchQty = Math.Round(BatchQty, 4);

                                batch.BatchNumber = row["DistNumber"].ToString().Trim();

                                if (BatchQty < item.Quantity)
                                {
                                    batch.Quantity = BatchQty;
                                    oTranBatchDetail[BatchNum] += BatchQty;
                                    item.Quantity -= BatchQty;
                                }
                                else
                                {
                                    batch.Quantity = item.Quantity;
                                    oTranBatchDetail[BatchNum] += item.Quantity;
                                    break;
                                }
                            }

                            Batch.Add(batch);

                            rin1.BatchNumbers = Batch;
                        }
                    }

                    RIN1.Add(rin1);

                }

                ORIN.DocumentLines = RIN1;

                string JSON = JsonConvert.SerializeObject(ORIN);

                CV1 = str_SessionID;
                strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSON, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                //request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                //request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;

            }
            catch (WebException ex)
            {
                str_Response = ex.Message.ToString();
            }
            return str_Response;
        }



        #endregion
        private string GetSingleValue(string query)
        {
            DataTable OC = this.sqlDataAccessLayer.GetDataTable_HANA_GetSingleValue(query);
            String Result = "";
            if (OC.Rows.Count > 0)
            {
                foreach (DataRow HR in OC.Rows)
                {
                    Result = HR[0].ToString();
                    return Result;
                }
            }
            return "";
        }

        public String getSingleValue(String strSQL)
        {
            try
            {
                SAPbobsCOM.Recordset rset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string strReturnVal = "";
                rset.DoQuery(strSQL);
                if (rset.RecordCount > 0)
                {
                    strReturnVal = rset.Fields.Item(0).Value.ToString();
                }
                else
                {
                    strReturnVal = "";
                }
                return strReturnVal;

            }
            catch
            {
                return null;
            }

        }


        private DataTable GetDataTable_HANA(string Query)
        {

            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();

            string connectionString = ConfigurationManager.ConnectionStrings["POSConnection"].ToString();

            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + connectionString + "";
                //this.connectionString
                oCon.Open();
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = Query;
                oCmd.Connection = (System.Data.Odbc.OdbcConnection)oCon;
                oCmd.CommandTimeout = 0;
                System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(oCmd);
                da.Fill(oDs);
                oCon.Close();
                oCon.Dispose();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return oDs.Tables[0];
        }

        public static bool NonExecuteHANA(string NoExQuery)
        {
            string connectionString1;
            connectionString1 = ConfigurationManager.ConnectionStrings["POSConnection"].ToString();
            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();
            int output = 0;
            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + connectionString1 + "";
                //this.connectionString
                oCon.Open();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = NoExQuery;
                oCmd.Connection = (System.Data.Odbc.OdbcConnection)oCon;
                oCmd.CommandTimeout = 0;
                oCmd.ExecuteNonQuery();
                oCon.Close();
                oCon.Dispose();

                return true;
            }

            catch (Exception ex)
            {
                return false;
                //LogTracker.ErrorLog.WriteLog("The Error is : " + ex.Message.ToString());
            }
            finally
            {
                //this.closeConnection();
            }
        }

        public static string GetSingleValue_HANA(string Query)
        {
            string connectionString1;
            connectionString1 = ConfigurationManager.ConnectionStrings["POSConnection"].ToString();
            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();
            string ret;
            ret = ""; ;
            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + connectionString1 + "";
                //this.connectionString
                oCon.Open();
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = Query;
                oCmd.Connection = (System.Data.Odbc.OdbcConnection)oCon;
                oCmd.CommandTimeout = 0;
                System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(oCmd);
                da.Fill(oDs);
                oCon.Close();
                oCon.Dispose();
                if (oDs.Tables[0].Rows.Count > 0)
                {
                    return ret = Convert.ToString(oDs.Tables[0].Rows[0][0]);
                }
                else
                {
                    return ret = "";
                }

            }
            catch (Exception ex)
            {
                return ret = "";
                //this.writeLog(ex);
            }
            finally
            {
                //this.closeConnection();
            }
            return ret;
        }


    }
}