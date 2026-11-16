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
    public class SalesQuotationController : ApiController
    {
        BusinessLogic BL = new BusinessLogic();

        [HttpPost]
        public ResponseModel SalesQuotation([FromBody] SalesQuotationPosting model)
        {
            return this.BL.SalesQuotation(model);
        }
    }
}
