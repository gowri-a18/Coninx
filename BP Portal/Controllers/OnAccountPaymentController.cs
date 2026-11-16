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
    public class OnAccountPaymentController : ApiController
    {
        BusinessLogic BL = new BusinessLogic();

        [HttpPost]
        public ResponseModel OnAccountPayment([FromBody] List<OnAccountPaymentPosting> model)
        {
            return this.BL.OnAccountPayment(model);
        }
    }
}