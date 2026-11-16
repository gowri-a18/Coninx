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
    public class ARInvoiceDetailsController : ApiController
    {
        BusinessLogic BL = new BusinessLogic();

        [HttpGet]
        public ResponseModel ARInvoiceDetails([FromBody] Body model)
        {
            return this.BL.getARinvoiceList(model);
        }
    }
}
