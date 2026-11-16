using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoProject.Models.DomainClasses
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public object ResponseData { get; set; }

       // public string SalesPeriod { get; set; }
    }
    public class POHeader
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string DocNum { get; set; }
        public string NumAtCard { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public List<PODetails> PODetails { get; set; }

    }
    public class PODetails
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UomName { get; set; }
        public string ItemType { get; set; }
        public string Qty { get; set; }
        public string ObjectType { get; set; }
        public string DocEntry { get; set; }
        public string LineNum { get; set; }
    }

}