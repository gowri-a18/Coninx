using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using Newtonsoft.Json;
using System.Collections;
using DemoProject.Models.DomainClasses;
using System.Collections.Generic;
using System.Xml;
using System.Data.Common;
using System.Globalization;

namespace DemoProject.Models.SqlConnections
{
    public class SqlConnectionData
    {
        #region local-fields

        private SqlConnection hanaConnection;
        private SqlCommand hanaCommand;
        private SqlDataAdapter hanaDataAdapter;

        private string connectionString;
        private string connectionString1;

        private string JFLconnsectionString;
        private DataTable dtTable;
        private DataTable dtTable2;
        private DataTable dtTable3;
        private int affectedRows;
        private string cellString;
        private ResponseModel responseModel;


        #endregion

        private bool IsSelectQuery { get; set; }

        public string Query { get; set; }
       // public string Query2 { get; set; }
        public bool IsStoredProcedure { get; set; }

        public object[] ObjParams { get; set; }

        public object[] ObjValues { get; set; }

        internal SqlConnectionData()
        {
            this.Initialization();
        }

        #region local-methods

        void Initialization()
        {
            this.affectedRows = 0;
            this.Query = string.Empty;
            this.IsSelectQuery = false;

            this.dtTable = new DataTable();
            this.cellString = string.Empty;
            this.connectionString = ConfigurationManager.ConnectionStrings["POSConnection"].ToString();
            //this.connectionString1 = ConfigurationManager.ConnectionStrings["HanaConnectionString_Jumra"].ToString();
            this.IsStoredProcedure = false;
            this.ObjParams = this.ObjValues = null;
            this.responseModel = new ResponseModel();
        }

        /* INSERT UPDATE & DELETE */
        private int ExecuteNonQuery()
        {
            try
            {
                this.setConnection();
                this.affectedRows = this.hanaCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.writeLog(ex);
            }
            finally
            {
                this.closeConnection();
            }
            return this.affectedRows;
        }


        /* GET CELL VALUE */
        private string ExecuteScalar()
        {
            try
            {
                this.setConnection();
                this.cellString = this.hanaCommand.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                this.writeLog(ex);
            }
            finally
            {
                this.closeConnection();
            }
            return this.cellString;
        }

        private DataTable GetDataTable_HANA_2(string Query2)
        {

            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();


            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + this.connectionString + "";
                //this.connectionString
                oCon.Open();
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = Query2;
                oCmd.Connection = (System.Data.Odbc.OdbcConnection)oCon;
                oCmd.CommandTimeout = 0;
                System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(oCmd);
                da.Fill(oDs);
                oCon.Close();
                oCon.Dispose();
            }
            catch (Exception ex)
            {
                this.writeLog(ex);
            }
            finally
            {
                //this.closeConnection();
            }
            return oDs.Tables[0];
        }
        private DataTable GetDataTable_HANA_3(string Query3)
        {

            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();


            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + this.connectionString + "";
                //this.connectionString
                oCon.Open();
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = Query3;
                oCmd.Connection = (System.Data.Odbc.OdbcConnection)oCon;
                oCmd.CommandTimeout = 0;
                System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(oCmd);
                da.Fill(oDs);
                oCon.Close();
                oCon.Dispose();
            }
            catch (Exception ex)
            {
                this.writeLog(ex);
            }
            finally
            {
                //this.closeConnection();
            }
            return oDs.Tables[0];
        }

        private DataTable GetDataTable_HANA_4(string Query4)
        {

            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();


            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + this.connectionString1 + "";
                //this.connectionString
                oCon.Open();
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = Query4;
                oCmd.Connection = (System.Data.Odbc.OdbcConnection)oCon;
                oCmd.CommandTimeout = 0;
                System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(oCmd);
                da.Fill(oDs);
                oCon.Close();
                oCon.Dispose();
            }
            catch (Exception ex)
            {
                this.writeLog(ex);
            }
            finally
            {
                //this.closeConnection();
            }
            return oDs.Tables[0];
        }
        /* GET TABLE */
        private DataTable GetDataTable_HANA(string Query)
        {

            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();


            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + this.connectionString + "";
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
                this.writeLog(ex);
            }
            finally
            {
                //this.closeConnection();
            }
            return oDs.Tables[0];
        }
        private DataTable GetDataTable_HANA(string query,string connectionName)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionName];

            if (settings == null)
            {
                throw new Exception(
                    "Connection string not found: " + connectionName);
            }

            string connectionString =
                "DRIVER={HDBODBC};" + settings.ConnectionString;

            DataTable dataTable = new DataTable();

            using (OdbcConnection connection =
                new OdbcConnection(connectionString))
            using (OdbcCommand command =
                new OdbcCommand(query, connection))
            using (OdbcDataAdapter adapter =
                new OdbcDataAdapter(command))
            {
                connection.Open();
                adapter.Fill(dataTable);
            }

            return dataTable;
        }

        private DataTable GetDataTable_HANA()
        {

            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();


            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + this.connectionString + "";
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
                this.writeLog(ex);
            }
            finally
            {
                //this.closeConnection();
            }
            return oDs.Tables[0];
        }
        /* GET TABLE */


        public DataTable GetDataTable_HANA_GetSingleValue(string sQry)
        {

            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();


            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + this.connectionString + "";
                //this.connectionString
                oCon.Open();
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = sQry;
                oCmd.Connection = (System.Data.Odbc.OdbcConnection)oCon;
                oCmd.CommandTimeout = 0;
                System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(oCmd);
                da.Fill(oDs);
                oCon.Close();
                oCon.Dispose();
            }
            catch (Exception ex)
            {
                this.writeLog(ex);
            }
            finally
            {
                //this.closeConnection();
            }
            return oDs.Tables[0];
        }

        internal ResponseModel responseDataMultiDatabase(string Query,string Query2,Dictionary<string, string> databases)
        {
            ResponseModel response = new ResponseModel();

            List<DatabaseInvoiceGroup> databaseGroups =
                new List<DatabaseInvoiceGroup>();

            try
            {
                foreach (KeyValuePair<string, string> database in databases)
                {
                    string connectionName = database.Key;
                    string databaseName = database.Value;

                    // Get invoice records from the current database.
                    DataTable dtTable = GetDataTable_HANA(Query, connectionName);

                    // Get total page count from the current database.
                    DataTable dtCount = GetDataTable_HANA(Query2, connectionName);

                    int totalIndex = 0;

                    if (dtCount.Rows.Count > 0 && dtCount.Rows[0]["CNT"] != DBNull.Value)
                    {
                        totalIndex = Convert.ToInt32( dtCount.Rows[0]["CNT"]);
                    }

                    List<MarketingDocumentHeader> records = new List<MarketingDocumentHeader>();

                    foreach (DataRow row in dtTable.Rows)
                    {
                        MarketingDocumentHeader header = new MarketingDocumentHeader();

                       // header.RowNum = row["RowNum"].ToString();
                        header.DocEntry = row["DocEntry"].ToString();
                        header.DocNum = row["DocNum"].ToString();
                        header.CardCode = row["CardCode"].ToString();
                        header.CardName = row["CardName"].ToString();
                        header.DocCur = row["DocCur"].ToString();
                        header.LicTradNum = row["LicTradNum"].ToString();
                        header.CuInvoiceNo = row["U_CuInvoiceNo"].ToString();
                        header.DocTotal = Convert.ToDouble(row["DocTotal"]);
                        header.BalanceDue = Convert.ToDouble(row["BalanceDue"]); 
                        header.TotalBeforeDiscount = Convert.ToDouble(row["TotalBeforeDiscount"]);
                        header.TotalIndex = totalIndex;

                        records.Add(header);
                    }

                    DatabaseInvoiceGroup group =
                        new DatabaseInvoiceGroup();

                    group.DatabaseName = databaseName;
                    group.Records = records;
                    group.TotalIndex = totalIndex;

                    databaseGroups.Add(group);
                }

                response.ResponseData = databaseGroups;
                response.StatusCode = (int)StatusCodes.Success;
                response.StatusMessage = StatusCodes.Success.ToString();
            }
            catch (Exception ex)
            {
               
            }

            return response;
        }
        private DataTable GetDataTable()
        {
            try
            {
                this.IsSelectQuery = true;
                this.dtTable = new DataTable();
                this.setConnection();
                this.hanaDataAdapter.Fill(this.dtTable);
            }
            catch (Exception ex)
            {
                this.writeLog(ex);
            }
            finally
            {
                this.closeConnection();
            }
            return this.dtTable;
        }
        /* WRITE LOG */
        void writeLog(Exception ex)
        {
            LogTracker.ErrorLog.WriteLog(string.Format("Query    :   {0} {1} {2}", this.Query, Environment.NewLine, ex.ToString()));
        }

        /* OPEN CONNECTION */
        public void setConnection()
        {
            this.hanaConnection = new SqlConnection(this.connectionString);
            if (this.hanaConnection.State == ConnectionState.Closed)
            {
                this.hanaConnection.Open();
            }
            this.hanaCommand = new SqlCommand(this.Query, this.hanaConnection);

            if (this.IsStoredProcedure)
            {
                //  this.hanaCommand.CommandText = this.Query;
                this.IsSelectQuery = false;
                this.hanaCommand.CommandType = CommandType.StoredProcedure;
                this.hanaDataAdapter = new SqlDataAdapter(this.Query, this.hanaConnection);
            }
            if (this.IsSelectQuery) { this.hanaDataAdapter = new SqlDataAdapter(this.hanaCommand); }
        }

        /* CLOSE CONNECTION */
        void closeConnection()
        {
            if (this.hanaConnection.State == ConnectionState.Open)
            {
                this.hanaConnection.Close();
            }
            this.dispose(true);
        }

        void dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.hanaConnection != null)
                {
                    this.hanaConnection.Dispose();
                    this.hanaConnection = null;
                }

                if (this.hanaCommand != null)
                {
                    this.hanaCommand.Dispose();
                    this.hanaCommand = null;
                }

                if (this.hanaDataAdapter != null)
                {
                    this.hanaDataAdapter.Dispose();
                    this.hanaDataAdapter = null;
                }
            }
        }

        #endregion


        private Byte[] GetImageStream()
        {
            string path;
            System.Drawing.Image dmaImg = null;

            path = "C:\\p2.jpg";

            dmaImg = System.Drawing.Image.FromFile(path); // path is the location of the image

            var stream = new System.IO.MemoryStream();
            dmaImg.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            dmaImg.Dispose();
            return stream.ToArray();
        }


        internal ResponseModel responseData(string Type, string Query, string Query2)
        {
            if(Type =="None")
            {
                this.responseModel.ResponseData = "";
                this.responseModel.StatusCode = (int)StatusCodes.Success;
                this.responseModel.StatusMessage = StatusCodes.Success.ToString();
            }

            else if (Type == "getINVList")
            {
                this.dtTable = this.GetDataTable_HANA(Query);
                this.dtTable3 = this.GetDataTable_HANA_3(Query2);
                List<MarketingDocumentHeader> Main = new List<MarketingDocumentHeader>();
                if (this.dtTable.Rows.Count > 0)
                {
                    foreach (DataRow DR in dtTable.Rows)
                    {
                        MarketingDocumentHeader header = new MarketingDocumentHeader();

                        // header.RowNum = DR["RowNum"].ToString();
                        header.DocDate = DR["DocDate"].ToString();
                        header.DocEntry = DR["DocEntry"].ToString();
                        header.DocNum = DR["DocNum"].ToString();
                        header.CardCode = DR["CardCode"].ToString();
                        header.CardName = DR["CardName"].ToString();
                        header.DocCur = DR["DocCur"].ToString();
                        header.LicTradNum = DR["LicTradNum"].ToString();
                        header.CuInvoiceNo = DR["U_CUInvNo"].ToString();
                        header.KRACUNumber = DR["U_U_KRA_CU_No"].ToString();
                        header.DocTotal = Convert.ToDouble(DR["DocTotal"]);
                        header.BalanceDue = Convert.ToDouble(DR["BalanceDue"]);
                        header.TotalBeforeDiscount = Convert.ToDouble(DR["TotalBeforeDiscount"]);
                        header.BranchId = DR["U_BrchId"].ToString();
                        Main.Add(header);
                        
                        //List<Products> Item = new List<Products>();
                        //string Querys = "SELECT T0.\"LineNum\", T0.\"ItemCode\", T0.\"Dscription\", T0.\"UomCode\", T0.\"VatPrcnt\", T0.\"Quantity\", T0.\"Price\", T0.\"LineTotal\" FROM INV1 T0 WHERE T0.\"DocEntry\" ='" + DR["DocEntry"].ToString() + "' and T0.\"LineStatus\" = 'O' order by T0.\"LineNum\"";
                        //this.dtTable2 = this.GetDataTable_HANA_2(Querys);
                        //foreach (DataRow DR2 in dtTable2.Rows)
                        //{
                        //    Products details = new Products();
                        //    details.LineNum = Convert.ToInt32(DR2["LineNum"].ToString());
                        //    details.ItemCode = DR2["ItemCode"].ToString();
                        //    details.ItemName = DR2["Dscription"].ToString();
                        //    details.UomCode = DR2["UomCode"].ToString();
                        //    details.VatPrcnt = DR2["VatPrcnt"].ToString();
                        //    details.Quantity = Convert.ToDouble(DR2["Quantity"].ToString());
                        //    details.Price = Convert.ToDouble(DR2["Price"].ToString());
                        //    details.LineTotal = Convert.ToDouble(DR2["LineTotal"].ToString());
                        //    Item.Add(details);
                        //}

                        //foreach (DataRow DR3 in dtTable3.Rows)
                        //{
                        //    header.TotalIndex = Convert.ToInt32(DR3["CNT"]);
                        //}

                       // header.ItemList = Item;
                    }

                    this.responseModel.ResponseData = Main;
                    this.responseModel.StatusCode = (int)StatusCodes.Success;
                    this.responseModel.StatusMessage = StatusCodes.Success.ToString();
                }
                else
                {
                    this.responseModel.ResponseData = Main;
                    this.responseModel.StatusCode = (int)StatusCodes.NotFound;
                    this.responseModel.StatusMessage = StatusCodes.NotFound.ToString();
                }

            }

            else if (Type == "getBnkList")
            {
                this.dtTable = this.GetDataTable_HANA(Query);
                this.dtTable3 = this.GetDataTable_HANA_3(Query2);
                List<BankDetails> Main = new List<BankDetails>();
                if (this.dtTable.Rows.Count > 0)
                {
                    foreach (DataRow DR in dtTable.Rows)
                    {
                        BankDetails header = new BankDetails();

                        header.BankCode = DR["BankCode"].ToString();
                        header.BankName = DR["BankName"].ToString();
                        Main.Add(header);
                    }

                    this.responseModel.ResponseData = Main;
                    this.responseModel.StatusCode = (int)StatusCodes.Success;
                    this.responseModel.StatusMessage = StatusCodes.Success.ToString();
                }
                else
                {
                    this.responseModel.ResponseData = Main;
                    this.responseModel.StatusCode = (int)StatusCodes.NotFound;
                    this.responseModel.StatusMessage = StatusCodes.NotFound.ToString();
                }

            }
            else if (Type == "getCustomerList")
            {
                this.dtTable = this.GetDataTable_HANA(Query);
                this.dtTable3 = this.GetDataTable_HANA_3(Query2);
                List<CustomerDetails> Main = new List<CustomerDetails>();
                if (this.dtTable.Rows.Count > 0)
                {
                    foreach (DataRow DR in dtTable.Rows)
                    {
                        CustomerDetails header = new CustomerDetails();

                        header.CardCode = DR["CardCode"].ToString();
                        header.CardName = DR["CardName"].ToString(); 
                        header.CustomerPIN = DR["LicTradNum"].ToString();
                        Main.Add(header);
                    }

                    this.responseModel.ResponseData = Main;
                    this.responseModel.StatusCode = (int)StatusCodes.Success;
                    this.responseModel.StatusMessage = StatusCodes.Success.ToString();
                }
                else
                {
                    this.responseModel.ResponseData = Main;
                    this.responseModel.StatusCode = (int)StatusCodes.NotFound;
                    this.responseModel.StatusMessage = StatusCodes.NotFound.ToString();
                }

            }
            else if (Type == "getCurrencyList")
            {
                this.dtTable = this.GetDataTable_HANA(Query);
                this.dtTable3 = this.GetDataTable_HANA_3(Query2);
                List<CurrencyDetails> Main = new List<CurrencyDetails>();
                if (this.dtTable.Rows.Count > 0)
                {
                    foreach (DataRow DR in dtTable.Rows)
                    {
                        CurrencyDetails header = new CurrencyDetails();

                        header.CurrencyCode = DR["CurrCode"].ToString();
                        header.CurrencyName = DR["CurrName"].ToString();
                        Main.Add(header);
                    }

                    this.responseModel.ResponseData = Main;
                    this.responseModel.StatusCode = (int)StatusCodes.Success;
                    this.responseModel.StatusMessage = StatusCodes.Success.ToString();
                }
                else
                {
                    this.responseModel.ResponseData = Main;
                    this.responseModel.StatusCode = (int)StatusCodes.NotFound;
                    this.responseModel.StatusMessage = StatusCodes.NotFound.ToString();
                }

            }

            else if (Type == "getPayList")
            {
                this.dtTable = this.GetDataTable_HANA_4(Query);
                List<PaymentStatusResult> PSR = new List<PaymentStatusResult>();
                if (this.dtTable.Rows.Count > 0)
                {
                    string id = string.Empty;
                    string DocNum = string.Empty;
                    string ReceiveDate = string.Empty;
                    string ErrMsg = string.Empty;
                    string Status = string.Empty;

                    foreach (DataRow DR in dtTable.Rows)
                    {

                        PaymentStatusResult header = new PaymentStatusResult();

                        id = DR["_id"].ToString();
                        DocNum = DR["DocNum"].ToString();
                        ReceiveDate = DR["ReceiveDate"].ToString();
                        ErrMsg = DR["ErrMsg"].ToString();
                        Status = DR["Status"].ToString();

                        if (string.IsNullOrEmpty(ErrMsg) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(ReceiveDate))
                        {
                            header.id = id;
                            header.DocNum = DocNum;
                            header.Status = "Processing";
                        }
                        else if (string.IsNullOrEmpty(ErrMsg) && !string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(ReceiveDate))
                        {
                            header.id = id;
                            header.DocNum = DocNum;
                            header.Status = "Success";
                        }
                        else if (!string.IsNullOrEmpty(ErrMsg) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(ReceiveDate))
                        {
                            header.id = id;
                            header.DocNum = DocNum;
                            header.Status = "Failed";
                            header.Message = ErrMsg;
                        }
                        else
                        {
                            header.id = id;
                            header.DocNum = DocNum;
                            header.Status = "Pending";
                            header.Message = ErrMsg;
                        }

                        PSR.Add(header);

                    }

                    this.responseModel.ResponseData = PSR;
                    this.responseModel.StatusCode = (int)StatusCodes.Success;
                    this.responseModel.StatusMessage = StatusCodes.Success.ToString();
                }
                else
                {
                    this.responseModel.ResponseData = "Data Not Found from Base Table";
                    this.responseModel.StatusCode = (int)StatusCodes.NotFound;
                    this.responseModel.StatusMessage = StatusCodes.NotFound.ToString();
                }

            }

            else if (Type == "Login")
            {
                this.dtTable = this.GetDataTable_HANA();
                if (this.dtTable.Rows.Count > 0)
                {
                    foreach (DataRow DR in dtTable.Rows)
                    {
                        string active = DR["Active"].ToString();// oRecords.Fields.Item("Active").Value;
                        if ("Y" == active)
                        {
                            string firsname = DR["firstName"].ToString();  //oRecords.Fields.Item("firstName").Value;

                            this.responseModel.ResponseData = "This User " + firsname + " Logged In Successfully..";
                            this.responseModel.StatusCode = (int)StatusCodes.Success;
                            this.responseModel.StatusMessage = StatusCodes.Success.ToString();
                            return this.responseModel;
                        }
                        else if ("N" == active)
                        {
                            this.responseModel.ResponseData = "Inactive User";
                            this.responseModel.StatusCode = (int)StatusCodes.Failed;
                            this.responseModel.StatusMessage = StatusCodes.Failed.ToString();
                            return this.responseModel;

                        }
                    }
                }
                else
                {
                    this.responseModel.ResponseData = "Invalid User and Password";
                    this.responseModel.StatusCode = (int)StatusCodes.Failed;
                    this.responseModel.StatusMessage = StatusCodes.Failed.ToString();
                    return this.responseModel;
                }
            }

            else
            {
                
                    this.responseModel.ResponseData = "NotFound";
                    this.responseModel.StatusCode = (int)StatusCodes.NotFound;
                    this.responseModel.StatusMessage = StatusCodes.NotFound.ToString();
               

            }
            return this.responseModel;

        }

        

        internal ResponseModel executeScalar()
        {
            this.cellString = this.ExecuteScalar();
            if (!string.IsNullOrEmpty(this.cellString))
            {
                this.responseModel.ResponseData = this.cellString;
                this.responseModel.StatusCode = (int)StatusCodes.Success;
                this.responseModel.StatusMessage = StatusCodes.Success.ToString();
            }
            else
            {
                this.responseModel.ResponseData = string.Empty;
                this.responseModel.StatusCode = (int)StatusCodes.NoData;
                this.responseModel.StatusMessage = StatusCodes.NoData.ToString();
            }
            return this.responseModel;
        }

        internal ResponseModel executeNonQuery()
        {
            this.affectedRows = this.ExecuteNonQuery();
            if (this.affectedRows > 0)
            {
                this.responseModel.StatusCode = (int)StatusCodes.Success;
                this.responseModel.StatusMessage = StatusCodes.Success.ToString();
            }
            else
            {
                this.responseModel.StatusCode = (int)StatusCodes.Failed;
                this.responseModel.StatusMessage = StatusCodes.Failed.ToString();
            }

            return this.responseModel;
        }
    }

}
