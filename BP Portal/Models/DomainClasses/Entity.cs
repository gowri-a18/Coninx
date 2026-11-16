using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoProject.Models.DomainClasses
{
    public enum ResponseStatus
    {
        Success,
        Failed,
        NotFound,
        Invalid,
        SapConnectionFailed,

    }
    public enum StatusCodes
    {
        IsPosted = 0, Success = 1, Failed, NotFound, InvalidData,
        InvalidCredentials, InternalServerError, SapConnectionError, NoData, InvalidInput
    }
    public enum Postings
    {
        postPurchaseRequest, postPurchaseOrder, postGoodsReceiptPurchaseOrder,
        postApInvoice, postGoodsReturnRequest, postInventoryTransferRequest,
        postInventoryTransfer, postInventoryCount, postInventoryPosting
    }
}