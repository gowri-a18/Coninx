using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;
using System.Linq;
using System.Web;
using DemoProject.Models.DomainClasses;

namespace DemoProject.Models.SqlConnections
{


    public class SqlCon
    {

        private string connectionString;
        private DataTable dtTable;

        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlTransaction sqlTransaction;
        private SqlDataAdapter hanaDataAdapter;
        private ResponseModel responseModel;
        //private DataTable dtTable;
        private bool IsSelectQuery { get; set; }
        internal string Query { get; set; }

        internal object[] objParams { get; set; }

        internal object[] objParamValues { get; set; }

        public SqlCon()
        {
            this.Initialization();
        }

        void Initialization()
        {
            this.dtTable = new DataTable();
            this.responseModel = new ResponseModel();

            this.Query = string.Empty;
            this.objParamValues = this.objParams = null;
            this.connectionString = ConfigurationManager.ConnectionStrings["POSConnection"].ConnectionString;
        }

        /* INSERT UPDATE & DELETE */
        internal int ExecuteNonQuery(bool IsSetCommandTimeOut = false)
        {
            this.sqlCommand = new SqlCommand(this.Query, this.sqlConnection);
            if (IsSetCommandTimeOut)
                this.sqlCommand.CommandTimeout = 10000;

            #region add-parameters-if-any

            if (this.objParams != null && this.objParamValues != null)
                for (int objIndex = 0; objIndex < this.objParams.Length; objIndex++)
                    this.sqlCommand.Parameters.AddWithValue(this.objParams[objIndex].ToString(), this.objParamValues[objIndex]);

            #endregion

            this.sqlCommand.Transaction = this.sqlTransaction;
            return this.sqlCommand.ExecuteNonQuery();
        }

        /* GET CELL VALUE */
        internal string GetScalarString()
        {
            this.sqlCommand = new SqlCommand(this.Query, this.sqlConnection);
            this.sqlCommand.Transaction = this.sqlTransaction;
            return this.sqlCommand.ExecuteScalar().ToString();
        }

        internal void setConnection()
        {
            this.sqlConnection = new SqlConnection(this.connectionString);
            if (this.sqlConnection.State == ConnectionState.Closed) { this.sqlConnection.Open(); }
            this.sqlTransaction = this.sqlConnection.BeginTransaction();
        }

        internal void rollbackTransaction()
        {
            this.sqlTransaction.Rollback();
        }

        internal void commitTransaction()
        {
            this.sqlTransaction.Commit();
        }

        internal void closeConnection()
        {
            if (this.sqlConnection.State == ConnectionState.Open)
            {
                this.sqlConnection.Close();
            }

            if (this.sqlCommand != null)
            {
                this.sqlCommand.Dispose();
            }

            if (this.sqlTransaction != null)
            {
                this.sqlTransaction.Dispose();
            }

            if (this.sqlConnection != null)
            {
                this.sqlConnection.Dispose();
            }
        }

        internal ResponseModel returnCatchLog(object model, Exception ex, Postings posting)
        {
            this.rollbackTransaction();

            this.responseModel.StatusCode = (int)StatusCodes.InternalServerError;
            this.responseModel.StatusMessage = ex.Message;

            LogTracker.ErrorLog.WriteLog(ex.ToString());
            LogTracker.ErrorLog.WriteLog(string.Format("Posting : {0} i/p JSON {1}", posting.ToString(), JsonConvert.SerializeObject(model)));
            return this.responseModel;
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
                LogTracker.ErrorLog.WriteLog(ex.ToString());
            }
            finally
            {
                this.closeConnection();
            }
            return this.dtTable;
        }


        internal ResponseModel responseData()
        {
            this.dtTable = this.GetDataTable();
            if (this.dtTable.Rows.Count > 0)
            {
                //this.responseModel.ResponseData = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(this.dtTable));
                this.responseModel.StatusCode = (int)StatusCodes.Success;
                this.responseModel.StatusMessage = StatusCodes.Success.ToString();
            }
            else
            {
                this.responseModel.StatusCode = (int)StatusCodes.NotFound;
                this.responseModel.StatusMessage = StatusCodes.NotFound.ToString();
            }
            return this.responseModel;
        }

        internal ResponseModel returnSuccessResponse(string documentId, Postings posting)
        {
            this.commitTransaction();

            this.responseModel.StatusCode = (int)StatusCodes.Success;
            this.responseModel.StatusMessage = StatusCodes.Success.ToString();
            this.responseModel.ResponseData = string.Format("new object key document '{0}' is created.", documentId);

            LogTracker.ErrorLog.WriteLog(string.Format("Posting : {0} {1}", posting.ToString(), this.responseModel.ResponseData.ToString()));
            return this.responseModel;
        }
    }
}
