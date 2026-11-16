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
    [Authorize]
    public class ARInvoiceWithPaymentController : ApiController
    {
        BusinessLogic BL = new BusinessLogic();

        [HttpPost]
        public ResponseModel ARInvoiceWithPayment([FromBody] List<IncomingPaymentPosting> model)
        {
            return this.BL.ARInvoiceWithPayment(model);
        }
    }
}