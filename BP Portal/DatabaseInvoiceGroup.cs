using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Models.DomainClasses
{
    public class DatabaseInvoiceGroup
    {
        public string DatabaseName { get; set; }

        public List<MarketingDocumentHeader> Records { get; set; }

        public int TotalIndex { get; set; }
    }
}