using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoProject.Models.DomainClasses
{
    public class PaymentPostingResult
    {
        public string CertificateNumber { get; set; }
        public string PaymentDocNum { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class SalesQuatation
    {
        public string CardCode { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }

        public List<SalesQuotationDetails> SalesQuotationDetails { get; set; }

    }

    public class SalesQuotationDetails
    {
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Warehouse { get; set; }
        public string UoM { get; set; }

    }


    public class SalesOrder
    {
        public string CardCode { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }

        public List<SalesOrderDetails> SalesOrderDetails { get; set; }

    }

    public class SalesOrderDetails
    {
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Warehouse { get; set; }
        public string UoM { get; set; }

    }


    public class SalesQuotationPosting
    {
        public string cardcode { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }

        public List<SalesQuotationPostingDetails> SalesQuotationDetails { get; set; }
    }

    public class SalesQuotationPostingDetails
    {

        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Warehouse { get; set; }
        public string UoM { get; set; }

    }

    public class AddSalesQuotation
    {
        public string CardCode { get; set; }
        public string NumAtCard { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string Comments { get; set; }

        public List<AddSalesQuotation_Details> DocumentLines { get; set; }

    }

    public class AddSalesQuotation_Details
    {
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Warehouse { get; set; }
        public string UoMCode { get; set; }
    }

    public class SalesOrderPosting
    {
        public string cardcode { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }

        public List<SalesOrderPostingDetails> SalesOrderDetails { get; set; }
    }

    public class SalesOrderPostingDetails
    {

        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Warehouse { get; set; }
        public string UoM { get; set; }

    }

    public class AddSalesOrder
    {
        public string CardCode { get; set; }
        public string NumAtCard { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string Comments { get; set; }

        public List<AddSalesOrder_Details> DocumentLines { get; set; }

    }

    public class AddSalesOrder_Details
    {
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Warehouse { get; set; }
        public string UoMCode { get; set; }

    }

    public class IncomingPaymentPosting
    {
        public DateTime DocDate { get; set; }
        public double ExchangeRate { get; set; }
        public double Amount { get; set; }
        //public double TransferSum { get; set; }
        public string CardCode { get; set; }
        public string DocCurrency { get; set; }
       public string Site { get; set; } 
        public string TransferAccount { get; set; }
        public string BPLID { get; set; }
        public string CertificateNumber { get; set; }
        //public double PaymentAmount { get; set; }
        public string WithholderName { get; set; }
        //public int InvoiceDocEntry { get; set; }
        public List<IncomingPaymentPostingDetails> InvoiceItemDetails { get; set; }

    }

    public class IncomingPaymentPostingDetails
    {
        public int LineNum { get; set; }
        public string InvoiceDocEntry { get; set; }
        public double SumApplied { get; set; }
        public double AppliedFC { get; set; }
    }

    public class IncomingPayments
    {
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string ProjectCode { get; set; }
        public double TransferSum { get; set; }
        public string CounterReference { get; set; }
        public string Remarks { get; set; }
        //public string CashAccount { get; set; }
        public string TransferAccount { get; set; }
        public string DocType { get; set; }
        public string DocCurrency { get; set; }
        public string JournalRemarks { get; set; }
        public string TransferReference { get; set; }

        public List<IncomingPaymentInvoice> PaymentInvoices { get; set; }
    }

    public class IncomingPaymentInvoice
    {

        public int DocEntry { get; set; }
        public string InvoiceType { get; set; }
        public double SumApplied { get; set; }
        public double AppliedFC { get; set; }
    }
    public class OnAccountPayments
    {
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string ProjectCode { get; set; }
        public double TransferSum { get; set; }
        public string CounterReference { get; set; }
        public string Remarks { get; set; }
        //public string CashAccount { get; set; }
        public string TransferAccount { get; set; }
        public string DocType { get; set; }
        public string DocCurrency { get; set; }
        public string JournalRemarks { get; set; }
        public string TransferReference { get; set; }

        public List<IncomingPaymentInvoice> PaymentInvoices { get; set; }
    }

    public class OnAccountPaymentPosting
    {
        public DateTime DocDate { get; set; }
        public double ExchangeRate { get; set; }
        public double Amount { get; set; }
        //public double TransferSum { get; set; }
        public string CardCode { get; set; }
        public string DocCurrency { get; set; }
        public string Site { get; set; }
        public string TransferAccount { get; set; }
        public string BPLID { get; set; }
        public string CertificateNumber { get; set; }
        //public double PaymentAmount { get; set; }
        public string WithholderName { get; set; }
        //public int InvoiceDocEntry { get; set; }
        

    }
    public class CreditNotePosting
    {

        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }
        public double DocTotal { get; set; }
        public List<CreditNotePostingDetails> CreditNoteDetails { get; set; }

    }

    public class CreditNotePostingDetails
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string WarehouseCode { get; set; }
        public string TaxCode { get; set; }
        public string InvoiceDocEntry { get; set; }
        public int InvoiceLineNum { get; set; }

    }

    public class CreditNote
    {
        public string DocType { get; set; }
        public string CardCode { get; set; }
        public string DocCurrency { get; set; }
        public string CardName { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public double DocTotal { get; set; }
        public List<CreditNote_Details> DocumentLines { get; set; }

    }

    public class CreditNote_Details
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string WarehouseCode { get; set; }
        public string TaxCode { get; set; }
        public int BaseType { get; set; }
        public string BaseEntry { get; set; }
        public int BaseLine { get; set; }

        public List<BatchDetails> BatchNumbers { get; set; }


    }

    public class BatchDetails
    {
        public string BatchNumber { get; set; }
        public double Quantity { get; set; }

    }

    public class Body
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        //public Int32 Index { get; set; }
    }

    public class MarketingDocumentHeader
    {
        public string DocDate { get; set; }
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string CuInvoiceNo { get; set; }
        public string KRACUNumber { get; set; }
        public string DocCur { get; set; }
        public double DocTotal { get; set; }
        public double BalanceDue { get; set; }
        public double TotalBeforeDiscount { get; set; }
        public string BranchId { get; set; }
        public int TotalIndex { get; set; }
        //public List<Products> ItemList { get; set; }

    }

    public class Products
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UomCode { get; set; }
        public string VatPrcnt { get; set; }
        public Double Quantity { get; set; }
        public Double Price { get; set; }
        public Double LineTotal { get; set; }

    }

    public class BankDetails
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
    }
    public class CustomerDetails
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CustomerPIN { get; set; }
    }
    public class CurrencyDetails
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
    }


    public class PaymentAdd
    {
        public string _id { get; set; }
        public string user { get; set; }
        public string trans_date { get; set; }
        public string payment_date { get; set; }
        public double payment_amount { get; set; }
        public List<invoices> invoices { get; set; }
        public string CardName { get; set; }
        public string paymentsTransIDs { get; set; }
        public double invoiceTotalDue { get; set; }
        public double remainingBalance { get; set; }
        public string TruckNumber { get; set; }
        public string SAPSyncStatus { get; set; }
        public string paymentAccount { get; set; }
        public string paymentType { get; set; }
        public string bankCode { get; set; }
        public string checkNo { get; set; }
    }

    public class invoices
    {
        public string _id { get; set; }
        public string trans_id { get; set; }
        public string docEntry { get; set; }
        public string docNum { get; set; }
        public string docDate { get; set; }
        public string docDueDate { get; set; }
        public string numAtCard { get; set; }
        public string cardCode { get; set; }
        public string cardName { get; set; }
        public double vatSum { get; set; }
        public double docTotal { get; set; }
        public double paidTotal { get; set; }
        public double balanceDue { get; set; }
        public string comments { get; set; }
        public string SAPSyncStatus { get; set; }
        public double __v { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public double appliedAmount { get; set; }

    }



    public class PaymentStatus
    {
        public string DocNum { get; set; }
        public string NumAtCard { get; set; }

    }

    public class PaymentStatusResult
    {
        public string id { get; set; }
        public string DocNum { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class ARInvoiceUpdate
    {
        public string DocEntry { get; set; }
        public string TruckNumber { get; set; }
        public string DispatchDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string LoadingStatus { get; set; }
    }
    public class ResponseModelList
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object ResponseData { get; set; }
        public string DocEntry { get; set; }

    }

    public class InvoicePosting
    {
        public string CardCode { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }
        public string BPL_IDAssignedToInvoice { get; set; }
        public List<DocumentLines> InvoiceLines { get; set; }

    }
    public class DocumentLines
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ItemCode { get; set; }

        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Warehouse { get; set; }
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
        public int BaseType { get; set; }
    }
    public class Invoices
    {
        public string CardCode { get; set; }
        public string DocDueDate { get; set; }
        public DateTime DocDate { get; set; }
        public int BPL_IDAssignedToInvoice { get; set; }
        public string NumAtCard { get; set; }
        public string Remarks { get; set; }
        public List<DeliveryNotes> DocumentLines { get; set; }
    }
    public class DeliveryNotes
    {
        public string BaseEntry { get; set; }
        public int BaseLine { get; set; }
        public int BaseType { get; set; }
        public string ItemCode { get; set; }
        public string Quantity { get; set; }

    }

    public class ReturnPosting
    {
        public string CardCode { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }
        public string BPL_IDAssignedToInvoice { get; set; }
        public List<DocumentLines> ReturnLines { get; set; }

    }

    public class Returns
    {
        public string CardCode { get; set; }
        public string DocDueDate { get; set; }
        public DateTime DocDate { get; set; }
        public int BPL_IDAssignedToInvoice { get; set; }
        public string NumAtCard { get; set; }
        public string Remarks { get; set; }
        public List<DeliveryNotes> DocumentLines { get; set; }
    }

}