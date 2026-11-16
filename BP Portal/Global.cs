using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;

namespace POS
{
    public class Global
    {

        public static SqlConnection SqlCon;
        public static string Function;
        public static string APIBaseURL;
        public static string APIBaseURLCN;
        public static string LogPath;
        public static string ApiAuth;
        public static string APIUserName;
        public static string CompanyDB;
        public static string APIPassWord;
        public static string ContentType;
        public static string APIErrorMsg;

        public static string IDB_SQLSERVER;
        public static string IDB_USERID;
        public static string IDB_PASSWORD;
        public static string IDB_DATABASE;

        public static string Token;


        public static int p_iDeleteDebugLog = 1;

        public static int p_iDebugMode = 1;
        public static int DEBUG_ON = 1;

        public static int RTN_SUCCESS = 1;
        public static int RTN_ERROR = 0;

        private const Int16 MAXFILESIZE_IN_MB = 5; // (2 MB)
        private const string LOG_FILE_ERROR = "ErrorLog";
        private const string LOG_FILE_ERROR_ARCH = "ErrorLog_";
        private const string LOG_FILE_DEBUG = "DebugLog";
        private const string LOG_FILE_DEBUG_ARCH = "DebugLog_";
        private const Int16 FILE_SIZE_CHECK_ENABLE = 1;
        private const Int16 FILE_SIZE_CHECK_DISABLE = 0;

        public static void ConfigFileget()
        {
            string strConnect = "";
            string sCon = "";
            string[] MyArr = null;

            strConnect = "LogPath";
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            MyArr = sCon.Split(';');
            LogPath = MyArr[0].ToString();

            strConnect = "APIBaseURL";
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            MyArr = sCon.Split(';');
            APIBaseURL = MyArr[0].ToString();

            //strConnect = "APIBaseURLCN";
            //sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            //MyArr = sCon.Split(';');
            //APIBaseURLCN = MyArr[0].ToString();

            //strConnect = "ApiAuth";
            //sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            //MyArr = sCon.Split(';');
            //ApiAuth = MyArr[0].ToString();

            strConnect = "APIUserName";
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            MyArr = sCon.Split(';');
            APIUserName = MyArr[0].ToString();

            strConnect = "APIPassWord";
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            MyArr = sCon.Split(';');
            APIPassWord = MyArr[0].ToString();

            strConnect = "CompanyDB";
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            MyArr = sCon.Split(';');
            CompanyDB = MyArr[0].ToString();

            strConnect = "ContentType";
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            MyArr = sCon.Split(';');
            ContentType = MyArr[0].ToString();


            string connstr = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ToString();
            SqlCon = new SqlConnection(connstr);


            //strConnect = "IDB_SQLSERVER";
            //sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            //MyArr = sCon.Split(';');
            //IDB_SQLSERVER = MyArr[0].ToString();

            //strConnect = "IDB_USERID";
            //sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            //MyArr = sCon.Split(';');
            //IDB_USERID = MyArr[0].ToString();

            //strConnect = "IDB_PASSWORD";
            //sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            //MyArr = sCon.Split(';');
            //IDB_PASSWORD = MyArr[0].ToString();

            //strConnect = "IDB_DATABASE";
            //sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect);
            //MyArr = sCon.Split(';');
            //IDB_DATABASE = MyArr[0].ToString();

        }

        #region "Log"
        public static long WriteToLogFile_Error(string strErrText, string strSourceName, Int16 intCheckFileForDelete = 1)
        {


            StreamWriter oStreamWriter = null;
            string strFileName = string.Empty;
            string strArchFileName = string.Empty;
            string strTempString = string.Empty;
            double lngFileSizeInMB;



            try
            {
                string sLogPath = string.Empty;

                sLogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                if (strSourceName.Length > 30)
                {
                    strTempString = "".PadLeft(0);
                }
                else
                {
                    strTempString = "".PadLeft(30 - strSourceName.Length);
                }


                //strSourceName = strTempString + strSourceName;

                strErrText = "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "]" + "[" + strSourceName + "] " + strErrText;
                strFileName = sLogPath + @"\" + LOG_FILE_ERROR + "_" + DateTime.Now.ToString("yyMMdd") + ".log";
                strArchFileName = sLogPath + @"\" + LOG_FILE_ERROR_ARCH + DateTime.Now.ToString("yyMMddHHMMss") + ".log";


                // strFileName = System.IO.Directory.GetCurrentDirectory() & "\" & LOG_FILE_ERROR & ".log"
                // strArchFileName = System.IO.Directory.GetCurrentDirectory() & "\" & LOG_FILE_ERROR_ARCH & Format(Now(), "yyMMddHHMMss") & ".log"

                if (intCheckFileForDelete == FILE_SIZE_CHECK_ENABLE)
                {
                    if (File.Exists(strFileName))
                    {
                        FileInfo info = new FileInfo(strFileName);


                        lngFileSizeInMB = (info.Length / (double)1024) / 1024;
                        if (lngFileSizeInMB >= MAXFILESIZE_IN_MB)
                        {
                            // If intCheckDeleteDebugLog=1 then remove all debug_log file
                            if (p_iDeleteDebugLog == 1)
                            {
                                foreach (string sFileName in Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), LOG_FILE_ERROR_ARCH + "*"))
                                    File.Delete(sFileName);
                            }
                            File.Move(strFileName, strArchFileName);
                        }
                    }
                }
                oStreamWriter = File.AppendText(strFileName);
                oStreamWriter.WriteLine(strErrText);
                return RTN_SUCCESS;
            }
            catch (Exception exc)
            {
                return RTN_ERROR;
            }
            finally
            {

                oStreamWriter.Flush();
                oStreamWriter.Close();
                oStreamWriter = null;

            }
        }
        public static long WriteToLogFile_Debug(string strErrText, string strSourceName, Int16 intCheckFileForDelete = 1)
        {


            StreamWriter oStreamWriter = null;
            string strFileName = string.Empty;
            string strArchFileName = string.Empty;
            string strTempString = string.Empty;
            double lngFileSizeInMB;



            try
            {
                string sLogPath = string.Empty;

                sLogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                if (strSourceName.Length > 30)
                {
                    strTempString = "".PadLeft(0);
                }
                else
                {
                    strTempString = "".PadLeft(30 - strSourceName.Length);
                }


                //strSourceName = strTempString + strSourceName;

                strErrText = "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "]" + "[" + strSourceName + "] " + strErrText;
                strFileName = sLogPath + @"\" + LOG_FILE_DEBUG + "_" + DateTime.Now.ToString("yyMMdd") + ".log";
                strArchFileName = sLogPath + @"\" + LOG_FILE_DEBUG_ARCH + DateTime.Now.ToString("yyMMddHHMMss") + ".log";


                // strFileName = System.IO.Directory.GetCurrentDirectory() & "\" & LOG_FILE_ERROR & ".log"
                // strArchFileName = System.IO.Directory.GetCurrentDirectory() & "\" & LOG_FILE_ERROR_ARCH & Format(Now(), "yyMMddHHMMss") & ".log"

                if (intCheckFileForDelete == FILE_SIZE_CHECK_ENABLE)
                {
                    if (File.Exists(strFileName))
                    {
                        FileInfo info = new FileInfo(strFileName);


                        lngFileSizeInMB = (info.Length / (double)1024) / 1024;
                        if (lngFileSizeInMB >= MAXFILESIZE_IN_MB)
                        {
                            // If intCheckDeleteDebugLog=1 then remove all debug_log file
                            if (p_iDeleteDebugLog == 1)
                            {
                                foreach (string sFileName in Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), LOG_FILE_DEBUG_ARCH + "*"))
                                    File.Delete(sFileName);
                            }
                            File.Move(strFileName, strArchFileName);
                        }
                    }
                }
                oStreamWriter = File.AppendText(strFileName);
                oStreamWriter.WriteLine(strErrText);
                return RTN_SUCCESS;
            }
            catch (Exception exc)
            {
                return RTN_ERROR;
            }
            finally
            {
                if (oStreamWriter != null)
                {
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                    oStreamWriter = null;
                }
            }
        }

        public static long WriteToLogFile_Debug_JSON(string strErrText, string strSourceName, Int16 intCheckFileForDelete = 1)
        {


            StreamWriter oStreamWriter = null;
            string strFileName = string.Empty;
            string strArchFileName = string.Empty;
            string strTempString = string.Empty;
            double lngFileSizeInMB;



            try
            {
                string sLogPath = string.Empty;

                //sLogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                sLogPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),"Logs");
                if (!Directory.Exists(sLogPath))
                {
                    Directory.CreateDirectory(sLogPath);
                }
                if (strSourceName.Length > 30)
                {
                    strTempString = "".PadLeft(0);
                }
                else
                {
                    strTempString = "".PadLeft(30 - strSourceName.Length);
                }


                //strSourceName = strTempString + strSourceName;

                strErrText = "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "]" + "[" + strSourceName + "] " + strErrText;
                strFileName = sLogPath + @"\" + LOG_FILE_DEBUG + "_" + DateTime.Now.ToString("yyMMdd") + ".log";
                strArchFileName = sLogPath + @"\" + LOG_FILE_DEBUG_ARCH + DateTime.Now.ToString("yyMMddHHMMss") + ".log";


                // strFileName = System.IO.Directory.GetCurrentDirectory() & "\" & LOG_FILE_ERROR & ".log"
                // strArchFileName = System.IO.Directory.GetCurrentDirectory() & "\" & LOG_FILE_ERROR_ARCH & Format(Now(), "yyMMddHHMMss") & ".log"

                if (intCheckFileForDelete == FILE_SIZE_CHECK_ENABLE)
                {
                    if (File.Exists(strFileName))
                    {
                        FileInfo info = new FileInfo(strFileName);


                        lngFileSizeInMB = (info.Length / (double)1024) / 1024;
                        if (lngFileSizeInMB >= MAXFILESIZE_IN_MB)
                        {
                            // If intCheckDeleteDebugLog=1 then remove all debug_log file
                            if (p_iDeleteDebugLog == 1)
                            {
                                foreach (string sFileName in Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), LOG_FILE_DEBUG_ARCH + "*"))
                                    File.Delete(sFileName);
                            }
                            File.Move(strFileName, strArchFileName);
                        }
                    }
                }
                oStreamWriter = File.AppendText(strFileName);
                oStreamWriter.WriteLine(strErrText);
                return RTN_SUCCESS;
            }
            catch (Exception exc)
            {
                return RTN_ERROR;
            }
            finally
            {
                if (oStreamWriter != null)
                {
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                    oStreamWriter = null;
                }
            }
        }
        #endregion

        #region "SQL Execution"
        public static long ExecuteSQLNonQuery(string sQuery)
        {
            string sFuncName = String.Empty;
            sFuncName = "ExecuteSQLNonQuery";
            try
            {
                //if (Global.p_iDebugMode == Global.DEBUG_ON)
                //    Global.WriteToLogFile_Debug("Executing SQL Query : " + sQuery, sFuncName);
                if (Global.SqlCon.State == ConnectionState.Open)
                    Global.SqlCon.Close();
                Global.SqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                //SqlDataAdapter da = new SqlDataAdapter(sQuery, Global.SqlCon);
                cmd.CommandText = sQuery;
                cmd.Connection = Global.SqlCon;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                return Global.RTN_SUCCESS;
            }
            catch (Exception exc)
            {
                Global.WriteToLogFile_Error("Error in Executing SQL Query : " + sQuery + "Msg: " + exc.ToString(), sFuncName);
                return Global.RTN_ERROR;
            }
            finally
            {
                Global.SqlCon.Close();

            }

        }
        public static DataTable ExecuteSQLQueryForDT(string sQuery)
        {
            DataTable dt = new DataTable();
            string sFuncName = String.Empty;
            sFuncName = "ExecuteSQLQueryForDT";
            try
            {
                //if (Global.p_iDebugMode == Global.DEBUG_ON)
                //    Global.WriteToLogFile_Debug("Executing SQL Query : " + sQuery, sFuncName);

                if (Global.SqlCon.State == ConnectionState.Open)
                    Global.SqlCon.Close();
                Global.SqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter(sQuery, Global.SqlCon);

                da.Fill(dt);
                Global.SqlCon.Close();
                return dt;
            }
            catch (Exception exc)
            {
                Global.WriteToLogFile_Error("Error in Executing SQL Query : " + sQuery + "Msg: " + exc.ToString(), sFuncName);
                return dt;
            }
            finally
            {

                Global.SqlCon.Close();

            }
        }
        public static string GetSingleValueFromSql(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ConnectionString;
            string result = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                result = command.ExecuteScalar()?.ToString();
            }
            return result;
        }

        #endregion

        #region "Json Conversion"
        public static string ds2json(DataSet ds)
        {
            return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
            // return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }
        public static string dt2json(DataTable ds)
        {
            return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
        }
        public string String2json(string str)
        {
            return JsonConvert.SerializeObject(str, Newtonsoft.Json.Formatting.Indented);
        }
        public static DataSet jsontodata(string sjon)
        {
            DataSet data = JsonConvert.DeserializeObject<DataSet>(sjon);
            return data;
        }
        public static DataTable jsontodt(string sjon)
        {
            DataTable data = JsonConvert.DeserializeObject<DataTable>(sjon);
            return data;
        }
        #endregion

        #region "API Execution"
        public static bool APIRequest(string Method, string URL, string Body, ref bool APIResponse, ref string APIOutput, ref string APIErrorMsg)
        {
            string sFuncName = String.Empty;
            sFuncName = "APIRequest";
            try
            {
                APIResponse = false;
                APIOutput = "";
                APIErrorMsg = "";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.Method = Method;
                httpWebRequest.ContentType = Global.ContentType;
                httpWebRequest.Headers.Add("ApiAuth", Global.ApiAuth);
                httpWebRequest.Headers.Add("Token", Global.Token);
                if (Body != "")
                {
                    StreamWriter requestWriter = new StreamWriter(httpWebRequest.GetRequestStream(), System.Text.Encoding.ASCII);
                    requestWriter.Write(Body);
                    requestWriter.Close();
                }

                try
                {


                    WebResponse webResponse = httpWebRequest.GetResponse();
                    webResponse = httpWebRequest.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    string response = responseReader.ReadToEnd();
                    responseReader.Close();
                    APIOutput = response;
                    APIResponse = true;
                }
                catch (WebException ex)
                {
                    APIErrorMsg = Global.HandleWebException(ex);
                    Global.WriteToLogFile_Debug("WebException Error : " + ex.ToString(), sFuncName);

                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                APIErrorMsg = ex.Message.ToString();
                Global.WriteToLogFile_Error("Error : " + ex.ToString(), sFuncName);
                return false;
            }
        }
        public static string HandleWebException(WebException wex)
        {
            string sFuncName = String.Empty;
            sFuncName = "HandleWebException";
            string msg = "Error in Response";
            try
            {

                HttpWebResponse hwr = wex.Response as HttpWebResponse;
                if (hwr != null)
                {
                    msg = string.Format("The status code is {0}", (int)hwr.StatusCode);
                    using (System.IO.StreamReader sreader = new System.IO.StreamReader(hwr.GetResponseStream()))
                    {
                        string errorMessage = sreader.ReadToEnd();
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            msg += " Error Msg: " + errorMessage;
                        }
                    }
                }

                return msg;
            }
            catch (Exception ex)
            {
                Global.WriteToLogFile_Debug("HandleWebException Error : " + ex.ToString(), sFuncName);
                return ex.ToString();
            }
        }
        #endregion

        #region "HANA Execution"
        public static bool Non_Execute_HANA(string NoExQuery)
        {
            string connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["HanaConnectionString"].ToString();
            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();
            int output = 0;
            try
            {
                string sFuncName = string.Empty;
                //oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" + p_oCompDef.sDBUser + ";PWD=" + p_oCompDef.sDBPwd + ";SERVERNODE=" + p_oCompDef.sServer + ";CS=" + p_oCompDef.sSAPDBName;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + connectionString + "";
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
                //LogTracker.ErrorLog.WriteLog("The Error is : " + ex.ToString());
            }
            finally
            {
                //this.closeConnection();
            }
        }
        public static bool NonExecuteQuery(string Query)
        {
            string connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["HanaConnectionString"].ToString();
            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();
            try
            {
                string sFuncName = string.Empty;
                oCon.ConnectionString = "DRIVER={HDBODBC};" + connectionString + "";
                oCon.Open();
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = Query;
                oCmd.Connection = (System.Data.Odbc.OdbcConnection)oCon;
                oCmd.CommandTimeout = 0;
                System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(oCmd);
                da.Fill(oDs);
                oCon.Close();
                oCon.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                //this.closeConnection();
            }
        }
        public static string GetSingleValue_HANA(string Query)
        {
            string connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["WadsWorthConnection"].ToString();
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
        public static DataTable GetDataTable_HANA(string Query)
        {
            string connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ToString();
            System.Data.Odbc.OdbcCommand oCmd = new System.Data.Odbc.OdbcCommand();
            DataSet oDs = new DataSet();
            DbProviderFactory oDbProviderFactoryObject = DbProviderFactories.GetFactory("System.Data.Odbc");
            DbConnection oCon = oDbProviderFactoryObject.CreateConnection();


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
                //this.writeLog(ex);
            }
            finally
            {
                //this.closeConnection();
            }
            return oDs.Tables[0];
        }


        public static string GetSingleValueFromHana(string query)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ConnectionString;
            string result = null;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    OleDbCommand command = new OleDbCommand(query, connection);

                    connection.Open();

                    result = command.ExecuteScalar()?.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
            return result;
        }
        #endregion

    }
}