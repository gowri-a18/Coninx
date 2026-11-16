using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DemoProject.Models.BusinessLayer;
using DemoProject.Models.DomainClasses;

namespace POS.Controllers
{
    public class ARInvoiceUpdateController : ApiController
    {
        BusinessLogic BL = new BusinessLogic();

        [HttpPatch]
        public ResponseModel ARInvoiceUpdate([FromBody] List<ARInvoiceUpdate> model)
        {
            return this.BL.ARInvoice(model);
        }
    }
}